﻿using System;
using System.Collections.Generic;
using System.Text;

namespace HZY.Toolkit
{
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using Microsoft.IdentityModel.Tokens;
    using System.Linq;

    /// <summary>
    /// JWT是由 . 分割的三部分组成：
    /// 头部(Header)
    /// 载荷(Payload) : 这一部分是JWT主要的信息存储部分，其中包含了许多种的声明（claims）。
    /// 签名(Signature)：使用保存在服务端的秘钥对其签名，用来验证发送者的JWT的同时也能确保在期间不被篡改。
    /// </summary>
    public class JwtTokenUtil
    {
        //private readonly IConfiguration _configuration;

        //public JwtTokenUtil(IConfiguration configuration)
        //{
        //    _configuration = configuration;
        //}

        public string GetToken(string Id, string JwtSecurityKey, string JwtKeyName)
        {
            // push the user’s name into a claim, so we can identify the user later on.
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, Id));
            //var claims = new[]
            //{
            //    new Claim(ClaimTypes.Name, Key),
            //       //new Claim(ClaimTypes.Role, admin)//在这可以分配用户角色，比如管理员 、 vip会员 、 普通用户等
            //};
            //sign the token using a secret key.This secret will be shared between your API and anything that needs to check that the token is legit.
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSecurityKey)); // 获取密钥
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256); //凭证 ，根据密钥生成
            //.NET Core’s JwtSecurityToken class takes on the heavy lifting and actually creates the token.
            /**
             * Claims (Payload)
                Claims 部分包含了一些跟这个 token 有关的重要信息。 JWT 标准规定了一些字段，下面节选一些字段:

                iss: The issuer of the token，token 是给谁的  发送者
                aud: 接收的
                sub: The subject of the token，token 主题
                exp: Expiration Time。 token 过期时间，Unix 时间戳格式
                iat: Issued At。 token 创建时间， Unix 时间戳格式
                jti: JWT ID。针对当前 token 的唯一标识
                除了规定的字段外，可以包含其他任何 JSON 兼容的字段。
             * */
            var token = new JwtSecurityToken(
                issuer: JwtKeyName,
                audience: JwtKeyName,
                claims: claims,
                expires: DateTime.Now.AddHours(12),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string ReadJwtToken(string token)
        {
            var _JwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var _ReadJwtToken = _JwtSecurityTokenHandler.ReadJwtToken(token);
            var key = string.Empty;
            if (_ReadJwtToken.Claims?.Count() > 0)
            {
                key = _ReadJwtToken.Claims.FirstOrDefault().Value;
            }
            return key;
        }



    }
}
