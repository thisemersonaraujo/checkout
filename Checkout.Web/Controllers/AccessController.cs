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
            try
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
            catch (System.Exception)
            {
                TempData["AlertLogin"] = "Ocorreu um erro ao efetuar o login. Por favor, tente novamente mais tarde.";
                return View("Index", "Access");
            }
        }

        public ActionResult LogOut()
        {
            try
            {
                FormsAuthentication.SignOut();
                Session.Remove("Account");
                Session.Abandon();

                return RedirectToAction("Index", "Account");
            }
            catch (System.Exception)
            {
                TempData["AlertLogin"] = "Ocorreu um erro, recomendamos que faça o procedimento de acessar o sistema e logo após sair.";
                return View("Index", "Access");
            }
        }
    }
}