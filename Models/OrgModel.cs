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
    public class OrgModel
    {
        public Int64 organizationId { get; set; }
        public string organizationName { get; set; }
        public override string ToString()
        {
            return organizationName;
        }

    }
}