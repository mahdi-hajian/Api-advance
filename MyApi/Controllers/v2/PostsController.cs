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

        public override Task<ApiResult<PostDto>> Create(PostDto dto, CancellationToken cancellationToken)
        {
            return base.Create(dto, cancellationToken);
        }

        public override Task<ApiResult> Delete(Guid id, CancellationToken cancellationToken)
        {
            return base.Delete(id, cancellationToken);
        }

        public override Task<ApiResult<PostDto>> Get(Guid id, CancellationToken cancellationToken)
        {
            return base.Get(id, cancellationToken);
        }

        [HttpGet]
        public async Task<ActionResult<List<PostDto>>> Get_v2(CancellationToken cancellationToken)
        {
            var list = await _repository.TableNoTracking.ProjectTo<PostDto>()
                .Cacheable().ToListAsync(cancellationToken);

            return Ok(list);
        }

        public override Task<ApiResult<PostDto>> Update(Guid id, PostDto dto, CancellationToken cancellationToken)
        {
            return base.Update(id, dto, cancellationToken);
        }
    }
}
