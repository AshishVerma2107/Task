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
    public class CreateTaskModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string taskname { get; set; }
        public string taskdescrip { get; set; }
        public string deadline { get; set; }
        public string through { get; set; }
        public string markto { get; set; }
        public string status { get; set; }
        
        [Ignore]
        public List<TaskFileMapping_Model> lstTaskFileMapping { get; set;}
        [Ignore]
        public List<ComplianceJoinTable> compModelattach { get; set; }
    }
}