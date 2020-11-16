using MII_Media.Models;
using System.Threading.Tasks;

namespace MII_Media.Repository
{
    public interface IFriendRepository
    {
        Task<Friend> SendRequestConfirmed(Friend friend);
        Task<Friend> ReceiveRequestConfirmed(Friend friend);
        Task<Friend> GetAllReceiveRequest(string email);

        Task<Friend> ConfirmedRequestSent(string user1, string user2);
        Task<Friend> ConfirmedRequestReceive(int friendId);
    }
}