using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Payment.EntityLayer.Concrete
{
    public class Product : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountRate { get; set; }
        public int Stock { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public virtual List<Wishlist> Wishlists { get; set; }
    }

    public class ProductConfiguration : BaseEntityConfiguration<Product>
    {
        public override void Configure(EntityTypeBuilder<Product> builder)
        {
            // Önce BaseEntityConfiguration'daki yapılandırmaları uygula
            base.Configure(builder);

            // Product'a özgü yapılandırmalar
            builder.Property(x => x.Name)
                   .HasMaxLength(100)
                   .IsRequired();

            builder.Property(x => x.Description)
                   .HasMaxLength(500)
                   .IsRequired(false);

            builder.Property(x => x.ImagePath)
                   .HasMaxLength(200)
                   .IsRequired(false);

            builder.Property(x => x.Price)
                   .HasColumnType("decimal(18,2)") // Decimal alanın hassasiyeti ve uzunluğu belirleniyor
                   .IsRequired();

            builder.Property(x => x.DiscountRate)
                   .HasColumnType("decimal(5,2)")
                   .IsRequired(false);

            builder.Property(x => x.Stock)
                   .IsRequired();

            builder.HasOne(x => x.Category)
                   .WithMany(x => x.Products)
                   .HasForeignKey(x => x.CategoryId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Wishlists)
                .WithOne(w => w.Product)
                     .HasForeignKey(w => w.ProductId)
                     .OnDelete(DeleteBehavior.Cascade);

            // Eğer Product entity'sine özgü başka yapılandırmalar varsa, burada ekleyebilirsiniz
        }
    }
}
