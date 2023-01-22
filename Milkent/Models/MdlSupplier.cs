using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Milkent.Models
{
    public class MdlSupplier
    {
        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Supplier Name is Required")]
        [RegularExpression("^[A-Za-z ]*$", ErrorMessage = "Name must be Alphabet")]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Supplier Address is Required")]
        [StringLength(200, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 3)]
        public string Address { get; set; }

        [Required(ErrorMessage = "Supplier Phone Number is Required")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Phone Number must be numeric")]
        [StringLength(11, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 11)]
        public string PhoneNo { get; set; }

        public string Image { get; set; }

        public string asc { get; set; }

        public string desc { get; set; }

        [Required(ErrorMessage = "Supplier Type is Required")]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Type { get; set; }


        [Required]
        public double PurchasePrice { get; set; }
    }
}