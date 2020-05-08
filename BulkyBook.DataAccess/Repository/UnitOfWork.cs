using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;

namespace BulkyBook.DataAccess.Repository
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ApplicationDbContext dbContext;

        public ICategoryRepository Category { get; private set;}
        public ICoverTypeRepostiory CoverType { get; set; }
        public IStoredProc_Call StoredProc_Call {get;}
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
            Category = new CategoryRepository(dbContext);
            CoverType = new CoverTypeRepository(dbContext);
            StoredProc_Call = new StoredProc_Call(dbContext);
        }

        public void Save() => dbContext.SaveChanges();
        public void Dispose() => dbContext.Dispose();
    }
}