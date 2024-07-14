using System.Security.Claims;
using GaragesStructure.Utils;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace GaragesStructure.Controllers{
    [ApiController]
    [ApiExplorerSettings(GroupName = "Api")]
    [Route("api/[controller]")]
    public abstract class BaseController : ControllerBase{
        protected virtual string GetClaim(string claimName) {
            var claims = (User.Identity as ClaimsIdentity)?.Claims;
            var claim = claims?.FirstOrDefault(c =>
                string.Equals(c.Type, claimName, StringComparison.CurrentCultureIgnoreCase) &&
                !string.Equals(c.Type, "null", StringComparison.CurrentCultureIgnoreCase));
            var rr = claim?.Value!.Replace("\"", "");

            return rr ?? "";
        }


        protected Guid Id => Guid.TryParse(GetClaim("Id"), out var id) ? id : Guid.Empty;
        protected string? PhoneNumber => GetClaim("PhoneNumber") ?? null ;

        protected string Role => GetClaim("Role");

        protected Guid? ParentId
        {
            get
            {
                var idString = GetClaim("ParentId");
                Guid? re;
                if (!string.Equals(idString, null, StringComparison.Ordinal) &&
                    !string.Equals(idString, "null", StringComparison.Ordinal))
                    re = Guid.Parse(idString);
                else
                    re = null;
                return re;
            }
        }

        protected string MethodType => HttpContext.Request.Method;


        protected ObjectResult OkObject<T>((T? data, string? error) result) =>
            result.error != null
                ? base.BadRequest(new { Message = result.error })
                : base.Ok(result.data);

        protected ObjectResult Ok<T>((List<T>? data, int? totalCount, string? error) result,
            int pageNumber, int pageSize = 10
        ) =>
            result.error != null
                ? base.BadRequest(new { Message = result.error })
                : base.Ok(new Respons<T>
                {
                    Data = result.data,
                    PagesCount = (result.totalCount - 1) / 10  + 1,
                    CurrentPage = pageNumber,
                    TotalPages = (int)Math.Ceiling((double)result.totalCount / pageSize)
                });

        protected ObjectResult Ok<T>((T obj, string? error) result) =>
            result.error != null
                ? base.BadRequest(new { Message = result.error })
                : base.Ok(result.obj);
        
        
        protected ObjectResult Ok<T>((T obj, string? error , bool? state) result) =>
            result.state == true ? base.Unauthorized(new {}) : result.error != null
                ? base.BadRequest(new { Message = result.error })
                : base.Ok(result.obj);
    }
}