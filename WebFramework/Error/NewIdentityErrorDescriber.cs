using Microsoft.AspNetCore.Identity;

namespace WebFramework.Configuration.Error
{
    public class NewIdentityErrorDescriber : IdentityErrorDescriber
    {
        public override IdentityError ConcurrencyFailure()
        {
            return new IdentityError()
            {
                Code = "عدم موفقیت همروندی",
                Description = "عدم موفقیت همروندی، شی تغییر کرده‌است"
            };
        }

        public override IdentityError DefaultError()
        {
            return new IdentityError()
            {
                Code = "شکست ناشناخته",
                Description = "شکست ناشناخته رخ داده است"
            };
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError()
            {
                Code = "ایمیل تکراری",
                Description = $"ایمیل {email} موجود است"
            };
        }

        public override IdentityError DuplicateRoleName(string role)
        {
            return new IdentityError()
            {
                Code = "نقش تکراری",
                Description = $"نقش {role} موجود است"
            };
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError()
            {
                Code = "نام کاربری تکراری",
                Description = $"نام کاربری {userName} قبلا انتخاب شده است"
            };
        }

        public override IdentityError InvalidEmail(string email)
        {

            return new IdentityError()
            {
                Code = "ایمیل نامعتبر",
                Description = $"ایمیل {email} نامعتبر است"
            };
        }

        public override IdentityError InvalidRoleName(string role)
        {
            return new IdentityError()
            {
                Code = "نقش نامعتبر",
                Description = $"نقش {role} نامعتبر است"
            };
        }

        public override IdentityError InvalidToken()
        {
            return new IdentityError()
            {
                Code = "توکن نامعتبر است",
                Description = "لینک فعال سازی نامعتبر است"
            };
        }

        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError()
            {
                Code = "نام کاربری نامعتبر",
                Description = $"نام کاربری {userName} نامعتبر است"
            };
        }

        public override IdentityError LoginAlreadyAssociated()
        {
            return new IdentityError()
            {
                Code = "کاربر موجود",
                Description = "یک کاربر با این لاگین موجود است "
            };
        }

        public override IdentityError PasswordMismatch()
        {
            return new IdentityError()
            {
                Code = "پسورد نادرست",
                Description = "پسورد نادرست است"
            };
        }

        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError()
            {
                Code = "پسورد بدون عدد",
                Description = "پسورد باید دارای عدد باشد"
            };
        }

        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError()
            {
                Code = "پسورد بدون حروف کوچک",
                Description = "(a-z) پسورد باید دارای حروف کوچک باشد"
            };
        }

        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError()
            {
                Code = "پسورد بدون حروف آلفا",
                Description = "(@, $, /, * , ... ) پسورد باید دارای حروف آلفا باشد"
            };
        }

        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
        {
            return new IdentityError()
            {
                Code = "پسورد بدون کاراکتر مختلف",
                Description = $"پسورد باید دارای {uniqueChars} کاراکتر مختلف باشد"
            };
        }

        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError()
            {
                Code = "پسورد بدون حروف بزرگ",
                Description = "(A-Z) پسورد باید دارای حروف بزرگ باشد"
            };
        }

        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError()
            {
                Code = "تعداد کم پسورد",
                Description = $"پسورد باید از {length} عدد بیشتر باشد"
            };
        }

        public override IdentityError RecoveryCodeRedemptionFailed()
        {
            return new IdentityError()
            {
                Code = "شکست بازیابی کد",
                Description = "بازیابی کد انجام نشد"
            };
        }

        public override IdentityError UserAlreadyHasPassword()
        {
            return new IdentityError()
            {
                Code = "کاربر پسورد دارد",
                Description = "کاربر در حال حاضر دارای یک رمز عبور است"
            };
        }

        public override IdentityError UserAlreadyInRole(string role)
        {
            return new IdentityError()
            {
                Code = "وجود کاربر در نقش",
                Description = $"کاربر در نقش {role} وجود دارد"
            };
        }

        public override IdentityError UserLockoutNotEnabled()
        {
            return new IdentityError()
            {
                Code = "فعال نشدن قفل",
                Description = "قفل برای این کاربر فعال نشده است"
            };
        }

        public override IdentityError UserNotInRole(string role)
        {
            return new IdentityError()
            {
                Code = "نبودن کاربر در نقش",
                Description = $"کاربر در نقش {role} وجود ندارد"
            };
        }
    }
}
