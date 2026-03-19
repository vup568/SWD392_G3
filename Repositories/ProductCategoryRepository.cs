using SWD392_MVC.Models;

namespace SWD392_MVC.Repositories;

public interface IProductCategoryRepository : IRepository<ProductCategory>
{
    IList<ProductCategory> GetAllCategories(string? search = null, bool? status = null);
    IList<ProductCategory> GetActiveCategories();
    bool CreateCategory(ProductCategory cat);
    bool UpdateCategory(ProductCategory cat);
    bool ToggleStatus(int id);
}

public class ProductCategoryRepository : BaseRepository<ProductCategory>, IProductCategoryRepository
{
    public ProductCategoryRepository(OnlineShopContext context) : base(context) { }

    public IList<ProductCategory> GetAllCategories(string? search = null, bool? status = null)
    {
        var q = _context.ProductCategories.AsQueryable();
        if (!string.IsNullOrWhiteSpace(search)) q = q.Where(c => c.CategoryName.Contains(search));
        if (status.HasValue) q = q.Where(c => c.Status == status);
        return q.ToList();
    }

    public IList<ProductCategory> GetActiveCategories()
        => _context.ProductCategories.Where(c => c.Status == true).ToList();

    public bool CreateCategory(ProductCategory cat)
    {
        _context.ProductCategories.Add(cat);
        _context.SaveChanges();
        return true;
    }

    public bool UpdateCategory(ProductCategory cat)
    {
        var existing = _context.ProductCategories.Find(cat.CategoryId);
        if (existing == null) return false;
        existing.CategoryName = cat.CategoryName;
        _context.SaveChanges();
        return true;
    }

    public bool ToggleStatus(int id)
    {
        var c = _context.ProductCategories.Find(id);
        if (c == null) return false;
        c.Status = !(c.Status ?? true);
        _context.SaveChanges();
        return true;
    }
}
