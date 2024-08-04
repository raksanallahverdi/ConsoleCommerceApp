using Core.Entities;
using Data.Contexts;
using Data.Repositories.Base;
using Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;


namespace Data.Repositories.Concrete;

    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        private readonly AppDbContext _context;
   
    public CustomerRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
    public void Update(Customer customer)
    {
        _context.Customers.Update(customer);
    }


}

