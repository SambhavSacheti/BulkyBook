using BulkyBook.Models;

namespace BulkyBook.DataAccess.Repository.IRepository{
    public interface ICoverTypeRepostiory:IRepository<CoverType>
    {
        void Update(CoverType coverType);
    }
}