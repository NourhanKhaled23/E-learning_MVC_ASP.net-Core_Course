using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace WebApplication1.Services
{
    public interface INotificationService
    {
        Task SendAsync(string recipient, string message);
    }

    public class EmailNotificationService : INotificationService
    {
        private readonly ILogger<EmailNotificationService> _logger;

        public EmailNotificationService(ILogger<EmailNotificationService> logger)
        {
            _logger = logger;
        }

        public Task SendAsync(string recipient, string message)
        {
            _logger.LogInformation($"[EMAIL SERVICE] Sending Email to {recipient}: {message}");
            return Task.CompletedTask;
        }
    }

    public class SmsNotificationService : INotificationService
    {
        private readonly ILogger<SmsNotificationService> _logger;

        public SmsNotificationService(ILogger<SmsNotificationService> logger)
        {
            _logger = logger;
        }

        public Task SendAsync(string recipient, string message)
        {
            _logger.LogInformation($"[SMS SERVICE] Sending SMS to {recipient}: {message}");
            return Task.CompletedTask;
        }
    }
}
