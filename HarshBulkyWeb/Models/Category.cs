using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HarshBulkyWeb.Models
{
    public class Category
    {
        public int CategoryId { get; set; }  // if the name is Id or CategoryId, then .NET CORE will treat it as a PK, so no need to write [Key] above, if the name is something else, then we have to use [Key] to explicitly specify that its a PK.
        [Required]
        [DisplayName("Category Name")]
        public string Name { get; set; }

        [DisplayName("Display Name")]
        public int DisplayOrder { get; set; }
    }
}
