namespace Payment.WebUI.DTOs.AppUserDtos;

public class ResultAppUserDto
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public string PhoneNumber { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreateTime { get; set; }
    public DateTime UpdateTime { get; set; }
}
