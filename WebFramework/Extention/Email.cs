using System;
using System.IO;
using System.Net.Mail;
using System.Text;

namespace WebFramework.Configuration.Extention
{
    public static class Email
    {
        #region Email Sender
        private static readonly string EmailSender = "AgentMahdihajian@gmail.com";
        private static readonly string PasswordEmailSender = "85X5Q28w9b7KmsH4";
        #endregion

        #region Send Email
        public async static void SendEmail(string EmailTo, string body, string subject)
        {
            try
            {
                //body = body.Replace("http://localhost:4200", "http://hamyarAmlak.tabandesign.ir/");
                MailMessage mail = new MailMessage();
                SmtpClient SmtpServer = new SmtpClient("smtp.gmail.com");

                mail.From = new MailAddress(EmailSender);
                mail.To.Add(EmailTo);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                mail.BodyEncoding = Encoding.UTF8;
                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential(EmailSender, PasswordEmailSender);
                SmtpServer.EnableSsl = false;

                await SmtpServer.SendMailAsync(mail);
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region SendEmailAfterRegistration


        /// <summary>
        ///متد ارسال ایمیل بعد از ثبت نام
        /// </summary>
        /// <param name="email"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="code"></param>
        /// <param name="firstName"></param>
        /// <param name="userId"></param>
        public static void SendEmailAfterRegistrationExternal(string email, string userName, string password, string code, string firstName, string userId)
        {
            try
            {
                string strRootRelativePathName =
                "App_Data/LocalizedEmailTemplates/UserEmailVerification.htm";

                //ایجاد یک رشته و مراحل تبدیل مراحل نسبی به فیزیکی
                string strPathName =
                    Path.GetFullPath(strRootRelativePathName);

                //استفاده از متد رید کلاس فایل برای خواندن مسیر فیزیکی
                string strEmailBody = File.ReadAllText(strPathName);
                //جایگزینی مقادیر موجود در فایل خوانده شده با مقادیر داده شده
                strEmailBody = strEmailBody
                                .Replace("[USER_NAME]", userName)
                                .Replace("[Code]", code)
                                .Replace("[PASSWORD]", password)
                                .Replace("[UserId]", userId)
                                .Replace("[FIRST_NAME]", firstName);

                SendEmail(email, strEmailBody, "تایید ایمیل");
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region SendComfirmEmailAgain
        /// متد ارسال کلید تائید به کاربراگر هنگام ثبت نام عمل تائیدیه انجام نشد و اقدام به لاگین نمود

        /// <summary>
        /// if user want to send confirm email again
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userName"></param>
        /// <param name="code"></param>
        /// <param name="firstName"></param>
        public static void SendComfirmEmailWithOutPass(string email, string userName, string code, string firstName, string userId)
        {
            try
            {
                string strRootRelativePathName =
                "App_Data/LocalizedEmailTemplates/UserEmailVerificationOutPass.htm";

                //ایجاد یک رشته و مراحل تبدیل مراحل نسبی به فیزیکی
                string strPathName =
                    Path.GetFullPath(strRootRelativePathName);

                //استفاده از متد رید کلاس فایل برای خواندن مسیر فیزیکی
                string strEmailBody = File.ReadAllText(strPathName);
                //جایگزینی مقادیر موجود در فایل خوانده شده با مقادیر داده شده
                strEmailBody = strEmailBody
                                .Replace("[USER_NAME]", userName)
                                .Replace("[Code]", code)
                                .Replace("[UserId]", userId)
                                .Replace("[FIRST_NAME]", firstName);

                SendEmail(email, strEmailBody, "تایید ایمیل");
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region SendEmailForgotPassword
        //متد ارسال ایمیل برای فراموشی رمز عبور
        /// <summary>
        /// send email for forget password
        /// </summary>
        /// <param name="firstName"></param>
        /// <param name="userName"></param>
        /// <param name="code"></param>
        public static void SendEmailForgotPassword(string email, string firstName, string userName, string code, string userId)
        {
            //استفاده از قالب موجود در ای پی پی دیتا
            string strRootRelativePathName =
                "App_Data/LocalizedEmailTemplates/ForgotPasswordUserEmail.htm";

            //ایجاد یک رشته و مراحل تبدیل مراحل نسبی به فیزیکی
            string strPathName =
                Path.GetFullPath(strRootRelativePathName);

            //استفاده از متد رید کلاس فایل برای خواندن مسیر فیزیکی
            string strEmailBody = File.ReadAllText(strPathName);

            //جایگزینی مقادیر موجود در فایل خوانده شده با مقادیر داده شده
            strEmailBody = strEmailBody
                            .Replace("[FIRST_NAME]", firstName)
                            .Replace("[USER_NAME]", userName)
                            .Replace("[USER_ID]", userId)
                            .Replace("[CODE]", code);

            SendEmail(email, strEmailBody, "بازیابی گذرواژه");
        }
        #endregion

        #region Change Email

        public static void ChangeEmail(string email, string userName, string code, string userId)
        {
            //استفاده از قالب موجود در ای پی پی دیتا
            string strRootRelativePathName =
                "App_Data/LocalizedEmailTemplates/ChangeEmail.htm";

            //ایجاد یک رشته و مراحل تبدیل مراحل نسبی به فیزیکی
            string strPathName =
                Path.GetFullPath(strRootRelativePathName);

            //استفاده از متد رید کلاس فایل برای خواندن مسیر فیزیکی
            string strEmailBody = File.ReadAllText(strPathName);

            //جایگزینی مقادیر موجود در فایل خوانده شده با مقادیر داده شده
            strEmailBody = strEmailBody
                            .Replace("[EMAIL]", email)
                            .Replace("[USER_NAME]", userName)
                            .Replace("[USERID]", userId)
                            .Replace("[CODE]", code);

            SendEmail(email, strEmailBody, "تغییر ایمیل");
        }
        #endregion

        #region ChangePassword
        public static void ChangePassword(string email, string userName, string firstName)
        {
            //استفاده از قالب موجود در ای پی پی دیتا
            string strRootRelativePathName =
                "App_Data/LocalizedEmailTemplates/ChangePassword.htm";

            //ایجاد یک رشته و مراحل تبدیل مراحل نسبی به فیزیکی
            string strPathName =
                Path.GetFullPath(strRootRelativePathName);

            //استفاده از متد رید کلاس فایل برای خواندن مسیر فیزیکی
            string strEmailBody = File.ReadAllText(strPathName);

            //جایگزینی مقادیر موجود در فایل خوانده شده با مقادیر داده شده
            strEmailBody = strEmailBody
                            .Replace("[FIRST_NAME]", firstName)
                            .Replace("[USER_NAME]", userName);

            SendEmail(email, strEmailBody, "تغییر پسورد");
        }
        #endregion

    }
}
