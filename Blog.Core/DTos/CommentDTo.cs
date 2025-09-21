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
    public class CommentDTo
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Comment Content Is Required")]
        [StringLength(150)]
        public string Content { get; set; }
        // Relations
        // 1. User Who Create Comment
        public int UserId { get; set; }
        // 2. Post which my comment belongs to
        public int PostId { get; set; }
    }
}
