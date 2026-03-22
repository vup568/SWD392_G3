using Microsoft.EntityFrameworkCore;
using SWD392_MVC.Models;

namespace SWD392_MVC.Repositories;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(OnlineShopContext context) : base(context) { }

    public Product? GetProductById(int id)
        => _context.Products.Include(p => p.Category)
            .Include(p => p.Feedbacks).ThenInclude(f => f.User)
            .FirstOrDefault(p => p.ProductId == id);

    public IList<Product> GetProducts(string? search = null, int? categoryId = null, bool? status = null)
    {
        var q = _context.Products.Include(p => p.Category).AsQueryable();
        if (!string.IsNullOrWhiteSpace(search)) q = q.Where(p => p.ProductName.Contains(search));
        if (categoryId.HasValue) q = q.Where(p => p.CategoryId == categoryId);
        if (status.HasValue) q = q.Where(p => p.Status == status);
        return q.OrderByDescending(p => p.CreatedAt).ToList();
    }

    public IList<Product> GetActiveProducts(string? search = null, int? categoryId = null)
        => GetProducts(search, categoryId, true);

    public bool CreateProduct(Product product)
    {
        product.CreatedAt = DateTime.Now;
        product.Status    = true;
        _context.Products.Add(product);
        _context.SaveChanges();
        return true;
    }

    public bool UpdateProduct(Product product)
    {
        var existing = _context.Products.Find(product.ProductId);
        if (existing == null) return false;
        existing.ProductName = product.ProductName;
        existing.Description = product.Description;
        existing.Price       = product.Price;
        existing.Quantity    = product.Quantity;
        existing.CategoryId  = product.CategoryId;
        existing.ImageUrl    = product.ImageUrl;
        _context.SaveChanges();
        return true;
    }

    public bool ToggleStatus(int id)
    {
        var p = _context.Products.Find(id);
        if (p == null) return false;
        p.Status = !(p.Status ?? true);
        _context.SaveChanges();
        return true;
    }
}
