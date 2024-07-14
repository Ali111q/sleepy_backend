using GaragesStructure.DATA.DTOs.User;
using GaragesStructure.Helpers;
using GaragesStructure.Services;
using GaragesStructure.Utils;
using Microsoft.AspNetCore.Mvc;

namespace GaragesStructure.Controllers;



public class UsersController : BaseController{
    private readonly IUserService _userService;

    public UsersController(IUserService userService) {
        _userService = userService;
    }

    [HttpPost("/api/Users")]
    public async Task<ActionResult> Create(RegisterForm registerForm) =>
        Ok(await _userService.Register(registerForm));
    
    
    [HttpGet("/api/Users/{id}")]
    public async Task<ActionResult> GetById(Guid id) => OkObject(await _userService.GetUserById(id));

    [HttpPut("/api/Users/{id}")]
    public async Task<ActionResult> Update(UpdateUserForm updateUserForm, Guid id) =>
        Ok(await _userService.UpdateUser(updateUserForm, id));

    [HttpDelete("/api/Users/{id}")]
    public async Task<ActionResult> Delete(Guid id) => Ok(await _userService.DeleteUser(id));


    [HttpGet("/api/Users")]
    public async Task<ActionResult<Respons<UserDto>>> GetAll([FromQuery] UserFilter filter) =>
        Ok(await _userService.GetAll(filter), filter.PageNumber, filter.PageSize);
    
    [HttpPut("/api/Users/edit-profile")]
    public async Task<ActionResult> ChangeProfileInfo(ChangeProfileInfo changeProfileInfo) =>
        Ok(await _userService.ChangeProfileInfo(Id, changeProfileInfo));
    
    [HttpPatch("/api/Users/change-state/{id}")]
    public async Task<ActionResult> ChangeUserState([FromBody] bool isActive) =>
        Ok(await _userService.ChangeUserState(Id, isActive));
    
}