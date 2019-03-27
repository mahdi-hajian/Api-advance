using Microsoft.IdentityModel.Tokens;
using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Principal;
using System.Text;

namespace Common.Utilities
{
    public static class DateTimeConvert
    {
        public static string GetPersianDate()
        {
            try
            {
                PersianCalendar PersianDate = new PersianCalendar();
                string NowaDate = PersianDate.GetYear(DateTime.Now).ToString() + "/" + PersianDate.GetMonth(DateTime.Now).ToString() + "/" + PersianDate.GetDayOfMonth(DateTime.Now).ToString();
                NowaDate += ' ' + DateTime.Now.ToShortTimeString().Replace("PM", "ب.ظ").Replace("AM", "ق.ظ");
                return NowaDate;
            }
            catch (Exception)
            {
                return "";
            }

        }
        public static DateTime ConvertToMiladi(string ShamsiDate)
        {
            try
            {
                ShamsiDate = ShamsiDate.Replace('۰', '0');
                ShamsiDate = ShamsiDate.Replace('۱', '1');
                ShamsiDate = ShamsiDate.Replace('۲', '2');
                ShamsiDate = ShamsiDate.Replace('۳', '3');
                ShamsiDate = ShamsiDate.Replace('۴', '4');
                ShamsiDate = ShamsiDate.Replace('۵', '5');
                ShamsiDate = ShamsiDate.Replace('۶', '6');
                ShamsiDate = ShamsiDate.Replace('۷', '7');
                ShamsiDate = ShamsiDate.Replace('۸', '8');
                ShamsiDate = ShamsiDate.Replace('۹', '9');
                int year = int.Parse(ShamsiDate.Split('/')[0]);
                int month = int.Parse(ShamsiDate.Split('/')[1]);
                int day = int.Parse(ShamsiDate.Split('/')[2]);
                PersianCalendar p = new PersianCalendar();
                DateTime date = p.ToDateTime(year, month, day, 0, 0, 0, 0);
                return date;
            }
            catch (Exception)
            {
                return new DateTime(0, 0, 0);
            }

        }
        public static DateTime ConvertToMiladiFullDay(string ShamsiDate)
        {
            try
            {
                ShamsiDate = ShamsiDate.Replace('۰', '0');
                ShamsiDate = ShamsiDate.Replace('۱', '1');
                ShamsiDate = ShamsiDate.Replace('۲', '2');
                ShamsiDate = ShamsiDate.Replace('۳', '3');
                ShamsiDate = ShamsiDate.Replace('۴', '4');
                ShamsiDate = ShamsiDate.Replace('۵', '5');
                ShamsiDate = ShamsiDate.Replace('۶', '6');
                ShamsiDate = ShamsiDate.Replace('۷', '7');
                ShamsiDate = ShamsiDate.Replace('۸', '8');
                ShamsiDate = ShamsiDate.Replace('۹', '9');
                int year = int.Parse(ShamsiDate.Split('/')[0]);
                int month = int.Parse(ShamsiDate.Split('/')[1]);
                int day = int.Parse(ShamsiDate.Split('/')[2]);
                PersianCalendar p = new PersianCalendar();
                DateTime date = p.ToDateTime(year, month, day, 11, 59, 59, 0);
                return date;
            }
            catch (Exception)
            {
                return new DateTime(0, 0, 0);
            }

        }
        public static string ConvertToSafeDate(string ShamsiDate)
        {
            try
            {
                ShamsiDate = ShamsiDate.Replace('۰', '0');
                ShamsiDate = ShamsiDate.Replace('۱', '1');
                ShamsiDate = ShamsiDate.Replace('۲', '2');
                ShamsiDate = ShamsiDate.Replace('۳', '3');
                ShamsiDate = ShamsiDate.Replace('۴', '4');
                ShamsiDate = ShamsiDate.Replace('۵', '5');
                ShamsiDate = ShamsiDate.Replace('۶', '6');
                ShamsiDate = ShamsiDate.Replace('۷', '7');
                ShamsiDate = ShamsiDate.Replace('۸', '8');
                ShamsiDate = ShamsiDate.Replace('۹', '9');
                return ShamsiDate;
            }
            catch (Exception)
            {
                return null;
            }

        }
        public static string ConvertToShamsi(DateTime MidaliDate)
        {
            try
            {
                PersianCalendar shamsi = new PersianCalendar();
                string ysh = shamsi.GetYear(MidaliDate).ToString();
                string msh = shamsi.GetMonth(MidaliDate).ToString();
                string dsh = shamsi.GetDayOfMonth(MidaliDate).ToString();
                return ysh + "/" + (msh.ToString().Length == 1 ? "0" + msh.ToString() : msh.ToString()) + "/" + (dsh.ToString().Length == 1 ? "0" + dsh.ToString() : dsh.ToString());
            }
            catch (Exception)
            {
            }
            return MidaliDate.ToShortDateString();
        }
        public static string SafeFarsiStr(string input)
        {
            return input.Replace("ی", "ي").Replace("ک", "ک");
        }
        #region ValidateToken
        public static bool ValidateToken(string authToken, string key)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = GetValidationParameters(key);

                IPrincipal principal = tokenHandler.ValidateToken(authToken, validationParameters, out SecurityToken validatedToken);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private static TokenValidationParameters GetValidationParameters(string key)
        {
            return new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = "MyApi",
                ValidIssuer = "MyApi",
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
            };
        }
        #endregion
    }
}
