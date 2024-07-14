using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace GaragesStructure.Controllers{
    [Route("api/[controller]")]
    [ApiExplorerSettings(GroupName = "Api")]
    [ApiController]
    public class EnumController : ControllerBase{
        [HttpGet("GetEnums")]
        public IActionResult GetEnums() {
            var enums = Assembly.GetAssembly(typeof(EnumController))
                ?.GetTypes()
                .Where(type => type.IsEnum)
                .Select(type => new
                {
                    type.Name,
                    Values = Enum.GetValues(type)
                        .Cast<Enum>()
                        .Select(value => new { Name = value.ToString(), Value = value })
                })
                .ToList();

            return Ok(new { Enums = enums });
        }
    }
}