using System.Threading.Tasks;
using Entities;
using Microsoft.AspNetCore.Identity;
using Services.Models;

namespace Services.Interfaces
{
    public interface IUserService
    {
        Task<IdentityResult> AddAsync(User model, string password);
    }
}