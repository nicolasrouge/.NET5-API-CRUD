using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using netwebapi.Dtos;
using netwebapi.Dtos.Fight;
using netwebapi.Models;

namespace netwebapi.Services.FightService
{
    public interface IFightService
    {
        Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request);
        Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto request);
    }
}