using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TaskAppWithLogin.Models
{
  public  class CommunicationModel
    {
        public string designation { get; set; }
        public string taskId { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public string fax { get; set; }
        public string skypeid { get; set; }
        public string role { get; set; }
        public string mobileCondition { get; set; }
        public string emailCondition { get; set; }
        public string faxCondition { get; set; }
        public string skypeidCondition { get; set; }
        

    }
}