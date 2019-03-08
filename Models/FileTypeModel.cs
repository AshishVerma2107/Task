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
  public  class FileTypeModel
    {
        public string Document_Category_id {get; set;}
        public string Document_Type_id { get; set;}
        public string Document_Type_name { get; set;}
        public string creation_date { get; set;}
        public string created_by { get; set;}
        public string created_by_ip { get; set; }
        public string Document_Type_code { get; set; }
        //public void doc_type( string docname)
        //{
        //    this.Document_Type_name = docname;
        //}
        public override string ToString()
        {
            return Document_Type_name;
        }
    }
}