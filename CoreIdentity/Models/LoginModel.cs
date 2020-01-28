using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoreIdentity.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage ="Email zorunlu alan")]
        [UIHint("email")]
        public string Email { get; set; }

        [Required(ErrorMessage ="Şifre zorunlu alan")]
        [UIHint("password")]
        public string Password { get; set; }
    }
}
