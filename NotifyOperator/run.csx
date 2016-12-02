#r "Microsoft.WindowsAzure.Storage"
#load "../Shared/tableEntities.csx"

using System;
using System.Net;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

public static void Run(string reportId, IQueryable<ReportFiles> reportBinding, ICollector<ReportStatus> statusBinding, ICollector<string> waitQueue, TraceWriter log)
{
    log.Info($"NotifyOperator  trigger function processed: {reportId}");

    ReportFiles firstFile = reportBinding.Where(p => p.PartitionKey == reportId).ToList().SingleOrDefault();
    if (firstFile == null) {
        log.Error($"[NotifyOperator] Unable to find reports for '{reportId}'");
        throw new ArgumentNullException("reportId");
    } 

    foreach (var file in reportBinding.Where(p => p.PartitionKey == reportId))
    {
        log.Info($"RowKey: {file.Partition} FileName: {file.RowKey} url: {file.url}");
    }

    log.Info($"ErrorUrl: http://somthing.goes.wrong/");

    statusBinding.Add(new ReportStatus { PartitionKey = reportId, RowKey="status", isOk=true});

    waitQueue.Add(reportId, null, new TimeSpan(0, 1, 0), null, null);
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

