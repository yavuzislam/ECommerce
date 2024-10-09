namespace Payment.DtoLayer.Dtos.NotificationDtos;

public class CreateNotificationDto
{
    public string Title { get; set; }
    public string Message { get; set; }
    public bool IsRead { get; set; }
    public string Url { get; set; }
    public int AppUserID { get; set; }
    public  bool IsUser { get; set; }
}
