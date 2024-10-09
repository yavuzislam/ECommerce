namespace Payment.DtoLayer.Dtos.AddressDtos;

public class GetByIdAddressDto
{
    public int Id { get; set; }
    public string AddressLine { get; set; }
    public string City { get; set; }
    public string District { get; set; }
    public int AppUserId { get; set; }
}
