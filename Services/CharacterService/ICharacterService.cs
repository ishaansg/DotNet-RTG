using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_RPG.Services.CharacterService
{
    public interface ICharacterService
    {
        Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters();
        Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id);
        Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter);
        Task<ServiceResponse<List<GetCharacterDto>>> UpdateCharacter(UpdateCharacterDto newCharacter, int id);

        Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacterById(int id);

        Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillsDto newCharacterSkill);
    }
}