#r "Microsoft.WindowsAzure.Storage"
#load "../Shared/tableEntities.csx"

using System;
using System.Net;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

public static void Run(string reportId, IQueryable<ReportFiles> reportBinding, IQueryable<ReportStatus> statusBinding, TraceWriter log)
{
    log.Info($"[GenerateEmail] Processing '{reportId}'");

    ReportStatus status = statusBinding.Where(p => p.PartitionKey == reportId).ToList().SingleOrDefault();
    if (status == null) {
        log.Error($"[GenerateEmail] Unable to find status for '{reportId}'");
        throw new ArgumentNullException("reportId");
    } 

    log.Info($"RowKey: {status.RowKey} FileName: ");

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
