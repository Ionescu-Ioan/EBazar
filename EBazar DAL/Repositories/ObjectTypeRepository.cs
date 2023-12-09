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
    public class ObjectTypeRepository : IObjectTypeRepository
    {
        public readonly AppDbContext _context;
        public ObjectTypeRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<bool> CreateObjectType(ObjectTypeModel objTypeModel)
        {
            var Type = await _context.ObjectTypes.Where(x => x.Type == objTypeModel.Type).FirstOrDefaultAsync();
            if (Type == null)
            {
                var objectType = new ObjectType
                {
                    Type = objTypeModel.Type,
                };
                _context.ObjectTypes.Add(objectType);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> DeleteObjectType(ObjectTypeModel objTypeModel)
        {
            var objType = await _context.ObjectTypes.Where(x => x.Type == objTypeModel.Type).FirstOrDefaultAsync();
            if (objType == null)
            {
                return false;
            }
            _context.Remove(objType);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ObjectTypeViewModel>> GetAllObjectTypes()
        {
            var types = await _context.ObjectTypes.ToListAsync();
            var typesReturn = new List<ObjectTypeViewModel>();
            if (types != null)
                foreach (var type in types)
                {
                    var newType = new ObjectTypeViewModel();
                    newType.Id = type.Id;
                    newType.Type = type.Type;
                    typesReturn.Add(newType);
                }
            return typesReturn;
        }
    }
}
