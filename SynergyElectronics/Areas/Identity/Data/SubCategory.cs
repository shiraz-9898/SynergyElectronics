using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SynergyElectronics.Areas.Identity.Data
{
    public class SubCategory
    {
        [Key]
        public int SubCategory_Id { get; set; }
        public string? SubCategory_Name { get; set; }

        [ForeignKey("Categories")]
        public int Category_Id { get; set; }
        public Category Categories { get; set; }
    }
}
