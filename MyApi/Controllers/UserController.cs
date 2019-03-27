using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Exceptions;
using Data;
using Data.Contracts;
using Data.Repositories;
using ElmahCore;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.JsonWebTokens;
using MyApi.Models;
using Services.Autorizes;
using Services.Interfaces;
using Services.Models;
using WebFramework.Api;
using WebFramework.Filter;

namespace MyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiResultFilter]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly ILogger<UserController> logger;
        private readonly IJWTService _jWTService;


        public UserController(IUserRepository userRepository, ILogger<UserController> logger, IJWTService JWTService)
        {
            this.userRepository = userRepository;
            this.logger = logger;
            _jWTService = JWTService;
        }

        [HttpGet("[action]")]
        public async Task<ApiResult<string>> Token(LoginModel model, CancellationToken cancellationToken)
        {
            var user = await userRepository.GetByUserAndPass(model.UserName, model.password, cancellationToken);
            if (user == null)
                return Unauthorized();
            var jwt = _jWTService.Generate(user, new List<string> { "Admin", "Manager" });
            return jwt;
        }

        [HttpGet]
        [Authorize]
        public async Task<List<User>> Get()
        {
            var users = await userRepository.TableNoTracking.ToListAsync();
            return users;
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<ApiResult<User>> Get(int id, [FromHeader] string Authorization, CancellationToken cancellationToken)
        {
            //به این صورت هم میشود دریافت کرد
            //var cancellationToken = HttpContext.RequestAborted;

            var user = await userRepository.GetByIdAsync(cancellationToken, id);
            if (user == null)
                return NotFound();
            return user;
        }


        [HttpPost]
        public async Task<ApiResult<User>> Create(UserDto userDto, CancellationToken cancellationToken)
        {
            var user = new User
            {
                UserName = userDto.UserName,
                FullName = userDto.FullName,
                Age = userDto.Age,
                Gender = userDto.Gender
            };
            await userRepository.AddAsync(user, userDto.Password, cancellationToken);
            logger.LogError("کاربر ساخته شد");
            return Ok(user);
        }

        [HttpPut]
        public async Task<ApiResult<User>> Update(int id, User user, CancellationToken cancellationToken)
        {
            var updateUser = await userRepository.GetByIdAsync(cancellationToken, id);

            updateUser.UserName = user.UserName;
            updateUser.PasswordHash = user.PasswordHash;
            updateUser.FullName = user.FullName;
            updateUser.Age = user.Age;
            updateUser.Gender = user.Gender;
            updateUser.IsActive = user.IsActive;
            updateUser.LastLoginDate = user.LastLoginDate;
            await userRepository.UpdateAsync(updateUser, cancellationToken);

            return Ok(user);
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