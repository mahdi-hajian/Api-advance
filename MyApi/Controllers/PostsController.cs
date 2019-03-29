﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Contracts;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApi.Models;
using WebFramework.Api;

namespace MyApi.Controllers
{
    public class PostsController : CrudController<PostDto, PostDto, Post, Guid>
    {
        public PostsController(IRepository<Post> repository): base(repository)
        {

        }
    }
}