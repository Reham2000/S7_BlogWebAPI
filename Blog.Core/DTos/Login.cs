using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.DTos
{
    public class Login
    {
        
        [Required(ErrorMessage = "Email Is Required")]
        [StringLength(100), EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password Is Required")]
        [StringLength(50, MinimumLength = 4)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
