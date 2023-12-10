using EBazar_BAL.Interfaces;
using EBazar_DAL.Interfaces;
using EBazar_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBazar_BAL.Managers
{
    public class CommentManager : ICommentManager
    {
        private readonly ICommentRepository _commentRepository;
        public CommentManager(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public async Task<bool> AddComment(string username, int productId, string text)
        {
            return await _commentRepository.AddComment(username, productId, text);
        }

        public async Task<bool> DeleteComment(int commentId)
        {
            return await _commentRepository.DeleteComment(commentId);
        }

        public async Task<ICollection<CommentModel>> ShowProductComments(int productId)
        {
            return await _commentRepository.ShowProductComments(productId);
        }

        public async Task<ICollection<CommentModelName>> GetCommentsByUsername(string username)
        {
            return await _commentRepository.GetCommentsByUsername(username);
        }
    }
}
