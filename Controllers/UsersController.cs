using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.Api.Data;
using DatingApp.Api.DTOS;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using DatingApp.Api.Helpers;
using System.Security.Claims;

namespace DatingApp.Api.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
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
        public async Task<IActionResult> GetUsers([FromQuery]UserParams userParams)
        {

             var currentUserId =int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);
             var userFromRepo = await Repository.GetUser(currentUserId);
             userParams.UserId = currentUserId;
             if(string.IsNullOrEmpty(userParams.Gender))
             {
                 userParams.Gender = userFromRepo.Gender == "male" ? "female": "male";
             }

            var user = await Repository.GetAllUsers(userParams);
            var usertoList = Mapper.Map<IEnumerable<UserForListDto>>(user);
            
            Response.AddPagination(user.CurrentPage, user.PageSize,
             user.TotalCount, user.TotalPages);

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
            
             if(id!=int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                 return Unauthorized();
                
                
            var userFromRepo = await Repository.GetUser(id);
            
            Mapper.Map(userForUpdateDto, userFromRepo);

            if(await Repository.SaveAll())
                return NoContent();
            
            throw new Exception($"Updating user {id} failed on save");

        }
    

    }
}