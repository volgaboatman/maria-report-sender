#r "Microsoft.WindowsAzure.Storage"
#load "../Shared/tableEntities.csx"

using System;
using System.Net;
using System.Text;
using System.Web;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage; 
using Microsoft.WindowsAzure.Storage.Table;

public static async Task<HttpResponseMessage> Run(HttpRequestMessage req, string reportIdBase64, CloudTable statusTable, TraceWriter log)
{
    log.Info($"Processing request. RequestUri='{req.RequestUri}'");
   
    var reportId = WebUtility.UrlDecode(reportIdBase64); 

    log.Info($"Updating status. ReportId='{reportId}'");

    await UpdateStatus(reportId, statusTable, false, log);

    return req.CreateResponse(HttpStatusCode.OK);
}
