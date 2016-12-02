#r "Microsoft.WindowsAzure.Storage"
#r "Microsoft.IdentityModel.Tokens";

using Microsoft.WindowsAzure.Storage; 
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.IdentityModel.Tokens;

public static string EncodeReportId(string reportId)
{
  return Base64UrlEncoder.Encode(reportId);
}

public static string DecodeReportId(string reportId)
{
  return Base64UrlEncoder.Decode(reportId);
}


public class ReportFiles : TableEntity
{
    public string url { get; set; }
    public string OperatorMail { get; set; }
}

public class ReportStatus : TableEntity
{
    public bool isOk { get; set; }
}

public const string ReportStatusRowKey = "Status";

public static async Task UpdateStatus(string reportId, CloudTable statusTable, bool status, TraceWriter log)
{
    log.Info($"Update status {reportId} to {status}");

    var operation = TableOperation.Retrieve<ReportStatus>(reportId, ReportStatusRowKey);
    var result = await statusTable.ExecuteAsync(operation);
    var reportStatus = (ReportStatus)result.Result;

    if (reportStatus == null) {
        reportStatus = new ReportStatus() { 
            PartitionKey = reportId, 
            RowKey = ReportStatusRowKey
        };
    }

    reportStatus.isOk = status;

    TableOperation insertOrReplaceOperation = TableOperation.InsertOrReplace(reportStatus);
    await statusTable.ExecuteAsync(insertOrReplaceOperation);
}
