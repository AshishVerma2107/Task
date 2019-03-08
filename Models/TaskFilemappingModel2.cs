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
  public  class TaskFilemappingModel2
    {
        [PrimaryKey, AutoIncrement]
        public Int64 id { get; set; }
        public string taskId { get; set; }
        public string fileId { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        public string FileSize { get; set; }
        public string Path { get; set; }
        public int Checked { get; set; }
    }
}