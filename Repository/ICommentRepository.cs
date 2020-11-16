using MII_Media.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MII_Media.Repository
{
    public interface ICommentRepository
    {
        Comment Add(Comment Comment);
        //Comment Update(Comment CommentChanges);
        Comment Delete(int Id);
        Comment GetComment(int Id);
    }
}
