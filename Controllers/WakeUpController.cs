using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public class WakeUpController : CustomBaseController
{
    [AllowAnonymous]
    [HttpGet]
    public ActionResult<bool> Get()
    {
        return Ok(true);
    }
}