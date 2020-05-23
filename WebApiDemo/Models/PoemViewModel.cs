using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiDemo.Models
{
    public class PoemViewModel
    {
        public int PoemId { get; set; }
        [Required(ErrorMessage = "Title is required")]
        [MaxLength(20)]
        public string Title { get; set; }
        [Required(ErrorMessage = "author is required")]
        [MaxLength(10)]
        public string Author { get; set; }
        public string Content { get; set; }
        public string Decription { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
