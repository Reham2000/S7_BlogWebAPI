using Blog.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.DTos
{
    public class CategoryDTo
    {
        
        public int Id { get; set; }
        [Required(ErrorMessage = "Category Name Is Required")]
        [MaxLength(50), MinLength(2)]
        public string Name { get; set; }
       
    }
}
