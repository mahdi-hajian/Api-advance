using System;
using System.Collections.Generic;
using System.Linq;
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
    [Authorize]
    public class CategoriesController: CrudController<CategoryDto, CategoryDto, Category, int>
    {
        public CategoriesController(IRepository<Category> repository): base(repository)
        {

        }
    }
}
