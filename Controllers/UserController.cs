using Api.Dtos;
using Api.Helper;
using Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class UserController : CustomBaseController
{
    private readonly IUserRepository _userRepository;

    public UserController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetUsers()
    {
        var users = await _userRepository.GetUsersAsync();
        if (users.Count <= 0) return NoContent();
        return Ok(users);
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<UserDto>> GetUserById(int userId)
    {
        var user = await _userRepository.GetUserByIdAsync(userId);
        if (user == null) return NotFound("Es wurde kein User mit dieser Id gefunden");
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<UserDto>> InsertUser(UserDto userDto)
    {
        try
        {
            if (userDto == null) return BadRequest("User darf nicht leer sein");
            var checkUserWithSameEmail = await _userRepository.GetUserByEmailAsync(userDto.Email);
            if (checkUserWithSameEmail != null) return BadRequest("E-Mail wurde bereits registriert");
            var newUser = await _userRepository.InsertUserAsync(userDto);
            if (newUser == null) return BadRequest("User konnte nicht hinzugefügt werden");
            return Ok(newUser);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPut("{userId}")]
    public async Task<ActionResult<UserDto>> UpdateUser(int userId, UserDto userDto)
    {
        try
        {
            if (userId <= 0) return BadRequest("Keine UserId!");
            var updateUser = await _userRepository.UpdateUserAsync(userId, userDto);
            if (updateUser == null) return BadRequest("User konnte nicht aktualisiert werden");
            return Ok(userDto);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("[action]")]
    public async Task<ActionResult<bool>> ChangeUserPassword(ChangePassword changePassword)
    {
        try
        {
            if (changePassword.UserId <= 0) return BadRequest("Fehler UserId");
            var checkChangePassword = await _userRepository.ChangeUserPasswordAsync(changePassword);
            if (!checkChangePassword) return BadRequest("Passwort konnte nicht geändert werden");
            return Ok(true);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpDelete("{userId}")]
    public async Task<ActionResult<bool>> DeleteUser(int userId)
    {
        try
        {
            if (userId <= 0) return BadRequest("Keine UserId");
            var checkDelete = await _userRepository.DeleteUserAsync(userId);
            if (!checkDelete) return BadRequest("User konnte nicht gelöscht werden");
            return Ok(true);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}