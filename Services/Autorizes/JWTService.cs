using Common;
using Common.Utilities;
using Entities;
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
            // 16 character or more
            var securityKey = Encoding.UTF8.GetBytes("kjashdf3;uewfhiefuef8dsfjdsfka'sdfadkfhadfakjhdfa;;'d;faasds");
            var signinCredentials = new SigningCredentials(new SymmetricSecurityKey(securityKey), SecurityAlgorithms.HmacSha256Signature);

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
                // سیگنیچر متقارن
                SigningCredentials = signinCredentials,
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
            var list = new List<Claim> {
                // .net claim
                new Claim(ClaimTypes.Name , user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                // .jwt claim
                new Claim(JwtRegisteredClaimNames.Gender, user.Gender.ToDisplay()),
            };
            foreach (var item in userRoles)
            {
                list.Add(new Claim(ClaimTypes.Role, item));
            }

            return list;
        }
    }
}
