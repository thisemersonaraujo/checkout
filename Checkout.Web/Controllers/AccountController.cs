using System.Web.Mvc;
using Checkout.Infra.Repositories;
using Checkout.Infra.Models;
using Checkout.Infra.Utilitys;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Checkout.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly AccountRepository _repository = new AccountRepository();

        public AccountController()
        {

        }

        public ActionResult Index()
        {
            var _account = (Account)Session["Account"];
            if (_account != null)
            {
                return View(LoadAccount(_account.Id));
            }
            else
            {
                return RedirectToAction("Index", "Access");
            }
        }

        public Account LoadAccount(int Id)
        {
            try
            {
                var _account = _repository.GetById(Id);
                return _account;
            }
            catch (Exception e)
            {
                throw new Exception("Error: " + e.Message);
            }
        }

        public ActionResult Insert()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Insert(Account a)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    IEnumerable<Account> _listAccounts = _repository.GetAll().Where(l => l.Email == a.Email);
                    if (_listAccounts.Count() > 0)
                    {
                        TempData["AlertLogin"] = "Este endereço de e-mail não pode ser utilizado, pois já existe uma conta vinculada.";
                        return RedirectToAction("Index", "Access");
                    }
                    else
                    {
                        a.Password = Cryptography.Encrypt(a.Password);
                        _repository.Add(a);
                    }
                }
                catch (Exception)
                {
                    TempData["AlertLogin"] = "Ocorreu um erro ao incluir o registro. Tente novamente mais tarde.";
                    return RedirectToAction("Index", "Access");
                }
                TempData["AlertLogin"] = "Conta cadastrada com sucesso.";
                return RedirectToAction("Index", "Access");
            }
            return View(a);
        }

        public ActionResult Delete(int id)
        {
            var _account = (Account)Session["Account"];
            if (_account != null)
            {
                try
                {
                    _repository.Delete(_repository.GetById(id));
                }
                catch (Exception)
                {
                    TempData["AlertMessage"] = "Existem cartões vinculados a sua conta, remova-os antes de deletar sua conta.";
                    return RedirectToAction("Index", "Account");
                }
                TempData["AlertLogin"] = "Registro excluído com sucesso.";
                return RedirectToAction("Index", "Access");
            }
            else
            {
                return View("Index", "Access");
            }
        }

        public ActionResult Edit(int id)
        {
            var _account = (Account)Session["Account"];
            if (_account != null)
            {
                var account = _repository.GetById(id);
                return View("Edit", account);
            }
            else
            {
                return RedirectToAction("Index", "Access");
            }
        }

        [HttpPost]
        public ActionResult Edit(Account a)
        {
            var _account = (Account)Session["Account"];
            if (_account != null)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        if (a.Password.ToString() != _account.Password.ToString())
                        {
                            a.Password = Cryptography.Encrypt(a.Password);
                        }
                        _repository.Update(a);
                    }
                    catch (Exception)
                    {
                        TempData["AlertMessage"] = "Erro: Ocorreu um erro ao atualizar o registro";
                        return RedirectToAction("Index");
                    }
                    TempData["AlertMessage"] = "Registro atualizado com sucesso.";
                    return RedirectToAction("Index");
                }
                return View(a);
            }
            else
            {
                return View("Index", "Access");
            }
        }

        public ActionResult Detail(int id)
        {
            var _account = (Account)Session["Account"];
            if (_account != null)
            {
                DetailAccount(id);
                return View();
            }
            else
            {
                return View("Index", "Access");
            }
        }

        public void DetailAccount(int id)
        {
            var _account = (Account)Session["Account"];
            if (_account != null)
            {
                try
                {
                    Account _a = new Account();
                    _a = _repository.GetById(id);
                    ViewBag.Account = _a;
                }
                catch (Exception e)
                {
                    throw new Exception("Error: " + e.Message);
                }
            }
            else
            {
                RedirectToAction("Index", "Access");
            }
        }
    }
}