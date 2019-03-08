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
    public class TaskInboxModel
    {
        [PrimaryKey, AutoIncrement]
        public Int64 id { get; set; }
        public string TaskPercentage { get; set; }
        //public string id { get; set; }
        public string task_id { get; set; }
        public string task_name { get; set; }
        public string description { get; set; }
        public DateTime deadlineDate { get; set; }
        public string mark_to { get; set; }
        public string taskStatus { get; set; }
        public string task_created_by { get; set; }
        public string task_creation_date { get; set; }
        public string lstAddedCompliance { get; set; }
        public string ip { get; set; }
        public string requestfordeadline { get; set; }
        public string task_mark_by { get; set; }
        public string MarkingDate { get; set; }
        public string task_marking_type { get; set; }
        public string lstCommunication { get; set; }
       
        public string lstTaskDocumentMapping { get; set; }
        public string TaskTemplate_ID { get; set; }
        public string RowNo { get; set; }
        public string Meeting_ID { get; set; }
        public string TaskThrough { get; set; }
    }
}