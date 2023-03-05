using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DormProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class ApiController : ControllerBase
    {
        public const string PathSeparator = "/";
        public const string Id = "{id}";
    }
}
