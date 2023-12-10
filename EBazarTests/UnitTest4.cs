using EBazar_DAL;
using EBazar_DAL.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBazarTests
{
    [TestFixture]
    public class CommentsTest
    {
        private AppDbContext _contextMock;
        private CommentRepository _commentRepository;

        string username = "rinualex";
        int productId = 1;
        string description = "Test DESCRIPTION";
        int comment1Id;
        int comment2Id;
        int comment3Id;
        [SetUp]
        public void Setup()
        {
            var dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseSqlServer("Data Source=LAPTOP-MO2LLB6V;Initial Catalog=EbazarDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False")  // Replace "your_connection_string_here" with your actual connection string
                .Options;

            _contextMock = new AppDbContext(dbContextOptions);
            _commentRepository = new CommentRepository(_contextMock);

        }

        [Test, Order(1)]
        public async Task AddComments()
        {

            var comment1Result = await _commentRepository.AddComment(username, productId, description+"1");
            var comment2Result = await _commentRepository.AddComment(username, productId, description+"2");
            var comment3Result = await _commentRepository.AddComment(username, productId, description+"3");

            Assert.IsTrue(comment1Result, "Error at adding the first test comment!");
            Assert.IsTrue(comment2Result, "Error at adding the second test comment!");
            Assert.IsTrue(comment3Result, "Error at adding the third test comment!");

        }

        [Test, Order(2)]
        public async Task GetUserComments()
        {
            var userComments = await _commentRepository.GetCommentsByUsername(username);
            Assert.IsNotNull(userComments, "Error at getting user comments!");
            Assert.IsTrue(userComments.Count==3, "Error at getting comments number!");
            foreach(var comment in userComments)
            {
                if (comment1Id != 0)
                {
                    if (comment2Id != 0)
                    {
                        comment3Id = comment.Id;
                    }
                    else
                    {
                        comment2Id = comment.Id;
                    }
                }
                else
                {
                    comment1Id= comment.Id; ;
                }
            }
            var i = 1;
            foreach(var comment in userComments) 
            {
                Assert.IsNotNull(comment, "Comment is null!");
                var equality = false;
                if (comment.Text.Equals(description + i.ToString()) && 
                    comment.UserName.Equals(username) &&
                    comment.ProductId ==1)
                {
                    equality = true;
                }
                i++;
                Assert.IsTrue(equality, "Error at getting user comments informations!");
            }

        }

        [Test, Order(3)]
        public async Task GetProductComments()
        {
            var productComments = await _commentRepository.ShowProductComments(productId);
            Assert.IsNotNull(productComments, "Error at getting product comments!");
            Assert.IsTrue(productComments.Count == 3, "Error at getting comments number!");

            var i = 1;
            foreach (var comment in productComments)
            {
                Assert.IsNotNull(comment, "Comment is null!");
                var equality = false;
                if (comment.Text.Equals(description + i.ToString()) &&
                    comment.UserName.Equals(username) &&
                    comment.ProductId == 1)
                {
                    equality = true;
                }
                i++;
                Assert.IsTrue(equality, "Error at getting product comments informations!");
            }
        }

        [Test, Order(4)]
        public async Task DeleteComments()
        {
            var delete1Comment = await _commentRepository.DeleteComment(comment1Id);
            var delete2Comment = await _commentRepository.DeleteComment(comment2Id);
            var delete3Comment = await _commentRepository.DeleteComment(comment3Id);

            Assert.IsTrue(delete1Comment, "Error at deleting the first test comment!");
            Assert.IsTrue(delete2Comment, "Error at deleting the second test comment!");
            Assert.IsTrue(delete3Comment, "Error at deleting the third test comment!");
        }
    }
}
