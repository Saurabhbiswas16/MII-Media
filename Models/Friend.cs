using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MII_Media.Models
{
    public class Friend
    {
        public int FriendId { get; set; }
        public string User1 { get; set; }

        public string User2 { get; set; }

        public bool Sent { get; set; }

        public bool Receive { get; set; }

        public bool Confirmed { get; set; }
    }
}
