using Data.Contracts;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MyApi.Model;
using MyApi.Models;
using Services.Interfaces;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebFramework.Api;

namespace MyApi.Controllers.v2
{
    [ApiVersion("2")]
    public class UserController : v1.UserController
    {
        private readonly IUserRepository _repository;

        public UserController(IUserRepository repository, ILogger<UserController> logger, IJWTService JWTService, IUserService userService,
            UserManager<User> userManager, RoleManager<Role> roleManager, SignInManager<User> signInManager)
            : base(repository, logger, JWTService,userService,userManager,roleManager,signInManager)
        {
            _repository = repository;
        }

        [HttpPost("SetProfile")]
        public IFormFile Set_Profile(IFormFile file1)
        {
            //IFormFileCollection file = Request.Form.Files;
            return file1;
        }

        [AddSwaggerFileUploadButton]
        [HttpPost("SetAvatar")]
        public IFormFile Set_Avatar()
        {
            IFormFileCollection files = Request.Form.Files;
            return files[0];
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="model"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public override Task<ActionResult> Token([FromForm] TokenRequest model, CancellationToken cancellationToken)
        {
            return base.Token(model, cancellationToken);
        }

        public override Task<ActionResult<List<User>>> Get()
        {
            return base.Get();
        }

        public override Task<ApiResult<User>> Get(int id, [FromHeader] string Authorization, CancellationToken cancellationToken)
        {
            return base.Get(id, Authorization, cancellationToken);
        }

        public override Task<IdentityResult> Create(UserDto userDto)
        {
            return base.Create(userDto);
        }

        public override Task<ApiResult<User>> Update(int id, User user, CancellationToken cancellationToken)
        {
            return base.Update(id, user, cancellationToken);
        }

        public override Task<ApiResult> Delete(int id, CancellationToken cancellationToken)
        {
            return base.Delete(id, cancellationToken);
        }
    }
}
