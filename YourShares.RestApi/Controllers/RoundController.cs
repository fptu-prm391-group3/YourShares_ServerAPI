using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace YourShares.RestApi.Controllers
{
    [ApiController]
    [Route("/api/rounds")]
    [Produces("application/json")]
    [Authorize]
    public class RoundController : ControllerBase
    {
        
    }
}