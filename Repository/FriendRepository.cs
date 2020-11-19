using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MII_Media.Data;
using MII_Media.Models;
using MII_Media.Service;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Providers.Entities;



namespace MII_Media.Repository
{
    public class FriendRepository : IFriendRepository
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly MiiContext miiContext;

        public FriendRepository(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,
             IConfiguration configuration, MiiContext miiContext)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.miiContext = miiContext;
        }
        public async Task<Friend> SendRequestConfirmed(string user1, string user2)
        {

            Friend addFriend = new Friend();
            addFriend.User1 = user1;
            addFriend.User2 = user2;
            addFriend.Sent = true;
            addFriend.Receive = false;
            addFriend.Confirmed = false;
            await miiContext.Friends.AddAsync(addFriend);
            await miiContext.SaveChangesAsync();
            Friend friend1 = await ReceiveRequestConfirmed(user1, user2);
            return addFriend;
        }
        public async Task<Friend> ReceiveRequestConfirmed(string user1, string user2)
        {
            Friend ReceiveAddFriend = new Friend();
            ReceiveAddFriend.User1 = user2;
            ReceiveAddFriend.User2 = user1;
            ReceiveAddFriend.Receive = true;
            ReceiveAddFriend.Sent = false;
            ReceiveAddFriend.Confirmed = false;
            await miiContext.Friends.AddAsync(ReceiveAddFriend);
            await miiContext.SaveChangesAsync();
            return ReceiveAddFriend;
        }


        public async Task<IEnumerable<Friend>> GetAllReceiveRequest(string email)
        {
            return   miiContext.Friends.Where(r=> r.User1 ==email && r.Receive==true);
        }
        public async Task<Friend>  ConfirmedRequestReceive(int friendId)
        {
            var user = miiContext.Friends.Find(friendId);
            string user1 = user.User1;
            string user2 = user.User2;
            var receiveUser= miiContext.Friends.Include(r => r.User1).FirstOrDefault(r => r.User1 == user1 && r.Receive == true && r.User2==user2);
            receiveUser.Receive = false;
            receiveUser.Sent = false;
            receiveUser.Confirmed = true;
             miiContext.Friends.Update(receiveUser);
            miiContext.SaveChanges();
           var result= await ConfirmedRequestSent(user1,user2);
            return result;

        }
        public async Task<Friend> ConfirmedRequestSent(string user1,string user2)
        {
            var receiveUser = miiContext.Friends.Include(r => r.User1).FirstOrDefault(r => r.User1 == user2 && r.Sent == true && r.User1==user2);
            receiveUser.Receive = false;
            receiveUser.Sent = false;
            receiveUser.Confirmed = true;
            miiContext.Friends.Update(receiveUser);
            miiContext.SaveChanges();
            return receiveUser;
        }
        public async Task<IEnumerable<ApplicationUser>>  FetchedAllFriends(string email)
        {
            var allEmail= (IEnumerable<string>)miiContext.Friends.Where(c => c.User1 == email).Select(c => c.User2);
            IList<ApplicationUser> userList = new List<ApplicationUser>();
            foreach(string user2Email in allEmail)
            {
               var  applicationUser = await userManager.FindByEmailAsync(user2Email);
                userList.Add(applicationUser);
            }
            return userList;
            
        }
        public async Task<bool> FriendsConfirmed(string user1, string user2)
        {

            var receiveUser = miiContext.Friends.Include(r => r.User2).FirstOrDefault(r => r.User1 == user1 && r.Confirmed == true && r.User2 == user2);
            if (receiveUser == null)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
    }
}
