using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SynergyElectronics.Areas.Identity.Data
{
    public class Product
    {
        [Key]
        public int Prod_Id { get; set; }
        public string? Prod_Img { get; set; }
        public string? Prod_Name { get; set; }
        public string? Prod_Desc { get; set; }
        [Column("Prod_Price", TypeName = "numeric(10,2)")]
        public double Prod_Price { get; set; }

        [ForeignKey("SubCategories")]
        public int SubCategory_Id { get; set; }
        public SubCategory SubCategories { get; set; }
    }
}
