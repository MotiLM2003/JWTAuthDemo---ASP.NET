using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using JWTAuthDemo.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace JWTAuthDemo.Controllers
{

    public class Test{
            public string username { set; get; }
           public string pass { set; get; }
        }

    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }

         
        [HttpPost]
        public IActionResult Login(Test test)
        {
           
            UserModel login =   new UserModel();
            login.UserName = test.username;
            login.Password = test.pass;
            IActionResult response = Unauthorized();


            var user = AuthenticeteUser(login);

            if(user != null)
            {
                var tokenStr = GenerateJSONWebToken(user);
                response = Ok(new { token = tokenStr });
            }

            return response;

        }

        private UserModel AuthenticeteUser(UserModel login)
        {
            UserModel user = null;
            if(login.UserName == "moti" && login.Password == "1234")
            {

                user = new UserModel()
                {
                    UserName = login.UserName,
                    Password = login.Password,
                    EmailAddress = "moti@gmail.com"
                };

            }
            return user;
        }

        private string GenerateJSONWebToken(UserModel userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var Claims = new[]
            {
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub, userInfo.UserName),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Email, userInfo.EmailAddress),
                new Claim(System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };



            var token = new JwtSecurityToken(
                    issuer: _config["Jwt:Issuer"],
                    audience: _config["Jwt:Issuer"],
                    Claims,
                    expires: DateTime.Now.AddMinutes(120),
                    signingCredentials: credentials
                );

            var encodedtoken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodedtoken;
        }

        [Authorize]
        [HttpPost("Post")]
        public string Post()
        {
            var indentity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = indentity.Claims.ToList();
            var userName = claim[0].Value;
            return $"Welcometo {userName}";
        }

      


        [Authorize]
        [HttpGet("GetValue")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "Valuue1", "Value2", "Value3" };
        }
    }
}
