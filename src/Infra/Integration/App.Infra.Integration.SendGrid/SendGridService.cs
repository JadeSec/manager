using System;
using SendGrid;
using System.Linq;
using SendGrid.Helpers.Mail;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using App.Infra.Integration.SendGrid.Models;
using App.Infra.Integration.SendGrid.Attributes;
using App.Infra.Bootstrap;
using App.Infra.Bootstrap.Attributes;
using System.Threading.Tasks;
using System.Net;
using Microsoft.Extensions.Logging;

namespace App.Infra.Integration.SendGrid
{
    [Singleton]
    public class SendGridService : IService<SendGridService>
    {
        readonly SendGridClient _client;

        readonly Option _option;

        ILogger<SendGridService> _logger;

        public SendGridService(
            ILogger<SendGridService> logger,
            IConfiguration configuration)
        {
            _option = Option.Parse(configuration);

            _client = new SendGridClient(_option.Apikey);

            _logger = logger;
        }

        public async Task SendAsync(string to, string body)
            => await ProcessAsync(string.Empty, new string[] { to }, body);

        public async Task SendAsync(string from, string to, string body)
            => await ProcessAsync(from, new string[] { to }, body);

        public async Task SendAsync<B>(string to, B body)
            => await ProcessAsync(string.Empty,new string[] { to }, body);

        public async Task SendAsync<B>(string from, string to, B body)
            => await ProcessAsync(from, new string[] { to }, body);

        public async Task SendAsync(string[] tos, string body)
            => await ProcessAsync(string.Empty, tos, body);

        public async Task SendAsync<B>(string[] tos, B body)
            => await ProcessAsync(string.Empty, tos, body);

        private async Task<bool> ProcessAsync<B>(string from, string[] tos, B body)
        {
            var attr = SendGridAttribute.Parse(body.GetType());

            var nFrom = _fromEmailAddress(from, _option.FromRandom(), attr.From);
            var nTemplateId = attr.TemplateId;            
            var nTos = tos.Select(x => new EmailAddress(x))
                          .ToList();

            var response = await _client.SendEmailAsync(new SendGridMessage()
            {
                From = nFrom,
                TemplateId = nTemplateId,                
                Personalizations = new List<Personalization>()
                {
                   new Personalization()
                   {
                       Tos = nTos,
                       TemplateData = body,
                       Bccs = _bccs(attr)
                   }
                }
            });

            if (response.StatusCode != HttpStatusCode.OK &&
                response.StatusCode != HttpStatusCode.Accepted &&
                response.StatusCode != HttpStatusCode.Created &&
                response.StatusCode != HttpStatusCode.NoContent)
            {                
                _logger.LogCritical($"Sendgrid  don't send the email, statusCode: {response.StatusCode}");

                return false;
            }

            return true;
        }

        private List<EmailAddress> _bccs(SendGridAttribute attr)
        {
            if (!string.IsNullOrEmpty(attr.Bcc))
            {
                return new List<EmailAddress>()
                {
                    new EmailAddress(attr.Bcc)
                };
            }

            return null;
        }


        private EmailAddress _fromEmailAddress(string from, string fromOption, string fromAttr)
        {
            if (!string.IsNullOrEmpty(from))
            {
                return new EmailAddress(from);
            }
            else if (!string.IsNullOrEmpty(fromAttr))
            {
                return new EmailAddress(fromAttr);
            }
            else if (!string.IsNullOrEmpty(fromOption))
            {
                return new EmailAddress(fromOption);
            }

            throw new Exception("Sendgrid (from) not found!");
        }
    }
}
