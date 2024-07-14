using GaragesStructure.Utils;
using GaragesStructure.DATA.DTOs.User;
using GaragesStructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GaragesStructure.Controllers{

    
    public class AuthController : Properties.BaseController{
        private readonly IUserService _userService;

        public AuthController(IUserService userService) {
            _userService = userService;
        }

        
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /
        ///     {
        ///       "email": "osamah@admin.com",
        ///       "password": "12345678"
        ///     }
        ///
        /// </remarks>
        [HttpPost("/api/Login")]
        public async Task<ActionResult> Login(LoginForm loginForm) => Ok(await _userService.Login(loginForm));
    }
}