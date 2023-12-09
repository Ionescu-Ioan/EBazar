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
    public class ObjectTypeManager : IObjectTypeManager
    {
        private readonly IObjectTypeRepository _objectTypeRepo;
        public ObjectTypeManager(IObjectTypeRepository objectTypeRepo)
        {
            _objectTypeRepo = objectTypeRepo;
        }
        public async Task<bool> CreateObjectType(ObjectTypeModel objTypeModel)
        {
            return await _objectTypeRepo.CreateObjectType(objTypeModel);
        }

        public async Task<bool> DeleteObjectType(ObjectTypeModel objTypeModel)
        {
            return await _objectTypeRepo.DeleteObjectType(objTypeModel);
        }

        public async Task<List<ObjectTypeViewModel>> GetAllObjectTypes()
        {
            return await _objectTypeRepo.GetAllObjectTypes();
        }
    }
}
