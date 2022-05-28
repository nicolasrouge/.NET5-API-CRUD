using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using netwebapi.Data;
using netwebapi.Dtos.User;
using netwebapi.Models;

namespace netwebapi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepository;
        public AuthController(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto request){

            var response = await _authRepository.Register(
                new User {Username = request.Username}, request.Password
                );
                if(!response.Success){
                    return BadRequest(response);
                }else{
                    return Ok(response);
                }
        }
        [HttpPost("Login")]
        public async Task<ActionResult<ServiceResponse<string>>> Login(UserLoginDto request){
            var response = await _authRepository.Login(
                request.Username, request.Password
            );
            if(!response.Success){
                return BadRequest(response);
            }else{
                return Ok(response);
            }
        }
    }
}