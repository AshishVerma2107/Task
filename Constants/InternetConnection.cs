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
using Plugin.Connectivity;

namespace TaskAppWithLogin.Constants
{
    class InternetConnection
    {
        public Boolean connectivity()
        {

            if (CrossConnectivity.Current.IsConnected)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}