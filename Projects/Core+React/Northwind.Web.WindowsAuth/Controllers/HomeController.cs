using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Northwind.Web.WindowsAuth.Models;

namespace Northwind.Web.WindowsAuth.Controllers
{
    public class HomeController : Controller
    {
        IConfiguration _configuration = null;
        public HomeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [EnableCors("MyPolicy")]
        public IActionResult Index()
        {
            var model = new
            {
                Name = User.Identity.Name,
                EncryptedValue = GetHash(User.Identity.Name, _configuration.GetValue<string>("hash"))
            };
            ViewBag.WebsiteUrl = _configuration["WebsiteUrl"];
            return PartialView(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
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
