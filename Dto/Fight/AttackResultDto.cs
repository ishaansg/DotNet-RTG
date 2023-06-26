using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_RPG.Dto.Fight
{
    public class AttackResultDto
    {
        public string Attacker { get; set; } = string.Empty;
        public string Oponent { get; set; } = string.Empty;

        public int AttackerHP { get; set; }
        public int OponentHP { get; set; }

        public int Damage { get; set; }

    }
}