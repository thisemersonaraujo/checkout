namespace Checkout.Infra.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("CreditCard")]
    public partial class CreditCard
    {
        public int Id { get; set; }

        [Required]
        [StringLength(60)]
        [MaxLength(60)]
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Required]
        [StringLength(19)]
        [MaxLength(19)]
        [Display(Name = "Número")]
        public string Number { get; set; }

        [Required]
        [StringLength(3)]
        [MaxLength(3)]
        [Display(Name = "CVC")]
        public string Cvc { get; set; }

        [Required]
        [Column(TypeName = "date")]
        [Display(Name = "Data de Vencimento")]
        public DateTime Expiry { get; set; }

        public int AccountId { get; set; }
    }
}