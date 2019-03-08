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
using SQLite;

namespace TaskAppWithLogin.Models
{
 public class ComplianceLatlngPath
    {
        //[PrimaryKey, AutoIncrement]
        //public Int64 Id { get; set; }
        public string lat { get; set; }
        public string lon { get; set; }
    }
}