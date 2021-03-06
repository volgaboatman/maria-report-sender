#r "Microsoft.WindowsAzure.Storage"

using System.Text;
using Microsoft.WindowsAzure.Storage; 
using Microsoft.WindowsAzure.Storage.Table;

public static string EncodeReportId(string reportId)
{
    var sb = new StringBuilder();

    var bytes = Encoding.Unicode.GetBytes(reportId);
    foreach (var t in bytes)
    {
        sb.Append(t.ToString("X2"));
    }

    return sb.ToString(); 
}

public static string DecodeReportId(string reportId)
{
    var bytes = new byte[reportId.Length / 2];
    for (var i = 0; i < bytes.Length; i++)
    {
        bytes[i] = Convert.ToByte(reportId.Substring(i * 2, 2), 16);
    }

    return Encoding.Unicode.GetString(bytes); 
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
