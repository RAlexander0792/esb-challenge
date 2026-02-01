using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ESBC.API.Controllers;

[Route("api/v{version:apiVersion}/[controller]")]
[ApiController]
public class BaseAPIController : ControllerBase
{
}
