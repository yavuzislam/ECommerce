namespace Payment.DtoLayer.Dtos.WishlistDtos;

public class ResultWishlistDto
{
    public int Id { get; set; }
    public int AppUserId { get; set; }
    public int ProductId { get; set; }
    public bool IsActive { get; set; }
}
