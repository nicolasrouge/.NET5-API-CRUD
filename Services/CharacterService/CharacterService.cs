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
using netwebapi.Models;

namespace netwebapi.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
        }

        private int GetUserId() => int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceReponse = new ServiceResponse<List<GetCharacterDto>>();
            Character character = _mapper.Map<Character>(newCharacter);
            character.User = await _context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());
            _context.Characters.Add(character);
            var dbCharacters = await _context.Characters.ToListAsync();
            await _context.SaveChangesAsync();
            serviceReponse.Data = await _context.Characters
            .Where(c => c.User.Id == GetUserId())
            .Select(c=> _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            return serviceReponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceReponse = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters = await _context.Characters.ToListAsync();

            try{
            Character character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());

            if(character != null){
                _context.Characters.Remove(character);
                await _context.SaveChangesAsync();

                serviceReponse.Data = _context.Characters.Where(c => c.User.Id == GetUserId()).Select(c=> _mapper.Map<GetCharacterDto>(c)).ToList();
            }else{
                            serviceReponse.Success = false;
                serviceReponse.Message = "character not found";
            }
            
            }catch(Exception ex){
                serviceReponse.Success = false;
                serviceReponse.Message = ex.Message;
            }
            
            return serviceReponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
                        var serviceReponse = new ServiceResponse<List<GetCharacterDto>>();

            try{
            var dbCharacters = await _context.Characters.Where(c => c.User.Id == GetUserId()).ToListAsync();
            serviceReponse.Data = dbCharacters.Select(c=> _mapper.Map<GetCharacterDto>(c)).ToList();
            }catch(Exception ex){
                serviceReponse.Success = false;
                serviceReponse.Message = ex.Message;
            }
            return serviceReponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceReponse = new ServiceResponse<GetCharacterDto>();
            var dbCharacters = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id && c.User.Id == GetUserId());
            serviceReponse.Data = _mapper.Map<GetCharacterDto>(dbCharacters);

            return serviceReponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updateCharacter)
        {
            var serviceReponse = new ServiceResponse<GetCharacterDto>();

            try{
            Character character = await _context.Characters
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == updateCharacter.Id);


            if(character.User.Id == GetUserId()){
                character.Name = updateCharacter.Name;
                character.HitPoints = updateCharacter.HitPoints;
                character.Strength = updateCharacter.Strength;
                character.Defense = updateCharacter.Defense;
                character.Intelligence = updateCharacter.Intelligence;
                character.Class = updateCharacter.Class;

                await _context.SaveChangesAsync();

                serviceReponse.Data = _mapper.Map<GetCharacterDto>(character);
            }else{
                serviceReponse.Success = false;
                serviceReponse.Message = "character not found";
            }


            }catch(Exception ex){
                serviceReponse.Success = false;
                serviceReponse.Message = ex.Message;
            }
            
            return serviceReponse;
        }
    }
}