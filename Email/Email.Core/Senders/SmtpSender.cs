using System;
using System.Net.Mail;
using Email.Core.Interfaces;

namespace Email.Core.Senders
{
    public class SmtpSender : ISender
    {
        private readonly Func<SmtpClient> _clientFactory;
        private readonly SmtpClient _smtpClient;
    }
}