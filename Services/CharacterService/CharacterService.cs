using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace DOTNET_RPG.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;

        private readonly IHttpContextAccessor _httpContextAccessor;
        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _mapper = mapper;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private int GetUserId() =>
            int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var characterNew = _mapper.Map<Character>(newCharacter);
            characterNew.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            await _context.Characters.AddAsync(characterNew);
            await _context.SaveChangesAsync();

            serviceResponse.Data = await _context.Characters.Where(c => c.User!.Id == GetUserId()).Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters = await _context.Characters.Include(c => c.Weapon)
                .Include(c => c.Skills)
                .Where(c => c.User!.Id == GetUserId()).ToListAsync();
            serviceResponse.Data = dbCharacters.Select(c => _mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var dbCharacter = await _context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());
            serviceResponse.Data = _mapper.Map<GetCharacterDto>(dbCharacter);
            return serviceResponse;

        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> UpdateCharacter(UpdateCharacterDto newCharacter, int Id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                var _character = await _context.Characters.
                Include(c => c.User).FirstOrDefaultAsync(c => c.Id == Id);
                if (_character is null || _character.User!.Id != GetUserId())
                {
                    throw new Exception("Charater with id Not Found !!!");
                }
                _mapper.Map(newCharacter, _character);
                // we can Map data using Mapper as well as manually
                // _character.Name = newCharacter.Name;
                // _character.HitPonts = newCharacter.HitPonts;
                // _character.Strength = newCharacter.Strength;
                // _character.Defence = newCharacter.Defence;
                // _character.Intelligence = newCharacter.Intelligence;
                // _character.Class = newCharacter.Class;
                await _context.SaveChangesAsync();
                serviceResponse.Data = await _context.Characters.Where(c => c.User!.Id == GetUserId()).Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Sucess = false;
                serviceResponse.Message = ex.Message;
            }


            return serviceResponse;

        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacterById(int Id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            try
            {
                var _character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == Id && c.User!.Id == GetUserId());
                if (_character is null)
                {
                    throw new Exception($"Charater with '{Id}' Not Found !!!");
                }
                _context.Remove(_character);
                await _context.SaveChangesAsync();
                serviceResponse.Data = await _context.Characters.Where(c => c.User!.Id == GetUserId()).Select(c => _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            }
            catch (Exception ex)
            {
                serviceResponse.Sucess = false;
                serviceResponse.Message = ex.Message;
            }


            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillsDto newCharacterSkill)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            try
            {
                var _character = await _context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .FirstOrDefaultAsync(c => c.Id == newCharacterSkill.CharacterId && c.User!.Id == GetUserId());
                if (_character is null)
                {
                    serviceResponse.Sucess = false;
                    serviceResponse.Message = "Character Not Found!!!";
                    return serviceResponse;

                }
                var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == newCharacterSkill.SkillId);
                if (skill is null)
                {
                    serviceResponse.Sucess = false;
                    serviceResponse.Message = "Skill Not Found!!!";
                    return serviceResponse;
                }

                _character.Skills!.Add(skill);
                await _context.SaveChangesAsync();
                serviceResponse.Data = _mapper.Map<GetCharacterDto>(_character);
                serviceResponse.Sucess = true;
                serviceResponse.Message = "Skill Added Successfully";

            }
            catch (Exception ex)
            {
                serviceResponse.Sucess = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;

        }
    }
}