using System.Linq;
using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository
{
    class CoverTypeRepository:Repository<CoverType>,ICoverTypeRepostiory
    {
        private readonly ApplicationDbContext dbContext;

        public CoverTypeRepository(ApplicationDbContext dbContext):base(dbContext)
        {
             this.dbContext = dbContext;
        }

        public void Update(CoverType coverType)
        {
           var objFromDb = 
                        dbContext.CoverTypes.FirstOrDefault(o=>o.Id == coverType.Id);
           if(objFromDb != null)
           {
               objFromDb.Name = coverType.Name;
           }
        }
    }
}
