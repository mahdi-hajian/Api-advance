using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data;
using Data.Contracts;
using Data.Repositories;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebFramework.Api;
using WebFramework.Filter;

namespace MyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;

        public UserController(IUserRepository userRepository)
        {
            this.userRepository = userRepository;
        }

        [HttpGet]
        [ApiResultFilter]
        public async Task<List<User>> Get()
        {
            var users = await userRepository.TableNoTracking.ToListAsync();
            return users;
        }

        [HttpGet("{id:int}")]
        public async Task<ApiResult<User>> Get(int id, CancellationToken cancellationToken)
        {
            // به این صورت هم میشود دریافت کرد
            //var cancellationToken = HttpContext.RequestAborted;

            var user = await userRepository.GetByIdAsync(cancellationToken, id);
            if (user == null)
                return NotFound();
            return user;
        }

        [HttpPost]
        [ApiResultFilter]
        public async Task<ApiResult<User>> Create(User user)
        {
            await userRepository.AddAsync(user, CancellationToken.None);
            return Ok(user);
        }

        [HttpPut]
        public async Task<ApiResult> Update(int id, User user, CancellationToken cancellationToken)
        {
            await userRepository.UpdateAsync(user, cancellationToken);
            //var updateUser = await userRepository.GetByIdAsync(cancellationToken, id);

            //updateUser.UserName = user.UserName;
            //updateUser.PasswordHash = user.PasswordHash;
            //updateUser.FullName = user.FullName;
            //updateUser.Age = user.Age;
            //updateUser.Gender = user.Gender;
            //updateUser.IsActive = user.IsActive;
            //updateUser.LastLoginDate = user.LastLoginDate;
            //await userRepository.UpdateAsync(updateUser, cancellationToken);

            return Ok();
        }

        [HttpDelete]
        public async Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByIdAsync(cancellationToken, id);
            await userRepository.DeleteAsync(user, cancellationToken);

            return Ok();
        }
    }
}