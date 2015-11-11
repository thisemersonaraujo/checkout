using Checkout.Infra.Utilitys;
using Checkout.Infra.Models;
using System.Linq;

namespace Checkout.Infra.Repositories
{
    public class AccountRepository : CRUDRepository<Account>
    {
        public Account GetAccount(string Email, string Password)
        {
            string _password = Cryptography.Encrypt(Password);
            return _context.DBSetAccount.Where(a => a.Email.Equals(Email) && a.Password.Equals(_password)).FirstOrDefault();
        }
    }
}