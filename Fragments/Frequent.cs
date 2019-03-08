using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Json;
using System.Linq;

using System.Text;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using TaskAppWithLogin.Adapter;
using TaskAppWithLogin.Constants;
using TaskAppWithLogin.Models;

namespace TaskAppWithLogin.Fragments
{
    public class Frequent : Fragment
    {
        Context context;
        public Frequent(Context context)
        {
            this.context = context;
        }
        GridView gv;
        InternetConnection con;
        FrequentAdapter adapter;
        List<FrequentList> list;
        public static FrequentList detail;
        List<FrequentList> im_model;
        List<FrequentList> freq;
        ServiceHelper restService = new ServiceHelper();
        DbHelper db;
        ISharedPreferences prefs;
        string desig_Id, geolocation;
        Geolocation geo;

        Android.App.ProgressDialog progress;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            list = new List<FrequentList>();
            HasOptionsMenu = true;

        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View rootView = inflater.Inflate(Resource.Layout.frequent, container, false);

            gv = rootView.FindViewById<GridView>(Resource.Id.gv);
            prefs = PreferenceManager.GetDefaultSharedPreferences(Activity);
            desig_Id = prefs.GetString("DesignationId", "");
            geo = new Geolocation();
            db = new DbHelper();
            con = new InternetConnection();
            geolocation = geo.GetGeoLocation(Activity);

            Boolean connectivity = con.connectivity();
            if (connectivity)
            {
                getData();
            }
            else
            {
                freq = db.GetFrequentList();
                if (freq.Count > 0)
                {
                    adapter = new FrequentAdapter(Activity, freq);
                    
                }
                else
                {
                    Toast.MakeText(Activity, "Couldn't find data for frequents. Please connect to the internet", ToastLength.Long).Show();
                }
                gv.Adapter = adapter;
            }

            return rootView;
        }


        public async Task getData()
        {
            progress = new Android.App.ProgressDialog(Activity);
            progress.Indeterminate = true;
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            progress.SetCancelable(false);
            progress.SetMessage("Please wait...");
            progress.Show();

            dynamic value = new ExpandoObject();
            value.DesignationId = desig_Id;

            string json = JsonConvert.SerializeObject(value);
            try
            {
                JsonValue item = await restService.FrequentList(Activity, json, geolocation).ConfigureAwait(false);
                freq = JsonConvert.DeserializeObject<List<FrequentList>>(item);

                db.InsertFrequentList(freq);
                if (freq.Count > 0)
                {
                    adapter = new FrequentAdapter(Activity, freq);
                }
                gv.Adapter = adapter;
                progress.Dismiss();
            }
            catch (Exception ex)
            {
                progress.Dismiss();
            }
            progress.Dismiss();
        }
        public override void OnResume()
        {
            base.OnResume();
        }
    }
}
