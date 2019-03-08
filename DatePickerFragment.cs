using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace TaskAppWithLogin
{
    public class DatePickerFragment : DialogFragment,
    Android.App.DatePickerDialog.IOnDateSetListener
    {
        // TAG can be any string of your choice.  
        public static readonly string TAG = "X:" + typeof(DatePickerFragment).Name.ToUpper();
        // Initialize this value to prevent NullReferenceExceptions.  
        Action<DateTime> _dateSelectedHandler = delegate { };
        public static DatePickerFragment NewInstance(Action<DateTime> onDateSelected)
        {
            DatePickerFragment frag = new DatePickerFragment();
            frag._dateSelectedHandler = onDateSelected;
            return frag;
        }
        public override Android.App.Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            DateTime currently = DateTime.Now;
            Android.App.DatePickerDialog dialog = new Android.App.DatePickerDialog(Activity, this, currently.Year, currently.Month, currently.Day);
            //dialog.DatePicker.MinDate = Java.Lang.JavaSystem.CurrentTimeMillis();
            return dialog;
        }
        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            // Note: monthOfYear is a value between 0 and 11, not 1 and 12!  
            DateTime selectedDate = new DateTime(year, monthOfYear + 1, dayOfMonth);
            Log.Debug(TAG, selectedDate.ToLongDateString());
            _dateSelectedHandler(selectedDate);
        }

        //private void OnDateSet(object sender, DatePickerDialog.DateSetEventArgs e)
        //{
        //    invoicedate.Text = e.Date.ToString("dd-MMM-yyyy");
        //    date = e.Date.ToString("yyyy-MM-dd");

        //}
    }
}