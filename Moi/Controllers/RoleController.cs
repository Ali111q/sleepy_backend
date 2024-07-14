using GaragesStructure.DATA.DTOs.roles;
using GaragesStructure.Interface;
using GaragesStructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace GaragesStructure.Controllers;

public class RoleController: BaseController
{
    private readonly IRoleService _roleService;


    public RoleController(IRoleService roleService)
    {
        _roleService = roleService;

    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] RoleForm form) => Ok(await _roleService.Add(form));

    [HttpPost("/api/add-permission")]
    public async Task<ActionResult> AddPermissionToRole([FromBody] AssignPermissionsForm form) =>
        Ok(await _roleService.AddPermissionToRole(form.RoleId, form.Permissions));
    
    [HttpGet]
    public async Task<ActionResult> GetPermissions([FromQuery] PermissionsFilter filter) => Ok(await _roleService.GetAllPermissions(filter));
}