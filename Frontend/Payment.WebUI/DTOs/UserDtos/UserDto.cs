namespace Payment.WebUI.DTOs.UserDtos
{
    public class UserDto
    {
        public string name { get; set; }
        public string surname { get; set; }
        public string gender { get; set; }
        public DateTime createTime { get; set; }
        public DateTime updateTime { get; set; }
        public object createUserId { get; set; }
        public object updateUserId { get; set; }
        public object createUser { get; set; }
        public object updateUser { get; set; }
        public int id { get; set; }
        public string userName { get; set; }
        public string normalizedUserName { get; set; }
        public string email { get; set; }
        public string normalizedEmail { get; set; }
        public bool emailConfirmed { get; set; }
        public string passwordHash { get; set; }
        public string securityStamp { get; set; }
        public string concurrencyStamp { get; set; }
        public string phoneNumber { get; set; }
        public bool phoneNumberConfirmed { get; set; }
        public bool twoFactorEnabled { get; set; }
        public object lockoutEnd { get; set; }
        public bool lockoutEnabled { get; set; }
        public int accessFailedCount { get; set; }

    }
}
