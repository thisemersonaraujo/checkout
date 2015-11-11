using System.Web.Mvc;
using Checkout.Infra.Models;

namespace Checkout.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var _account = (Account)Session["Account"];
            if (_account != null)
            {
                @ViewBag.AccountName = _account.Name;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Access");
            }
        }
    }
}