using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SampleWebApiAspNetCore.Entities;

namespace SampleWebApiAspNetCore.Jwt.Handler;

public class JwtHandler : IJwtHandler
{
    private readonly JwtSetting _jwtSetting;
    public JwtHandler(IOptions<JwtSetting> jwtSetting)
    {
        _jwtSetting = jwtSetting.Value;
    }

    public TokenResponse GenerateAccessToken(UserEntity user, ExpireType expireType, int ExpireTime)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSetting.IssuerSigningKey));


        SigningCredentials _signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


        ClaimsIdentity _claimsIdentity = new ClaimsIdentity(new List<Claim>()
        {
            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
            new Claim(ClaimTypes.Email,user.Email.ToString()),
            new Claim(ClaimTypes.Role,user.Role.Role)
        });


        DateTime _expires = SetExpireDate(expireType, ExpireTime);
        SecurityTokenDescriptor _securityTokenDescriptor = new()
        {
            Issuer = _jwtSetting.ValidateIssuer == true ? _jwtSetting.ValidIssuer : string.Empty,
            Audience = _jwtSetting.ValidateAudience == true ? _jwtSetting.ValidAudience : string.Empty,
            SigningCredentials = _signingCredentials,
            Subject = _claimsIdentity,
            Expires = _expires
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var createToken = tokenHandler.CreateToken(_securityTokenDescriptor);
        var token = tokenHandler.WriteToken(createToken);
        return new TokenResponse(token, _expires);
    }

    public TokenResponse GenerateRefreshToken(ExpireType expireType, int ExpireTime)
    {
        var _expires = SetExpireDate(expireType, ExpireTime);
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        string refreshToken = Convert.ToBase64String(randomNumber);
        return new TokenResponse(refreshToken, _expires);
    }


    DateTime SetExpireDate(ExpireType expireType, int ExpireTime)
    {
        var _expires = DateTime.Now;
        _expires = expireType switch
        {
            ExpireType.Second => _expires.AddSeconds(ExpireTime),
            ExpireType.Minute => _expires.AddMinutes(ExpireTime),
            ExpireType.Hour => _expires.AddHours(ExpireTime),
            ExpireType.Day => _expires.AddDays(ExpireTime),
            _ => _expires.AddSeconds(ExpireTime)
        };

        return _expires;
    }
}