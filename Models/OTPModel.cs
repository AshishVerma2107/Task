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
    public class OTPModel
    {
        public string MobileNo { get; set; }
        public string SMS { get; set; }
    }
}