using System;
using System.Web.Mvc;
using Checkout.Infra.Models;
using Checkout.Infra.Repositories;
using GatewayApiClient.DataContracts;
using GatewayApiClient.DataContracts.EnumTypes;
using GatewayApiClient;

namespace Checkout.Web.Controllers
{
    public class CreditCardController : Controller
    {
        private readonly CreditCardRepository _repository = new CreditCardRepository();

        public CreditCardController()
        {

        }

        public ActionResult Index()
        {
            var _account = (Account)Session["Account"];
            if (_account != null)
            {
                LoadCreditCards();
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Access");
            }
        }

        public JsonResult ListAll()
        {
            var list = _repository.GetAll();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public void LoadCreditCards()
        {
            try
            {
                Infra.Models.CreditCard c = new Infra.Models.CreditCard();
                Account _account = new Account();
                _account = (Account)Session["Account"];
                ViewBag.CreditCards = _repository.GetCardsByAccount(_account.Id);
            }
            catch (Exception e)
            {
                throw new Exception("Error: " + e.Message);
            }
        }

        public ActionResult Insert()
        {
            var _account = (Account)Session["Account"];
            if (_account != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Access");
            }
        }

        [HttpPost]
        public ActionResult Insert(Infra.Models.CreditCard c)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Account _account = new Account();
                    _account = (Account)Session["Account"];
                    c.AccountId = _account.Id;
                    if (SendRequest(c).ToString() == "OK")
                    {
                        _repository.Add(c);
                    }
                    else
                    {
                        TempData["AlertMessage"] = "Cartão Inválido, verifique os dados e tente novamente!";
                        return RedirectToAction("Insert", c);
                    }
                }
                catch (Exception)
                {
                    TempData["AlertMessage"] = "Ocorreu um erro ao incluir o registro. Verifique os dados do cartão e tente novamente.";
                    return RedirectToAction("Index");
                }
                TempData["AlertMessage"] = "Cartão validado e cadastrado com sucesso!";
                return RedirectToAction("Index");
            }
            return View(c);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                _repository.Delete(_repository.GetById(id));
            }
            catch (Exception)
            {
                TempData["AlertMessage"] = "Ocorreu um erro ao excluir o registro.";
                return RedirectToAction("Index");
            }
            TempData["AlertMessage"] = "Registro excluído com sucesso.";
            return RedirectToAction("Index");
        }

        public string SendRequest(Infra.Models.CreditCard c)
        {
            var transaction = new CreditCardTransaction()
            {
                AmountInCents = 100,
                CreditCard = new GatewayApiClient.DataContracts.CreditCard()
                {
                    CreditCardNumber = c.Number,
                    CreditCardBrand = CreditCardBrandEnum.Visa,
                    ExpMonth = c.Expiry.Date.Month,
                    ExpYear = c.Expiry.Date.Year,
                    SecurityCode = c.Cvc,
                    HolderName = c.Name
                }
            };

            Guid merchantKey = Guid.Parse("2feddd0e-174d-4a1e-889b-e7f6785ccf11");

            var serviceClient = new GatewayServiceClient(merchantKey);

            var httpResponse = serviceClient.Sale.Create(transaction);

            var httpResponse2 = serviceClient.Sale.QueryOrder(httpResponse.Response.OrderResult.OrderKey);

            return httpResponse2.HttpStatusCode.ToString();
        }
    }
}