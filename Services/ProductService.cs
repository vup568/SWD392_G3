using SWD392_MVC.Models;
using SWD392_MVC.Repositories;

namespace SWD392_MVC.Services;

public interface IProductService
{
    IList<Product> GetActiveProducts(string? search = null, int? categoryId = null);
    IList<Product> GetAllProducts(string? search = null, int? categoryId = null, bool? status = null);
    Product? GetProductById(int id);
    bool CreateProduct(Product product);
    bool UpdateProduct(Product product);
    bool ToggleProductStatus(int id);
    IList<ProductCategory> GetCategories();
}

public class ProductService : IProductService
{
    private readonly IProductRepository         _productRepo;
    private readonly IProductCategoryRepository _categoryRepo;

    public ProductService(IProductRepository productRepo, IProductCategoryRepository categoryRepo)
    { _productRepo = productRepo; _categoryRepo = categoryRepo; }

    public IList<Product> GetActiveProducts(string? search = null, int? categoryId = null)
        => _productRepo.GetActiveProducts(search, categoryId);

    public IList<Product> GetAllProducts(string? search = null, int? categoryId = null, bool? status = null)
        => _productRepo.GetProducts(search, categoryId, status);

    public Product? GetProductById(int id) => _productRepo.GetProductById(id);

    public bool CreateProduct(Product product) => _productRepo.CreateProduct(product);

    public bool UpdateProduct(Product product) => _productRepo.UpdateProduct(product);

    public bool ToggleProductStatus(int id) => _productRepo.ToggleStatus(id);

    public IList<ProductCategory> GetCategories() => _categoryRepo.GetActiveCategories();
}
