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
using Java.Text;
using Java.Util;

namespace TaskAppWithLogin
{
    public static class Utility
    {
        public static string imageURL1 = "";
        public static string imageName1 = "";
        public static int currentPosition = -1;
        public static string report_name = "";
        public static string type = "";
        public static int groupPosition = -1;

        public static String fileName()
        {
            try
            {
                String timeStamp = new SimpleDateFormat("yyyyMMdd_HHmmss").Format(new Date());
                String name = "IMG_" + timeStamp + ".jpg";
                return name;
            }
            catch (Exception e)
            {
                return null;
            }
        }
        public static byte[] GetStreamFromFile(string filePath)
        {
            try
            {
                Android.Net.Uri uri = Android.Net.Uri.FromFile(new Java.IO.File(filePath));
                byte[] byteArray = System.IO.File.ReadAllBytes(uri.Path);
                return byteArray;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static String fileName1()
        {
            try
            {
                String timeStamp = new SimpleDateFormat("yyyyMMdd_HHmmss").Format(new Date());
                String name = "AUD_" + timeStamp + ".mp3";
                return name;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static String fileName2()
        {
            try
            {
                String timeStamp = new SimpleDateFormat("yyyyMMdd_HHmmss").Format(new Date());
                String name = "VID_" + timeStamp + ".mp4";
                return name;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }
}