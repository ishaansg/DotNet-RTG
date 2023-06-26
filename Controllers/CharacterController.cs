using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace DOTNET_RPG.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CharacterController : ControllerBase
    {

        private readonly ICharacterService _characterService;

        public CharacterController(ICharacterService characterService)
        {
            _characterService = characterService;
        }
        [HttpGet("GetAll")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Get()
        {
            return Ok(await _characterService.GetAllCharacters());
        }

        [HttpGet("GetSingle{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> GetSingle([FromRoute] int id)
        {
            return Ok(await _characterService.GetCharacterById(id));
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<List<AddCharacterDto>>>> AddCharacter([FromBody] AddCharacterDto _character)
        {
            return Ok(await _characterService.AddCharacter(_character));
        }

        [HttpPost("Skill")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> AddCharacterSkill([FromBody] AddCharacterSkillsDto _skill)
        {
            var response = await _characterService.AddCharacterSkill(_skill);
            if (!response.Sucess)
            {
                return NotFound(response);
            }
            return Ok(response);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> UpdateCharacter([FromBody] UpdateCharacterDto _character, [FromRoute] int id)
        {
            var response = await _characterService.UpdateCharacter(_character, id);
            if (!response.Sucess)
            {
                return NotFound(response);
            }
            return Ok(response);
        }

        [HttpDelete("Delete{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> DeleteSingleCharacter([FromRoute] int id)
        {
            var response = await _characterService.DeleteCharacterById(id);
            if (!response.Sucess)
            {
                return NotFound(response);
            }
            return Ok(response);
        }
    }
}