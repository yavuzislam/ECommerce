using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Payment.EntityLayer.Concrete;

public class Category : BaseEntity
{
    public string Name { get; set; }    
    public string Description { get; set; }
    public string ImagePath { get; set; }
    public List<Product> Products { get; set; }

}

public class CategoryConfiguration : BaseEntityConfiguration<Category>
{
    public override void Configure(EntityTypeBuilder<Category> builder)
    {
        // Önce BaseEntityConfiguration'daki yapılandırmaları uygula
        base.Configure(builder);

        // Category'ye özgü yapılandırmalar
        builder.Property(x => x.Name)
               .HasMaxLength(100)
               .IsRequired();

        builder.Property(x => x.Description)
               .HasMaxLength(500)
               .IsRequired(false);

        builder.Property(x => x.ImagePath)
               .HasMaxLength(200)
               .IsRequired(false);

        builder.HasMany(x => x.Products)
               .WithOne(x => x.Category)
               .HasForeignKey(x => x.CategoryId)
               .OnDelete(DeleteBehavior.Cascade);

        // Eğer Category entity'sine özgü başka yapılandırmalar varsa, burada ekleyebilirsiniz
    }
}
