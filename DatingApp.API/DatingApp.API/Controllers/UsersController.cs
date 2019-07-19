using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using DatingApp.API.DTOS;
using System.Diagnostics;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IDatingRepository _repo;
        private readonly IMapper _mapper;

        public UsersController(IDatingRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repo.GetUsers();
            var usersReturn = _mapper.Map<IEnumerable<UserForDetailedDto>>(users);
            return Ok(usersReturn);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetUser(id);
            var userReturn = _mapper.Map<UserForDetailedDto>(user);
            return Ok(userReturn);
        }




    }
}