using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MII_Media.ViewModels
{
    public class EditProfile
    {
        public String Email { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        public string PhoneNumber { get; set; }

        public string Bio { get; set; }
        public IFormFile ProfilePic { get; set; }
        public bool EditSuccess { get; set; }
    }
}
