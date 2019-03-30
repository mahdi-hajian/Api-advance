using AutoMapper;
using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using WebFramework.Api;

namespace MyApi.Models
{
    /// <summary>
    /// post model for get and send post
    /// </summary>
    public class PostDto : BaseDto<PostDto, Post, Guid> // => Post
    {
        /// <summary>
        /// title of post
        /// </summary>
        [Required(ErrorMessage = "{0} اجباری میباشد")]
        [Display(Name = "عنوان")]
        public string Title { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [Required(ErrorMessage = "{0} اجباری میباشد")]
        [Display(Name = "توضیحات")]
        public string Description { get; set; }

        /// <summary>
        /// id of category
        /// </summary>
        [Required(ErrorMessage = "{0} اجباری میباشد")]
        [Display(Name = "دسته بندی")]
        public int CategoryId { get; set; }

        /// <summary>
        /// id of author
        /// </summary>
        [Required(ErrorMessage = "{0} اجباری میباشد")]
        [Display(Name = "کاربر")]
        public int AuthorId { get; set; }

        /// <summary>
        /// category name
        /// </summary>
        public string CategoryName { get; set; } //Category.Name
        /// <summary>
        /// author name
        /// </summary>
        public string AuthorFullName { get; set; } //Author.FullName

        /// <summary>
        /// title + category name
        /// </summary>
        public string FullTitle { get; set; } // => mapped from "Title (Category.Name)"

        //[IgnoreMap]
        //public string Category { get; set; }

        /// <summary>
        /// set custom property full title
        /// </summary>
        /// <param name="mappingExpression"></param>
        public override void CustomMappings(IMappingExpression<Post, PostDto> mappingExpression)
        {
            mappingExpression.ForMember(
                    dest => dest.FullTitle,
                    config => config.MapFrom(src => $"{src.Title} ({src.Category.Name})"));
        }
    }
}
