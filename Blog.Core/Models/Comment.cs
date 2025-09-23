using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Blog.Core.Models
{
    public class Comment
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "Comment Content Is Required")]
        [StringLength(150)]
        public string Content { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        // Relations
        // 1. User Who Create Comment
        [ForeignKey("User")]
        public string UserId { get; set; }
        public virtual User User { get; set; }
        // 2. Post which my comment belongs to
        [ForeignKey("Post")]
        public int PostId { get; set; }
        public virtual Post Post { get; set; }
    }
}
