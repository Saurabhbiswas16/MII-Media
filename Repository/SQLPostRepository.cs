using Microsoft.EntityFrameworkCore;
using MII_Media.Data;
using MII_Media.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MII_Media.Repository
{
    public class SQLPostRepository : IPostRepository
    {
        private readonly MiiContext context;

        public SQLPostRepository(MiiContext context)
        {
            this.context = context;
        }

        Post IPostRepository.Add(Post Post)
        {
            context.Posts.Add(Post);
            context.SaveChanges();
            return Post;
        }

        Post IPostRepository.Delete(int Id)
        {
            Post post = context.Posts.Find(Id);
            if (post != null)
            {
                context.Posts.Remove(post);
                context.SaveChanges();
            }
            return post;
        }

        IEnumerable<Post> IPostRepository.GetAllPosts()
        {
            return context.Posts;
        }

        Post IPostRepository.GetPost(int PostId)
        {
            return context.Posts.Include(p => p.Comments).FirstOrDefault(p => p.PostId == PostId);
        }

        Post IPostRepository.Update(Post PostChanges)
        {
            var post = context.Posts.Attach(PostChanges);
            post.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            context.SaveChanges();
            return PostChanges;
        }
    }
}
