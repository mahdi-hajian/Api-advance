using System.Collections.Generic;
using Entities;

namespace Services.Interfaces
{
    public interface IJWTService
    {
        string Generate(User user, List<string> userRoles);
    }
}