using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MII_Media.Models
{
    public class ApplicationUser : IdentityUser
    {
        

        public String FirstName { get; set; }
        public String LastName { get; set; }
        public DateTime DOB { get; set; }
        public string ProfilePicPath { get; set; }
        public string Bio { get; set; }
    }
}
