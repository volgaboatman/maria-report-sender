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
