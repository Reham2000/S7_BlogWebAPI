using Blog.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.DTos
{
    public class PostDTo
    {
        public int Id { get; set; }  // PK
        [Required(ErrorMessage = "Title is Requried")]
        [StringLength(50, MinimumLength = 3)]  // 3 : 50 Char
        //[MaxLength(50,ErrorMessage = "Max Length is 50"),MinLength(3)]   // 3 : 50 Char
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is Requried")]
        [MaxLength(300), MinLength(10)]
        public string Content { get; set; }

        // Relations
        // 1. belongs to one category
        public int CategoryId { get; set; }
        // 2. One User Create Post
        public int UserId { get; set; }

    }
}
