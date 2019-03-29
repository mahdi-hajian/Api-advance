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
    /// <summary>
    /// version 2 posts method
    /// </summary>
    [ApiVersion(version:"2")]
    public class PostsController: v1.PostsController
    {
        private readonly IRepository<Post> _repository;

        /// <summary>
        /// constractor
        /// </summary>
        /// <param name="repository"></param>
        public PostsController(IRepository<Post> repository) : base(repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// ساخت پست
        /// </summary>
        /// <param name="dto"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
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

        public override Task<ActionResult<List<PostDto>>> GetAsync(CancellationToken cancellationToken)
        {
            return base.GetAsync(cancellationToken);
        }

        /// <summary>
        /// default get method
        /// get all posts
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>all posts with parent and author</returns>
        [HttpGet]
        public async Task<ActionResult<List<PostDto>>> GetAsyncv2(CancellationToken cancellationToken)
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
