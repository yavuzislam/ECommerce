namespace Payment.DtoLayer.Dtos.NotificationDtos;

public class GetByIdNotificationDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Message { get; set; }
    public bool IsRead { get; set; }
    public string Url { get; set; }
    public DateTime CreatedAt { get; set; }
    public int AppUserID { get; set; }
}
