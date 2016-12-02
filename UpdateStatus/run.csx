#r "Microsoft.WindowsAzure.Storage"
#load "../Shared/tableEntities.csx"

using System;
using System.Net;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;

using System.Net;
using System.Threading.Tasks;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, IQueryable<ReportStatus> statusBinding, TraceWriter log)
{
    log.Info($"Processing request. RequestUri='{req.RequestUri}'");

    req.CreateResponse(HttpStatusCode.OK);
}
