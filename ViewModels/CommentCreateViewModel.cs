using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MII_Media.ViewModels
{
    public class CommentCreateViewModel
    {
        [HiddenInput]
        public int PostId { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
