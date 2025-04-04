using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyWebRazor_Temp.Models
{
    public class Category
    {
        public int CategoryId { get; set; }  // if the name is Id or CategoryId, then .NET CORE will treat it as a PK, so no need to write [Key] above, if the name is something else, then we have to use [Key] to explicitly specify that its a PK.
        [Required]
        [DisplayName("Category Name")]
        [MaxLength(30)]
        public string Name { get; set; }

        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "Display Order must be between 1-100")]
        public int DisplayOrder { get; set; }
    }
}
