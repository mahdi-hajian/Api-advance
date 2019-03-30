using Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Common.Utilities;

namespace MyApi.Models
{
    /// <summary>
    /// model dto for user
    /// </summary>
    public class UserDto: IValidatableObject // => User
    {
        /// <summary>
        /// userName
        /// </summary>
        [Required(ErrorMessage = "{0} اجباری میباشد")]
        [Display(Name = "نام کاربری")]
        [MaxLength(100)]
        public string UserName { get; set; }

        /// <summary>
        /// password
        /// </summary>
        [Required(ErrorMessage = "{0} اجباری میباشد")]
        [Display(Name = "رمز عبور")]
        [MaxLength(500)]
        public string Password { get; set; }

        /// <summary>
        /// firstName + LastName
        /// </summary>
        [Required(ErrorMessage = "{0} اجباری میباشد")]
        [MaxLength(100)]
        [Display(Name = "نام کامل")]
        public string FullName { get; set; }

        /// <summary>
        /// email address
        /// </summary>
        [Required(ErrorMessage = "{0} اجباری میباشد")]
        [Display(Name = "ایمیل")]
        [EmailAddress(ErrorMessage = "{0} را درست وارد کنید")]
        public string Email { get; set; }

        /// <summary>
        /// age
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// gender
        /// </summary>
        public GenderType Gender { get; set; }

        /// <summary>
        /// costom validation for get userDto model
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (UserName.Equals("test", StringComparison.OrdinalIgnoreCase))
                yield return new ValidationResult("نام کاربری نمیتواند test باشد", new[] { nameof(UserName) });
            if (Password.Equals("123456"))
                yield return new ValidationResult("رمز عبور نمیتواند 123456 باشد", new[] { nameof(Password) });
            if (Gender == GenderType.Male && Age > 30)
                yield return new ValidationResult("آقایان بالای سی سال معتبر نیستند", new[] { nameof(Gender), nameof(Age) });
        }
    }
}
