using System;
using System.Security.Claims;
using System.Threading.Tasks;
using DatingApp.API.DTOS;
using DatingApp.API.Models;
using DatingApp.API.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using IdentityModel.Jwk;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthRepository _repo;
        private IConfiguration _config;

        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _repo = repo;     
            _config = config;       
        }

       
        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterUserDTO userDTO) 
        { 
            userDTO.Username = userDTO.Username.ToLower();

            if(await _repo.UserExists(userDTO.Username))
                return BadRequest("Username exits");

            var userToCreate = new User
            {
                Username = userDTO.Username
            };

            var createdUser = await _repo.Register(userToCreate,userDTO.Password);

            return StatusCode(202);

        }

        [HttpPost("auth")]
        public async Task<IActionResult> Login(LoginUserDTO userDTO)
        {
            //Buscamos el usuario en la base de Dato
            var searchUser = await _repo.Login(userDTO.Usernanme.ToLower(),userDTO.Password);

            if(searchUser == null)
                return Unauthorized();

            //Comenzamos a construir nuestro Token

            //Necesitamos esta dos Reclamaciones al servidor 
             var claims = new[]
             {
                 new Claim(ClaimTypes.NameIdentifier, searchUser.Id.ToString()),
                 new Claim(ClaimTypes.Name, searchUser.Username)
             };

             //Creamos una clave
            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSetting:Token").Value));

            //Creamos la credenciales
            var creds = new SigningCredentials(key,SecurityAlgorithms.HmacSha512Signature);

            //Una decripcion hacia el token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new {
                token = tokenHandler.WriteToken(token)
            });
        }

    }
}