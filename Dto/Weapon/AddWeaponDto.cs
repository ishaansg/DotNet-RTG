using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DOTNET_RPG.Dto.AddWeaponDto
{
    public class AddWeaponDto
    {
        public string Name { get; set; } = string.Empty;

        public int Damage { get; set; }
        public int CharacterId { get; set; }
    }
}