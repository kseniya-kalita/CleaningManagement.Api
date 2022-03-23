using Microsoft.AspNetCore.Mvc;

namespace CleaningManagement.Api.Controllers
{
    [ApiController]
    [Route("api/cleaningplans")]
    public class CleaningPlansController : ControllerBase
    {

        [HttpGet]
        public IActionResult GetCleaningPlans() => Ok();

        /*
         * TODO: Provide your implementation.
         */
    }
}