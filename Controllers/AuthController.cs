using Api.Data;
using Api.Dtos;
using Api.Helper;
using Api.Interfaces;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Api.Controllers;

public class AuthController : CustomBaseController
{
    private readonly FelchenContext _context;
    private readonly EmailService _emailService;
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;

    public AuthController(FelchenContext context, ITokenService tokenService, IUserRepository userRepository,
        IOptions<EmailSettings> options)
    {
        _context = context;
        _tokenService = tokenService;
        _userRepository = userRepository;
        _emailService = new EmailService(options);
    }

    [AllowAnonymous]
    [HttpPost("[action]")]
    public async Task<ActionResult<AppUserDto>> Login(LoginDto loginDto)
    {
        var user = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email.Equals(loginDto.Email));
        if (user == null) return BadRequest("E-Mail oder Password falsch!");
        if (!user.IsActive) return BadRequest("User ist inaktiv, bitte Kontaktiere ein Administrator");

        var verifyPassword = PasswordService.VerifyPassword(loginDto.Password, user.Password, user.Salt);
        if (!verifyPassword) return BadRequest("E-Mail oder Password falsch!");

        return Ok(new AppUserDto
        {
            UserId = user.Id,
            Token = _tokenService.CreateToken(user),
            FirstName = user.FirstName
        });
    }

    [AllowAnonymous]
    [HttpPost("[action]")]
    public async Task<ActionResult<AppUserDto>> Register(Register register)
    {
        var userDto = new UserDto
        {
            Email = register.Email,
            FirstName = register.FirstName,
            LastName = register.LastName,
            CreatedAt = DateTime.Now,
            IsActive = true,
            Password = register.Password
        };
        var newUser = await _userRepository.InsertUserAsync(userDto);
        return Ok(new AppUserDto
        {
            FirstName = newUser.FirstName,
            UserId = newUser.Id,
            Token = _tokenService.CreateToken(await _context.Users.FindAsync(newUser.Id))
        });
    }

    [AllowAnonymous]
    [HttpGet("[action]")]
    public async Task<ActionResult<bool>> ForgotPassword([FromQuery] string email)
    {
        try
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null) return BadRequest("E-Mail nicht registriert");
            var randomPassword = PasswordService.CreateRandomPassword();
            var changePassword = new ChangePassword
            {
                UserId = user.Id,
                Password = randomPassword
            };
            var checkUpdate = await _userRepository.ChangeUserPasswordAsync(changePassword);
            if (!checkUpdate) return BadRequest("Passwort konnte nicht zurückgesetzt werden");
            var message = EmailMessages.CreateForgotPasswordMessage(randomPassword);
            var checkMailSend = await _emailService.SendEmailAsync(user.Email, message, "Neues Passwort");
            if (!checkMailSend) return BadRequest("E-Mail mit neuem Passwort konnte nicht gesendet werden");
            return Ok(true);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [AllowAnonymous]
    [HttpGet("[action]")]
    public async Task<bool> CheckEmailExists([FromQuery] string email)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email.Equals(email));

        return user != null;
    }
}