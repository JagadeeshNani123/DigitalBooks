using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DigitalBooksWebAPI.Models;
using Microsoft.AspNetCore.Razor.Language;
using UserValidation.Services;
using DigitalBooksWebAPI.Services;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Microsoft.AspNetCore.Authorization;

namespace UserValidation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly DigitalBooksContext _context;
        public IConfiguration _config;

        public UsersController(DigitalBooksContext context, IHostingEnvironment env)
        {
            _context = context;
            _config = new BuildConfiguration(env).Configuration;
        }

        
        [HttpPost]
        public TokenDescriptor GenerateToken(UserValidationRequestModel request)
        {
            var user = new UserValidationCheck(request.UserName, request.Password);
            var isValidUser = user.IsValidUser();
            if (isValidUser)
            {
                var tokenService = new TokenService();
                var jwtToken = new JwtToken()
                {
                    Jwtkey = _config.GetValue<string>("jwt:key"),
                    JwtIssuer = _config.GetValue<string>("jwt:issuer"),
                    JwtAud = _config.GetValue<string>("jwt:Aud")
                };
                var token = tokenService.buildToken(jwtToken.Jwtkey, jwtToken.JwtIssuer, new[]{ jwtToken.JwtAud }, request.UserName);

                return new TokenDescriptor
                {
                    Token = token,
                    IsAuthenticated = true
                };
            }
            return new TokenDescriptor
            {
                Token = string.Empty,
                IsAuthenticated = false
            };
        }


        
    }
}
