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
 public   class Shapes
    {
        [PrimaryKey, AutoIncrement]
        public Int64 id { get; set; }
        [Ignore]
        public List<ComplianceShapes> shapes { get; set; }
        public string task_id { get; set; }
    }
}