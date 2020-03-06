using System;

using System.Collections.Generic;

using System.Linq;

using System.Threading.Tasks;
using DatingApp.Api.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.Api.Controllers

{
    [Authorize]

    [Route("api/[controller]")]

    [ApiController]

    public class ValuesController : ControllerBase

    {
        private readonly DataContext context;
        public ValuesController(DataContext context)
        {
            this.context = context;

        }
        // GET api/values

        [HttpGet]

        public async Task<IActionResult> GetValues()

        {
            var values = await context.Values.ToListAsync();
            return Ok(values);
        }



        // GET api/values/5

        [AllowAnonymous]

        [HttpGet("{id}")]

        public async Task<IActionResult> GetValue(int id)

        {
            var values = await context.Values.FirstOrDefaultAsync(i=>i.Id==id);
            return Ok(values);
        }



        // POST api/values

        [HttpPost]

        public void Post([FromBody] string value)

        {

        }



        // PUT api/values/5

        [HttpPut("{id}")]

        public void Put(int id, [FromBody] string value)

        {

        }



        // DELETE api/values/5

        [HttpDelete("{id}")]

        public void Delete(int id)

        {

        }

    }

}





