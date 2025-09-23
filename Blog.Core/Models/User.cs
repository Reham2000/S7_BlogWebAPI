using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Models
{
    public class User : IdentityUser
    {
        [Key]
        //public Guid Id { get; set; }
        //public int Id { get; set; }
        //[Required(ErrorMessage = "UserName Is Required")]
        //[StringLength(50,MinimumLength =3)]
        //public string UserName { get; set; }
        //[Required(ErrorMessage = "Email Is Required")]
        //[StringLength(100),EmailAddress]
        ////[RegularExpression("/^[^ ]+@/")]
        //public string Email { get; set; }
        //[Required(ErrorMessage = "Password Is Required")]
        //[StringLength(50, MinimumLength = 4)]
        //[DataType(DataType.Password)]
        //public string Password { get; set; }


        // Relations

        public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
        public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}
