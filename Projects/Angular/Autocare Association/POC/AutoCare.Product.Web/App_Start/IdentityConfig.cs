using System.Threading.Tasks;
using System.Web;
using AutoCare.Product.Application.Infrastructure;
using AutoCare.Product.Web.Infrastructure.IdentityAuthentication;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;

namespace AutoCare.Product.Web
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }
    }

    public class SmsService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            // Plug in your SMS service here to send a text message.
            return Task.FromResult(0);
        }
    }

    // Configure the application user manager used in this application. UserManager is defined in ASP.NET Identity and is used by the application.
    public class ApplicationUserManager : UserManager<AutoCareUser>
    {
        public ApplicationUserManager(IUserStore<AutoCareUser> store)
            : base(store)
        {
        }

        public static ApplicationUserManager Create(IdentityFactoryOptions<ApplicationUserManager> options,
            IOwinContext context)
        {
            var manager = new ApplicationUserManager(new AutoCareUserStore());
            // Configure validation logic for usernames
            //manager.UserValidator = new UserValidator<AutoCareUser>(manager)
            //{
            //    AllowOnlyAlphanumericUserNames = false,
            //    RequireUniqueEmail = true
            //};

            //// Configure validation logic for passwords
            //manager.PasswordValidator = new PasswordValidator
            //{
            //    RequiredLength = 6,
            //    RequireNonLetterOrDigit = true,
            //    RequireDigit = true,
            //    RequireLowercase = true,
            //    RequireUppercase = true,
            //};

            //// Configure user lockout defaults
            //manager.UserLockoutEnabledByDefault = true;
            //manager.DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
            //manager.MaxFailedAccessAttemptsBeforeLockout = 5;

            //// Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
            //// You can write your own provider and plug it in here.
            //manager.RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
            //{
            //    MessageFormat = "Your security code is {0}"
            //});
            //manager.RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
            //{
            //    Subject = "Security Code",
            //    BodyFormat = "Your security code is {0}"
            //});
            //manager.EmailService = new EmailService();
            //manager.SmsService = new SmsService();
            //var dataProtectionProvider = options.DataProtectionProvider;
            //if (dataProtectionProvider != null)
            //{
            //    manager.UserTokenProvider = 
            //        new DataProtectorTokenProvider<ApplicationUser>(dataProtectionProvider.Create("ASP.NET Identity"));
            //}
            return manager;
        }

        public override Task<bool> CheckPasswordAsync(AutoCareUser user, string password)
        {
            if (user.UserName == password)
            {
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }

    // Configure the application sign-in manager which is used in this application.
    public class ApplicationSignInManager : SignInManager<AutoCareUser, string>
    {
        private readonly IJwtTokenHelper _jwtTokenHelper;

        public ApplicationSignInManager(ApplicationUserManager userManager, 
            IAuthenticationManager authenticationManager, IJwtTokenHelper jwtTokenHelper = null)
            : base(userManager, authenticationManager)
        {
            _jwtTokenHelper = jwtTokenHelper ?? new JwtTokenHelper();
        }

        //public override Task<ClaimsIdentity> CreateUserIdentityAsync(AutoCareUser user)
        //{
        //    //var identity = new ClaimsIdentity("JWT");
        //    var identity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
        //    var appUserManager = (ApplicationUserManager)UserManager;
        //    identity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
        //    identity.AddClaim(new Claim(ClaimTypes.Role, "user"));
        //    return Task.FromResult(identity);
        //    //return user.GenerateUserIdentityAsync((ApplicationUserManager)UserManager);
        //}

        public override async Task<SignInStatus> PasswordSignInAsync(string userName, string password, bool isPersistent, bool shouldLockout)
        {
            var token = await _jwtTokenHelper.GetTokenAsync(userName, password);
            if (token == null)
            {
                return SignInStatus.Failure;
            }

            int tokenLifeSpanInMinutes = AppSettingConfiguration.Instance.DefaultTokenExpirationTimeInMinutes;
            if (isPersistent) tokenLifeSpanInMinutes = AppSettingConfiguration.Instance.RememberMeTokenExpirationTimeInMinutes;

            var claimsIdentity = await _jwtTokenHelper.GetClaimsAsync(token, DefaultAuthenticationTypes.ApplicationCookie, tokenLifeSpanInMinutes);
            
            var context = HttpContext.Current.Request.GetOwinContext();
            var authenticationManager = context.Authentication;
            
            authenticationManager.SignIn(claimsIdentity);

            return SignInStatus.Success;
        }

        public static ApplicationSignInManager Create(IdentityFactoryOptions<ApplicationSignInManager> options, 
            IOwinContext context)
        {
            return new ApplicationSignInManager(context.GetUserManager<ApplicationUserManager>(), 
                context.Authentication, new JwtTokenHelper());
        }


    }
}
