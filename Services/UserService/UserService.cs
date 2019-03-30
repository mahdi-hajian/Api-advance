using Common;
using Entities;
using Microsoft.AspNetCore.Identity;
using Services.Interfaces;
using Services.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Services.UserService
{
    public class UserService : IUserService, IScopedDependency
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public UserService(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IdentityResult> AddAsync(User model, string password)
        {
            var result = await _userManager.CreateAsync(model, password);
            return result;
        }
    }
}
