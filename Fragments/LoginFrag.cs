using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Auth.Api;
using Android.Gms.Auth.Api.SignIn;
using Android.Gms.Common;
using Android.Gms.Common.Apis;
using Android.Locations;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using TaskAppWithLogin.Constants;
using TaskAppWithLogin.Models;

namespace TaskAppWithLogin.Fragments
{
    //[Activity(Label = "LoginFrag")]
    public class LoginFrag : Fragment, GoogleApiClient.IConnectionCallbacks, GoogleApiClient.IOnConnectionFailedListener
    {
        EditText user, pass;
        Button log, log_google, log_fb, log_twitt, log_phone;
        ISharedPreferences prefs;
        public static LoginModel detail;
        ServiceHelper restService;
        string licenceid;
        string geolocation="0";
        Geolocation geo;
        GoogleApiClient mGoogleApiClient;
        int count = 1;
        //ISharedPreferences prefs;
        public int SIGN_In_ID = 9001;
        InternetConnection ic;
        string version;
        DbHelper db = new DbHelper();
        string username="", npid="", mobile="", userid;
        bool update = false;
        Android.App.ProgressDialog progress;
        
        RegisterModel register_data;

        string provider_id = "", provider_name = "", email_id = "",selfie_path="", register_mobile="";

        public override void OnCreate(Bundle savedInstanceState)
        {
            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());
            StrictMode.ThreadPolicy.Builder builder1 = new StrictMode.ThreadPolicy.Builder().PermitAll();
            StrictMode.SetThreadPolicy(builder1.Build());
            base.OnCreate(savedInstanceState);
            prefs = PreferenceManager.GetDefaultSharedPreferences(Activity);
            geo = new Geolocation();
            ic = new InternetConnection();
            restService = new ServiceHelper();
            version = Android.App.Application.Context.ApplicationContext.PackageManager.GetPackageInfo(Android.App.Application.Context.ApplicationContext.PackageName, 0).VersionName;
            geolocation = geo.GetGeoLocation(Activity);
            //if (permissionmethodAsync() == true)
            //{
               
            //}
            //else
            //{
            //    permissionmethodAsync();
            //}
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.loginFinal, null);
            user = view.FindViewById<EditText>(Resource.Id.username);
            pass = view.FindViewById<EditText>(Resource.Id.password);
            log = view.FindViewById<Button>(Resource.Id.login_snp);
            log_google = view.FindViewById<Button>(Resource.Id.login_google);
            log_fb = view.FindViewById<Button>(Resource.Id.login_fb);
            log_twitt = view.FindViewById<Button>(Resource.Id.login_twitter);
            log_phone = view.FindViewById<Button>(Resource.Id.login_phone);

            if (prefs.GetBoolean("GoogleLogin", false))
            {
                Intent intent = new Intent(Activity, typeof(MainActivity));
                intent.AddFlags(ActivityFlags.NewTask);
                StartActivity(intent);
                Activity.Finish();
            }

          
            //log_phone.Click += delegate
            //{
            //    FragmentManager.BeginTransaction().Replace(Resource.Id.container_mainlogin, new PhoneLoginFrag()).Commit();

            //};
            log_google.Click += delegate
            {
                GoogleSignInClick();
                GetLogin();
            };


            ConfigureGoogleSign();

            log.Click += delegate
            {
                UserLogin();
            };

            main_method();

            return view;
        }
        //public bool permissionmethodAsync()
        //{

        //    GetPermissionAsync();
        //    return true;
        //}
        //private async Task GetPermissionAsync()
        //{
        //    List<String> permissions = new List<String>();
        //    try
        //    {

        //        if (ContextCompat.CheckSelfPermission(Activity, Manifest.Permission.AccessFineLocation) == Permission.Denied)
        //        {
        //            permissions.Add(Manifest.Permission.AccessFineLocation);
        //        }
        //        if (ContextCompat.CheckSelfPermission(Activity, Manifest.Permission.RecordAudio) == Permission.Denied)
        //        {
        //            permissions.Add(Manifest.Permission.RecordAudio);
        //        }
        //        if (ContextCompat.CheckSelfPermission(Activity, Manifest.Permission.AccessCoarseLocation) == Permission.Denied)
        //        {
        //            permissions.Add(Manifest.Permission.AccessCoarseLocation);
        //        }

        //        if (ContextCompat.CheckSelfPermission(Activity, Manifest.Permission.Camera) == Permission.Denied)
        //        {
        //            permissions.Add(Manifest.Permission.Camera);
        //        }

        //        if (ContextCompat.CheckSelfPermission(Activity, Manifest.Permission.ReadExternalStorage) == Permission.Denied)
        //        {
        //            permissions.Add(Manifest.Permission.ReadExternalStorage);
        //        }

        //        if (ContextCompat.CheckSelfPermission(Activity, Manifest.Permission.WriteExternalStorage) == Permission.Denied)
        //        {
        //            permissions.Add(Manifest.Permission.WriteExternalStorage);
        //        }

        //        if (ContextCompat.CheckSelfPermission(Activity, Manifest.Permission.CallPhone) == Permission.Denied)
        //        {
        //            permissions.Add(Manifest.Permission.CallPhone);
        //        }

        //        if (ContextCompat.CheckSelfPermission(Activity, Manifest.Permission.ReadPhoneState) == Permission.Denied)
        //        {
        //            permissions.Add(Manifest.Permission.ReadPhoneState);
        //        }



        //        if (permissions.Count > 0)
        //        {
        //            ActivityCompat.RequestPermissions(Activity, permissions.ToArray(), 100);
        //        }
        //        if (ContextCompat.CheckSelfPermission(Activity, Manifest.Permission.AccessFineLocation) == Permission.Granted)
        //        {
        //            geo = new Geolocation();
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        System.Console.WriteLine("Error", e.Message);
        //    }

        //}

        public async Task GetLogin()
        {
            Boolean result = ic.connectivity();
            if (result)
            {
                progress = new Android.App.ProgressDialog(Activity);
                progress.Indeterminate = true;
                progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
                progress.SetCancelable(false);
                progress.SetMessage("Please wait...");
                progress.Show();
                JsonValue login_value = null;
                try
                {
                    login_value = await nextActivity("", "");
                }
                catch (Exception e)
                {

                }
                if (login_value != null)
                {
                    await ParseAndDisplay(login_value, "");
                }
            }
        }
        public async void main_method()
        {

           // await GetPermissionAsync();
            try
            {
                licenceid = prefs.GetString("LicenceId", "");

                if (licenceid != null && licenceid != "")
                {
                    bool isRegistered = prefs.GetBoolean("IsRegistered", false);
                    if (isRegistered)
                    {
                        Intent intent = new Intent(Activity, typeof(MainActivity));
                        intent.AddFlags(ActivityFlags.NewTask);
                        StartActivity(intent);
                        Activity.Finish();

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

        


        public void UserLogin()
        {
            LocationManager mlocManager = (LocationManager)Activity.GetSystemService(Context.LocationService); 
            bool enabled = mlocManager.IsProviderEnabled(LocationManager.GpsProvider);
            if (enabled == false)
            {
                Toast.MakeText(Activity, "GPS Not Enable", ToastLength.Long).Show();
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

                Toast.MakeText(Activity, errorMsg, ToastLength.Long).Show();
                return;
            }
            else
            {
                Boolean result = ic.connectivity();
                if (result)
                {
                    progress = new Android.App.ProgressDialog(Activity);
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

                    //  loginId1 = user.Text;
                    // password1 = pass.Text;
                }
                else
                {

                    Toast.MakeText(Activity, "No Internet", ToastLength.Long).Show();
                }

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
                            string isRegistered = await restService.RegisterUser(Activity, licenceid, geolocation, version, register_json).ConfigureAwait(false);

                            progress.Dismiss();

                            if (isRegistered.Contains("Success"))
                            {

                                editor.PutBoolean("IsRegistered", true);
                                editor.Commit();

                                Intent intent = new Intent(Activity, typeof(MainActivity));
                                intent.AddFlags(ActivityFlags.NewTask);
                                StartActivity(intent);
                                Activity.Finish();


                            }
                            else
                            {
                                progress.Dismiss();
                                Toast.MakeText(Activity, "Try after some time", ToastLength.Short).Show();
                            }
                        }
                        catch (Exception ex)
                        {
                            progress.Dismiss();
                            Toast.MakeText(Activity, "Try after some time", ToastLength.Short).Show();
                        }
                    }
                    else
                    {
                        progress.Dismiss();
                        Toast.MakeText(Activity, "Invalid User name or Password", ToastLength.Short).Show();
                    }
                }
                else
                {
                    //editor.PutString("NaturalPersonId", lst[0].NaturalPersonId);
                    Intent intent = new Intent(Activity, typeof(Switch_User));
                    intent.AddFlags(ActivityFlags.NewTask);
                   
                    StartActivity(intent);
                    Activity.Finish();
                }
            }
            else
            {
                progress.Dismiss();
                Toast.MakeText(Activity, "Invalid User name or Password", ToastLength.Short).Show();
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

            //   geolocation = geo.GetGeoLocation(ApplicationContext);
            dynamic value = new ExpandoObject();
            value.UserId = un;
            value.Password = p;
            string json = JsonConvert.SerializeObject(value);

            JsonValue item = await restService.LoginUser2(Activity, version, un, json, geolocation).ConfigureAwait(false);
            return item;

        }

        public async Task<string> Get_Licence_Id()
        {
            string licenceId = "";
            Boolean connectivity = ic.connectivity();

            if (connectivity)
            {
                dynamic value = new ExpandoObject();
                value.gcmid = "1";
                string json = JsonConvert.SerializeObject(value);
                // geolocation = geo.GetGeoLocation(ApplicationContext);
                try
                {
                    JsonValue json_licence = await restService.GetLicenceId(Activity, geolocation, version, json).ConfigureAwait(false);
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
                Toast.MakeText(Activity, "No Internet, Try after sometime", ToastLength.Short).Show();

            }

            return licenceId;
        }

        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {

            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == SIGN_In_ID)
            {
                var result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
                HandleSignInResult(result);
            }

        }

        private void HandleSignInResult(GoogleSignInResult result)
        {

            if (result.IsSuccess)
            {
                var accountDetails = result.SignInAccount;

                ISharedPreferencesEditor editor = prefs.Edit();
                editor.PutBoolean("GoogleLogin", true);
                editor.Apply();

                Intent intent = new Intent(Activity, typeof(MainActivity));
                intent.AddFlags(ActivityFlags.NewTask);
                StartActivity(intent);
                Activity.Finish();
            }
        }

        private void GoogleSignInClick()
        {
            var signInIntent = Auth.GoogleSignInApi.GetSignInIntent(mGoogleApiClient);

            StartActivityForResult(signInIntent, SIGN_In_ID);

            // Toast.MakeText(this, "You have Already Logged In", ToastLength.Short).Show();
        }

        private void ConfigureGoogleSign()
        {
            GoogleSignInOptions gso = new GoogleSignInOptions.Builder(GoogleSignInOptions.DefaultSignIn)
                                                                    .RequestEmail()
                                                                    .Build();
            mGoogleApiClient = new GoogleApiClient.Builder(Activity)
                                       .EnableAutoManage(Activity, this)
                                       .AddApi(Auth.GOOGLE_SIGN_IN_API, gso)
                                       .AddConnectionCallbacks(this)
                                       .Build();
        }

        public void OnConnected(Bundle connectionHint)
        {
            // throw new NotImplementedException();
        }

        public void OnConnectionSuspended(int cause)
        {
            // throw new NotImplementedException();
        }

        public void OnConnectionFailed(ConnectionResult result)
        {
            // throw new NotImplementedException();
        }

        public void OnComplete(Task task)
        {
            throw new NotImplementedException();
        }

        public void OnComplete(Android.Gms.Tasks.Task task)
        {
            throw new NotImplementedException();
        }

        public override void OnPause()
        {
            base.OnPause();
            mGoogleApiClient.StopAutoManage(Activity);
            mGoogleApiClient.Disconnect();
        }


    }
}
