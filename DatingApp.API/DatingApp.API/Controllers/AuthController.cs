using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.DTOS;
using DatingApp.API.Models;
using DatingApp.API.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DatingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private IAuthRepository _repo;
        private IConfiguration _config;
        private readonly IMapper _mapper;

        public AuthController(IAuthRepository repo, IConfiguration config, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
            _config = config;
        }


        [HttpPost("Register")]
        public async Task<IActionResult> Register(RegisterUserDto userDTO)
        {
            userDTO.Username = userDTO.Username.ToLower();

            if (await _repo.UserExists(userDTO.Username))
                return BadRequest("Username exits");

            var userToCreate = new User
            {
                Username = userDTO.Username
            };

            var createdUser = await _repo.Register(userToCreate, userDTO.Password);

            return StatusCode(202);

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto userDTO)
        {

            //Buscamos el usuario en la base de Dato
            var searchUser = await _repo.Login(userDTO.Username.ToLower(), userDTO.Password);

            if (searchUser == null)
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
                .GetBytes(_config.GetSection("AppSettings:Token").Value));

            //Creamos la credenciales
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            //Una decripcion hacia el token
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var user = _mapper.Map<UserForListDto>(searchUser);

            return Ok(new
            {
                token = tokenHandler.WriteToken(token),
                user
            });
        }
    }

}
