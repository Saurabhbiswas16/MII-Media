using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace MII_Media.Models
{
    public partial class ProfileModel
    {
        public String Id { get; set; }
        public String UserName { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }

        [DataType(DataType.Date)]
        public DateTime DOB { get; set; }

        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        [NotMapped]
        public virtual ApplicationUser ApplicationUser { get; set; }


    }
}
