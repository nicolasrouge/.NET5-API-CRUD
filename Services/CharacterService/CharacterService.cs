using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
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

        public CharacterService(IMapper mapper, DataContext context)
        {
            _context = context;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceReponse = new ServiceResponse<List<GetCharacterDto>>();
            Character character = _mapper.Map<Character>(newCharacter);
            _context.Characters.Add(character);
            var dbCharacters = await _context.Characters.ToListAsync();
            await _context.SaveChangesAsync();
            serviceReponse.Data = await _context.Characters.Select(c=> _mapper.Map<GetCharacterDto>(c)).ToListAsync();
            return serviceReponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int id)
        {
            var serviceReponse = new ServiceResponse<List<GetCharacterDto>>();
            var dbCharacters = await _context.Characters.ToListAsync();

            try{
            Character character = await _context.Characters.FirstAsync(c => c.Id == id);
            _context.Characters.Remove(character);
            await _context.SaveChangesAsync();

            serviceReponse.Data = _context.Characters.Select(c=> _mapper.Map<GetCharacterDto>(c)).ToList();
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
            var dbCharacters = await _context.Characters.ToListAsync();
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
            var dbCharacters = await _context.Characters.FirstOrDefaultAsync(c => c.Id == id);
            serviceReponse.Data = _mapper.Map<GetCharacterDto>(dbCharacters);

            return serviceReponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updateCharacter)
        {
            var serviceReponse = new ServiceResponse<GetCharacterDto>();

            try{
            Character character = await _context.Characters.FirstOrDefaultAsync(c => c.Id == updateCharacter.Id);
            
            character.Name = updateCharacter.Name;
            character.HitPoints = updateCharacter.HitPoints;
            character.Strength = updateCharacter.Strength;
            character.Defense = updateCharacter.Defense;
            character.Intelligence = updateCharacter.Intelligence;
            character.Class = updateCharacter.Class;

            await _context.SaveChangesAsync();

            serviceReponse.Data = _mapper.Map<GetCharacterDto>(character);
            }catch(Exception ex){
                serviceReponse.Success = false;
                serviceReponse.Message = ex.Message;
            }
            
            return serviceReponse;
        }
    }
}