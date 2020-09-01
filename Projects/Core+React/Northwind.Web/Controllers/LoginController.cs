using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using NLog;
using Northwind.Core.Entities;
using Northwind.Core.Interfaces;
using Northwind.Web.Models.ViewModels.Login;
using Northwind.Web.AuditLog;
using Northwind.Web.Helpers;

namespace Northwind.Web.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IActiveDirectoryContext _adContext;
        private readonly Logger _logger;

        public LoginController(IConfiguration configuration, IActiveDirectoryContext adContext, IUserService userService)
        {
            _configuration = configuration;
            _userService = userService;
            _adContext = adContext;
            _logger = LogManager.GetCurrentClassLogger();
        }

        [HttpGet]
        public ActionResult Index()
        {
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
                var user = _userService.Authenticate(_adContext, request.Username, request.Password);
                var data = web.Helpers.FormatHelper.ObectToJson(new { UserName = request.Username });

                if (user != null)
                {
                    AuditLogHandler.Log(user.DisplayName, data, "Login", Guid.Empty, UserHelper.GetHostedIp(HttpContext), "Login to application", "Login Successful", "Provided correct credientials", "", "");
                    _logger.Info(string.Empty);

                    return await SetClaimsAndLogin(user);
                }
                else
                {
                    AuditLogHandler.Log(request.Username, data, "Login", Guid.Empty, UserHelper.GetHostedIp(HttpContext), "Login to application", "Login Unsuccessful", "Provided incorrect credientials", "", "");
                    _logger.Info(string.Empty);

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
                    if (arrIdentity[1].ToLower() == "shishir")
                    {
                        arrIdentity[1] = "shishir.baral";
                    }
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
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
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

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties { IsPersistent = true });
            return Redirect("~/Home");
        }

        [HttpGet("/Logout")]
        public async Task<ActionResult> Logout()
        {
            var data = web.Helpers.FormatHelper.ObectToJson(new { UserName = User.FindFirst("fullName").Value });
            AuditLogHandler.Log(User.FindFirst("fullName").Value, data, "", Guid.Empty, UserHelper.GetHostedIp(HttpContext), "Logout", "Logout Successful", "", "", "");
            _logger.Info(string.Empty);

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