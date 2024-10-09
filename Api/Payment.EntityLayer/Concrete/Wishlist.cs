using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Payment.EntityLayer.Concrete;

public class Wishlist : BaseEntity
{
    public int AppUserId { get; set; }
    public virtual AppUser AppUser { get; set; }
    public int ProductId { get; set; }
    public virtual Product Product { get; set; }
}


public class WihslistConfiguration : BaseEntityConfiguration<Wishlist>
{
    public override void Configure(EntityTypeBuilder<Wishlist> builder)
    {
        base.Configure(builder);

        builder.HasOne(x => x.AppUser)
               .WithMany(x => x.Wishlists)
               .HasForeignKey(x => x.AppUserId)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.Product)
            .WithMany(x => x.Wishlists)
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);
    }
}