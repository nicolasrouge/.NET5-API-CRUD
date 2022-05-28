using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace netwebapi.Dtos
{
    public class SkillAttackDto
    {
        public int AttackerId { get; set; }

        public int SkillId { get; set; }
        public int OpponentId { get; set; }
    }
}