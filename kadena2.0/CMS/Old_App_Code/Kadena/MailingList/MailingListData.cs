using System;

namespace Kadena.Old_App_Code.Kadena.MailingList
{
  public class MailingListData
  {
    public string id { get; set; }
    public string customerName { get; set; }
    public string name { get; set; }
    public DateTime createDate { get; set; }
    public DateTime updateDate { get; set; }
    public DateTime validTo { get; set; }
    public object state { get; set; }
    public string mailType { get; set; }
    public string productType { get; set; }
    public int version { get; set; }
    public int addressCount { get; set; }
    public int errorCount { get; set; }
  }
}