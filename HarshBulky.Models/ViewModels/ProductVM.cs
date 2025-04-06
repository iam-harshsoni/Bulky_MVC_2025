using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;



namespace HarshBulky.Models.ViewModels
{
    public class ProductVM
    {
        public Product Product { get; set; }

        /* 
            ValidatNever  to exclude a property from validation, even if it's part of a form submission. 
            This is helpful when certain properties don't require validation logic.       
            - It will not be validated in (ModelState.IsValid) in controller.          
         */

        [ValidateNever]
        public IEnumerable<SelectListItem> CategoryList { get; set; }
    }
}
