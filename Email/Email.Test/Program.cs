using System;
using System.IO;
using System.Net.Mail;
using System.Threading.Tasks;
using Email.Core.Senders;

namespace Email.Test
{
    internal class Program
    {
        public static async Task Main(string[] args)
        {
            var smtpClient = new SmtpClient("10.124.147.201");
            Core.Email.DefaultSender = new SmtpSender(smtpClient);

            await Core.Email
                .From("Xiang.Zhu@elekta.com")
                .To("Xiang.Zhu@elekta.com", "Zhu Xiang")
                .Subject("hows it going bob")
                .UsingTemplateFromFile($"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Demo.html")}", new { Name = "Rad Dude" })
                .SendAsync();

            Console.WriteLine("Email Send Successfully");
            Console.ReadLine();
        }
    }
}