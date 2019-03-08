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
   public class FileExtension
    {
        public string Value { get;set;}
        public string Text { get; set;}
        public override string ToString()
        {
            return Text;
        }

    }
}