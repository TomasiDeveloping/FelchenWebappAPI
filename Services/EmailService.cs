﻿using System.Net;
using System.Net.Mail;
using Api.Helper;
using Microsoft.Extensions.Options;

namespace Api.Services;

public class EmailService
{
    private readonly string _host;
    private readonly string _password;
    private readonly int _port;
    private readonly string _senderAddress;
    private readonly string _userName;

    public EmailService(IOptions<EmailSettings> conf)
    {
        _host = conf.Value.Host;
        _port = conf.Value.Port;
        _userName = conf.Value.UserName;
        _password = conf.Value.Password;
        _senderAddress = conf.Value.SenderAddress;
    }

    public async Task<bool> SendEmailAsync(string receiverAddress, string message, string subject)
    {
        var smtpServer = new SmtpClient(_host, _port);
        var mail = new MailMessage();

        smtpServer.EnableSsl = false;
        smtpServer.Credentials = new NetworkCredential(_userName, _password);

        mail.From = new MailAddress(_senderAddress);
        mail.To.Add(receiverAddress);
        mail.Subject = subject;
        mail.Body = message;
        mail.IsBodyHtml = true;

        try
        {
            await smtpServer.SendMailAsync(mail);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}