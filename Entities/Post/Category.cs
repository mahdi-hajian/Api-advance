using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities
{
    public class Category: BaseEntity
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        public int? ParentCateforyId { get; set; }

        [ForeignKey(nameof(ParentCateforyId))]
        public Category ParentCategory { get; set; }

        public ICollection<Category> ChildCategory { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
