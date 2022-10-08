using Core.Entities;

namespace Core.Interfaces
{
    public interface IProductService
    {
        Task<Product> CreateProduct(string name, string description, int rate);
        Task<Product> GetProductById(int id);
        Task<IReadOnlyList<Product>> GetAllProducts();  
    }
}
