using EBazar_BAL.Interfaces;
using EBazar_BAL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EBazar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : Controller
    {
        private readonly ICommentManager _commentManager;

        public CommentController(ICommentManager commentManager)
        {
            _commentManager = commentManager;
        }

        [HttpPost("createComment")]
        public async Task<IActionResult> AddComment([FromBody] AddCommentRequest request)
        {
            var result = await _commentManager.AddComment(request.username, request.productId, request.text);
            if (result)
            {
                return Ok("Success");
            }
            else
            {
                return BadRequest("Failed to add Comment");
            }
        }

        [HttpDelete("deleteComment")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var result = await _commentManager.DeleteComment(id);
            if (result)
            {
                return Ok("Comment deleted successfully");
            }
            else
            {
                return BadRequest("Failed to delete Comment");
            }
        }

        [HttpGet("showProductComments")]
        public async Task<IActionResult> ShowProductComments(int productId)
        {
            var result = await _commentManager.ShowProductComments(productId);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Failed to show Comments");
            }
        }

        [HttpGet("getCommentsByUsername")]
        public async Task<IActionResult> GetCommentsByUsername(string username)
        {
            var result = await _commentManager.GetCommentsByUsername(username);
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Failed to show Comments");
            }
        }

    }
}
