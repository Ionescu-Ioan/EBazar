using EBazar_DAL.Entities;
using EBazar_DAL.Interfaces;
using EBazar_DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBazar_DAL.Repositories
{
    public class CommentRepository : ICommentRepository
    {
        public readonly AppDbContext _context;
        public CommentRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddComment(string username, int productId, string text)
        {
            var user = await _context.Users.Where(x => x.UserName == username).FirstOrDefaultAsync();
            var product = await _context.Products.Where(x => x.Id == productId).FirstOrDefaultAsync();
            if (user == null || text == "" || product == null)
            {
                return false;
            }
            var comment = new Comment
            {
                UserId = user.Id,
                Text = text,
                Date = DateTime.Now,
                ProductId = productId
            };
            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteComment(int commentId)
        {
            var comment = await _context.Comments.Where(x => x.Id == commentId).FirstOrDefaultAsync();
            if (comment == null)
            {
                return false; // nu exista acest comment
            }
            _context.Remove(comment);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<ICollection<CommentModelName>> GetCommentsByUsername(string username)
        {
            var User = await _context.Users.Where(x => x.UserName.ToLower() == username.ToLower()).FirstOrDefaultAsync();
            var comments = await _context.Comments.Where(x => x.UserId == User.Id).ToListAsync();
            var commentsReturn = new List<CommentModelName>();

            if (comments != null)
            {
                foreach (var comment in comments)
                {
                    var product = await _context.Products.Where(x => x.Id == comment.ProductId).FirstOrDefaultAsync();
                    var commentReturn = new CommentModelName();
                    commentReturn.Id = comment.Id;
                    commentReturn.Text = comment.Text;
                    commentReturn.Date = comment.Date;
                    commentReturn.ProductId = comment.ProductId;
                    commentReturn.ProductName = product.Name;
                    var user = await _context.Users.Where(x => x.Id == comment.UserId).FirstOrDefaultAsync();
                    commentReturn.UserName = user.UserName;
                    commentsReturn.Add(commentReturn);
                }
            }
            return commentsReturn;
        }

        public async Task<ICollection<CommentModel>> ShowProductComments(int productId)
        {
            var comments = await _context.Comments.Where(x => x.ProductId == productId).ToListAsync();
            var commentsReturn = new List<CommentModel>();

            if (comments != null)
            {
                foreach (var comment in comments)
                {
                    var commentReturn = new CommentModel();
                    commentReturn.Id = comment.Id;
                    commentReturn.Text = comment.Text;
                    commentReturn.Date = comment.Date;
                    commentReturn.ProductId = comment.ProductId;
                    var user = await _context.Users.Where(x => x.Id == comment.UserId).FirstOrDefaultAsync();
                    commentReturn.UserName = user.UserName;
                    commentsReturn.Add(commentReturn);
                }
            }
            return commentsReturn;
        }
    }
}
