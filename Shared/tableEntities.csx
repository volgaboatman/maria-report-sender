#r "Microsoft.WindowsAzure.Storage"

using Microsoft.WindowsAzure.Storage; 
using Microsoft.WindowsAzure.Storage.Table;

public class ReportFiles : TableEntity
{
    public string url { get; set; }
    public string OperatorMail { get; set; }
}

public class ReportStatus : TableEntity
{
    public bool isOk { get; set; }
}

public static void UpdateStatus(string reportId, CloudTable statusTable, bool status, TraceWriter log)
{
    log.Info($"Update status {reportIt} to {status}");

    TableOperation operation = TableOperation.Retrieve<ReportStatus>(reportId, ReportStatusRowKey);
    TableResult result = await statusTable.ExecuteAsync(operation);
    ReportStatus reportStatus = (ReportStatus)result.Result;

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
