using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Milkent.Models
{
    public class MdlLogin
    {

        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [EmailAddress]
        [RegularExpression(@"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$", ErrorMessage = "Email is not valid.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string Password { get; set; }

        public string Role { get; set; }

        [Required(ErrorMessage = "Email is Required")]
        [RegularExpression("^[A-Za-z ]*$", ErrorMessage = "Name must be Alphabet")]
        [StringLength(30, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        public string UserName { get; set; }

        public string Image { get; set; }
    }
}