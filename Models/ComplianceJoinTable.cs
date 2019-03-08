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
    public class ComplianceJoinTable
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string task_id { get; set; }
        public string file_type { get; set; }
        public string file_format { get; set; }
        public int max_numbers { get; set; }
        public string task_overview { get; set; }
        public string complianceType { get; set; }
        public string Uploaded { get; set; }
        
    }
}