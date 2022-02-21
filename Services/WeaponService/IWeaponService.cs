using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using netwebapi.Dtos.Character;
using netwebapi.Dtos.Weapon;
using netwebapi.Models;

namespace netwebapi.Services.WeaponService
{
    public interface IWeaponService
    {
        Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newweapon);
    }
}