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

namespace Services.Autorizes
{
    public class JWTService : IJWTService
    {
        private readonly SiteSettings _siteSetting;

        // گرفتن یک ولیو از اپ ستینگ
        public JWTService(IOptionsSnapshot<SiteSettings> settings)
        {
            _siteSetting = settings.Value;
        }

        public string Generate(User user, List<string> userRoles)
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
                Subject = new  ClaimsIdentity(_getClaims(user, userRoles)),
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

        private IEnumerable<Claim> _getClaims(User user, List<string> userRoles)
        {
            // هرچیزی میتواند باشد ولی اصولش این است
            var securityStampClaimType = new ClaimsIdentityOptions().SecurityStampClaimType;

            var list = new List<Claim> {
                // .net claim
                new Claim(ClaimTypes.Name , user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                // .jwt claim
                new Claim(JwtRegisteredClaimNames.Gender, user.Gender.ToDisplay()),

                new Claim(securityStampClaimType, user.SecurityStamp.ToString())
            };
            foreach (var item in userRoles)
            {
                list.Add(new Claim(ClaimTypes.Role, item));
            }

            return list;
        }
    }
}
