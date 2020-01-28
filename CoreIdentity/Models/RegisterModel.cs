using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreIdentity.Models
{
    public class RegisterModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(10,ErrorMessage ="{0} en az {2} en fazla {1} karakter olmalı", MinimumLength = 7) ]
        [DataType(DataType.Password)]
        [DisplayName("Password")]
        public string Password { get; set; }


        [Required]
        [DataType(DataType.Password)]
        [DisplayName("Confirm Password")]
        [Compare("Password",ErrorMessage ="The password and confirm password does not match")]
        public string RePassword { get; set; }
    }
}
