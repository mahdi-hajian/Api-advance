using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Services.Models;

namespace Services.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> AddAsync(UserDto userDto);
    }
}