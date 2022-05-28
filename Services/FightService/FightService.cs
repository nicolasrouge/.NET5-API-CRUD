using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using netwebapi.Data;
using netwebapi.Dtos;
using netwebapi.Dtos.Fight;
using netwebapi.Models;

namespace netwebapi.Services.FightService
{
    //we could use CharacterService here
    public class FightService : IFightService
    {
        private readonly DataContext _dataContext;

        public FightService(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

public async Task<ServiceResponse<AttackResultDto>> SkillAttack(SkillAttackDto request)
        {
            var response = new ServiceResponse<AttackResultDto>();
            try{

                
                var attacker = await _dataContext.Characters.Include(c=> c.Skills).FirstOrDefaultAsync(c => c.Id == request.AttackerId);

                var opponent = await _dataContext.Characters.FirstOrDefaultAsync(c => c.Id == request.OpponentId);

                var skill = attacker.Skills.FirstOrDefault(s => s.Id == request.SkillId);
                if(skill == null){
                    response.Success = false;
                    response.Message = $"{attacker.Name} doesnt know this skill.";
                }

                int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));
                damage -= new Random().Next(opponent.Defense);

                if(damage > 0){
                    opponent.HitPoints -= damage;
                }
                if(opponent.HitPoints <=0){
                    response.Message = $"{opponent.Name} has been defeated!";
                }
                await _dataContext.SaveChangesAsync();

                response.Data = new AttackResultDto {
                    Attacker = attacker.Name,
                    AttackerHP = attacker.HitPoints,
                    Opponent = opponent.Name,
                    OpponentHP = opponent.HitPoints,
                    Damage = damage
                };
                
            }catch(Exception ex){
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request)
        {
            var response = new ServiceResponse<AttackResultDto>();
            try{

                
                var attacker = await _dataContext.Characters.Include(c=> c.Weapon).FirstOrDefaultAsync(c => c.Id == request.AttackerId);

                var opponent = await _dataContext.Characters.FirstOrDefaultAsync(c => c.Id == request.OpponentId);

                int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strength));
                damage -= new Random().Next(opponent.Defense);

                if(damage > 0){
                    opponent.HitPoints -= damage;
                }
                if(opponent.HitPoints <=0){
                    response.Message = $"{opponent.Name} has been defeated!";
                }
                await _dataContext.SaveChangesAsync();

                response.Data = new AttackResultDto {
                    Attacker = attacker.Name,
                    AttackerHP = attacker.HitPoints,
                    Opponent = opponent.Name,
                    OpponentHP = opponent.HitPoints,
                    Damage = damage
                };
                
            }catch(Exception ex){
                response.Success = false;
                response.Message = ex.Message;
            }

            return response;
        }
    }
}