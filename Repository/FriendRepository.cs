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

        public async Task<Friend> SendRequestConfirmed(Friend friend)
        {
            Friend addFriend = new Friend();
            addFriend.User1 = friend.User1;
            addFriend.User2 = friend.User2;
            addFriend.Sent = true;
            addFriend.Receive = false;
            addFriend.Confirmed = false;
            await miiContext.Friends.AddAsync(addFriend);
            await miiContext.SaveChangesAsync();
            Friend friend1 = await ReceiveRequestConfirmed(friend);
            return addFriend;
        }
        public async Task<Friend> ReceiveRequestConfirmed(Friend friend)
        {
            Friend ReceiveAddFriend = new Friend();
            ReceiveAddFriend.User1 = friend.User2;
            ReceiveAddFriend.User2 = friend.User1;
            ReceiveAddFriend.Receive = true;
            ReceiveAddFriend.Sent = false;
            ReceiveAddFriend.Confirmed = false;
            await miiContext.Friends.AddAsync(ReceiveAddFriend);
            await miiContext.SaveChangesAsync();
            return ReceiveAddFriend;
        }

        public async Task<Friend> GetAllReceiveRequest(string email)
        {
            return   miiContext.Friends.Include(r => r.User2).FirstOrDefault(r=> r.User1 ==email && r.Receive==true);
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
    }
}
