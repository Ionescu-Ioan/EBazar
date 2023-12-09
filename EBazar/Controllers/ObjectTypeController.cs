using EBazar_BAL.Interfaces;
using EBazar_DAL.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EBazar.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ObjectTypeController : Controller
    {
        private readonly IObjectTypeManager _objectTypeManager;

        public ObjectTypeController(IObjectTypeManager objectTypeManager)
        {
            _objectTypeManager = objectTypeManager;
        }

        [HttpPost("createObjectType")]
        public async Task<IActionResult> CreateObjectType(ObjectTypeModel objTypeModel)
        {
            var result = await _objectTypeManager.CreateObjectType(objTypeModel);
            if (result)
            {
                return Ok("ObjectType created successfully");
            }
            else
            {
                return BadRequest("Failed to create ObjectType");
            }
        }

        [HttpDelete("deleteObjectType/{name}")]
        public async Task<IActionResult> DeleteObjectType(string name)
        {
            ObjectTypeModel objTypeModel = new ObjectTypeModel() { Type = name };
            var result = await _objectTypeManager.DeleteObjectType(objTypeModel);
            if (result)
            {
                return Ok("ObjectType deleted successfully");
            }
            else
            {
                return BadRequest("Failed to delete ObjectType");
            }
        }

        [HttpGet("getAllObjectTypes")]
        public async Task<IActionResult> GetAllObjectTypes()
        {
            var result = await _objectTypeManager.GetAllObjectTypes();
            if (result != null)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest("Failed to get ObjectTypes");
            }
        }
    }
}
