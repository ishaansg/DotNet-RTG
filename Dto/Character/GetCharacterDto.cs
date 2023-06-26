using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DOTNET_RPG.Dto.Skill;
using DOTNET_RPG.Dto.Weapon;

namespace DOTNET_RPG.Dto.Character
{
    public class GetCharacterDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Anindya";

        public int HitPonts { get; set; } = 100;
        public int Strength { get; set; } = 10;
        public int Defence { get; set; } = 10;
        public int Intelligence { get; set; } = 10;
        public RpgClass Class { get; set; } = RpgClass.Knight;
        public GetWeaponDto? Weapon { get; set; }
        public List<GetSkillDto>? Skills { get; set; }
        public int Fight { get; set; }
        public int Victories { get; set; }
        public int Defeats { get; set; }

    }
}