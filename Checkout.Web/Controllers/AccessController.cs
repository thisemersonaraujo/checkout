using System.Web.Mvc;
using Checkout.Infra.Models;
using Checkout.Infra.Repositories;
using System.Web.Security;

namespace Checkout.Web.Controllers
{
    public class AccessController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogIn(string Email, string Password)
        {
            if (ModelState.IsValid)
            {
                AccountRepository _repository = new AccountRepository();

                Account _account = new Account();
                _account = _repository.GetAccount(Email, Password);

                if (_account != null)
                {
                    Session.Add("Account", _account);

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["AlertLogin"] = "Usuário ou Senha inválidos, verifique e tente novamente.";
                    return RedirectToAction("Index", "Access");
                }
            }

            return View();
        }

        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            Session.Remove("Account");
            Session.Abandon();

            return RedirectToAction("Index", "Account");
        }
    }
}