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
