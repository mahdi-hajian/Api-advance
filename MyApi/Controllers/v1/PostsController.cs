using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Data.Contracts;
using Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApi.Models;
using WebFramework.Api;

namespace MyApi.Controllers.v1
{
    [ApiVersion(version: "1", Deprecated = true)]
    [Authorize]
    public class PostsController : CrudController<PostDto, PostDto, Post, Guid>
    {
        private readonly IRepository<Post> _repository;

        public PostsController(IRepository<Post> repository): base(repository)
        {
            _repository = repository;
        }

        [NonAction]
        public override Task<ActionResult<List<PostDto>>> GetAsync(CancellationToken cancellationToken)
        {
            return base.GetAsync(cancellationToken);
        }
    }
}