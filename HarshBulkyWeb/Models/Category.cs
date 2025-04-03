using System.ComponentModel.DataAnnotations;

namespace HarshBulkyWeb.Models
{
    public class Category
    {
        public int CategoryId { get; set; }  // if the name is Id or CategoryId, then .NET CORE will treat it as a PK, so no need to write [Key] above, if the name is something else, then we have to use [Key] to explicitly specify that its a PK.
        [Required]
        public string Name { get; set; }
        public int DisplayOrder { get; set; }
    }
}
