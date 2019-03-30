using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyApi.Model
{
    public class TokenRequest
    {
        [Required(ErrorMessage = "{0} اجباری میباشد")]
        [Display(Name = "نوع رمز عبور")]
        public string grant_type { get; set; }
        [Required(ErrorMessage = "{0} اجباری میباشد")]
        [Display(Name = "نام کاربری")]
        public string username { get; set; }
        [Required(ErrorMessage = "{0} اجباری میباشد")]
        [Display(Name = "رمز عبور")]
        public string password { get; set; }
        public string refresh_token { get; set; }
        public string scope { get; set; }

        public string client_id { get; set; }
        public string client_secret { get; set; }
    }
}
