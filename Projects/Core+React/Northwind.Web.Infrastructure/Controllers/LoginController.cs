using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using NLog;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Core.Interfaces.HomePage;
using Northwind.Web.Infrastructure.AuditLog;
using Northwind.Web.Infrastructure.Authorization;
using Northwind.Web.Infrastructure.Helpers;
using Northwind.Web.Infrastructure.Models;
using Northwind.Web.Infrastructure.Models.ViewModels;
using Northwind.Web.Infrastructure.Models.ViewModels.Login;
using Northwind.Web.Models.ViewModels;

namespace Northwind.Web.Infrastructure
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IActiveDirectoryContext _adContext;
        IGroupPermissionService _groupPermission;
        private readonly Logger _logger;

        public LoginController(IConfiguration configuration, IActiveDirectoryContext adContext, IUserService userService, IGroupPermissionService groupPermission)
        {
            _configuration = configuration;
            _userService = userService;
            _adContext = adContext;
            _logger = LogManager.GetCurrentClassLogger();
            _groupPermission = groupPermission;
        }

        [HttpGet]
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                if (!string.IsNullOrEmpty(Request.Query["redir"]))
                {
                    return Redirect(Request.Query["redir"]);
                }
                else if (!string.IsNullOrEmpty(Request.Query["ReturnUrl"]))
                {
                    return Redirect(Request.Query["ReturnUrl"]);
                }
                else
                {
                    return Redirect(_configuration.GetValue<string>("SiteUrl") + "/Home");
                }
            }
            ViewBag.Method = "GET";
            ViewBag.WindowsAuthUrl = _configuration["WindowsAuthUrl"];
            ViewBag.WindowsAuthWait = _configuration["WindowsAuthWait"];
            return View("Index");
        }

        [HttpPost]
        public async Task<ActionResult> Index(LoginRequest request)
        {
            ViewBag.Method = "POST";
            try
            {
                var data = new { UserName = request.Username };
                var user = _userService.Authenticate(_adContext, request.Username, request.Password);
                if (user != null)
                {
                    AuditLogHandler.InfoLog(_logger, user.DisplayName, user.UserGuid, data, "Login", Guid.Empty, UserHelper.GetHostedIp(HttpContext), "Login to application", Guid.Empty, "Login Successful", "Provided correct credientials", "", "");

                    return await SetClaimsAndLogin(user);
                }
                else
                {
                    AuditLogHandler.InfoLog(_logger, request.Username, Guid.Empty, data, "Login", Guid.Empty, UserHelper.GetHostedIp(HttpContext), "Login to application", Guid.Empty, "Login Unsuccessful", "Provided incorrect credientials", "", "");

                    ModelState.AddModelError("", "Invalid username or password.");
                    return View("Index");
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Index");
            }

        }

        [IgnoreAntiforgeryToken]
        [HttpPost]
        public async Task<ActionResult> WindowsAuth(WindowsAuthRequest request)
        {
            ViewBag.Method = "POST";
            try
            {
                var hashedIdentity = GetHash(request.Identity, _configuration.GetValue<string>("hash"));

                if (hashedIdentity == request.Secret)
                {
                    string[] arrIdentity = request.Identity.Split("\\".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    var users = _userService.GetUsersByUsername(new List<string>() { arrIdentity[1] });
                    if (users.Count() > 0)
                    {
                        var user = users.ToList()[0];
                        return await SetClaimsAndLogin(user);
                    }
                    return View("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                    return View("Index");
                }

            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View("Index");
            }
        }

        [Authorize]
        public async Task<IActionResult> SwitchUser([FromBody] UserViewModel userViewModel)
        {
            try
            {
                var user = _userService.GetUserByUsername(userViewModel.Username);
                userViewModel.UserGuid = user.UserGuid;
                await SetClaimsForDifferentUser(userViewModel);
                return Json(new { status = true });
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [Authorize]
        public IActionResult IsSimulatedUser([FromBody] UserViewModel userViewModel)
        {
            try
            {
                var claims = (User.Identity as ClaimsIdentity).Claims;
                var orgClaims = claims.Select(c => c.Type == "OriginalUserClaims").ToList();
                if (orgClaims.Contains(true))
                {
                    return Ok(new { status = true });
                }
                else
                {
                    return Ok(new { status = false });
                }
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                ModelState.AddModelError("", ex.Message);
                return BadRequestFormatter.BadRequest(this, ex);
            }
        }

        [Authorize]
        public async Task<IActionResult> SwitchBackToNormaluser()
        {
            var identity = new ClaimsIdentity("Northwind.App");
            var claims = (User.Identity as ClaimsIdentity).Claims;
            var orgClaims = claims.Select(c => c.Type == "OriginalUserClaims").ToList();
            var dictionary = new Dictionary<string, string>();
            if (orgClaims.Contains(true))
            {
                Claim claim = null;
                foreach (var c in claims)
                {
                    if (c.Type == "OriginalUserClaims")
                    {
                        claim = c;
                        break;
                    }
                }
                if (claim != null)
                {
                    dictionary = Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, string>>(claim.Value);
                    identity.AddClaim(new Claim(ClaimTypes.Name, dictionary[ClaimTypes.Name]));
                    identity.AddClaim(new Claim(ClaimTypes.Email, dictionary[ClaimTypes.Email]));
                    identity.AddClaim(new Claim(ClaimTypes.GivenName, dictionary[ClaimTypes.GivenName]));
                    identity.AddClaim(new Claim(ClaimTypes.Surname, dictionary[ClaimTypes.Surname]));
                    identity.AddClaim(new Claim(ClaimTypes.Upn, dictionary[ClaimTypes.Upn]));
                    identity.AddClaim(new Claim("fullName", dictionary["fullName"]));
                }
                await HttpContext.SignInAsync("Northwind.App", new ClaimsPrincipal(identity), new AuthenticationProperties { IsPersistent = true });
                return Ok(new { status = true });
            }
            else
            {
                return BadRequest();
            }

        }

        [Authorize]
        private async Task SetClaimsForDifferentUser(UserViewModel user)
        {
            var identity = new ClaimsIdentity("Northwind.App");
            var claims = (User.Identity as ClaimsIdentity).Claims;
            var orgClaims = claims.Select(c => c.Type == "OriginalUserClaims").ToList();
            if (!orgClaims.Contains(true))
            {
                var dictionary = new Dictionary<string, string>();
                foreach (var c in claims)
                {
                    dictionary.Add(c.Type, c.Value);
                }

                identity.AddClaim(new Claim("OriginalUserClaims", Newtonsoft.Json.JsonConvert.SerializeObject(dictionary)));
            }
            else
            {
                Claim claim = null;
                foreach (var c in claims)
                {
                    if (c.Type == "OriginalUserClaims")
                    {
                        claim = c;
                        break;
                    }
                }
                if (claim != null)
                    identity.AddClaim(claim);
            }

            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserGuid.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Email, user.WorkEmail == null ? "": user.WorkEmail));
            identity.AddClaim(new Claim(ClaimTypes.GivenName, user.Firstname));
            identity.AddClaim(new Claim(ClaimTypes.Surname, user.Lastname));
            identity.AddClaim(new Claim(ClaimTypes.Upn, user.Username));
            identity.AddClaim(new Claim("fullName", user.Firstname + " " + user.Lastname));

            await HttpContext.SignInAsync("Northwind.App", new ClaimsPrincipal(identity), new AuthenticationProperties { IsPersistent = true });
        }

        private async Task<ActionResult> SetClaimsAndLogin(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("Application")["secret"]);
            IdentityModelEventSource.ShowPII = true;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserGuid.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);
            var identity = new ClaimsIdentity("Northwind.App");
            identity.AddClaim(new Claim(ClaimTypes.Name, user.UserGuid.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Email, user.WorkEmail));
            identity.AddClaim(new Claim(ClaimTypes.GivenName, user.Firstname));
            identity.AddClaim(new Claim(ClaimTypes.Surname, user.Lastname));
            identity.AddClaim(new Claim(ClaimTypes.Upn, user.Username));
            identity.AddClaim(new Claim("token", tokenString));
            identity.AddClaim(new Claim("fullName", user.Firstname + " " + user.Lastname));

            //added for testing job request base on role
            Random randomRole = new Random();
            var role = "1";
            if (user.Firstname.ToLower() == "ashish" || user.Firstname.ToLower() == "santosh")
                role = "sa";
            identity.AddClaim(new Claim(ClaimTypes.Role, role.ToString()));

            await HttpContext.SignInAsync("Northwind.App", new ClaimsPrincipal(identity), new AuthenticationProperties { IsPersistent = true });

            if (!string.IsNullOrEmpty(Request.Query["redir"]))
            {
                var redir = Request.Query["redir"];
                return Redirect(redir);
            }
            else if (!string.IsNullOrEmpty(Request.Query["ReturnUrl"]))
            {
                var redir = Request.Query["ReturnUrl"];
                return Redirect(redir);
            }
            else
            {
                return Redirect(_configuration.GetValue<string>("SiteUrl") + "/Home");
            }

        }

        [HttpGet("/IsAuthorized")]
        [Authorize]
        public IActionResult IsAuthorized(EnumGlobal.ResourceType resourceType, EnumGlobal.ResourceActionPermission action)
        {
            try
            {
                var userGuid = UserHelper.CurrentUserGuid(HttpContext);
                var result = _groupPermission.IsUserPermitted(userGuid, resourceType, action);
                return Json(new { status = result });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }

        [HttpGet("/IsAuthorizedAction")]
        [Authorize]
        public IActionResult IsAuthorized(string resourceType, string resourceAction)
        {
            try
            {
                var userGuid = UserHelper.CurrentUserGuid(HttpContext);
                var result = _groupPermission.IsUserPermitted(userGuid, resourceType, resourceAction);
                return Json(new { status = result });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }

        [HttpPost("/IsAuthorizedAction")]
        [Authorize]
        public IActionResult IsAuthorized([FromBody] List<AuthorizeActionRequest> actions)
        {
            try
            {
                var userGuid = UserHelper.CurrentUserGuid(HttpContext);
                var list = new List<AuthorizeActionResponse>();
                foreach(var resource in actions)
                {
                    var result = _groupPermission.IsUserPermitted(userGuid, resource.ResourceType, resource.ResourceAction);
                    var item = new AuthorizeActionResponse
                    {
                        IsAuthorized = result,
                        ResourceAction = resource.ResourceAction,
                        ResourceType = resource.ResourceType,
                        ExtraParam = resource.ExtraParam
                    };
                    list.Add(item);
                }
                
                
                return Json(new { status = true, output = list });
            }
            catch (Exception ex)
            {
                return Json(new { status = false, message = ex.Message });
            }
        }

        [HttpGet("/Logout")]
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            var data = new { UserName = User.FindFirst("fullName").Value };
            AuditLogHandler.InfoLog(_logger, User.FindFirst("fullName").Value, UserHelper.CurrentUserGuid(HttpContext), data, "", Guid.Empty, UserHelper.GetHostedIp(HttpContext), "Logout", Guid.Empty, "Logout Successful", "", "", "");

            await HttpContext.SignOutAsync();
            return Redirect("~/Login");
        }

        private string GetHash(String text, String key)
        {
            ASCIIEncoding encoding = new ASCIIEncoding();

            Byte[] textBytes = encoding.GetBytes(text);
            Byte[] keyBytes = encoding.GetBytes(key);

            Byte[] hashBytes;

            using (HMACSHA256 hash = new HMACSHA256(keyBytes))
                hashBytes = hash.ComputeHash(textBytes);

            return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        }

    }
}