using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Payment.EntityLayer.Concrete;

public class RefreshToken
{
    public int Id { get; set; }
    public string Token { get; set; }
    public string UserId { get; set; }
    public DateTime Expires { get; set; }
    public bool IsRevoked { get; set; }
    public DateTime Created { get; set; }
    public DateTime? Revoked { get; set; }
}

//public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
//{
//    public void Configure(EntityTypeBuilder<RefreshToken> builder)
//    {
//        builder.HasKey(x => x.Token);
//        builder.Property(x => x.Token).HasMaxLength(200).IsRequired();
//        builder.Property(x => x.UserId).HasMaxLength(450).IsRequired();
//        builder.Property(x => x.Created).IsRequired();
//        builder.Property(x => x.Expires).IsRequired();
//        builder.Property(x => x.IsRevoked).IsRequired();
//        builder.Property(x => x.Revoked).IsRequired(false);

//        builder.HasIndex(x => x.UserId);
//    }
//}
