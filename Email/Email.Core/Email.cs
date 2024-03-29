﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using Email.Core.Helpers;
using Email.Core.Interfaces;
using Email.Core.Models;
using Email.Core.Renderers;
using Email.Core.Senders;

namespace Email.Core
{
    public class Email : IEmail
    {
        public EmailData Data { get; set; }
        public ITemplateRenderer Renderer { get; set; }
        public ISender Sender { get; set; }

        public static ITemplateRenderer DefaultRenderer { get; set; } = new ReplaceRenderer();
        public static ISender DefaultSender { get; set; } = new SaveToDiskSender("/");

        /// <summary>
        /// Creates a new email instance with default settings.
        /// </summary>
        public Email() : this(DefaultRenderer, DefaultSender)
        {
        }

        /// <summary>
        /// Creates a new email instance with overrides for the rendering and sending engines.
        /// </summary>
        /// <param name="renderer">The template rendering engine</param>
        /// <param name="sender">The email sending implementation</param>
        public Email(ITemplateRenderer renderer, ISender sender)
            : this(renderer, sender, null, null)
        {
        }

        /// <summary>
        ///  Creates a new Email instance with default settings, from a specific mailing address.
        /// </summary>
        /// <param name="emailAddress">Email address to send from</param>
        /// <param name="name">Name to send from</param>
        public Email(string emailAddress, string name = "")
            : this(DefaultRenderer, DefaultSender, emailAddress, name)
        {
        }

        /// <summary>
        ///  Creates a new Email instance using the given engines and mailing address.
        /// </summary>
        /// <param name="renderer">The template rendering engine</param>
        /// <param name="sender">The email sending implementation</param>
        /// <param name="emailAddress">Email address to send from</param>
        /// <param name="name">Name to send from</param>
        public Email(
            ITemplateRenderer renderer,
            ISender sender,
            string emailAddress,
            string name = default)
        {
            Data = new EmailData
            {
                FromAddress = new Address(emailAddress, name)
            };
            Renderer = renderer;
            Sender = sender;
        }

        /// <summary>
        /// Creates a new Email instance and sets the from property
        /// </summary>
        /// <param name="emailAddress">Email address to send from</param>
        /// <param name="name">Name to send from</param>
        /// <returns>Instance of the Email class</returns>
        public static IEmail From(string emailAddress, string name = null)
        {
            var email = new Email
            {
                Data = { FromAddress = new Address(emailAddress, name) }
            };

            return email;
        }

        /// <summary>
        /// Set the send from email address
        /// </summary>
        /// <param name="emailAddress">Email address of sender</param>
        /// <param name="name">Name of sender</param>
        /// <returns>Instance of the Email class</returns>
        public IEmail SetFrom(string emailAddress, string name = null)
        {
            Data.FromAddress = new Address(emailAddress, name);
            return this;
        }

        /// <summary>
        /// Adds a recipient to the email, Splits name and address on ';'
        /// </summary>
        /// <param name="emailAddress">Email address of recipient</param>
        /// <param name="name">Name of recipient</param>
        /// <returns>Instance of the Email class</returns>
        public IEmail To(string emailAddress, string name)
        {
            if (emailAddress.Contains(";"))
            {
                //email address has semi-colon, try split
                var nameSplit = name?.Split(';') ?? Array.Empty<string>();
                var addressSplit = emailAddress.Split(';');
                for (int i = 0; i < addressSplit.Length; i++)
                {
                    var currentName = string.Empty;
                    if ((nameSplit.Length - 1) >= i)
                    {
                        currentName = nameSplit[i];
                    }

                    Data.ToAddresses.Add(new Address(addressSplit[i].Trim(), currentName.Trim()));
                }
            }
            else
            {
                Data.ToAddresses.Add(new Address(emailAddress.Trim(), name?.Trim()));
            }

            return this;
        }

        /// <summary>
        /// Adds a recipient to the email
        /// </summary>
        /// <param name="emailAddress">Email address of recipient (allows multiple splitting on ';')</param>
        /// <returns></returns>
        public IEmail To(string emailAddress)
        {
            if (emailAddress.Contains(";"))
            {
                foreach (var address in emailAddress.Split(';'))
                {
                    Data.ToAddresses.Add(new Address(address));
                }
            }
            else
            {
                Data.ToAddresses.Add(new Address(emailAddress));
            }

            return this;
        }

        /// <summary>
        /// Adds all recipients in list to email
        /// </summary>
        /// <param name="mailAddresses">List of recipients</param>
        /// <returns>Instance of the Email class</returns>
        public IEmail To(IEnumerable<Address> mailAddresses)
        {
            foreach (var address in mailAddresses)
            {
                Data.ToAddresses.Add(address);
            }

            return this;
        }

        /// <summary>
        /// Adds a Carbon Copy to the email
        /// </summary>
        /// <param name="emailAddress">Email address to cc</param>
        /// <param name="name">Name to cc</param>
        /// <returns>Instance of the Email class</returns>
        public IEmail CC(string emailAddress, string name = default)
        {
            Data.CcAddresses.Add(new Address(emailAddress, name));
            return this;
        }

        /// <summary>
        /// Adds all Carbon Copy in list to an email
        /// </summary>
        /// <param name="mailAddresses">List of recipients to CC</param>
        /// <returns>Instance of the Email class</returns>
        public IEmail CC(IEnumerable<Address> mailAddresses)
        {
            foreach (var address in mailAddresses)
            {
                Data.CcAddresses.Add(address);
            }

            return this;
        }

        /// <summary>
        /// Adds a blind carbon copy to the email
        /// </summary>
        /// <param name="emailAddress">Email address of bcc</param>
        /// <param name="name">Name of bcc</param>
        /// <returns>Instance of the Email class</returns>
        public IEmail BCC(string emailAddress, string name = default)
        {
            Data.BccAddresses.Add(new Address(emailAddress, name));
            return this;
        }

        /// <summary>
        /// Adds all blind carbon copy in list to an email
        /// </summary>
        /// <param name="mailAddresses">List of recipients to BCC</param>
        /// <returns>Instance of the Email class</returns>
        public IEmail BCC(IEnumerable<Address> mailAddresses)
        {
            foreach (var address in mailAddresses)
            {
                Data.BccAddresses.Add(address);
            }

            return this;
        }

        /// <summary>
        /// Sets the ReplyTo address on the email
        /// </summary>
        /// <param name="address">The ReplyTo Address</param>
        /// <returns></returns>
        public IEmail ReplyTo(string address)
        {
            Data.ReplyToAddresses.Add(new Address(address));

            return this;
        }

        /// <summary>
        /// Sets the ReplyTo address on the email
        /// </summary>
        /// <param name="address">The ReplyTo Address</param>
        /// <param name="name">The Display Name of the ReplyTo</param>
        /// <returns></returns>
        public IEmail ReplyTo(string address, string name)
        {
            Data.ReplyToAddresses.Add(new Address(address, name));

            return this;
        }

        /// <summary>
        /// Sets the subject of the email
        /// </summary>
        /// <param name="subject">email subject</param>
        /// <returns>Instance of the Email class</returns>
        public IEmail Subject(string subject)
        {
            Data.Subject = subject;
            return this;
        }

        /// <summary>
        /// Adds a Body to the Email
        /// </summary>
        /// <param name="body">The content of the body</param>
        /// <param name="isHtml">True if Body is HTML, false for plain text (default)</param>
        public IEmail Body(string body, bool isHtml = false)
        {
            Data.IsHtml = isHtml;
            Data.Body = body;
            return this;
        }

        /// <summary>
        /// Adds a Plaintext alternative Body to the Email. Used in conjunction with an HTML email,
        /// this allows for email readers without html capability, and also helps avoid spam filters.
        /// </summary>
        /// <param name="body">The content of the body</param>
        public IEmail PlaintextAlternativeBody(string body)
        {
            Data.PlaintextAlternativeBody = body;
            return this;
        }

        /// <summary>
        /// Marks the email as High Priority
        /// </summary>
        public IEmail HighPriority()
        {
            Data.Priority = Priority.High;
            return this;
        }

        /// <summary>
        /// Marks the email as Low Priority
        /// </summary>
        public IEmail LowPriority()
        {
            Data.Priority = Priority.Low;
            return this;
        }

        /// <summary>
        /// Set the template rendering engine to use, defaults to RazorEngine
        /// </summary>
        public IEmail UsingTemplateEngine(ITemplateRenderer renderer)
        {
            Renderer = renderer;
            return this;
        }

        /// <summary>
        /// Adds template to email from embedded resource
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">Path the the embedded resource eg [YourAssembly].[YourResourceFolder].[YourFilename.txt]</param>
        /// <param name="model">Model for the template</param>
        /// <param name="assembly">The assembly your resource is in. Defaults to calling assembly.</param>
        /// <param name="isHtml">True if Body is HTML (default), false for plain text</param>
        /// <returns></returns>
        public IEmail UsingTemplateFromEmbedded<T>(string path, T model, Assembly assembly, bool isHtml = true)
        {
            var template = EmbeddedResourceHelper.GetResourceAsString(assembly, path);
            var result = Renderer.Parse(template, model, isHtml);
            Data.IsHtml = isHtml;
            Data.Body = result;

            return this;
        }

        /// <summary>
        /// Adds template to email from embedded resource
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path">Path the the embedded resource eg [YourAssembly].[YourResourceFolder].[YourFilename.txt]</param>
        /// <param name="model">Model for the template</param>
        /// <param name="assembly">The assembly your resource is in. Defaults to calling assembly.</param>
        /// <returns></returns>
        public IEmail PlaintextAlternativeUsingTemplateFromEmbedded<T>(string path, T model, Assembly assembly)
        {
            var template = EmbeddedResourceHelper.GetResourceAsString(assembly, path);
            var result = Renderer.Parse(template, model, false);
            Data.PlaintextAlternativeBody = result;

            return this;
        }


        /// <summary>
        /// Adds the template file to the email
        /// </summary>
        /// <param name="filename">The path to the file to load</param>
        /// <param name="model">Model for the template</param>
        /// <param name="isHtml">True if Body is HTML (default), false for plain text</param>
        /// <returns>Instance of the Email class</returns>
        public IEmail UsingTemplateFromFile<T>(string filename, T model, bool isHtml = true)
        {
            var template = "";

            using (var reader = new StreamReader(File.OpenRead(filename)))
            {
                template = reader.ReadToEnd();
            }

            var result = Renderer.Parse(template, model, isHtml);
            Data.IsHtml = isHtml;
            Data.Body = result;

            return this;
        }

        /// <summary>
        /// Adds the template file to the email
        /// </summary>
        /// <param name="filename">The path to the file to load</param>
        /// <param name="model">Model for the template</param>
        /// <returns>Instance of the Email class</returns>
        public IEmail PlaintextAlternativeUsingTemplateFromFile<T>(string filename, T model)
        {
            var template = "";

            using (var reader = new StreamReader(File.OpenRead(filename)))
            {
                template = reader.ReadToEnd();
            }

            var result = Renderer.Parse(template, model, false);
            Data.PlaintextAlternativeBody = result;

            return this;
        }

        /// <summary>
        /// Adds a culture specific template file to the email
        /// </summary>
        /// <param name="filename">The path to the file to load</param>
        /// /// <param name="model">The razor model</param>
        /// <param name="culture">The culture of the template (Default is the current culture)</param>
        /// <param name="isHtml">True if Body is HTML (default), false for plain text</param>
        /// <returns>Instance of the Email class</returns>
        public IEmail UsingCultureTemplateFromFile<T>(string filename, T model, CultureInfo culture, bool isHtml = true)
        {
            var cultureFile = GetCultureFileName(filename, culture);
            return UsingTemplateFromFile(cultureFile, model, isHtml);
        }

        /// <summary>
        /// Adds a culture specific template file to the email
        /// </summary>
        /// <param name="filename">The path to the file to load</param>
        /// /// <param name="model">The razor model</param>
        /// <param name="culture">The culture of the template (Default is the current culture)</param>
        /// <returns>Instance of the Email class</returns>
        public IEmail PlaintextAlternativeUsingCultureTemplateFromFile<T>(string filename, T model, CultureInfo culture)
        {
            var cultureFile = GetCultureFileName(filename, culture);
            return PlaintextAlternativeUsingTemplateFromFile(cultureFile, model);
        }

        /// <summary>
        /// Adds razor template to the email
        /// </summary>
        /// <param name="template">The razor template</param>
        /// <param name="model">Model for the template</param>
        /// <param name="isHtml">True if Body is HTML, false for plain text (Optional)</param>
        /// <returns>Instance of the Email class</returns>
        public IEmail UsingTemplate<T>(string template, T model, bool isHtml = true)
        {
            var result = Renderer.Parse(template, model, isHtml);
            Data.IsHtml = isHtml;
            Data.Body = result;

            return this;
        }

        /// <summary>
        /// Adds razor template to the email
        /// </summary>
        /// <param name="template">The razor template</param>
        /// <param name="model">Model for the template</param>
        /// <returns>Instance of the Email class</returns>
        public IEmail PlaintextAlternativeUsingTemplate<T>(string template, T model)
        {
            var result = Renderer.Parse(template, model, false);
            Data.PlaintextAlternativeBody = result;

            return this;
        }

        /// <summary>
        /// Adds an Attachment to the Email
        /// </summary>
        /// <param name="attachment">The Attachment to add</param>
        /// <returns>Instance of the Email class</returns>
        public IEmail Attach(Attachment attachment)
        {
            if (!Data.Attachments.Contains(attachment))
            {
                Data.Attachments.Add(attachment);
            }

            return this;
        }

        /// <summary>
        /// Adds Multiple Attachments to the Email
        /// </summary>
        /// <param name="attachments">The List of Attachments to add</param>
        /// <returns>Instance of the Email class</returns>
        public IEmail Attach(IEnumerable<Attachment> attachments)
        {
            foreach (var attachment in attachments.Where(attachment => !Data.Attachments.Contains(attachment)))
            {
                Data.Attachments.Add(attachment);
            }

            return this;
        }

        public IEmail AttachFromFilename(string filename, string contentType = null, string attachmentName = null)
        {
            var stream = File.OpenRead(filename);
            Attach(new Attachment
            {
                Data = stream,
                Filename = attachmentName ?? filename,
                ContentType = contentType
            });

            return this;
        }

        /// <summary>
        /// Adds tag to the Email. This is currently only supported by the Mailgun and SendGrid providers. <see href="https://documentation.mailgun.com/en/latest/user_manual.html#tagging"/> and <see href="https://docs.sendgrid.com/for-developers/sending-email/categories"/>
        /// </summary>
        /// <param name="tag">Tag name, max 128 characters, ASCII only</param>
        /// <returns>Instance of the Email class</returns>
        public IEmail Tag(string tag)
        {
            Data.Tags.Add(tag);
            return this;
        }

        /// <summary>
        /// Adds headers to the Email. This is currently only supported by the Mailgun and SendGrid providers. <see href="https://documentation.mailgun.com/en/latest/user_manual.html#tagging"/> and <see href="https://docs.sendgrid.com/for-developers/sending-email/categories"/>
        /// </summary>
        /// <param name="header">Header name</param>
        /// <param name="body">Body</param>
        public IEmail Header(string header, string body)
        {
            Data.Headers.Add(header, body);
            return this;
        }

        /// <summary>
        /// Sends email synchronously
        /// </summary>
        /// <returns>Instance of the Email class</returns>
        public virtual SendResponse Send()
        {
            return Sender.Send(this);
        }

        /// <summary>
        /// Sends email asynchronously
        /// </summary>
        /// <returns>Instance of the Email class</returns>
        public virtual Task<SendResponse> SendAsync(CancellationToken cancellationToken = default)
        {
            return Sender.SendAsync(this, cancellationToken);
        }

        private static string GetCultureFileName(string fileName, CultureInfo culture)
        {
            var extension = Path.GetExtension(fileName);
            var cultureExtension = $"{culture.Name}{extension}";

            var cultureFile = Path.ChangeExtension(fileName, cultureExtension);
            return File.Exists(cultureFile) ? cultureFile : fileName;
        }
    }
}