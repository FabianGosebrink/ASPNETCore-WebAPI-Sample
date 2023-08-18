namespace SampleWebApiAspNetCore.Helpers;

public static class PasswordHashingHelper
{
    public static string HashPassword(this string password) => BCrypt.Net.BCrypt.HashPassword(password);
    public static bool VerifyHashPassword(this string password, string userPassword) => BCrypt.Net.BCrypt.Verify(password, userPassword);
}