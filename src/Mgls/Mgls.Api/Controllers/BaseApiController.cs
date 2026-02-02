using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Mgls.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class BaseAPIController : ControllerBase
{
}
