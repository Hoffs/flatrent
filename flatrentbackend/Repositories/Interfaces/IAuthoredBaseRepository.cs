namespace FlatRent.Repositories.Interfaces
{
    public interface IAuthoredBaseRepository<T> : IAuthoredRepository<T>, IBaseRepository<T>
    {
    }
}