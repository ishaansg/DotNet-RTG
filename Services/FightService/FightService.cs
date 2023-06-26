using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DOTNET_RPG.Dto.Fight;

namespace DOTNET_RPG.Services.FightService
{
    public class FightService : IFightService
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public FightService(DataContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<FightResultDto>> Fight(FightRequestDto request)
        {
            var response = new ServiceResponse<FightResultDto>()
            {
                Data = new FightResultDto()
            };
            try
            {
                var charactes = await _context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .Where(c => request.CharacterIds.Contains(c.Id))
                .ToListAsync();

                bool defeted = false;
                while (!defeted)
                {
                    foreach (var attacker in charactes)
                    {
                        var opponents = charactes.Where(c => c.Id != attacker.Id).ToList();
                        var opponent = opponents[new Random().Next(opponents.Count)];

                        int damage = 0;
                        string attackUsed = String.Empty;

                        bool userWeapon = new Random().Next(2) == 0;

                        if (userWeapon && attacker.Weapon is not null)
                        {
                            attackUsed = attacker.Weapon.Name;
                            damage = DoWeaponAttack(attacker, opponent);
                        }
                        else if (!userWeapon && attacker.Skills is not null)
                        {
                            var skill = attacker.Skills[new Random().Next(attacker.Skills.Count)];

                            attackUsed = skill.Name;
                            damage = DoSkillAttack(attacker, opponent, skill);
                        }
                        else
                        {
                            response.Data.Log
                            .Add($"{attacker.Name} wasn't able to attack");
                            continue;
                        }
                        response.Data.Log.Add($"{attacker.Name} attacks {opponent.Name} using {attackUsed} with  {(damage >= 0 ? damage : 0)} damage");
                        if (opponent.HitPonts <= 0)
                        {
                            defeted = true;
                            attacker.Victories++;
                            opponent.Defeats++;
                            response.Data.Log.Add($"{opponent.Name} has been defeted");
                            response.Data.Log.Add($"{attacker.Name} wins with {attacker.HitPonts} HP left!");
                            break;
                        }
                    }
                }
                charactes.ForEach(c =>
                {
                    c.Fight++;
                    c.HitPonts = 100;
                });
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Sucess = false;
            }
            return response;
        }

        public async Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto request)
        {
            var response = new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker = await _context.Characters.Include(c => c.Skills).FirstOrDefaultAsync(c => c.Id == request.AttackerId);

                var opponent = await _context.Characters.FirstOrDefaultAsync(c => c.Id == request.OponentId);
                if (attacker is null || opponent is null || attacker.Skills is null)
                {
                    throw new Exception("Some Data doesnot Match !!!");
                }

                var skill = attacker.Skills.FirstOrDefault(s => s.Id == request.SkillId);

                if (skill is null)
                {
                    response.Sucess = false;
                    response.Message = $"{attacker.Name} doesnot have this skill !";
                    return response;
                }
                int damage = DoSkillAttack(attacker, opponent, skill);

                if (opponent.HitPonts <= 0)
                {
                    response.Message = $"{opponent.Name} has been defeted !";
                }

                await _context.SaveChangesAsync();

                response.Data = new AttackResultDto
                {
                    Attacker = attacker.Name,
                    Oponent = opponent.Name,
                    AttackerHP = attacker.HitPonts,
                    OponentHP = opponent.HitPonts,
                    Damage = damage


                };
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Sucess = false;

            }
            return response;
        }

        private static int DoSkillAttack(Character attacker, Character opponent, Skill skill)
        {
            if (attacker.Skills is null)
                throw new Exception("Attacker has no Skills");
            var damage = skill.Damage + (new Random().Next(attacker.Intelligence));

            damage -= new Random().Next(opponent.Defence);
            if (damage > 0)
            {
                opponent.HitPonts -= damage;
            }

            return damage;
        }

        public async Task<ServiceResponse<AttackResultDto>> WeaponnAttack(WeaponAttackDto request)
        {
            var response = new ServiceResponse<AttackResultDto>();
            try
            {
                var attacker = await _context.Characters.Include(c => c.Weapon).FirstOrDefaultAsync(c => c.Id == request.AttackerId);

                var opponent = await _context.Characters.Include(c => c.Weapon).FirstOrDefaultAsync(c => c.Id == request.OpponentId);
                if (attacker is null || opponent is null || attacker.Weapon is null)
                {
                    throw new Exception("Some Data doesnot Match !!!");
                }

                int damage = DoWeaponAttack(attacker, opponent);

                if (opponent.HitPonts <= 0)
                {
                    response.Message = $"{opponent.Name} has been defeted !";
                }

                await _context.SaveChangesAsync();

                response.Data = new AttackResultDto
                {
                    Attacker = attacker.Name,
                    Oponent = opponent.Name,
                    AttackerHP = attacker.HitPonts,
                    OponentHP = opponent.HitPonts,
                    Damage = damage


                };
            }
            catch (Exception ex)
            {
                response.Message = ex.Message;
                response.Sucess = false;

            }
            return response;
        }

        private static int DoWeaponAttack(Character attacker, Character opponent)
        {
            if (attacker.Weapon is null)
                throw new Exception("Attacker has no Weapon");
            int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));
            damage -= new Random().Next(opponent.Defence);
            if (damage > 0)
            {
                opponent.HitPonts -= damage;
            }

            return damage;
        }

        public async Task<ServiceResponse<List<HighscoreDto>>> GetHighScore()
        {
            var charactes = await _context.Characters
            .Where(c => c.Fight > 0)
            .OrderByDescending(c => c.Victories)
            .ThenBy(c => c.Defeats).ToListAsync();

            var response = new ServiceResponse<List<HighscoreDto>>()
            {
                Data = charactes.Select(c => _mapper.Map<HighscoreDto>(c)).ToList()
            };
            return response;
        }
    }
}