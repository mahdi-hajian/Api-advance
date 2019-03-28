using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Services.Models.Dtos
{
    public class PostDto // => Post
    {
        public Guid Id { get; set; }
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
    }
}
