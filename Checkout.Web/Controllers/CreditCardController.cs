using System;
using System.Web.Mvc;
using Checkout.Infra.Models;
using Checkout.Infra.Repositories;
using GatewayApiClient.DataContracts;
using GatewayApiClient.DataContracts.EnumTypes;
using GatewayApiClient;
using GatewayApiClient.Utility;
using System.Net;
using System.Linq;
using System.Collections.ObjectModel;

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
            var _account = (Account)Session["Account"];
            if (_account != null)
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        c.AccountId = _account.Id;
                        if (SendRequest(c).ToString() == "OK")
                        {
                            _repository.Add(c);
                        }
                        else
                        {
                            return RedirectToAction("Insert", c);
                        }
                    }
                    catch (Exception)
                    {
                        return RedirectToAction("Insert", c);
                    }
                    TempData["AlertMessage"] = "Cartão validado e cadastrado com sucesso!";
                    return RedirectToAction("Index");
                }
                return View(c);
            }
            else
            {
                return View("Index", "Access");
            }
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
                    TempData["AlertMessage"] = "Ocorreu um erro ao excluir o registro.";
                    return RedirectToAction("Index");
                }
                TempData["AlertMessage"] = "Registro excluído com sucesso.";
                return RedirectToAction("Index");
            }
            else
            {
                return View("Index", "Access");
            }
        }

        public string SendRequest(Infra.Models.CreditCard c)
        {
            try
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

                string statusCode = String.Empty;
                string errorCode = String.Empty;
                string descriptionError = String.Empty;

                var serviceClient = new GatewayServiceClient(merchantKey);
                var httpResponse = serviceClient.Sale.Create(transaction);

                if (httpResponse.HttpStatusCode.ToString() != "Created")
                {
                    errorCode = httpResponse.Response.ErrorReport.ErrorItemCollection[0].ErrorCode.ToString();
                    descriptionError = httpResponse.Response.ErrorReport.ErrorItemCollection[0].Description.ToString();
                    switch (errorCode)
                    {
                        case "400":
                            TempData["AlertMessage"] = "Algum campo não foi enviado ou foi enviado de maneira incorreta. " + descriptionError;
                            break;
                        case "404":
                            TempData["AlertMessage"] = "Recurso não encontrado. " + descriptionError;
                            break;
                        case "500":
                            TempData["AlertMessage"] = "Erro nos servidores da MundiPagg, tente novamente mais tarde. " + descriptionError;
                            break;
                        case "504":
                            TempData["AlertMessage"] = "Tempo comunicação excedida entre a MundiPagg e a adquirente. " + descriptionError;
                            break;
                        default:
                            break;
                    }

                    return TempData["AlertMessage"].ToString();
                }
                else
                {
                    var httpResponse2 = serviceClient.Sale.QueryOrder(httpResponse.Response.OrderResult.OrderKey);
                    statusCode = httpResponse2.HttpStatusCode.ToString();

                    return statusCode;
                }
            }
            catch (Exception e)
            {
                TempData["AlertMessage"] = "Ocorreu um erro ao cadastrar o Cartão de Crédito. Por favor, tente novamente";
                throw new Exception("Erro: " + e.Message);
            }
        }
    }
}