//using CMS;
//using CMS.DataEngine;
//using CMS.DocumentEngine;
//using CMS.Membership;
//using System;

//[assembly: RegisterModule(typeof(Kadena.Old_App_Code.EventHandlers.ProductCreationHandler))]

//namespace Kadena.Old_App_Code.EventHandlers
//{
//  public class ProductCreationHandler : Module
//  {
//    public ProductCreationHandler() : base("ProductCreationHandler") { }

//    protected override void OnInit()
//    {
//      base.OnInit();
//      DocumentEvents.Insert.Before += Insert_Before;

//    }

//    private void Insert_Before(object sender, DocumentEventArgs e)
//    {
//      //e.Cancel();

//      //throw new Exception("This user test test test.");

//    }
//  }
//}