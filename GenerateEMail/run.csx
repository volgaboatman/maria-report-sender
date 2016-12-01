using System;

public static void Run(string report, TraceWriter log)
{
    log.Info($"C# Queue trigger function processed: {report}");


    string fromEmail = "kirill@marya.ru";
    string toEmail = "volgaboatman@mail.ru";
    int smtpPort = 25; //587;

    bool smtpEnableSsl = true;
    string smtpHost = "smtp.bokov.net"; // your smtp host
    string smtpUser = "maria - hackfest@bokov.net"; // your smtp user
    string smtpPass = "puI83nd38"; // your smtp password
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
        return req.CreateResponse(HttpStatusCode.InternalServerError, new
        {
            status = false,
            message = "Message has not been sent. Check Azure Function Logs for more information."
        });
    }
}
