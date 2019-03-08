using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using TaskAppWithLogin.Adapter;
using TaskAppWithLogin.Constants;
using TaskAppWithLogin.Models;

namespace TaskAppWithLogin
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme")]
    public class Switch_User : AppCompatActivity
    {
        ListView userlist;
        SwitchUser_Adapter switchdata;
        List<LoginModel> loginModel;
        DbHelper db;
        ISharedPreferences prefs;
        ServiceHelper restService;
        string version, geolocation="0";
        string provider_id = "", provider_name = "", email_id = "", selfie_path = "", register_mobile = "";
        bool update = false;
        Geolocation geo;
        string licenceid;
        ProgressDialog progress;
        string npid;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.switchuser_layout);
            loginModel = new List<LoginModel>();
            db = new DbHelper();
            restService = new ServiceHelper();
            geo = new Geolocation();
            prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            version = Android.App.Application.Context.ApplicationContext.PackageManager.GetPackageInfo(Android.App.Application.Context.ApplicationContext.PackageName, 0).VersionName;
            geolocation = geo.GetGeoLocation(this);
            
            licenceid = prefs.GetString("LicenceId", "");
            npid = prefs.GetString("NaturalPersonId", "");
            loginModel = db.getDetail();
            bool isRegistered = prefs.GetBoolean("IsRegistered", false);
            //if (isRegistered)
            //{
            //    Intent intent = new Intent(this, typeof(MainActivity));
            //    intent.AddFlags(ActivityFlags.NewTask);
            //    StartActivity(intent);
            //    Finish();

            //}
           
            userlist = FindViewById<ListView>(Resource.Id.listview1);

            switchdata = new SwitchUser_Adapter(this, loginModel );
            userlist.SetAdapter(switchdata);

            userlist.ItemClick += ItemSelected;

            // Create your application here
        }

        private void ItemSelected(object sender, AdapterView.ItemClickEventArgs e)
        {
            var rowData = loginModel[e.Position];
            

            ISharedPreferencesEditor editor = prefs.Edit();
            editor.PutString("OrganizationId", rowData.OrganizationId);
            editor.PutString("Organization", rowData.Organization);
            editor.PutString("OfficeId", rowData.OfficeId);
            editor.PutString("OfficeName", rowData.OfficeName);
            editor.PutString("NaturalPersonId", rowData.NaturalPersonId);
            editor.PutString("UserName", rowData.UserName);
            editor.PutString("NpToOrgRelationID", rowData.NpToOrgRelationID);
            editor.PutString("DesignationId", rowData.DesignationId);
            editor.PutString("Designation", rowData.Designation);
            editor.PutString("MobileNumber", rowData.MobileNumber);
            editor.PutString("NPPhoto", rowData.NPPhoto);
            editor.PutString("EmailAddress", rowData.EmailAddress);
            editor.PutString("LoginIdentity", rowData.LoginIdentity);

            editor.Apply();
            VerifyUser();
        }

        public async Task  VerifyUser()
        {
            progress = new Android.App.ProgressDialog(this);
            progress.Indeterminate = true;
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            progress.SetCancelable(false);
            progress.SetMessage("Please wait...");
            progress.Show();

            string username = prefs.GetString("UserName", "");
            string mobile = prefs.GetString("MobileNumber", "");
            string npid = prefs.GetString("NaturalPersonId", "");
            //userid = prefs.GetString("")
            if (username != null && username != "")
            {
                RegisterModel register_data = new RegisterModel();
                register_data.EmailID = email_id;
                register_data.selfiePath = selfie_path;
                register_data.ProvideId = provider_id;
                register_data.ProviderName = provider_name;
                register_data.MobileNumber = register_mobile;
                register_data.NPID = npid;
                register_data.Name = username;
                register_data.IsUpdate = update;
                string register_json = JsonConvert.SerializeObject(register_data);
                try
                {
                    string isRegistered = await restService.RegisterUser(this, licenceid, geolocation, version, register_json).ConfigureAwait(false);

                    progress.Dismiss();

                    if (isRegistered.Contains("Success"))
                    {
                        ISharedPreferencesEditor editor = prefs.Edit();
                        editor.PutBoolean("IsRegistered", true);
                        editor.Commit();

                        Intent intent = new Intent(this, typeof(MainActivity));
                        intent.AddFlags(ActivityFlags.NewTask);
                        StartActivity(intent);
                        Finish();


                    }
                    else
                    {
                        progress.Dismiss();
                        Toast.MakeText(this, "Try after some time", ToastLength.Short).Show();
                    }
                }
                catch (Exception ex)
                {
                    progress.Dismiss();
                    Toast.MakeText(this, "Try after some time", ToastLength.Short).Show();
                }

            }
            else
            {
                progress.Dismiss();
                Toast.MakeText(this, "Try after some time", ToastLength.Short).Show();
            }
        }
    }
}