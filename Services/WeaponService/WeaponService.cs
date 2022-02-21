using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using netwebapi.Data;
using netwebapi.Dtos.Character;
using netwebapi.Dtos.Weapon;
using netwebapi.Models;

namespace netwebapi.Services.WeaponService
{
    public class WeaponService : IWeaponService
    {
        private readonly IHttpContextAccessor _httpcontextaccessor;
        private readonly IMapper _mapper;
        public readonly DataContext _Context;

        public WeaponService(DataContext context, IHttpContextAccessor httpcontextaccessor, IMapper mapper )
        {
            _mapper = mapper;
            _httpcontextaccessor = httpcontextaccessor;
            _Context = context;
        }
        public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newweapon)
        {
            var response = new ServiceResponse<GetCharacterDto>();
            try{

                var character = await  _Context.Characters.FirstOrDefaultAsync( c=> c.Id == newweapon.CharacterId && c.User.Id == int.Parse(_httpcontextaccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
                if(character == null){
                    response.Success = false;
                    response.Message = "Cahracter not found";
                    return response;
                }
                var weapon = new Weapon{
                    Name = newweapon.Name,
                    Damage = newweapon.Damage,
                    Character = character
                };

                _Context.Weapons.Add(weapon);

                await _Context.SaveChangesAsync();

                response.Data = _mapper.Map<GetCharacterDto>(character);

            }catch(Exception ex){
                response.Success = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}