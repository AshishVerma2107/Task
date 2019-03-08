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
    public class RegisterModel
    {
        public string ProviderName { get; set; }
        public string ProvideId { get; set; }
        public string MobileNumber { get; set; }
        public string EmailID { get; set; }
        public string NPID { get; set; }
        public string Name { get; set; }
        public string selfiePath { get; set; }
        public bool IsUpdate { get; set; }
    }
}