using System.Data.Entity;
using Checkout.Infra.Models;

namespace Checkout.Infra.Contexts
{
    public class CheckoutContext : DbContext
    {
        public CheckoutContext()
            : base("ConnectionContext")
        {

        }

        public DbSet<Account> DBSetAccount { get; set; }
        public DbSet<CreditCard> DBSetCreditCard { get; set; }
    }
}
