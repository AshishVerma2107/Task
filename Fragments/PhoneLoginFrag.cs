using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using TaskAppWithLogin.Constants;
using TaskAppWithLogin.Models;

namespace TaskAppWithLogin.Fragments
{
    public class PhoneLoginFrag : Fragment
    {
        TextView method;
        EditText phonenum;
        Button verify, resend;
        string licenceId = "";
        string number, verify_code, service_response, licenceid, geolocation;
        ISharedPreferences prefs;
        InternetConnection ic = new InternetConnection();
        ServiceHelper restService = new ServiceHelper();
        //Geolocation geo = new Geolocation();
        string mobile_num = "";
        string lice_id;
        string location_geo;
        //string version;
        Geolocation geo = new Geolocation();
        string otp, username;
        string version;
        Android.App.ProgressDialog progress;

        public override void OnCreate(Bundle savedInstanceState)
        {
            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());
            StrictMode.ThreadPolicy.Builder builder1 = new StrictMode.ThreadPolicy.Builder().PermitAll();
            StrictMode.SetThreadPolicy(builder1.Build());
            version = Android.App.Application.Context.ApplicationContext.PackageManager.GetPackageInfo(Android.App.Application.Context.ApplicationContext.PackageName, 0).VersionName;
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.public_login, null);
            method = view.FindViewById<TextView>(Resource.Id.methodforlogin);
            phonenum = view.FindViewById<EditText>(Resource.Id.phone_p);
            verify = view.FindViewById<Button>(Resource.Id.verify_p);
            geolocation = geo.GetGeoLocation(Activity);
            version = Android.App.Application.Context.ApplicationContext.PackageManager.GetPackageInfo(Android.App.Application.Context.ApplicationContext.PackageName, 0).VersionName;
            method.Click += delegate
              {
                  FragmentManager.BeginTransaction().Replace(Resource.Id.container_mainlogin, new LoginFrag()).Commit();
              };

            prefs = PreferenceManager.GetDefaultSharedPreferences(Activity);
            string status = prefs.GetString("register_status", "");
            if (status.Contains("yes"))
            {
                //Intent intent = new Intent(Activity, typeof(DetailActivity));
                //StartActivity(intent);
                //Activity.Finish();
            }
            main_method();

            //location_geo = geo.GetGeoLocation(this);
            lice_id = licenceId.ToString();
            verify.Click += delegate
            {
                if (phonenum.Text == null || phonenum.Text.Length < 10)
                {
                    Toast.MakeText(Activity, "Please enter the phone number correctly", ToastLength.Short).Show();
                }
                else
                {

                    number = phonenum.Text;
                    
                    Send_Number();
                }
            };
            return view;
        }
        public async void main_method()
        {


            try
            {
                licenceid = prefs.GetString("LicenceId", "");

                if (licenceid != null && licenceid != "")
                {

                }
                else
                {
                    await Get_Licence_Id();
                }
            }
            catch (Exception ex)
            {

            }

        }

        public async Task<string> Get_Licence_Id()
        {

            Boolean connectivity = ic.connectivity();

            if (connectivity)
            {
                // geolocation = geo.GetGeoLocation(ApplicationContext);
                dynamic value = new ExpandoObject();
                value.gcmid = "1";
                string json = JsonConvert.SerializeObject(value);
                // geolocation = geo.GetGeoLocation(ApplicationContext);
                try
                {
                    licenceId = await restService.GetLicenceId(Activity, geolocation, version, json).ConfigureAwait(false);
                }
                catch (Exception ex)
                {

                }

                ISharedPreferencesEditor editor = prefs.Edit();
                editor.PutString("LicenceId", licenceId);
                editor.Commit();
            }
            else
            {
                Toast.MakeText(Activity, "No Internet, Try after sometime", ToastLength.Short).Show();

            }

            return licenceId;
        }

        public void verification()
        {
            View view = LayoutInflater.Inflate(Resource.Layout.verify_layout, null);
            AlertDialog builder = new AlertDialog.Builder(Activity).Create();
            builder.SetView(view);
            builder.SetCanceledOnTouchOutside(false);
            EditText code = view.FindViewById<EditText>(Resource.Id.code_p);
            resend = view.FindViewById<Button>(Resource.Id.resend_p);
            Button submit = view.FindViewById<Button>(Resource.Id.submit_p);
            resend.Click += delegate
            {
                Send_Number();
            };

            submit.Click += delegate
            {
                verify_code = code.Text;
                submit_Code();
                builder.Dismiss();
                //Toast.MakeText(this, "Alert dialog dismissed!", ToastLength.Short).Show();
            };
            builder.Show();
        }

        public void submit_Code()
        {
            //verify_code = ("" + verify_code);
            if (otp == verify_code)
            {

                Toast.MakeText(Activity, "Code is Verified", ToastLength.Short).Show();
                verify.Enabled = false;

                ISharedPreferencesEditor editor = prefs.Edit();
                editor.PutString("username1", username);
                editor.PutString("mobile", number);
                editor.Commit();
                //Intent intent = new Intent(Activity, typeof(DetailActivity));
                //StartActivity(intent);
                //Activity.Finish();
                //sendToServiceAsync();


            }
            else
            {
                Toast.MakeText(Activity, "Code is Incorrect", ToastLength.Short).Show();


            }
        }

        //public async Task sendToServiceAsync()
        //{
        //    try
        //    {
        //        string l_id = prefs.GetString("LicenceId", "");
        //        dynamic value = new ExpandoObject();
        //        //  value.GeoLocation = loc;
        //        string json = JsonConvert.SerializeObject(value);


        //        string item = await restService.RegisterUser(l_id, number, username, location_geo,
        //                    "0", "0").ConfigureAwait(false);

        //        if (item.Contains("Success"))
        //        {
        //            ISharedPreferencesEditor editor = prefs.Edit();
        //            editor.PutString("register_status", "yes");
        //            editor.PutString("username1", username);
        //            editor.PutString("mobile", number);
        //            editor.Commit();

        //            Intent intent = new Intent(Activity, typeof(DetailActivity));

        //            //intent.PutExtra("username", username);
        //            StartActivity(intent);
        //            Activity.Finish();

        //            //Intent intent = new Intent(this, typeof(MainActivity));
        //            //// intent.AddFlags(ActivityFlags.NewTask);
        //            //StartActivity(intent);
        //            ////   Finish();
        //        }
        //        else
        //        {
        //            //  Toast.MakeText(Activity, "Oops! Something went wrong, Please try later. ", ToastLength.Long).Show();
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //}
        public void AlertBox()
        {
            Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(Activity);
            Android.App.AlertDialog alert = dialog.Create();
            alert.SetTitle("Alert");
            alert.SetMessage("Phone Number is not registered");
            alert.SetButton("OK", (c, ev) =>
            {
                alert.Dismiss(); 
            });
            alert.Show();
        }
        public async Task Send_Number()
        {
            if (ic.connectivity())
            {
                progress = new Android.App.ProgressDialog(Activity);
                progress.Indeterminate = true;
                progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
                progress.SetCancelable(false);
                progress.SetMessage("Please wait...");
                progress.Show();

                OTPModel otp_data = new OTPModel();
                otp_data.MobileNo = number;
                otp_data.SMS = "OTP";
                string otp_json = JsonConvert.SerializeObject(otp_data);
                try
                {
                    string item = await restService.GetOtp(Activity, licenceid, geolocation, version, otp_json);
                    if(item.Contains("Authentication Failed"))
                    {
                        progress.Dismiss();
                        AlertBox();
                        
                    }
                    else
                    {
                        var primeArray = item.Split(',');
                        otp = primeArray[0];
                        otp = otp.Substring(1);
                        username = primeArray[1];
                        progress.Dismiss();
                        verification();
                    }
                }
                catch (Exception e)
                {
                    progress.Dismiss();
                }

            }
            else
            {
                progress.Dismiss();
            }
        }
    }
}