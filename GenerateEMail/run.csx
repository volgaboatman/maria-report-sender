#r "Microsoft.WindowsAzure.Storage"

using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Table; // Namespace for Table storage types

public class ReportFiles : TableEntity
{
    public string url { get; set; }
    public string OperatorMail { get; set; }
}

public static void Run(string report, IQueryable<ReportFiles> tableBinding, TraceWriter log)
{
    log.Info($"C# Queue trigger function processed: {report}");
    foreach (ReportFiles files in tableBinding)
    {
        log.Info($"RowKey: {files.RowKey}");
    }
/*
    CloudStorageAccount storageAccount;
    storageAccount = CloudStorageAccount.Parse(
        "DefaultEndpointsProtocol=https;AccountName=mariastorage2016;AccountKey=ydy7HO72kF10+mFpENEGQ7STg5g3VrFaymtoW9eSgYPR9lATHLC32uphetb2xb+OWZu5ljCguLUNfBOFk5vrOQ==");

        CloudBlobClient blobClient;
        CloudBlobContainer container;
        blobClient = storageAccount.CreateCloudBlobClient();
        container = blobClient.GetContainerReference("telemetry");
        CloudBlockBlob blockBlob2 = container.GetBlockBlobReference("myblob.txt");
    string text;
    using (var memoryStream = new MemoryStream())
    {
        blockBlob2.DownloadToStream(memoryStream);
        text = System.Text.Encoding.UTF8.GetString(memoryStream.ToArray());
    }

        */
    // Create the table client.
    /*
    CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
    CloudTable table = tableClient.GetTableReference("reportfiles");
    TableQuery<CustomerEntity> query = new TableQuery<CustomerEntity>().
        Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "BigBossRepornOn-2016-12-01 11:00"));

    // Print the fields for each customer.
    foreach (CustomerEntity entity in table.ExecuteQuery(query))
    {
        log.Info($"{entity.PartitionKey} - {entity.RowKey} url={entity.url}, mail={entity.OperatorMail}");
    }*/

    ServicePointManager.ServerCertificateValidationCallback =
        delegate (object s, X509Certificate certificate,
                X509Chain chain, SslPolicyErrors sslPolicyErrors) { return true; };


    var client = new SmtpClient("smtp-mail.outlook.com", 587)
    {
        Credentials = new NetworkCredential("maria-hack@outlook.com", "Maria123"),
        EnableSsl = true
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
    }
}
