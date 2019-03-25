using System.ComponentModel.DataAnnotations;

namespace WebFramework.Api
{
    public enum ApiResultStatusCode
    {
        [Display(Name = "عملیات با موفقیت انجام شد")]
        Success = 1,
        [Display(Name = "خطایی در سرور رخ داده است")]
        serverError,
        [Display(Name = "پارامتر های ارسالی معتبر نیستند")]
        BadRequest,
        [Display(Name = "یافت نشد")]
        NotFound,
        [Display(Name = "لیست خالی است")]
        ListEmpty,
        [Display(Name = "خطایی در پردازش رخ داد")]
        LogicError,
        [Display(Name = "خطای احراز هویت")]
        UnAuthorized
    }
}
