using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Payment.EntityLayer.Concrete
{
    public abstract class BaseEntity
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime UpdateTime { get; set; }

        public int? CreateUserId { get; set; }
        public int? UpdateUserId { get; set; }

        public virtual AppUser CreateUser { get; set; }
        public virtual AppUser UpdateUser { get; set; }
    }

    public class BaseEntityConfiguration<T> : IEntityTypeConfiguration<T> where T : BaseEntity
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.IsActive)
                   .IsRequired();

            builder.Property(x => x.CreateTime)
                   .IsRequired();

            builder.Property(x => x.UpdateTime)
                   .IsRequired();

            builder.HasOne(x => x.CreateUser)
                   .WithMany()
                   .HasForeignKey(x => x.CreateUserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.UpdateUser)
                   .WithMany()
                   .HasForeignKey(x => x.UpdateUserId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
