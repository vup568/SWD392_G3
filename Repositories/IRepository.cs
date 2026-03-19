namespace SWD392_MVC.Repositories;

public interface IRepository<T> where T : class
{
    IList<T> GetAll();
    T? GetById(int id);
    void Add(T entity);
    void Update(T entity);
    void Delete(int id);
}
