using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Android.Content;

using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Text.Format;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace TaskAppWithLogin
{
    public class TimePick : DialogFragment, Android.App.TimePickerDialog.IOnTimeSetListener
    {
        public static readonly string TAG = "MyTimePickerFragment";
        Action<DateTime> timeSelectedHandler = delegate { };

        public static TimePick NewInstance(Action<DateTime> onTimeSelected)
        {
            TimePick frag = new TimePick();
            frag.timeSelectedHandler = onTimeSelected;
            return frag;
        }

        public override Android.App.Dialog OnCreateDialog(Bundle savedInstanceState)
        {
            DateTime currentTime = DateTime.Now;
            //bool is24HourFormat =
            //dateTimePicker1.CustomFormat = "HH:mm tt";
            Android.App.TimePickerDialog dialog = new Android.App.TimePickerDialog
                (Activity, this, currentTime.Hour, currentTime.Minute, false);

            return dialog;
        }

        public void OnTimeSet(TimePicker view, int hourOfDay, int minute)
        {
            DateTime currentTime = DateTime.Now;
            DateTime selectedTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, hourOfDay, minute, 0);
            Log.Debug(TAG, selectedTime.ToLongTimeString());
            timeSelectedHandler(selectedTime);
        }

    }
}