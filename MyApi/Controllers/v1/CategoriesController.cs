using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Data.Contracts;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyApi.Models;
using WebFramework.Api;

namespace MyApi.Controllers.v1
{
    /// <summary>
    /// کنترلر دسته بندی ها
    /// </summary>
    public class CategoriesController: CrudController<CategoryDto, CategoryDto, Category, int>
    {
        /// <summary>
        /// سازنده کلاس
        /// </summary>
        /// <param name="repository"></param>
        public CategoriesController(IRepository<Category> repository): base(repository)
        {

        }
    }
}
