#r "Microsoft.WindowsAzure.Storage"
#load "../Shared/tableEntities.csx"

using System;
using System.Net;
using System.Web;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;


public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, IQueryable<ReportStatus> statusBinding, TraceWriter log)
{
    log.Info($"Processing request. RequestUri='{req.RequestUri}'");

    return req.CreateResponse(HttpStatusCode.OK);
}
