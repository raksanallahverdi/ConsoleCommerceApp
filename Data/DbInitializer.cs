using Microsoft.AspNetCore.Identity;
using Data.Contexts;
namespace Data;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
public static class DbInitializer
{
    private static readonly AppDbContext _context;
     static DbInitializer() {
        _context = new AppDbContext(); }
    public static void SeedData()
    {
        SeedAdmin(); }
    public static void SeedAdmin()  {
        if (!_context.Admins.Any()) {
            Admin admin = new Admin     {
                Name = "Admin",
                Surname = "Admin"
            };
            PasswordHasher<Admin> passwordHasher = new PasswordHasher<Admin>();
            admin.Password = passwordHasher.HashPassword(admin, "Admin123");
            _context.Admins.Add(admin);
            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                throw new Exception("Failed to add Admin");
            }
        }
    }
}
