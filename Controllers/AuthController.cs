using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DatingApp.Api.Data;
using DatingApp.Api.DTOS;
using DatingApp.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;


namespace DatingApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository repository;
        public IConfiguration _Configuration { get; set; }
        public AuthController(IAuthRepository repository, IConfiguration configuration)
        {
            this._Configuration = configuration;
            this.repository = repository;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterForNewUser registerForNewUSer)
        {
             //var userFromRepo = await repository.Login(registerForNewUSer.UserName.ToLower(),registerForNewUSer.Password);

            if (await repository.UserExists(registerForNewUSer.UserName.ToLower()))
                return BadRequest("User Not Exists");

            var user = new User { Username = registerForNewUSer.UserName };
            var createdUser = await repository.Register(user, registerForNewUSer.Password);
            return StatusCode(201);

        }

         [HttpGet("{id}")]
        public async Task<IActionResult> GetValue(int id)
        {
            var values = await repository.GetValue(id);
            return Ok(values);
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        {
           // var isResult = await repository.UserExists(userForLoginDto.UserName);

           var userFromRepo = await repository.Login(userForLoginDto.Username.ToLower(), userForLoginDto.Password);

            if (userFromRepo == null)
                return Unauthorized();

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name,userFromRepo.Username)
            };

            var key = new SymmetricSecurityKey
            (Encoding.UTF8.GetBytes(_Configuration.GetSection("Appsettings:Token").Value));

            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(30),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok( new {
                token = tokenHandler.WriteToken(token)
            }

            );



        }

    }
}