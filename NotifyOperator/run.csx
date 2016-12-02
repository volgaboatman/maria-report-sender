#r "Microsoft.WindowsAzure.Storage"
#load "../Shared/tableEntities.csx"

using System;
using System.Net;
using System.Text;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using Microsoft.WindowsAzure.Storage.Queue;

public static async Task Run(string reportId, IQueryable<ReportFiles> reportBinding, CloudTable statusTable, CloudQueue waitQueue, TraceWriter log)
{
    log.Info($"NotifyOperator  trigger function processed: {reportId}");

    ReportFiles firstFile = reportBinding.Where(p => p.PartitionKey == reportId).ToList().FirstOrDefault();
    if (firstFile == null) {
        log.Error($"[NotifyOperator] Unable to find reports for '{reportId}'");
        throw new ArgumentNullException("reportId");
    } 

    foreach (var file in reportBinding.Where(p => p.PartitionKey == reportId))
    {
        log.Info($"RowKey: {file.PartitionKey} FileName: {file.RowKey} url: {file.url}");
    }

    var base64Encode = Convert.ToBase64String(reportId);
    log.Info($"ErrorUrl: https://maria-function-email.azurewebsites.net/api/status/{base64Encode}?code=ypZAlNP81P7PlapSx06CtefY6vhH0nF0VPfoZKJlD55r46TbSlofUg==");

    await UpdateStatus(reportId, statusTable, true, log);

//    waitQueue.Add(reportId);
    waitQueue.AddMessage(new CloudQueueMessage(reportId), null, new TimeSpan(0, 2, 0), null, null);
/*
public virtual void AddMessage(
    CloudQueueMessage message,
    Nullable<TimeSpan> timeToLive = null,
    Nullable<TimeSpan> initialVisibilityDelay = null,
    QueueRequestOptions options = null,
    OperationContext operationContext = null
)
*/
}

