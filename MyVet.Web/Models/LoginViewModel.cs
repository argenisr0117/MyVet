using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyVet.Web.Models
{
    public class LoginViewModel
    {
        [EmailAddress]
        [Required]
        public string Username { get; set; }

        [MinLength(6)]
        [Required]
        public string Password { get; set; }

        public bool RememberMe { get; set; }
    }
}
