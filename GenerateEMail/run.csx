#r "Microsoft.WindowsAzure.Storage"
#load "../Shared/tableEntities.csx"

using System;
using System.Net;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

public static void Run(string reportId, IQueryable<ReportFiles> reportBinding, IQueryable<ReportStatus> statusBinding, TraceWriter log)
{
    log.Info($"Processing '{reportId}'");

    reportId = "07:24:14.5321439";
    ReportStatus status = statusBinding.Where(p => p.PartitionKey == reportId).SingleOrDefault();
    if (status == null) {
        log.Error($"Unable to find status for '{reportId}'");
        throw new ArgumentNullException("reportId");
    } 

    log.Info($"Status for report '{reportId}': {status.isOk}");
    if (!status.isOk) {
        return;
    }

    List<ReportFiles> reportFiles = reportBinding.Where(p => p.PartitionKey == reportId).ToList();
    List<string> attachments = new List<string>();
    foreach (var url in reportFiles.Select(r => r.url).ToList()) {
        string filename = Path.GetFileName(new Uri(url).AbsolutePath);
        string filepath = Path.Combine(Path.GetTempPath(), filename);

        using (var client = new WebClient()) {
            client.DownloadFile(url, filepath);
        }
        attachments.Add(filepath);
    }

    log.Info(String.Join(", ", attachments.ToArray()));

    // send email

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
