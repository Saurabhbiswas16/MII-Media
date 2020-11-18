using MII_Media.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MII_Media.Repository
{
    public interface IPostRepository
    {
        Post GetPost(int PostId);
        IEnumerable<Post> GetAllPosts(string email);
        Post Add(Post Post);
        Post Update(Post PostChanges);
        Post Delete(int Id);
        //IEnumerable<Comment> GetAllComments(int PostId);
    }
}
