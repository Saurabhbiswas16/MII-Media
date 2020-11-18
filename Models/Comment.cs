using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MII_Media.Models
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Message { get; set; }
        public int Likes { get; set; }
        public DateTime CommentTime { get; set; }
        public int PostId { get; set; }
        public Post Post { get; set; }
        public string Commenter { get; set; }
        //public int ApplicationUserId { get; set; }
        //public ApplicationUser ApplicationUser { get; set; }

    }
}
