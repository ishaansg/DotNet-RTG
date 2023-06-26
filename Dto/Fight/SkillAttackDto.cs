using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_RPG.Dto.Fight
{
    public class SkillAttackDto
    {
        public int AttackerId { get; set; }
        public int OponentId { get; set; }
        public int SkillId { get; set; }
    }
}