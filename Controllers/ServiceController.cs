using System;
using System.Threading.Tasks;
using Api.Helper;
using Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    public class ServiceController : CustomBaseController
    {
        private readonly IServiceRepository _serviceRepository;

        public ServiceController(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }
        [HttpPost("[action]")]
        public async Task<ActionResult<bool>> SendContactMail(Contact contact)
        {
            try
            {
                if (contact == null) return BadRequest("Keine Daten");
                var checkSendMail =  await _serviceRepository.SendContactMailAsync(contact);
                return Ok(checkSendMail);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}