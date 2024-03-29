using System.ComponentModel.DataAnnotations;

namespace SynergyElectronics.Areas.Identity.Data
{
    public class Category
    {
        [Key]
        public int Category_Id { get; set; }
        public string? Category_Name { get; set; }
    }
}
