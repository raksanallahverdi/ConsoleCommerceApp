using Data.Contexts;
using Data.Repositories.Concrete;
namespace Data.UnitOfWork.Concrete;
public class UnitOfWork:IUnitOfWork
{
    public readonly AdminRepository Admins;
    public readonly SellerRepository Sellers;
    public readonly CustomerRepository Customers;
    public readonly ProductRepository Products;
    public readonly OrderRepository Orders;
    public readonly CategoryRepository Categories;



    private readonly AppDbContext _context;
    public UnitOfWork()
    {
        _context = new AppDbContext();

        Admins = new AdminRepository(_context);
        Sellers = new SellerRepository(_context);
        Customers = new CustomerRepository(_context);
        Products = new ProductRepository(_context);
        Orders = new OrderRepository(_context);
        Categories = new CategoryRepository(_context);
    }
    public void Commit()
    {
        try
        {
            _context.SaveChanges();
        }
        catch (Exception)
        {

            throw;
        }

    }
}
