using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
    public class User: IdentityUser<int>, IEntity
    {
        public User()
        {
            IsActive = true;
        }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        public int Age { get; set; }

        public GenderType Gender { get; set; }

        public bool IsActive { get; set; }

        public DateTimeOffset? LastLoginDate { get; set; }

        public ICollection<Post> Posts { get; set; }
    }

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(c => c.UserName).HasMaxLength(100);
            builder.Property(c => c.FullName).IsRequired();
        }
    }

    public enum GenderType
    {
        [Display(Name = "مرد")]
        Male = 1,
        [Display(Name = "زن")]
        Female
    }
}
