using SWD392_MVC.Models;

namespace SWD392_MVC.Repositories;

public interface IProductRepository : IRepository<Product>
{
    Product? GetProductById(int id);
    IList<Product> GetProducts(string? search = null, int? categoryId = null, bool? status = null);
    IList<Product> GetActiveProducts(string? search = null, int? categoryId = null);
    bool CreateProduct(Product product);
    bool UpdateProduct(Product product);
    bool ToggleStatus(int id);
}
