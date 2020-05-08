namespace BulkyBook.DataAccess.Repository.IRepository
{
    public interface  IUnitOfWork
    {
        ICategoryRepository Category {get;}

        ICoverTypeRepostiory CoverType {get;}
        IStoredProc_Call StoredProc_Call {get;}
        void Dispose();

        void Save();
    }
}