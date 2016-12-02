#r "Microsoft.WindowsAzure.Storage"
#load "../Shared/tableEntities.csx"

using System;
using System.Net;
using System.Web;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

public const string ReportStatusRowKey = "Status";

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, string reportId, CloudTable statusTable, TraceWriter log)
{
    log.Info($"Processing request. RequestUri='{req.RequestUri}'");

    TableOperation operation = TableOperation.Retrieve<ReportStatus>(reportId, ReportStatusRowKey);
    TableResult result = await statusTable.ExecuteAsync(operation);
    ReportStatus reportStatus = (ReportStatus)result.Result;

    if (reportStatus == null) {
        reportStatus = new ReportStatus(reportId, ReportStatusRowKey);
    }

    reportStatus.isOk = false;

    TableOperation insertOrReplaceOperation = TableOperation.InsertOrReplace(updateEntity);
    await table.ExecuteAsync(insertOrReplaceOperation);

    return req.CreateResponse(HttpStatusCode.OK);
}
