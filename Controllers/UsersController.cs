using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.Api.Data;
using DatingApp.Api.DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DatingApp.Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public IDatingRepository Repository { get; set; }
        public IMapper Mapper { get; set; }
        public UsersController(IDatingRepository repository, IMapper mapper)
        {
            this.Mapper = mapper;
            this.Repository = repository;

        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var user = await Repository.GetAllUsers();
            var usertoList = Mapper.Map<IEnumerable<UserForListDto>>(user);
            return Ok(usertoList);
        }

        [HttpGet("{id}",Name="GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await Repository.GetUser(id);
            var usertoReturn = Mapper.Map<UserForDetailedDto>(user);
            return Ok(usertoReturn);
        }

        
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id,UserForUpdateDto userForUpdateDto)
        {
            /*
             if(id!=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                 return Unauthorized();
                */
                
            var userFromRepo = await Repository.GetUser(id);
            
            Mapper.Map(userForUpdateDto, userFromRepo);

            if(await Repository.SaveAll())
                return NoContent();
            
            throw new Exception($"Updating user {id} failed on save");

        }
    

    }
}