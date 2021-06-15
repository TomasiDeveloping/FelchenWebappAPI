using System.Threading.Tasks;
using Api.Helper;
using Api.Interfaces;
using Api.Services;
using Microsoft.Extensions.Options;

namespace Api.Data.Repositories
{
    public class ServiceRepository : IServiceRepository
    {
        private readonly EmailService _emailService;
        
        public ServiceRepository(IOptions<EmailSettings> options)
        {
            _emailService = new EmailService(options);
        }
        
        public async Task<bool> SendContactMailAsync(Contact contact)
        {
            var message = EmailMessages.CreateContactMessage(contact);
            return await _emailService.SendEmailAsync("info@tomasi-developing.ch", message, "Kontakt FelchenApp");
        }
    }
}