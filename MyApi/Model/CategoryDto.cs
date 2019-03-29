using Entities;
using WebFramework.Api;

namespace MyApi.Models
{
    /// <summary>
    /// categoru dto for send and get model
    /// </summary>
    public class CategoryDto : BaseDto<CategoryDto, Category, int> // => Category
    {
        /// <summary>
        /// name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// parent Id
        /// </summary>
        public int? ParentCategoryId { get; set; }

        /// <summary>
        /// name of parent
        /// </summary>
        public string ParentCategoryName { get; set; } //=> mapped from ParentCategory.Name
    }
}
