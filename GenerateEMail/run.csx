using System;
using System.Net;
using System.Net.Mail;

public static void Run(string report, TraceWriter log)
{
    log.Info($"C# Queue trigger function processed: {report}");

    ServicePointManager.ServerCertificateValidationCallback =
    delegate (object s, X509Certificate certificate,
             X509Chain chain, SslPolicyErrors sslPolicyErrors)
    { return true; };

    string fromEmail = "kirill@marya.ru";
    string toEmail = "volgaboatman@mail.ru";
    int smtpPort = 25; // 587

    bool smtpEnableSsl = true;
    string smtpHost = "smtp.bokov.net"; // your smtp host
    string smtpUser = "bokov7"; // your smtp user
    string smtpPass = "did9e0ev7sf7s"; // your smtp password
    string subject = "First email";
    string message = report;

    MailMessage mail = new MailMessage(fromEmail, toEmail);
    SmtpClient client = new SmtpClient();
    client.Port = smtpPort;
    client.EnableSsl = smtpEnableSsl;
    client.DeliveryMethod = SmtpDeliveryMethod.Network;
    client.UseDefaultCredentials = false;
    client.Host = smtpHost;
    client.Credentials = new System.Net.NetworkCredential(smtpUser, smtpPass);
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
