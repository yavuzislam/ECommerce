using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Payment.EntityLayer.Concrete;

public class Address : BaseEntity
{
    public string AddressLine { get; set; }
    public string City { get; set; }
    public string District { get; set; }
    public int AppUserId { get; set; }
    public AppUser AppUser { get; set; }
}

public class AddressConfiguration : BaseEntityConfiguration<Address>
{
    public override void Configure(EntityTypeBuilder<Address> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.AddressLine)
               .HasMaxLength(255)
               .IsRequired();

        builder.Property(x => x.City)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(x => x.District)
               .HasMaxLength(100)
               .IsRequired();

        builder.HasOne(x => x.AppUser)
            .WithMany(x => x.Addresses)
               .HasForeignKey(x => x.AppUserId)
               .OnDelete(DeleteBehavior.Cascade);
    }
}