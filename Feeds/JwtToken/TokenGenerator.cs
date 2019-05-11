using Feeds.Controllers.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Feeds.JwtToken
{
    public class TokenGenerator : ITokenGenerator
    {
        private readonly IConfiguration _configuration;
        private readonly IJwtSigningEncodingKey _signingEncodingKey;
        private readonly IJwtEncryptingEncodingKey _encryptingEncodingKey;

        public TokenGenerator(IConfiguration configuration,
                              IJwtSigningEncodingKey signingEncodingKey,
                              IJwtEncryptingEncodingKey encryptingEncodingKey)
        {
            _signingEncodingKey = signingEncodingKey;
            _encryptingEncodingKey = encryptingEncodingKey;
            _configuration = configuration;
        }

        public string Generate(RegistrationRequest registrationRequest)
        {

            var claims = new Claim[]
            {
                new Claim(ClaimTypes.Email, registrationRequest.Email),
                new Claim(ClaimTypes.Role, registrationRequest.AsAdmin? "Admin" : "User")
            };

            
            var tokenHandler = new JwtSecurityTokenHandler();

            JwtSecurityToken token = tokenHandler.CreateJwtSecurityToken(
                issuer: _configuration["JwtIssuer"],
                audience: _configuration["JwtAudience"],
                subject: new ClaimsIdentity(claims),
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddYears(1),
                issuedAt: DateTime.Now,
                signingCredentials: SigningCredentialsPrepare(),
                encryptingCredentials: EncryptingCredentialsPrepare());


            return tokenHandler.WriteToken(token);
        }

        private SigningCredentials SigningCredentialsPrepare()
        {
            return new SigningCredentials(
                    _signingEncodingKey.GetKey(),
                    _signingEncodingKey.SigningAlgorithm);
        }

        private EncryptingCredentials EncryptingCredentialsPrepare()
        {
            return new EncryptingCredentials(
                    _encryptingEncodingKey.GetKey(),
                    _encryptingEncodingKey.SigningAlgorithm,
                    _encryptingEncodingKey.EncryptingAlgorithm);
        }
    }
}
