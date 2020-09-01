using System;
using System.Web.Mvc;
using AutoCare.Product.Application.ApplicationServices;
using Microsoft.Practices.Unity;

namespace AutoCare.Product.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        public HomeController(IUnityContainer container)
        {
            try
            {
                var makeAppService = container.Resolve<IMakeApplicationService>();
            }
            catch (Exception ex)
            {
                
            }
        }

       // [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}