using Data.Contexts;
using Data.Repositories.Base;
using Data.Repositories.Abstract;

using Core.Entities;

namespace Data.Repositories.Concrete;

public class AdminRepository : Repository<Admin>, IAdminRepository
{
    private readonly AppDbContext _context;
    public AdminRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

}
