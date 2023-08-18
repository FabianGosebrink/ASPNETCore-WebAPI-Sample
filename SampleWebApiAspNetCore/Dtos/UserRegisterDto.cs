namespace SampleWebApiAspNetCore.Dtos;

public class UserRegisterDto
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? UserName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
}

