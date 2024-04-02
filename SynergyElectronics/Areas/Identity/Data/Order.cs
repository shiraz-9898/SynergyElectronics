using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SynergyElectronics.Areas.Identity.Data
{
    public class Order
    {
        public int Id { get; set; }
        public string? Invoice_Id { get; set; }
        [Required]
        public string Payment { get; set; }
        [Required]
        public string Name_On_Card { get; set; }
        [Required]
        public string Card_Num { get; set; }
        [Required]
        public string Expiry { get; set; }
        [Required]
        public int CVV { get; set; }
        public int Qty { get; set;}
        
        [Column("Price_Total", TypeName = "numeric(10,2)")]
        public decimal Price_Total { get; set; }
        public string? Created_Date { get; set; }

        [ForeignKey("Users")]
        public string User_Id { get; set; }
        public ApplicationUser Users { get; set; }

        [ForeignKey("Products")]
        public int Prod_Id { get; set; }
        public Product Products { get; set; }
    }
}
