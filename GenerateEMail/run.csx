#r "Microsoft.WindowsAzure.Storage"
#load "..\Shared\tableEntities.csx"

using System;
using System.Net;

public static void Run(string report, IQueryable<ReportFiles> reportBinding, ICollector<ReportStatus> statusBinding, TraceWriter log)
{
    log.Info($"C# Queue trigger function processed: {report}");
    foreach (ReportFiles files in tableBinding)
    {
        log.Info($"RowKey: {files.RowKey} FileName: ");
    }

    /*
    // Send Email
    var client = new SmtpClient("smtp-mail.outlook.com", 587)
    {
        EnableSsl = true,
        DeliveryMethod = SmtpDeliveryMethod.Network,
        UseDefaultCredentials = false,
        Credentials = new NetworkCredential("maria-msft-hack@outlook.com", "MariaHack"),
    };

    string fromEmail = "kirill@marya.ru";
    string toEmail = "volgaboatman@mail.ru";
    MailMessage mail = new MailMessage(fromEmail, toEmail)
    {
        Subject = "Test",
        Body = "Report",
    };

    try
    {
        client.Send(mail);
        log.Verbose("Email sent.");
    }
    catch (Exception ex)
    {
        log.Verbose(ex.ToString());
    }*/
}
