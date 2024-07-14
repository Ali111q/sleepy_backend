using System.Security.Claims;
using GaragesStructure.Utils;
using Microsoft.AspNetCore.Mvc;

namespace GaragesStructure.Properties
{
  
        [ApiController]
        [Route("api/[controller]")]
        public abstract class BaseController : ControllerBase {
            protected virtual string GetClaim(string claimName) {
                var claims = (User.Identity as ClaimsIdentity)?.Claims;
                var claim = claims?.FirstOrDefault(c =>
                string.Equals(c.Type, claimName, StringComparison.CurrentCultureIgnoreCase) &&
                !string.Equals(c.Type, "null", StringComparison.CurrentCultureIgnoreCase));
                var rr = claim?.Value!.Replace("\"", "");

                return rr??"";
            }


            protected Guid Id => Guid.TryParse(GetClaim("Id"), out var id) ? id : Guid.Empty;

            // protected UserRole Role => GetClaim("Role") switch {
            //     "Admin" => UserRole.Admin,
            //     "User" => UserRole.User,
            //     _ => UserRole.None
            // };

            protected Guid? ParentId {
                get {
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

            protected ObjectResult Ok<T>((List<T>? data, int? totalCount, string? error) result,
            int pageNumber
            ) =>
            result.error != null
            ? base.BadRequest(new {Message = result.error})
            : base.Ok(new Respons<T> {
                Data = result.data,
                PagesCount = (result.totalCount - 1) / Util.PageSize + 1,
                CurrentPage = pageNumber,
            });

            protected ObjectResult Ok<T>((T obj, string? error) result) =>
            result.error != null
            ? base.BadRequest(new {Message = result.error})
            : base.Ok(result.obj);
        }
    }