using auth.Interfaces;
using auth.Model.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace auth.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailControllers
    {

        public class EmailController : ControllerBase
        {
            private readonly IEmailService _emailService;

            public EmailController(IEmailService emailService)
            {
                _emailService = emailService;
            }

            [HttpPost("fogotPassword")]
            public IActionResult SendEmail([FromBody] EmailDto model)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                _emailService.SendEmail(model.From, model.To, model.Subject, model.Body);

                return Ok();
            }
        }
    }
}
