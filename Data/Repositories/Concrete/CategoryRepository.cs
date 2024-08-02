using Data.Contexts;
using Data.Repositories.Base;
using Data.Repositories.Abstract;

using Core.Entities;

namespace Data.Repositories.Concrete;

public class CategoryRepository : Repository<Category>, ICategoryRepository
{
    private readonly AppDbContext _context;
    public CategoryRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

}
