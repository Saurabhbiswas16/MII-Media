using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MII_Media.Models
{
    public class Post
    {
        public int PostId { get; set; }
        public string Caption { get; set; }
        public string PostPath { get; set; }
        public int Likes { get; set; }
        //public int ApplicationUserId { get; set; }
        //public ApplicationUser ApplicationUser { get; set; }
        public string AppUser { get; set; }
        public List<Comment> Comments { get; set; }
    }
}
