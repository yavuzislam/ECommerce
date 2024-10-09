using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;


namespace Payment.EntityLayer.Concrete
{
    public class AppUser : IdentityUser<int>
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Gender { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

        public int? CreateUserId { get; set; }
        public int? UpdateUserId { get; set; }

        public virtual AppUser CreateUser { get; set; }
        public virtual AppUser UpdateUser { get; set; }

        public List<Address> Addresses { get; set; }
        public List<Wishlist> Wishlists { get; set; }
        //public List<Notification> Notifications { get; set; }
    }

    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasOne(a => a.CreateUser)
                   .WithMany()
                   .HasForeignKey(a => a.CreateUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.UpdateUser)
                   .WithMany()
                   .HasForeignKey(a => a.UpdateUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Addresses)
                   .WithOne(a => a.AppUser)
                   .HasForeignKey(a => a.AppUserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Wishlists)
                   .WithOne(w => w.AppUser)
                   .HasForeignKey(w => w.AppUserId)
                   .OnDelete(DeleteBehavior.Cascade);

            //builder.HasMany(x => x.Notifications)
            //       .WithOne(n => n.AppUser)
            //       .HasForeignKey(n => n.AppUserId)
            //       .OnDelete(DeleteBehavior.Cascade);

            builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Surname).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Gender).HasMaxLength(10).IsRequired();
            builder.Property(x => x.IsActive).HasDefaultValue(true).IsRequired();

            builder.Property(x => x.CreateTime)
                   .HasDefaultValueSql("GETDATE()")
                   .IsRequired();

            builder.Property(x => x.UpdateTime)
                   .HasDefaultValueSql("GETDATE()")
                   .IsRequired();
        }
    }
}
