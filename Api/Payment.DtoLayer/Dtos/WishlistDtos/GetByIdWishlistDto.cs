namespace Payment.DtoLayer.Dtos.WishlistDtos;

public class GetByIdWishlistDto
{
    public int Id { get; set; }
    public int AppUserId { get; set; }
    public int ProductId { get; set; }
    public bool IsActive { get; set; }
}
