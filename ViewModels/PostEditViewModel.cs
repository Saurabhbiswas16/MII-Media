using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MII_Media.ViewModels
{
    public class PostEditViewModel
    {
        [HiddenInput]
        public int Id { get; set; }
        public string Caption { get; set; }
    }
}
