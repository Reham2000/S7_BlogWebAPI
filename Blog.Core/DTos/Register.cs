using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.DTos
{
    public class Register
    {
        [Required(ErrorMessage = "UserName Is Required")]
        [StringLength(50, MinimumLength = 3)]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Email Is Required")]
        [StringLength(100), EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password Is Required")]
        [StringLength(50, MinimumLength = 4)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string PasswordConfirm { get; set; }
    }
}
