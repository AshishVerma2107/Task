using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.View;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using Newtonsoft.Json;
using TaskAppWithLogin.Constants;
using TaskAppWithLogin.Fragments;
using TaskAppWithLogin.Models;

namespace TaskAppWithLogin
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", WindowSoftInputMode = SoftInput.AdjustPan)]
    //[Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, NavigationView.IOnNavigationItemSelectedListener
    {
        ISharedPreferences prefs;
        DbHelper db;
        List<LoginModel> login_data;
        LogoutModel logout_data;
        string identity;
        Geolocation geo;
        ServiceHelper restService;
        InternetConnection ic;
        ProgressDialog progress;
        string version;
        Spinner spinner;
        string logout;
        string location;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);


            prefs = PreferenceManager.GetDefaultSharedPreferences(ApplicationContext);
            db = new DbHelper();
            geo = new Geolocation();
            restService = new ServiceHelper();
            ic = new InternetConnection();
            login_data = new List<LoginModel>();

            version = Android.App.Application.Context.ApplicationContext.PackageManager.GetPackageInfo(Android.App.Application.Context.ApplicationContext.PackageName, 0).VersionName;
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            ActionBarDrawerToggle toggle = new ActionBarDrawerToggle(this, drawer, toolbar, Resource.String.navigation_drawer_open, Resource.String.navigation_drawer_close);
            drawer.AddDrawerListener(toggle);
            toggle.SyncState();
             
            NavigationView navigationView = FindViewById<NavigationView>(Resource.Id.nav_view);
            navigationView.SetNavigationItemSelectedListener(this);
            List<LoginModel> loginModel = db.getDetail();
            if (loginModel.Count <= 1)
            {
                IMenu nav_Menu = navigationView.Menu;
                nav_Menu.FindItem(Resource.Id.switchuser).SetVisible(false);
            }
            
            location = geo.GetGeoLocation(this);
            SupportFragmentManager.BeginTransaction().Replace(Resource.Id.container, new MainFrag(), "MainFrag").Commit();

            ManualSyncData();

            View header = navigationView.GetHeaderView(0);
            string userName = prefs.GetString("UserName", "");
            string number = prefs.GetString("MobileNumber", "");
            string propic = prefs.GetString("NPPhoto", "");
            string org = prefs.GetString("Organization", "");
            string desig = prefs.GetString("Designation", "");
            identity = prefs.GetString("LoginIdentity", "");

            TextView name1 = (TextView)header.FindViewById(Resource.Id.user);
            TextView mobile1 = (TextView)header.FindViewById(Resource.Id.number);
            ImageView image = (ImageView)header.FindViewById(Resource.Id.propic);
            TextView desig1 = (TextView)header.FindViewById(Resource.Id.desig);
            TextView org1 = (TextView)header.FindViewById(Resource.Id.org);

            name1.Text = userName;
            mobile1.Text = number;
            desig1.Text = desig;
            org1.Text = org;

            if (propic != "" && propic != null)
            {
                Glide.With(this).Load(propic).Into(image);
            }
           
        }

       
        public override void OnBackPressed()
        {
            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            if (drawer.IsDrawerOpen(GravityCompat.Start))
            {
                drawer.CloseDrawer(GravityCompat.Start);
            }
            else
            {
                
                FragmentManager fm = FragmentManager;
                if (fm.BackStackEntryCount > 0)
                {
                    // Log.i("MainActivity", "popping backstack");
                   // fm.PopBackStack();
                }
                else
                {
                    
                    Fragment myFragment = FragmentManager.FindFragmentByTag("MainFrag");
                    Fragment myFragment1 = FragmentManager.FindFragmentByTag("CreateTaskFrag");
                    Fragment myFragment2 = FragmentManager.FindFragmentByTag("TaskInboxFrag");
                    Fragment myFragment3 = FragmentManager.FindFragmentByTag("TaskOutboxFrag");
                    if (fm.FindFragmentByTag("MainFrag")!=null && fm.FindFragmentByTag("MainFrag").IsVisible)
                    {
                        this.RunOnUiThread(
                         async () =>
                         {
                           var isCloseApp = await AlertAsync(this, "Task", "Do you want to close this app? if you will close the app data will be  lost.", "Yes", "No");

                              if (isCloseApp)
                              {
                                  this.FinishAffinity();
                              }
                         });
                    }
                    else
                    {
                        base.OnBackPressed();
                    }
                    //  Log.i("MainActivity", "nothing on backstack, calling super");
                    //   base.onBackPressed();

                }

                
            }

        }
        public Task<bool> AlertAsync(Context context, string title, string message, string positiveButton, string negativeButton)
        {
            var tcs = new TaskCompletionSource<bool>();

            using (var db = new Android.App.AlertDialog.Builder(context))
            {
                db.SetTitle(title);
                db.SetMessage(message);
                db.SetPositiveButton(positiveButton, (sender, args) => { tcs.TrySetResult(true); });
                db.SetNegativeButton(negativeButton, (sender, args) => { tcs.TrySetResult(false); });
                db.Show();
            }

            return tcs.Task;
        }
        //public override bool OnCreateOptionsMenu(IMenu menu)
        //{
        //    MenuInflater.Inflate(Resource.Menu.menu_main, menu);
        //    return true;
        //}
        public async Task ManualSyncData()
        {
            if (ic.connectivity())
            {
                try
                {
                    progress = new ProgressDialog(this);
                    progress.Indeterminate = true;
                    progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
                    progress.SetCancelable(false);
                    progress.SetMessage("Please wait...");
                    progress.Show();
                    List<CreateTaskModel> dataList = db.getCreateTaskData("no");
                    if (dataList != null && dataList.Count > 0)
                    {
                        long id;
                        foreach (var val in dataList)
                        {
                            id = val.Id;
                            CreateTaskModel model = new CreateTaskModel();
                            model.taskname = val.taskname;
                            model.taskdescrip = val.taskdescrip;
                            model.deadline = val.deadline;
                            model.through = val.through;
                            model.markto = val.markto;
                            model.lstTaskFileMapping = val.lstTaskFileMapping;
                            string json = JsonConvert.SerializeObject(model);

                            try
                            {

                                string item = await restService.CreateTaskMethod(this, json, location);
                                if (item.Contains("Success"))
                                {
                                    //db.InsertCreateTaskData(taskname, taskdescri, deadline, "mobile", markto, "yes");
                                    //Toast.MakeText(Activity, "Task Assign Successfully..", ToastLength.Long).Show();
                                    Toast.MakeText(this, "Oops! something went wrong. Please try again later.", ToastLength.Long).Show();
                                    progress.Dismiss();
                                }
                                else
                                {
                                    //db.InsertCreateTaskData(taskname, taskdescri, deadline, "mobile", markto, "yes");
                                    Toast.MakeText(this, "Data Synced", ToastLength.Long).Show();
                                    db.updatetaskstatus(id);
                                    //Toast.MakeText(Activity, "Oops! something went wrong. Please try again later.", ToastLength.Long).Show();
                                    progress.Dismiss();
                                }
                            }
                            catch (Exception ex)
                            {
                                progress.Dismiss();
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    progress.Dismiss();
                }
                progress.Dismiss();
            }
            progress.Dismiss();
        }

           private void FabOnClick(object sender, EventArgs eventArgs)
        {
            View view = (View)sender;
            Snackbar.Make(view, "Replace with your own action", Snackbar.LengthLong)
                .SetAction("Action", (Android.Views.View.IOnClickListener)null).Show();
        }

        //public override bool OnKeyDown([GeneratedEnum] Keycode keyCode, KeyEvent e)
        //{
        //    //  if()
        //    if (e.KeyCode == Keycode.Back)
        //    {
        //        if (FragmentManager.BackStackEntryCount > 0)
        //            FragmentManager.PopBackStack();
        //        else
        //            base.OnBackPressed();
        //    }
        //    return base.OnKeyDown(keyCode, e);
        //}
        



        public bool OnNavigationItemSelected(IMenuItem item)
        {
            Android.Support.V4.App.Fragment fragment = null;
            int id = item.ItemId;
            string tag = "";
            if (id == Resource.Id.nav_camera)
            {
                fragment = new MainFrag();
                tag = "MainFrag";
            }
            //else if (id == Resource.Id.nav_gallery)
            //{
            //    fragment = new CreateTaskFrag();
            //    tag = "CreateTaskFrag";
            //}
            //else if (id == Resource.Id.nav_slideshow)
            //{
            //    fragment = new TaskInboxFrag();
            //    tag = "TaskInboxFrag";
            //}
            //else if (id == Resource.Id.nav_manage)
            //{
            //    fragment = new TaskOutboxFrag();
            //    tag = "TaskOutboxFrag";
            //}
            else if (id == Resource.Id.saved)
            {
                fragment = new SavedTaskFrag();
                tag = "SavedTaskFrag";
            }
            else if (id == Resource.Id.switchuser)
            {
                Intent intent = new Intent(this, typeof(Switch_User));
                StartActivity(intent);
            }
            else if (id == Resource.Id.nav_send)
            {
                LogOut();
            }
            if (fragment != null)
            {
                SupportFragmentManager.BeginTransaction().Replace(Resource.Id.container, fragment).Commit();
            }

            DrawerLayout drawer = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);
            drawer.CloseDrawer(GravityCompat.Start);
            return true;
        }

        public async Task LogOut()
        {

            progress = new ProgressDialog(this);
            progress.Indeterminate = true;
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Horizontal);
            progress.SetCancelable(false);
            progress.SetMessage("Please wait...");
            progress.Show();

            logout_data = new LogoutModel();
            logout_data.LoginIdentity = identity;
            string logout_json = JsonConvert.SerializeObject(logout_data);
            try
            {
                logout = await restService.LogoutUser(this, location, version, logout_json).ConfigureAwait(false);
            }
            catch (Exception ex)
            {

            }

            if (logout.Equals("Ok"))
            {
                ISharedPreferencesEditor editor = prefs.Edit();
                editor.Remove("OrganizationId");
                editor.Remove("Organization");
                editor.Remove("OfficeId");
                editor.Remove("OfficeName");
                editor.Remove("NaturalPersonId");
                editor.Remove("UserName");
                editor.Remove("NpToOrgRelationID");
                editor.Remove("DesignationId");
                editor.Remove("Designation");
                editor.Remove("MobileNumber");
                editor.Remove("NPPhoto");
                editor.Remove("EmailAddress");
                editor.Remove("LoginIdentity");
                editor.Remove("IsRegistered");
                editor.Remove("GoogleLogin");
                editor.Remove("username1");
                editor.Remove("mobile");
                editor.Remove("mobile12");
                editor.Remove("username12");
                editor.Remove("Location");
                editor.Commit();

                db.deleteTables();

                //((ActivityManager)Application.Context.GetSystemService(ActivityService)).ClearApplicationUserData();
                Intent intent = new Intent(this, typeof(LoginMain_Activity));

                StartActivity(intent);
                Finish();
            }
        }


    }
}

