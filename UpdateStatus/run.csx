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
    TableResult result = statusTable.Execute(operation);
    ReportStatus reportStatus = (ReportStatus)result.Result;

    log.Verbose($"{person.Name} is {person.Status}");

    if (reportStatus == null) {
        reportStatus = new ReportStatus(reportId, ReportStatusRowKey);
    }

    reportStatus.isOk = false;

    TableOperation insertOrReplaceOperation = TableOperation.InsertOrReplace(updateEntity);
    table.Execute(insertOrReplaceOperation);

    return req.CreateResponse(HttpStatusCode.OK);
}
