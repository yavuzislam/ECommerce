using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Payment.EntityLayer.Concrete;

namespace Payment.DataAccessLayer.Concrete;

public class Context : IdentityDbContext<AppUser, AppRole, int>
{
    public Context(DbContextOptions<Context> options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Wishlist> Wishlists { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new CategoryConfiguration());
        builder.ApplyConfiguration(new AppUserConfiguration());
        builder.ApplyConfiguration(new ProductConfiguration());
        builder.ApplyConfiguration(new AddressConfiguration());
        builder.ApplyConfiguration(new WihslistConfiguration());
    }

}
