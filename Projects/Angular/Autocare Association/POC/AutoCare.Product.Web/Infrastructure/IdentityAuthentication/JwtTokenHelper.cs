using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using AutoCare.Product.Application.Infrastructure;

namespace AutoCare.Product.Web.Infrastructure.IdentityAuthentication
{
    public class JwtTokenHelper : IJwtTokenHelper
    {
        private readonly string _tokenUri;

        public JwtTokenHelper()
        {
            _tokenUri = ToAbsoluteUrl(AppSettingConfiguration.Instance.AuthTokenUrl);
        }

        public async Task<string> GetTokenAsync(string userName, string password)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(_tokenUri);
                client.DefaultRequestHeaders.Accept
                    .Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var content = new FormUrlEncodedContent(new[]
                {
                        new KeyValuePair<string, string>("username", userName),
                        new KeyValuePair<string, string>("password", password),
                        new KeyValuePair<string, string>("grant_type", "password")
                    });
                var response = await client.PostAsync(string.Empty, content);
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    var tokenResponse = await response.Content.ReadAsStringAsync();
                    JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();
                    dynamic tokenTemp =
                           (dynamic)jsonSerializer.DeserializeObject(tokenResponse);
                    if (tokenTemp is Dictionary<string, object>)
                    {
                        object tokenValue;
                        if (((Dictionary<string, object>)tokenTemp).TryGetValue("access_token", out tokenValue))
                        {
                            return tokenValue.ToString();
                        }
                    }
                }

                //return null if unauthenticated
                return null;
            }
        }

        public Task<CustomClaimsIdentity> GetClaimsAsync(string token, string authenticationType)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.ReadToken(token);
            if (securityToken == null)
            {
                return Task.FromResult(default(CustomClaimsIdentity));
            }
            return Task.FromResult(new CustomClaimsIdentity(((JwtSecurityToken)securityToken).Claims, authenticationType));
        }

        public  Task<CustomClaimsIdentity> GetClaimsAsync(string token, string authenticationType, int tokenLifeSpanInMinutes)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.TokenLifetimeInMinutes = tokenLifeSpanInMinutes;

            var securityToken = tokenHandler.ReadToken(token);
            if (securityToken == null)
            {
                return Task.FromResult(default(CustomClaimsIdentity));
            }
            return Task.FromResult(new CustomClaimsIdentity(((JwtSecurityToken)securityToken).Claims, authenticationType));
        }

        private string ToAbsoluteUrl(string relativeUrl)
        {
            if (string.IsNullOrEmpty(relativeUrl))
                return relativeUrl;

            if (HttpContext.Current == null)
                return relativeUrl;

            if (relativeUrl.StartsWith("/"))
                relativeUrl = relativeUrl.Insert(0, "~");
            if (!relativeUrl.StartsWith("~/"))
                relativeUrl = relativeUrl.Insert(0, "~/");

            var url = HttpContext.Current.Request.Url;
            var port = url.Port != 80 ? (":" + url.Port) : String.Empty;

            return $"{url.Scheme}://{url.Host}{port}{VirtualPathUtility.ToAbsolute(relativeUrl)}";
        }
    }

    public interface IJwtTokenHelper
    {
        Task<string> GetTokenAsync(string userName, string password);
        Task<CustomClaimsIdentity> GetClaimsAsync(string token, string authenticationType, int tokenLifeSpanInMinutes);
        Task<CustomClaimsIdentity> GetClaimsAsync(string token, string authenticationType);
    }
}