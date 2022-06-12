using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Email.Core.Interfaces;
using Email.Core.Models;

namespace Email.Core.Senders
{
    public class SaveToDiskSender : ISender
    {
        private readonly string _directory;

        public SaveToDiskSender(string directory)
        {
            _directory = directory;
        }

        public SendResponse Send(IEmail email)
        {
            return SendAsync(email).GetAwaiter().GetResult();
        }

        public async Task<SendResponse> SendAsync(
            IEmail email,
            CancellationToken cancellationToken = default)
        {
            var response = new SendResponse();
            await SaveEmailToDiskAsync(email);
            return response;
        }

        private async Task SaveEmailToDiskAsync(IEmail email)
        {
            var uid = Guid.NewGuid().ToString();
            var fileName = Path.Combine(_directory, $"{DateTime.Now: yyyy-MM-dd_HH-mm-ss}_{uid}");

            using (var sw = new StreamWriter(File.OpenWrite(fileName)))
            {
                await sw.WriteLineAsync($"From: {email.Data.FromAddress.Name} <{email.Data.FromAddress.EmailAddress}>");
                await sw.WriteLineAsync($"To: {string.Join(",", email.Data.ToAddresses.Select(x => $"{x.Name} <{x.EmailAddress}>"))}");
                await sw.WriteLineAsync($"Cc: {string.Join(",", email.Data.CcAddresses.Select(x => $"{x.Name} <{x.EmailAddress}>"))}");
                await sw.WriteLineAsync($"Bcc: {string.Join(",", email.Data.BccAddresses.Select(x => $"{x.Name} <{x.EmailAddress}>"))}");
                await sw.WriteLineAsync($"ReplyTo: {string.Join(",", email.Data.ReplyToAddresses.Select(x => $"{x.Name} <{x.EmailAddress}>"))}");
                await sw.WriteLineAsync($"Subject: {email.Data.Subject}");
                foreach (var dataHeader in email.Data.Headers)
                {
                    await sw.WriteLineAsync($"{dataHeader.Key}:{dataHeader.Value}");
                }

                await sw.WriteLineAsync();
                await sw.WriteAsync(email.Data.Body);
            }
        }
    }
}