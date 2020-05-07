using System.Linq;
using BulkyBook.DataAccess.Data;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            this.dbContext = dbContext;
        }

        public void Update(Category category)
        {
            var objFromDb = dbContext.Categories.FirstOrDefault(
                s => s.Id == category.Id);

            if (objFromDb != null)
            {
                objFromDb.Name = category.Name;
            }
        }
    }
}