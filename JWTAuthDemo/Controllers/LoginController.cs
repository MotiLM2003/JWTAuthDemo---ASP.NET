using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JWTAuthDemo.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace JWTAuthDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;

        }

        [HttpGet]
        public IActionResult Login(string username, string pass)
        {
            UserModel login = new UserModel();
            login.UserName = username;
            login.Password = pass;
            IActionResult response = Unauthorized();


            var user =  AuthenticeteUser(login)



        }

        private UserModel AuthenticeteUser(UserModel login)
        {
            UserModel user = null;
            if(login.UserName == "Moti" && login.Password == "1234")
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
    }
}
