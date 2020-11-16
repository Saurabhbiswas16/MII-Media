using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MII_Media.ViewModels
{
    public class PostCreateViewModel
    {
        [Required]
        [NotMapped]
        public IFormFile Post { get; set; }
        public string Caption { get; set; }
        public DateTime UploadTime { get; set; }
    }
}
