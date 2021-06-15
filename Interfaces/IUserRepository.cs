using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Dtos;
using Api.Helper;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace Api.Interfaces
{
    public interface IUserRepository
    {
        public Task<List<UserDto>> GetUsersAsync();
        public Task<UserDto> GetUserByIdAsync(int userId);
        public Task<UserDto> GetUserByEmailAsync(string email);
        public Task<UserDto> InsertUserAsync(UserDto userDto);
        public Task<UserDto> UpdateUserAsync(int userId, UserDto userDto);
        public Task<bool> ChangeUserPasswordAsync(ChangePassword changePassword);
        public Task<bool> DeleteUserAsync(int userId);
        public Task<bool> Complete();
    }
}