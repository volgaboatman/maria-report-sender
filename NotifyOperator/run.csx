#r "Microsoft.WindowsAzure.Storage"
#load "../Shared/tableEntities.csx"

using System;
using System.Net;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

public static void Run(string reportId, IQueryable<ReportFiles> reportBinding, ICollector<ReportStatus> statusBinding, ICollector<string> waitQueue, TraceWriter log)
{
    log.Info($"NotifyOperator  trigger function processed: {report}");

    ReportFiles file = reportBinding.Where(p => p.PartitionKey == reportId).ToList().SingleOrDefault();
    if (file == null) {
        log.Error($"[NotifyOperator] Unable to find reports for '{reportId}'");
        throw new ArgumentNullException("reportId");
    } 

    foreach (ReportFiles files in tableBinding.Where(p => p.PartitionKey == report))
    {
        log.Info($"RowKey: {files.Partition} FileName: {files.RowKey} url: {files.url}");
    }

    log.Info($"ErrorUrl: http://somthing.goes.wrong/");

    statusBinding.Add(new ReportStatus { PartitionKey = report, RowKey="status", isOk=True});

    waitQueue.Add(report, null, new TimeSpan(0, 1, 0), null, null);
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

