using System;
using System.Net;
using System.Net.Mail;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Microsoft.Azure; // Namespace for CloudConfigurationManager
using Microsoft.WindowsAzure.Storage; // Namespace for CloudStorageAccount
using Microsoft.WindowsAzure.Storage.Table; // Namespace for Table storage types

public static void Run(string report, TraceWriter log)
{
    log.Info($"C# Queue trigger function processed: {report}");

    CloudStorageAccount storageAccount;
    storageAccount = CloudStorageAccount.Parse(
        "DefaultEndpointsProtocol=https;AccountName=mariastorage2016;AccountKey=ydy7HO72kF10+mFpENEGQ7STg5g3VrFaymtoW9eSgYPR9lATHLC32uphetb2xb+OWZu5ljCguLUNfBOFk5vrOQ==");

    /*    CloudBlobClient blobClient;
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
    CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
    CloudTable table = tableClient.GetTableReference("reportfiles");
    TableQuery<CustomerEntity> query = new TableQuery<CustomerEntity>().
        Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "BigBossRepornOn-2016-12-01 11:00"));

    // Print the fields for each customer.
    foreach (CustomerEntity entity in table.ExecuteQuery(query))
    {
        log.Info($"{entity.PartitionKey} - {entity.RowKey} url={entity.url}, mail={entity.OperatorMail}");
    }

/*
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
    */
}
