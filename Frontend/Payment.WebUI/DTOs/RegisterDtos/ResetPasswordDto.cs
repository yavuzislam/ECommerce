﻿namespace Payment.WebUI.DTOs.RegisterDtos;

public class ResetPasswordDto
{
    public string Email { get; set; }
    public string Token { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}
