using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Common;
using Data.Contracts;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MyApi.Model;
using MyApi.Models;
using Services.Autorizes;
using Services.Interfaces;
using Services.Models;
using Services.Models.Identity;
using WebFramework.Api;
using WebFramework.Filter;

namespace MyApi.Controllers.v1
{
    [ApiVersion(version:"1")]
    [Authorize]
    public class UserController : CustomBaseController
    {
        private readonly IUserRepository userRepository;
        private readonly ILogger<UserController> logger;
        private readonly IJWTService _jWTService;
        private readonly IUserService _userService;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SignInManager<User> _signInManager;

        public UserController(IUserRepository userRepository, ILogger<UserController> logger, IJWTService JWTService, IUserService userService,
            UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager)
        {
            this.userRepository = userRepository;
            this.logger = logger;
            _jWTService = JWTService;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;
            _userService = userService;
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        [AllowAnonymous]
        public async Task<ActionResult> Token([FromForm]TokenRequest model, CancellationToken cancellationToken)
        {
            if (!model.grant_type.Equals("password", StringComparison.OrdinalIgnoreCase))
                return BadRequest("OAuth flow is not password.");

            var user = await _userManager.FindByNameAsync(model.username);
            if (user == null)
                return Unauthorized();

            var isPasswordValid = await _userManager.CheckPasswordAsync(user, model.password);
            if (!isPasswordValid)
                return Unauthorized();

            var jwt = await _jWTService.GenerateAsync(user);
            return new JsonResult(jwt);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<User>>> Get()
        {
            var users = await userRepository.TableNoTracking.ToListAsync();
            if (users == null)
            {
                return NotFound("کاربری وجود ندارد");
            }
            return users;
        }

        [HttpGet("{id:int}")]
        [Authorize]
        public async Task<ApiResult<User>> Get(int id, [FromHeader] string Authorization, CancellationToken cancellationToken)
        {
            //به این صورت هم میشود دریافت کرد
            var cancellationToken2 = HttpContext.RequestAborted;
            // دریافت کاربر با توکن
            User user2 = await _userManager.GetUserAsync(HttpContext.User);

            var user = await userRepository.GetByIdAsync(cancellationToken, id);
            if (user == null)
                return NotFound();
            return user;
        }


        [HttpPost]
        [AllowAnonymous]
        public async Task<IdentityResult> Create(UserDto userDto)
        {
            //var user = Mapper.Map<User>(userDto);
            var user = CustomAutoMapper<User>.GetFrom(userDto);


            return await _userService.AddAsync(user, userDto.Password);
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