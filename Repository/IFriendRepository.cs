using MII_Media.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MII_Media.Repository
{
    public interface IFriendRepository
    {
        Task<IEnumerable<Friend>> GetAllReceiveRequest(string email);

        Task<Friend> ConfirmedRequestSent(string user1, string user2);
        Task<Friend> ConfirmedRequestReceive(int friendId);
        Task<Friend> SendRequestConfirmed(string user1, string user2);
        Task<Friend> ReceiveRequestConfirmed(string user1, string user2);
        Task<bool> FriendsConfirmed(string userEmail, string friendEmail);

        Task<IEnumerable<ApplicationUser>> FetchedAllFriends(string email);
    }
}