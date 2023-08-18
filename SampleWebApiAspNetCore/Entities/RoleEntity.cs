namespace SampleWebApiAspNetCore.Entities;

public class RoleEntity
{
    public int Id { get; set; }
    public string Role { get; set; }
    public string Information{get;set;}
    public ICollection<UserEntity>Users { get; set; }
}