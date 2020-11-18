using Microsoft.EntityFrameworkCore;
using MII_Media.Data;
using MII_Media.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MII_Media.Repository
{
    public class SQLCommentRepository : ICommentRepository
    {
        private readonly MiiContext context;

        public SQLCommentRepository(MiiContext context)
        {
            this.context = context;
        }

        Comment ICommentRepository.Add(Comment Comment)
        {
            context.Comments.Add(Comment);
            context.SaveChanges();
            return Comment;
        }

        Comment ICommentRepository.Delete(int Id)
        {
            Comment comment = context.Comments.Find(Id);
            if (comment != null)
            {
                context.Comments.Remove(comment);
                context.SaveChanges();
            }
            return comment;
        }

        Comment ICommentRepository.GetComment(int Id)
        {
            return context.Comments.Find(Id);//Include(c => c.Commenter).FirstOrDefault(c => c.CommentId == Id);
        }

        //Comment ICommentRepository.Update(Comment CommentChanges)
        //{
        //    var comment = context.Comments.Attach(CommentChanges);
        //    comment.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
        //    context.SaveChanges();
        //    return CommentChanges;
        //}
    }
}
