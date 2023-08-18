namespace SampleWebApiAspNetCore.Entities;

public class UserEntity
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? UserName { get; set;}
    public string Email { get; set;}
    public string PasswordHash { get; set; }
    public string? Token { get; set; }
    public DateTime? TokenExpiredDate{ get; set; }
    public int? RoleId { get; set; }
    public RoleEntity Role { get; set; }
}