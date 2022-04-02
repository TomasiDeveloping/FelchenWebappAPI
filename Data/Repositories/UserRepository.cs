using Api.Dtos;
using Api.Entities;
using Api.Helper;
using Api.Interfaces;
using Api.Services;
using Microsoft.EntityFrameworkCore;

namespace Api.Data.Repositories;

public class UserRepository : IUserRepository
{
    private readonly FelchenContext _context;
    private readonly ILoggerService _loggerService;

    public UserRepository(FelchenContext context, ILoggerService loggerService)
    {
        _context = context;
        _loggerService = loggerService;
    }

    public async Task<List<UserDto>> GetUsersAsync()
    {
        return await _context.Users
            .Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                CreatedAt = u.CreatedAt,
                FirstName = u.FirstName,
                LastName = u.LastName,
                IsActive = u.IsActive
            })
            .AsNoTracking()
            .ToListAsync();
    }


    public async Task<UserDto> GetUserByIdAsync(int userId)
    {
        return await _context.Users
            .Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                CreatedAt = u.CreatedAt,
                FirstName = u.FirstName,
                LastName = u.LastName,
                IsActive = u.IsActive
            })
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId);
    }

    public async Task<UserDto> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .Select(u => new UserDto
            {
                Id = u.Id,
                Email = u.Email,
                CreatedAt = u.CreatedAt,
                FirstName = u.FirstName,
                LastName = u.LastName,
                IsActive = u.IsActive
            })
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email.Equals(email));
    }

    public async Task<UserDto> InsertUserAsync(UserDto userDto)
    {
        var passwordHashAndSalt = PasswordService.CreatePassword(userDto.Password);

        var newUser = new User
        {
            Email = userDto.Email,
            CreatedAt = DateTime.Now,
            FirstName = userDto.FirstName,
            LastName = userDto.LastName,
            IsActive = userDto.IsActive,
            Password = passwordHashAndSalt.PasswordHash,
            Salt = passwordHashAndSalt.PasswordSalt
        };


        await _context.Users.AddAsync(newUser);
        var checkInsert = await Complete();
        if (!checkInsert) return null;
        var log = new Log
        {
            Message = $"Neuer User Registriert {userDto.FirstName} {userDto.LastName}",
            CratedAt = DateTime.Now,
            LogTypeId = Convert.ToInt32(Constantes.LogTypeNames.Insert)
        };
        await _loggerService.InsertLogAsync(log);
        userDto.Id = newUser.Id;
        return userDto;
    }

    public async Task<UserDto> UpdateUserAsync(int userId, UserDto userDto)
    {
        var userToUpdate = await _context.Users.FindAsync(userId);
        if (userToUpdate == null) return null;
        if (userDto.Email != userToUpdate.Email)
        {
            var checkUserWithSameEmail =
                await _context.Users.FirstOrDefaultAsync(u => u.Email.Equals(userDto.Email));
            if (checkUserWithSameEmail != null)
                if (checkUserWithSameEmail.Id != userToUpdate.Id)
                    throw new Exception("E-Mail bereits verwendet");
            userToUpdate.Email = userDto.Email;
        }

        userToUpdate.FirstName = userDto.FirstName;
        userToUpdate.LastName = userDto.LastName;
        userToUpdate.IsActive = userDto.IsActive;

        var checkUpdate = await Complete();
        return checkUpdate ? userDto : null;
    }

    public async Task<bool> ChangeUserPasswordAsync(ChangePassword changePassword)
    {
        var userToUpdate = await _context.Users.FindAsync(changePassword.UserId);
        if (userToUpdate == null) return false;

        var passwordAndSalt = PasswordService.CreatePassword(changePassword.Password);
        userToUpdate.Password = passwordAndSalt.PasswordHash;
        userToUpdate.Salt = passwordAndSalt.PasswordSalt;
        return await Complete();
    }

    public async Task<bool> DeleteUserAsync(int userId)
    {
        var userToDelete = await _context.Users.FindAsync(userId);
        if (userToDelete == null) return false;
        var userCatches = await _context.FischCatches.Where(c => c.UserId == userToDelete.Id).ToListAsync();
        if (userCatches.Count > 0) _context.RemoveRange(userCatches);
        _context.Users.Remove(userToDelete);
        var checkDelete = await Complete();
        if (!checkDelete) return false;
        var log = new Log
        {
            Message =
                $"Benutzer: {userToDelete.Id} ({userToDelete.FirstName} {userToDelete.LastName}) wurde gelöscht",
            CratedAt = DateTime.Now,
            LogTypeId = Convert.ToInt32(Constantes.LogTypeNames.Delete)
        };
        await _loggerService.InsertLogAsync(log);
        return true;
    }

    public async Task<bool> Complete()
    {
        try
        {
            await _context.SaveChangesAsync();
            return true;
        }
        catch (Exception ex)
        {
            var log = new Log
            {
                CratedAt = DateTime.Now,
                ExceptionMessage = ex.Message,
                InnerException = ex.InnerException?.ToString(),
                Message = "Error in UserRepository",
                LogTypeId = Convert.ToInt32(Constantes.LogTypeNames.Error)
            };
            var error = await _loggerService.InsertLogAsync(log);
            throw new Exception($"Fehler ! Bitte kontaktire den Support! ErrorLog Id: {error.Id}");
        }
    }
}