using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Models
{
    public class Post
    {
        [Key]
        public int Id { get; set; }  // PK
        [Required(ErrorMessage = "Title is Requried")]
        [StringLength(50,MinimumLength = 3)]  // 3 : 50 Char
        //[MaxLength(50,ErrorMessage = "Max Length is 50"),MinLength(3)]   // 3 : 50 Char
        public string Title { get; set; }

        [Required(ErrorMessage = "Content is Requried")]
        [MaxLength(300),MinLength(10)]
        public string Content { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Relations
        // 1. belongs to one category
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        // 2. One User Create Post
        [ForeignKey("User")]
        public int UserId { get; set; }
        public User User { get; set; }

        ICollection<Comment> comments { get; set; } = new List<Comment>();

    }
}
