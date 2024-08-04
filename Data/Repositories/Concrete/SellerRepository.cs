using Core.Entities;
using Data.Contexts;
using Data.Repositories.Base;
using Data.Repositories.Abstract;
using Microsoft.EntityFrameworkCore;



namespace Data.Repositories.Concrete;

public class SellerRepository : Repository<Seller>, ISellerRepository
    {
        private readonly AppDbContext _context;
   
    public SellerRepository(AppDbContext context) : base(context)
        {
            _context = context;
      
    }
    public void Update(Seller seller)
    {
        _context.Sellers.Update(seller);
    }
   


}

