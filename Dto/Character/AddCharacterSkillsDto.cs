using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_RPG.Dto.Character
{
    public class AddCharacterSkillsDto
    {
        public int CharacterId { get; set; }
        public int SkillId { get; set; }
    }
}