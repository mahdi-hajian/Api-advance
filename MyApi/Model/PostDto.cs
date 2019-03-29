using AutoMapper;
using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WebFramework.Api;

namespace MyApi.Models
{
    public class PostDto : BaseDto<PostDto, Post, Guid> // => Post
    {
        [Required(ErrorMessage = "{0} اجباری میباشد")]
        [Display(Name = "عنوان")]
        public string Title { get; set; }
        [Required(ErrorMessage = "{0} اجباری میباشد")]
        [Display(Name = "توضیحات")]
        public string Description { get; set; }
        [Required(ErrorMessage = "{0} اجباری میباشد")]
        [Display(Name = "دسته بندی")]
        public int CategoryId { get; set; }
        [Required(ErrorMessage = "{0} اجباری میباشد")]
        [Display(Name = "کاربر")]
        public int AuthorId { get; set; }

        public string CategoryName { get; set; } //Category.Name
        public string AuthorFullName { get; set; } //Author.FullName

        public string FullTitle { get; set; } // => mapped from "Title (Category.Name)"

        //[IgnoreMap]
        //public string Category { get; set; }

        public override void CustomMappings(IMappingExpression<Post, PostDto> mappingExpression)
        {
            mappingExpression.ForMember(
                    dest => dest.FullTitle,
                    config => config.MapFrom(src => $"{src.Title} ({src.Category.Name})"));
        }
    }
}
