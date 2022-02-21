using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using netwebapi.Dtos.Character;
using netwebapi.Dtos.Weapon;
using netwebapi.Models;
using netwebapi.Services.WeaponService;

namespace netwebapi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("controller")]
    public class WeaponController : ControllerBase
    {
        public readonly IWeaponService _weaponService;
        public WeaponController(IWeaponService weaponService)
        {
            _weaponService = weaponService;
        }
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetCharacterDto>>> AddWeapon(AddWeaponDto newWeapon){
            return Ok(await _weaponService.AddWeapon(newWeapon));
        }
    }
}