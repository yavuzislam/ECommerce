namespace Payment.BusinessLayer.Utilities.Tool;

public class GetCheckAppUserViewModel
{
    public string ID { get; set; }
    public string Email { get; set; }
    public List<string> Roles { get; set; }
    public string Provider { get; set; }
    public bool IsExist { get; set; }
}
