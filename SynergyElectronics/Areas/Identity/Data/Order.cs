using System.ComponentModel.DataAnnotations.Schema;

namespace SynergyElectronics.Areas.Identity.Data
{
    public class Order
    {
        public int Id { get; set; }
        public string? Invoice_Id { get; set; }
        public string? Product { get; set; }
        public int Qty { get; set; }
        [Column("Price", TypeName = "numeric(10,2)")]
        public decimal Price { get; set; }
        [Column("Price_Total", TypeName = "numeric(10,2)")]
        public decimal Price_Total { get; set; }
        public string? Created_Date { get; set; }

        [ForeignKey("Users")]
        public string User_Id { get; set; }
        public ApplicationUser Users { get; set; }
    }
}
