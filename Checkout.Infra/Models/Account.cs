namespace Checkout.Infra.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Account")]
    public partial class Account
    {
        public int Id { get; set; }

        [Required]
        [StringLength(60)]
        [MaxLength(60)]
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        [MaxLength(100)]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required]
        [StringLength(32)]
        [MaxLength(32)]
        [Display(Name = "Senha")]
        public string Password { get; set; }

        [Column(TypeName = "date")]
        [Display(Name = "Data de Nascimento")]
        public DateTime? Birthday { get; set; }

        [Required]
        [StringLength(11)]
        [MaxLength(11)]
        [Display(Name = "CPF")]
        public string Cpf { get; set; }

        [Required]
        [StringLength(1)]
        [Display(Name = "Sexo")]
        public string Sex { get; set; }

        [Required]
        [StringLength(80)]
        [MaxLength(80)]
        [Display(Name = "Endereço")]
        public string Address { get; set; }

        [StringLength(10)]
        [MaxLength(10)]
        [Display(Name = "Número")]
        public string Number { get; set; }

        [StringLength(30)]
        [MaxLength(30)]
        [Display(Name = "Complemento")]
        public string Complement { get; set; }

        [Required]
        [StringLength(40)]
        [MaxLength(40)]
        [Display(Name = "Cidade")]
        public string District { get; set; }

        [Required]
        [StringLength(2)]
        [MaxLength(2)]
        [Display(Name = "UF")]
        public string State { get; set; }

        [Required]
        [StringLength(8)]
        [MaxLength(8)]
        [Display(Name = "CEP")]
        public string ZipCode { get; set; }
    }
}
