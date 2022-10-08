using Core.Entities;
using Core.Interfaces;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services
{

    public class ProductService : IProductService
    {
        private readonly DataContext _context;

        public ProductService(DataContext context)
        {
            _context = context;
        }
        public async Task<Product> CreateProduct(string name, string description, int rate)
        {
            var product= new Product(name, description, rate);  
            _context.Products.Add(product);
            await _context.SaveChangesAsync();  
            return product;
        }

        public async Task<IReadOnlyList<Product>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _context.Products.FindAsync(id);
        }
    }
}
