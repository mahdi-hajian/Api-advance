using System.Collections.Generic;
using System.Threading.Tasks;
using Entities;

namespace Services.Interfaces
{
    public interface IJWTService
    {
        Task<string> GenerateAsync(User user);
    }
}