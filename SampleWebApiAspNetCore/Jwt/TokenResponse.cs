namespace SampleWebApiAspNetCore.Jwt;

public record TokenResponse(string Token, DateTime ExpiredDate);