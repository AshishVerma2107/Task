using System;
using System.Threading.Tasks;
using Android;
using Android.App;
using Android.Content;
using Android.OS;
using System.Collections.Generic;
using Android.Content.PM;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using TaskAppWithLogin.Constants;
using TaskAppWithLogin.Fragments;
using Android.Gms.Common.Apis;
using Android.Gms.Location;
using Android.Widget;
using Android.Preferences;
using Android.Locations;
using System.Json;
using Newtonsoft.Json;
using System.Dynamic;
using TaskAppWithLogin.Models;
using Android.Util;

namespace TaskAppWithLogin
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class LoginMain_Activity : AppCompatActivity 
    {
        Context context;
        Geolocation geo;
        EditText user, pass;
        Button log, log_google, log_fb, log_twitt, log_phone;
        ISharedPreferences prefs;
        InternetConnection ic;
        string version;
        ServiceHelper restService;
        Android.App.ProgressDialog progress;
        string geolocation = "";
        DbHelper db = new DbHelper();
        string licenceid;
        string username = "", npid = "", mobile = "", userid;
        LoginModel detail;
        RegisterModel register_data;
        bool update = false;
        string provider_id = "", provider_name = "", email_id = "", selfie_path = "", register_mobile = "";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.loginFinal);

            prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            geo = new Geolocation();
            ic = new InternetConnection();
            restService = new ServiceHelper();
            version = Android.App.Application.Context.ApplicationContext.PackageManager.GetPackageInfo(Android.App.Application.Context.ApplicationContext.PackageName, 0).VersionName;

            user = FindViewById<EditText>(Resource.Id.username);
            pass = FindViewById<EditText>(Resource.Id.password);
            log = FindViewById<Button>(Resource.Id.login_snp);
            log_google = FindViewById<Button>(Resource.Id.login_google);
            log_fb = FindViewById<Button>(Resource.Id.login_fb);
            log_twitt = FindViewById<Button>(Resource.Id.login_twitter);
            log_phone = FindViewById<Button>(Resource.Id.login_phone);

            if (prefs.GetBoolean("GoogleLogin", false))
            {
                Intent intent = new Intent(this, typeof(MainActivity));
                intent.AddFlags(ActivityFlags.NewTask);
                StartActivity(intent);
                Finish();
            }

            //log_google.Click += delegate
            //{
            //    GoogleSignInClick();
            //    GetLogin();
            //};


           // ConfigureGoogleSign();

            log.Click += delegate
            {
                UserLogin();
            };
            DisplayLocationSettingsRequest();
            main_method();
            // SupportFragmentManager.BeginTransaction().Replace(Resource.Id.container_mainlogin, new LoginFrag()).Commit();
            
           
            //if (permissionmethodAsync() == true)
            //{

            //}
            //else
            //{
            //    permissionmethodAsync();
            //}
        }

        public void UserLogin()
        {
            LocationManager mlocManager = (LocationManager)GetSystemService(Context.LocationService);
            bool enabled = mlocManager.IsProviderEnabled(LocationManager.GpsProvider);
            if (enabled == false)
            {
                Toast.MakeText(this, "GPS Not Enable", ToastLength.Long).Show();
            }
            Validate();

        }

        public async void Validate()
        {

            var errorMsg = "";
            if (user.Text.Length == 0 && pass.Text.Length == 0)
            {
                if (user.Text.Length == 0 || pass.Text.Length == 0)
                {
                    errorMsg = "Please enter User Name ";


                }
                if (pass.Text.Length == 0 || pass.Text.Length == 0)
                {
                    errorMsg = errorMsg + "Please enter Password";
                }

                Toast.MakeText(this, errorMsg, ToastLength.Long).Show();
                return;
            }
            else
            {
                Boolean result = ic.connectivity();
                if (result)
                {
                    progress = new Android.App.ProgressDialog(this);
                    progress.Indeterminate = true;
                    progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
                    progress.SetCancelable(false);
                    progress.SetMessage("Please wait...");
                    progress.Show();
                    JsonValue login_value = null;
                    try
                    {
                        login_value = await nextActivity(user.Text, pass.Text);
                    }
                    catch (Exception e)
                    {

                    }
                    if (login_value != null)
                    {
                        await ParseAndDisplay(login_value, user.Text);
                    }
                    else
                    {
                        Toast.MakeText(this, "No Internet", ToastLength.Long).Show();
                    }

                    //  loginId1 = user.Text;
                    // password1 = pass.Text;
                }
                else
                {

                    Toast.MakeText(this, "No Internet", ToastLength.Long).Show();
                }

            }
        }

        public async void main_method()
        {

            await GetPermissionAsync();
            try
            {
                licenceid = prefs.GetString("LicenceId", "");

                if (licenceid != null && licenceid != "")
                {
                    bool isRegistered = prefs.GetBoolean("IsRegistered", false);
                    if (isRegistered)
                    {
                        Intent intent = new Intent(this, typeof(MainActivity));
                        intent.AddFlags(ActivityFlags.NewTask);
                        StartActivity(intent);
                        Finish();

                    }
                    else
                    {

                    }
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

        async Task ParseAndDisplay(JsonValue json, String login_Id)
        {
            ISharedPreferencesEditor editor = prefs.Edit();
            if (json != null && json != "")
            {

                List<LoginModel> lst = JsonConvert.DeserializeObject<List<LoginModel>>(json);
                for (int i = 0; i < lst.Count; i++)
                {
                    try
                    {
                        detail = new LoginModel
                        {
                            OrganizationId = lst[i].OrganizationId,
                            Organization = lst[i].Organization,
                            OfficeId = lst[i].OfficeId,
                            OfficeName = lst[i].OfficeName,
                            NaturalPersonId = lst[i].NaturalPersonId,
                            UserName = lst[i].UserName,
                            NpToOrgRelationID = lst[i].NpToOrgRelationID,
                            DesignationId = lst[i].DesignationId,
                            NPPhoto = lst[i].NPPhoto,
                            Designation = lst[i].Designation,
                            MobileNumber = lst[i].MobileNumber,
                            Message = lst[i].Message,
                            ProjectArea = lst[i].ProjectArea,
                            Controller = lst[i].Controller,
                            ControllerAction = lst[i].ControllerAction,
                            IsActive = lst[i].IsActive.ToString(),
                            EmailAddress = lst[i].EmailAddress
                        };


                        db.insertIntoTable(detail);

                        //User_List.Add(detail);
                    }
                    catch (Exception e)
                    {
                        Log.Error("Error", e.Message);
                    }
                }


                if (lst.Count == 1)
                {
                    for (int i = 0; i < lst.Count; i++)
                    {
                        editor.PutString("OrganizationId", lst[0].OrganizationId);
                        editor.PutString("Organization", lst[0].Organization);
                        editor.PutString("OfficeId", lst[0].OfficeId);
                        editor.PutString("OfficeName", lst[0].OfficeName);
                        editor.PutString("NaturalPersonId", lst[0].NaturalPersonId);
                        editor.PutString("UserName", lst[0].UserName);
                        editor.PutString("NpToOrgRelationID", lst[0].NpToOrgRelationID);
                        editor.PutString("DesignationId", lst[0].DesignationId);
                        editor.PutString("Designation", lst[0].Designation);
                        editor.PutString("MobileNumber", lst[0].MobileNumber);
                        editor.PutString("NPPhoto", lst[0].NPPhoto);
                        editor.PutString("EmailAddress", lst[0].EmailAddress);
                        editor.PutString("LoginIdentity", lst[0].LoginIdentity);

                        editor.Apply();
                    }



                    username = prefs.GetString("UserName", "");
                    mobile = prefs.GetString("MobileNumber", "");
                    npid = prefs.GetString("NaturalPersonId", "");
                    //userid = prefs.GetString("")
                    if (username != null && username != "")
                    {

                        register_data = new RegisterModel();
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
                        Toast.MakeText(this, "Invalid User name or Password", ToastLength.Short).Show();
                    }
                }
                else if(lst.Count>1)
                {
                    //editor.PutString("NaturalPersonId", lst[0].NaturalPersonId);
                    Intent intent = new Intent(this, typeof(Switch_User));
                    intent.AddFlags(ActivityFlags.NewTask);

                    StartActivity(intent);
                    Finish();
                }
                else
                {
                    progress.Dismiss();
                    Toast.MakeText(this, "Invalid User name or Password", ToastLength.Short).Show();
                }
            }
            else
            {
                progress.Dismiss();
                Toast.MakeText(this, "Invalid User name or Password", ToastLength.Short).Show();
            }
        }

        async Task<JsonValue> nextActivity(string un, string p)
        {
            licenceid = prefs.GetString("LicenceId", "");
            if (licenceid == null || licenceid == "")
            {
                await Get_Licence_Id();
                licenceid = prefs.GetString("LicenceId", "");
            }

             geolocation = geo.GetGeoLocation(this);
            dynamic value = new ExpandoObject();
            value.UserId = un;
            value.Password = p;
            value.gcmid = "";
            string json = JsonConvert.SerializeObject(value);

            JsonValue item = await restService.LoginUser2(this, version, un, json, geolocation).ConfigureAwait(false);
            return item;

        }

        public async Task<string> Get_Licence_Id()
        {
            string licenceId = "";
            Boolean connectivity = ic.connectivity();
            geolocation = geo.GetGeoLocation(this);
            if (connectivity)
            {
                dynamic value = new ExpandoObject();
                value.gcmid = "1";
                string json = JsonConvert.SerializeObject(value);
                // geolocation = geo.GetGeoLocation(ApplicationContext);
                try
                {
                    JsonValue json_licence = await restService.GetLicenceId(this, geolocation, version, json).ConfigureAwait(false);
                    List<string> lst1 = JsonConvert.DeserializeObject<List<string>>(json_licence);
                    licenceId = lst1[0].ToString();
                }
                catch (Exception ex)
                {

                }


                if (licenceId != null)
                {
                    ISharedPreferencesEditor editor = prefs.Edit();
                    editor.PutString("LicenceId", licenceId);
                    editor.Commit();
                }
                else
                {
                    //Toast.MakeText(ApplicationContext, "Licence Id is not generated", ToastLength.Short).Show();
                }
            }
            else
            {
                Toast.MakeText(this, "No Internet, Try after sometime", ToastLength.Short).Show();

            }

            return licenceId;
        }

        private bool DisplayLocationSettingsRequest()
        {
            bool islocationOn = false;
            var googleApiClient = new GoogleApiClient.Builder(this).AddApi(LocationServices.API).Build();
            googleApiClient.Connect();

            var locationRequest = LocationRequest.Create();
            locationRequest.SetPriority(LocationRequest.PriorityHighAccuracy);
            locationRequest.SetInterval(10000);
            locationRequest.SetFastestInterval(10000 / 2);

            var builder = new LocationSettingsRequest.Builder().AddLocationRequest(locationRequest);
            builder.SetAlwaysShow(true);

            var result = LocationServices.SettingsApi.CheckLocationSettings(googleApiClient, builder.Build());
            result.SetResultCallback((LocationSettingsResult callback) =>
            {
                switch (callback.Status.StatusCode)
                {
                    case LocationSettingsStatusCodes.Success:
                        {
                            islocationOn = true;
                            break;
                        }
                    case LocationSettingsStatusCodes.ResolutionRequired:
                        {
                            try
                            {
                                callback.Status.StartResolutionForResult(this, 100);
                            }
                            catch (IntentSender.SendIntentException e)
                            {
                            }

                            break;
                        }
                    default:
                        {
                            StartActivity(new Intent(Android.Provider.Settings.ActionLocationSourceSettings));
                            break;
                        }
                }
            });
            return islocationOn;
        }

        private async Task GetPermissionAsync()
        {
            List<String> permissions = new List<String>();
            try
            {

                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessFineLocation) == Permission.Denied)
                {
                    permissions.Add(Manifest.Permission.AccessFineLocation);
                }
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.RecordAudio) == Permission.Denied)
                {
                    permissions.Add(Manifest.Permission.RecordAudio);
                }
                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.AccessCoarseLocation) == Permission.Denied)
                {
                    permissions.Add(Manifest.Permission.AccessCoarseLocation);
                }

                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.Camera) == Permission.Denied)
                {
                    permissions.Add(Manifest.Permission.Camera);
                }

                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadExternalStorage) == Permission.Denied)
                {
                    permissions.Add(Manifest.Permission.ReadExternalStorage);
                }

                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.WriteExternalStorage) == Permission.Denied)
                {
                    permissions.Add(Manifest.Permission.WriteExternalStorage);
                }

                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.CallPhone) == Permission.Denied)
                {
                    permissions.Add(Manifest.Permission.CallPhone);
                }

                if (ContextCompat.CheckSelfPermission(this, Manifest.Permission.ReadPhoneState) == Permission.Denied)
                {
                    permissions.Add(Manifest.Permission.ReadPhoneState);
                }

                if (permissions.Count > 0)
                {
                    ActivityCompat.RequestPermissions(this, permissions.ToArray(), 100);
                }
               
            }
            catch (Exception e)
            {
                System.Console.WriteLine("Error", e.Message);
            }

        }

    }
}