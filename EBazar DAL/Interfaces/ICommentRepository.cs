using EBazar_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBazar_DAL.Interfaces
{
    public interface ICommentRepository
    {
        Task<bool> AddComment(string username, int productId, string text);
        Task<bool> DeleteComment(int commentId);
        Task<ICollection<CommentModel>> ShowProductComments(int productId);
        Task<ICollection<CommentModelName>> GetCommentsByUsername(string username);

    }
}
