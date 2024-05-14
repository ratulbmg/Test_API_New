using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Test_API_New.BusinessLogicLayer.DataTransferObject;
using Test_API_New.BusinessLogicLayer.Exception;
using Test_API_New.BusinessLogicLayer.Services;
using Test_API_New.DataAccessLayer.Entities;

namespace Test_API_New.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await this.userService.GetAll();
            return Ok(result);
        }

        [HttpGet]
        [Route("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserResponseDTO))]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0) return BadRequest();
            var result = await this.userService.GetById(id);
            if (result is not null) return Ok(result);
            return NotFound(new { message = $"Entity with ID => {id} Not Found" });
            // throw new UpdateNotFoundException("NEW ERROR");
        }

        [HttpPost]
        public async Task<IActionResult> Post(UserRequestDTO userRequest)
        {
            var result = await this.userService.Add(userRequest);
            if (result is null) return BadRequest(new { message = "User already exiest in database" });
            return Ok(result);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UserRequestDTO userRequest)
        {
            if (id <= 0) return BadRequest();
            var result = await this.userService.Update(id, userRequest);
            if (result is not null) return Ok(result);
            return NotFound(new { message = $"Entity with ID => {id} Not Found" });
        }

        [HttpPatch]
        [Route("{id}")]
        public async Task<IActionResult> UpdatePatch(int id, [FromBody] JsonPatchDocument<User> userRequest)
        {
            if (id <= 0) return BadRequest();
            var result = await this.userService.UpdatePatch(id, userRequest);
            if (result is not null) return Ok(result);
            return NotFound(new { message = $"Entity with ID => {id} Not Found" });
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0) return BadRequest();
            var result = await this.userService.Delete(id);
            if (result) return Ok(new { message = $"Entity with ID => {id} Deleted" });
            return NotFound(new { message = $"Entity with ID => {id} Not Found" });
        }
    }
}
