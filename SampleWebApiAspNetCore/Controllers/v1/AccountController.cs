using System.IO.Compression;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SampleWebApiAspNetCore.Dtos;
using SampleWebApiAspNetCore.Entities;
using SampleWebApiAspNetCore.Helpers;
using SampleWebApiAspNetCore.Jwt;
using SampleWebApiAspNetCore.Jwt.Handler;
using SampleWebApiAspNetCore.Repositories;

namespace SampleWebApiAspNetCore.Controllers.v1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/[controller]/[action]")]
public class AccountController : ControllerBase
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtHandler _jwtHandler;
    private readonly IMapper _mapper;

    public AccountController(IUserRepository userRepository, IJwtHandler jwtHandler, IMapper mapper)
    {
        _userRepository = userRepository;
        _jwtHandler = jwtHandler;
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult Login([FromBody] UserLoginDto loginDto)
    {

        var user = _userRepository.Get(x => x.Email == loginDto.Email && loginDto.Password.VerifyHashPassword(x.PasswordHash), x => x.Role);
        if (user == null)
        {
            return BadRequest("Email or Password is wrong!");
        }

        var accessToken = _jwtHandler.GenerateAccessToken(user, ExpireType.Minute, 10);
        var refreshToken = _jwtHandler.GenerateRefreshToken(ExpireType.Hour, 1);
        user.Token = refreshToken.Token;
        user.TokenExpiredDate = refreshToken.ExpiredDate;
        return Ok(new { Token = accessToken.Token, Expires = accessToken.ExpiredDate, Email = user.Email });
    }

    [HttpPost]
    public IActionResult Register([FromBody] UserRegisterDto registerDto)
    {
        if (ModelState.IsValid)
        {
            registerDto.PasswordHash = registerDto.PasswordHash.HashPassword();
            var user = _mapper.Map<UserEntity>(registerDto);
            user.RoleId = 2;
            _userRepository.Add(user);
            _userRepository.Save();
            return Created("", registerDto);
        }

        return BadRequest();

    }

}