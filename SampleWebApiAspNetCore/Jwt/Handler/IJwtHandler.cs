using SampleWebApiAspNetCore.Entities;

namespace SampleWebApiAspNetCore.Jwt.Handler;

public interface IJwtHandler
{
    TokenResponse GenerateAccessToken(UserEntity user, ExpireType expireType, int ExpireTime);
    TokenResponse GenerateRefreshToken(ExpireType expireType, int ExpireTime);
}