using AutoMapper.QueryableExtensions;
using Data.Contracts;
using EFSecondLevelCache.Core;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WebFramework.Api;

namespace MyApi.Controllers.v2
{
    [ApiVersion(version:"2")]
    public class PostsController: v1.PostsController
    {
        private readonly IRepository<Post> _repository;

        public PostsController(IRepository<Post> repository) : base(repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<List<PostDto>>> GetAsyncv2(CancellationToken cancellationToken)
        {
            var list = await _repository.TableNoTracking.ProjectTo<PostDto>()
                .Cacheable().ToListAsync(cancellationToken);

            return Ok(list);
        }
    }
}
