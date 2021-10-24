﻿using RestWithAspNet5.Authentication.Configurations;
using RestWithAspNet5.Authentication.Repository;
using RestWithAspNet5.Authentication.VO;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RestWithAspNet5.Authentication.Services
{
    public class LoginService : ILoginService
    {
        private const string DATE_FORMAT = "yyyy-MM-dd HH:mm:ss";
        private TokenConfigurations _configuration;
        private IUserRepository _repository;
        private readonly ITokenService _tokenService;

        public LoginService(TokenConfigurations configuration, IUserRepository repository, ITokenService tokenService)
        {
            _configuration = configuration;
            _repository = repository;
            _tokenService = tokenService;
        }

        public TokenVO ValidateCredentials(UserVO userCredentials)
        {
            var user = _repository.ValidateCredentials(userCredentials);

            if (user == null)
            {
                return null;
            }

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName)
            };

            var accessToken = _tokenService.GenerateAccessToken(claims);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpireTime = DateTime.Now.AddDays(_configuration.DaysToExpire);

            _repository.RefreshUserInfo(user);

            DateTime createDate = DateTime.Now;
            DateTime expirationDate = createDate.AddMinutes(_configuration.Minutes);            

            return new TokenVO
            (
                true,
                createDate.ToString(DATE_FORMAT),
                expirationDate.ToString(DATE_FORMAT),
                accessToken,
                refreshToken
            );
        }

        public TokenVO ValidateCredentials(TokenVO token)
        {
            var accessToken = token.AccessToken;
            var refreshToken = token.RefreshToken;

            var principal = _tokenService.GetPrincipalFromExpiredToken(accessToken);
            var username = principal.Identity.Name;

            var user = _repository.ValidateCredentials(username);

            if(user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpireTime <= DateTime.Now)
            {
                return null;
            }

            accessToken = _tokenService.GenerateAccessToken(principal.Claims);
            refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshToken = refreshToken;

            _repository.RefreshUserInfo(user);

            DateTime createDate = DateTime.Now;
            DateTime expirationDate = createDate.AddMinutes(_configuration.Minutes);

            return new TokenVO
            (
                true,
                createDate.ToString(DATE_FORMAT),
                expirationDate.ToString(DATE_FORMAT),
                accessToken,
                refreshToken
            );
        }

        public bool RevokeToken(string username)
        {
            return _repository.RevokeToken(username);
        }
    }
}
