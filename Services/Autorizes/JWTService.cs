using Common;
using Common.Utilities;
using Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Autorizes
{
    public class JWTService : IJWTService, IScopedDependency
    {
        private readonly SiteSettings _siteSetting;
        private readonly SignInManager<User> _signInManager;

        // گرفتن یک ولیو از اپ ستینگ
        public JWTService(IOptionsSnapshot<SiteSettings> settings, SignInManager<User> signInManager)
        {
            _siteSetting = settings.Value;
            _signInManager = signInManager;
        }

        public async Task<string> GenerateAsync(User user)
        {
            var securityKey = Encoding.UTF8.GetBytes(_siteSetting.JwtSettings.SecretKey); // longer that 16 character
            var signinCredentials = new SigningCredentials(new SymmetricSecurityKey(securityKey), SecurityAlgorithms.HmacSha256Signature);

            var encryptionkey = Encoding.UTF8.GetBytes(_siteSetting.JwtSettings.Encryptkey); //must be 16 character
            var encryptingCredentials = new EncryptingCredentials(new SymmetricSecurityKey(encryptionkey), SecurityAlgorithms.Aes128KW, SecurityAlgorithms.Aes128CbcHmacSha256);

            var Descriptor = new SecurityTokenDescriptor() {
                // سازنده
                Issuer = _siteSetting.JwtSettings.Issuer,
                // مصرف کننده
                Audience = _siteSetting.JwtSettings.Audience,
                // این توکن چ زمانی صادر شده است
                IssuedAt = DateTime.Now,
                // از چه زمانی قابل استفاده است
                NotBefore = DateTime.Now.AddMinutes(_siteSetting.JwtSettings.NotBeforeMinutes),
                // تاریخ انقضای توکن
                Expires = DateTime.Now.AddHours(_siteSetting.JwtSettings.ExpirationMinutes),
                // سیگنیچر
                SigningCredentials = signinCredentials,
                // رمزنگاری توکن
                EncryptingCredentials = encryptingCredentials,
                // متغیر های درون توکن
                Subject = new  ClaimsIdentity(await _getClaimsAsync(user)),
            };

            // غیرفعال کردن تغییر نام فیلد های توکن
            //JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            //JwtSecurityTokenHandler.DefaultMapInboundClaims = false;
            //JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            var tokenHandler = new JwtSecurityTokenHandler();
            var securityToken = tokenHandler.CreateToken(Descriptor);
            var jwt = tokenHandler.WriteToken(securityToken);
            return jwt;
        }

        private async Task<IEnumerable<Claim>> _getClaimsAsync(User user)
        {
            var result = await _signInManager.ClaimsFactory.CreateAsync(user);
            //add custom claims
            List<Claim> list = new List<Claim>(result.Claims);
            return list;

            //var securityStampClaimType = new ClaimsIdentityOptions().SecurityStampClaimType;

            //var list = new List<Claim>
            //{
            //    new Claim(ClaimTypes.Name, user.UserName),
            //    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            //    new Claim(ClaimTypes.MobilePhone, "09123456987"),
            //    new Claim(securityStampClaimType, user.SecurityStamp.ToString())
            //};

            //return list;
        }
    }
}
