namespace Payment.WebUI.DTOs.AddressDtos;

public class CreateAddressDto
{
    public string AddressLine { get; set; }
    public string City { get; set; }
    public string District { get; set; }
    public int AppUserId { get; set; }
}
