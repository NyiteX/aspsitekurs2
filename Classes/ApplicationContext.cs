using aspsitekurs2.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace aspsitekurs2.Classes
{
    public class ApplicationContext : DbContext
    {
        public DbSet<UserModel> User { get; set; } = null!;
        public DbSet<ProductModel> Product { get; set; } = null!;
        public DbSet<CartModel> Cart { get; set; } = null!;

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
