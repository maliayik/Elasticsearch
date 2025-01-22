using System.Net;
using Elasticsearch.API.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elasticsearch.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        //Base controller oluşturarak dönen response kodlarımızı tek bir yerden yönetebiliriz.
        [NonAction]
        public IActionResult CreateActionResult<T>(ResponseDto<T> response)
        {
            if (response.Status == HttpStatusCode.NoContent)
                return new ObjectResult(null) { StatusCode = response.Status.GetHashCode() };

            return new ObjectResult(response) { StatusCode = response.GetHashCode() };
        }
    }
}
