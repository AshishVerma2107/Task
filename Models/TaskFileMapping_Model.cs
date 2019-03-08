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
  public  class TaskFileMapping_Model
    {
        [PrimaryKey, AutoIncrement]
        public Int64 id { get; set; }
        public string taskId { get; set; }
        public string localtaskId { get; set; }
        public string designationid { get; set; }
        public string fileId { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string Path { get; set; }
        public string localPath { get; set; }
        public int Checked { get; set; }
    }
}