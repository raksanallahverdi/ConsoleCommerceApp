using Core.Entities;
using Data.Contexts;
using Data.Repositories.Base;
using Data.Repositories.Abstract;



namespace Data.Repositories.Concrete;

public class SellerRepository : Repository<Seller>, ISellerRepository
    {
        private readonly AppDbContext _context;
        public SellerRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }

    }

