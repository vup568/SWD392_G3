using Microsoft.EntityFrameworkCore;
using SWD392_MVC.Models;

namespace SWD392_MVC.Repositories;

public class BaseRepository<T> : IRepository<T> where T : class
{
    protected readonly OnlineShopContext _context;
    protected readonly DbSet<T> _dbSet;

    public BaseRepository(OnlineShopContext context)
    {
        _context = context;
        _dbSet   = context.Set<T>();
    }

    public IList<T> GetAll() => _dbSet.ToList();

    public T? GetById(int id) => _dbSet.Find(id);

    public void Add(T entity)
    {
        _dbSet.Add(entity);
        _context.SaveChanges();
    }

    public void Update(T entity)
    {
        _dbSet.Update(entity);
        _context.SaveChanges();
    }

    public void Delete(int id)
    {
        var entity = GetById(id);
        if (entity != null) { _dbSet.Remove(entity); _context.SaveChanges(); }
    }
}
