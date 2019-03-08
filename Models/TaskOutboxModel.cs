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
    public class TaskOutboxModel
    {
        [PrimaryKey, AutoIncrement]
        public Int64 Id { get; set; }
        public string Task_id { get; set; }
        public string TaskPercentage { get; set; }
        public string Task_name { get; set; }
        public string Description { get; set; }
        public string deadline_date { get; set; }
        public string mark_to { get; set; }
        public string task_status { get; set; }
        public string Task_created_by { get; set; }
        public string Task_creation_date { get; set; }
        // public List<ListAddedComplianceModel> lstAddedCompliance { get; set; }
        public string ip { get; set; }
        public string task_mark_by { get; set; }
        public string MarkingDate { get; set; }
        public string task_marking_type { get; set; }
        //public List<ListCommunicationModel> lstCommunication { get; set; }
        //public List<ListTaskDocumentMapping> lstTaskDocumentMapping { get; set; }
    }
}