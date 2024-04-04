using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SynergyElectronics.Areas.Identity.Data
{
    public class Cart
    {
        [Key]
        public int Cart_Id { get; set; }
        public bool isSelected { get; set; } = false;
        public int Cart_Qty { get; set; }
        [Column("Cart_Price", TypeName = "numeric(10,2)")]
        public double Cart_Price { get; set; }

        [ForeignKey("Users")]
        public string User_Id { get; set; }
        public ApplicationUser Users { get; set; }

        [ForeignKey("Products")]
        public int Prod_Id { get; set; }
        public Product Products { get; set; }
    }
}
