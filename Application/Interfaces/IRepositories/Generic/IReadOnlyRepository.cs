namespace Application.Inerfaces.IRepositories.Generic
{
    public interface IReadOnlyRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
    }
}
