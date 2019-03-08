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
    public class SwitchModel
    {
        public string NPID { get; set; }
        public string NPName { get; set; }
        public string DesignationId { get; set; }
        public string Mobile { get; set; }
        public string PhotoPath { get; set; }
    }
}