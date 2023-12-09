using EBazar_DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EBazar_DAL.Interfaces
{
    public interface IObjectTypeRepository
    {
        Task<bool> CreateObjectType(ObjectTypeModel objTypeModel);
        Task<bool> DeleteObjectType(ObjectTypeModel objTypeModel);
        Task<List<ObjectTypeViewModel>> GetAllObjectTypes();
    }
}
