using Data.Contexts;
using Data.Repositories.Base;
using Data.Repositories.Abstract;

using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Data.Repositories.Concrete;

public class ProductRepository : Repository<Product>, IProductRepository
{
    private readonly AppDbContext _context;
    protected readonly DbSet<Product> _dbTable;
    public ProductRepository(AppDbContext context) : base(context)
    {
        _context = context;

        _dbTable = context.Set<Product>();

    }
    public void Update(Product product)
    {
        _context.Products.Update(product); 
    }
    public override List<Product> GetAll()
    {
        return _dbTable.Include(p => p.Seller).ToList();
    }

}
