using Checkout.Infra.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Checkout.Infra.Repositories
{
    public class CreditCardRepository : CRUDRepository<CreditCard>
    {
        public IEnumerable<CreditCard> GetCardsByAccount(int Id)
        {
            try
            {
                return _context.DBSetCreditCard.Where(c => c.AccountId.Equals(Id));
            }
            catch (Exception ex)
            {
                throw new Exception("Erro: " + ex.Message);
            }
        }
    }
}
