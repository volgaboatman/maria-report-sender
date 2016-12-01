using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

public static void Run(string report, TraceWriter log)
{
    log.Info($"C# Queue trigger function processed: {report}");

    ServicePointManager.ServerCertificateValidationCallback =
    delegate (object s, X509Certificate certificate,
             X509Chain chain, SslPolicyErrors sslPolicyErrors)
    { return true; };

    string fromEmail = "kirill@marya.ru";
    string toEmail = "volgaboatman@mail.ru";

    string subject = "First email";
    string message = report;

    MailMessage mail = new MailMessage(fromEmail, toEmail);

    var client = new SmtpClient("smtp-mail.outlook.com", 587)
    {
        Credentials = new NetworkCredential("maria-hack@outlook.com", "Maria123"),
        EnableSsl = true
    };
    mail.Subject = message;
    mail.Body = message;

    try
    {
        client.Send(mail);
        log.Verbose("Email sent.");
    }
    catch (Exception ex)
    {
        log.Verbose(ex.ToString());
    }
}
