#r "Microsoft.WindowsAzure.Storage"

using System.Net;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Threading.Tasks;
using Microsoft.Azure;

public static string Run(HttpRequestMessage req, TraceWriter log, out string outputSbQueue, ICollector<Report> tableBinding)
{
         CloudStorageAccount storageAccount;
         CloudBlobClient blobClient;
         CloudBlobContainer container;
         storageAccount = CloudStorageAccount.Parse(
            "DefaultEndpointsProtocol=https;AccountName=mariastorage2016;AccountKey=ydy7HO72kF10+mFpENEGQ7STg5g3VrFaymtoW9eSgYPR9lATHLC32uphetb2xb+OWZu5ljCguLUNfBOFk5vrOQ==");
            blobClient = storageAccount.CreateCloudBlobClient();
            container = blobClient.GetContainerReference("telemetry");
            container.CreateIfNotExists();
        string partitionKey = string.Format("{0:d.M.yyyy HH:mm:ss}", DateTime.Now);


         log.Info("C# HTTP trigger function processed a request.");

  string typeper = req.GetQueryNameValuePairs()
        .FirstOrDefault(q => string.Compare(q.Key, "param_type_per", true) == 0)
        .Value;

    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create( "http://52.169.105.27/reportserver/?/TestReport/ReportTest&rs:Format=PDF&type_per="+typeper);
        
    request.Method = "GET";
    request.UseDefaultCredentials = false;
    request.PreAuthenticate = true;
    request.Credentials = new NetworkCredential("kirill", "AmSNAt5cPk", "dom" );

    HttpWebResponse response = (HttpWebResponse)request.GetResponse(); 
    log.Info( response.ContentType.ToString());
    Stream stream = response.GetResponseStream();
    string fileName = response.Headers["Content-Disposition"].Replace("attachment; filename=", String.Empty).Replace("\"", String.Empty);
    CloudAppendBlob blob = container.GetAppendBlobReference(fileName);

    blob.UploadFromStream(stream, null);
 //blob.Uri.ToString()
    log.Info($"Adding entity");
            tableBinding.Add(
             new Report() { 
                    PartitionKey = partitionKey, 
                    RowKey = fileName, 
                    url = GetBlobSasUri(container, fileName),
                    OperatorMail = "kirill@marya.ru" }
                );
    outputSbQueue = partitionKey;
    return GetBlobSasUri(container, fileName);
}


private static string GetBlobSasUri(CloudBlobContainer container, string blobName, string policyName = null)
{
    string sasBlobToken;

    // Get a reference to a blob within the container.
    // Note that the blob may not exist yet, but a SAS can still be created for it.
    CloudBlockBlob blob = container.GetBlockBlobReference(blobName);

    if (policyName == null)
    {
        // Create a new access policy and define its constraints.
        // Note that the SharedAccessBlobPolicy class is used both to define the parameters of an ad-hoc SAS, and 
        // to construct a shared access policy that is saved to the container's shared access policies. 
        SharedAccessBlobPolicy adHocSAS = new SharedAccessBlobPolicy()
        {
            // When the start time for the SAS is omitted, the start time is assumed to be the time when the storage service receives the request. 
            // Omitting the start time for a SAS that is effective immediately helps to avoid clock skew.
            SharedAccessExpiryTime = DateTime.UtcNow.AddHours(24),
            Permissions = SharedAccessBlobPermissions.Read | SharedAccessBlobPermissions.Write | SharedAccessBlobPermissions.Create
        };

        // Generate the shared access signature on the blob, setting the constraints directly on the signature.
        sasBlobToken = blob.GetSharedAccessSignature(adHocSAS);

        Console.WriteLine("SAS for blob (ad hoc): {0}", sasBlobToken);
        Console.WriteLine();
    }
    else
    {
        // Generate the shared access signature on the blob. In this case, all of the constraints for the
        // shared access signature are specified on the container's stored access policy.
        sasBlobToken = blob.GetSharedAccessSignature(null, policyName);

        Console.WriteLine("SAS for blob (stored access policy): {0}", sasBlobToken);
        Console.WriteLine();
    }

    // Return the URI string for the container, including the SAS token.
    return blob.Uri + sasBlobToken;
}


public class Report
{
    public string PartitionKey { get; set; }
    public string RowKey { get; set; }
    public string url { get; set; }
        public string OperatorMail { get; set; }
}