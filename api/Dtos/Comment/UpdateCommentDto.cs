using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Comment
{
    public class UpdateCommentDto
    {
        [Required]
        [MinLength(5)]
        [MaxLength(100)]
        public string Title { get; set; } = string.Empty;
        [Required]
        [MinLength(5)]
        [MaxLength(280)]
        public string Content { get; set; } = string.Empty;
    }
}