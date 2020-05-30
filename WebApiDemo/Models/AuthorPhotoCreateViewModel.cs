using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApiDemo.Models
{
    public class AuthorPhotoCreateViewModel
    {
        public int AuthorId { get; set; }
        public string Extension { get; set; }
        public byte[] Data { get; set; }
    }
}
