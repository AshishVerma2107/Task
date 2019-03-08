    using Android.Content;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using System.Collections.Generic;
using TaskAppWithLogin.Adapter;
using TaskAppWithLogin.Constants;
using TaskAppWithLogin.Models;

namespace TaskAppWithLogin.Fragments
{
    public class SavedTaskFrag : Fragment
    {
        ServiceHelper restService;
        Geolocation geo;
        DbHelper db;
        InternetConnection ic;
        ISharedPreferences prefs;
        BlobFileUpload blob;

        private RecyclerView recyclerview;
        LinearLayout LinearLayout;
        Android.Widget.SearchView search;
        RecyclerView.LayoutManager recyclerview_layoutmanger;
        public static SavedTaskAdapter recyclerview_adapter;
        List<CreateTaskLicenceIdReturnModel> taskidmodel;
        string geolocation, designationid;

        public static List<InitialTaskModel> initialTasks;
        public List<InitialTaskModel> initialtaskservicelist;
        List<InitialTaskModel> dbtasklist;
        string temp_var;
        InternetConnection con;

        public override void OnCreate(Bundle savedInstanceState)
        {
            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());
            StrictMode.ThreadPolicy.Builder builder1 = new StrictMode.ThreadPolicy.Builder().PermitAll();
            StrictMode.SetThreadPolicy(builder1.Build());

            base.OnCreate(savedInstanceState);

            restService = new ServiceHelper();
            geo = new Geolocation();
            db = new DbHelper();
            ic = new InternetConnection();
            initialTasks = new List<InitialTaskModel>();
            initialtaskservicelist = new List<InitialTaskModel>();
            taskidmodel = new List<CreateTaskLicenceIdReturnModel>();
            dbtasklist = new List<InitialTaskModel>();
            blob = new BlobFileUpload();
            con = new InternetConnection();
            geolocation = geo.GetGeoLocation(Activity);
            prefs = PreferenceManager.GetDefaultSharedPreferences(Activity);
            designationid = prefs.GetString("DesignationId", "");
            taskidmodel = CreateTaskFrag.licenceidmodel;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            View view = inflater.Inflate(Resource.Layout.taskin_layout, null);
            // temp_var = taskidmodel[0].taskid.ToString();
            //initialTasks = db.GetinitialTaskssavelist(designationid);
            recyclerview = view.FindViewById<RecyclerView>(Resource.Id.recyclerview1);
            LinearLayout = view.FindViewById<LinearLayout>(Resource.Id.linearlayout1);

            DividerItemDecoration horizontalDecoration = new DividerItemDecoration(recyclerview.Context,
            DividerItemDecoration.Vertical);
            Drawable horizontalDivider = ContextCompat.GetDrawable(Activity, Resource.Drawable.divider);
            horizontalDecoration.SetDrawable(horizontalDivider);
            recyclerview.AddItemDecoration(horizontalDecoration);
            search = view.FindViewById<Android.Widget.SearchView>(Resource.Id.searchview);

            markdataAsync();


            //else
            //{
            //    if (initialTasks.Count != 0)
            //    {
            //        recyclerview_layoutmanger = new LinearLayoutManager(Activity, LinearLayoutManager.Vertical, false);
            //        recyclerview.SetLayoutManager(recyclerview_layoutmanger);
            //        recyclerview_adapter = new SavedTaskAdapter(Activity, initialTasks, recyclerview, FragmentManager);
            //        recyclerview.SetAdapter(recyclerview_adapter);
            //    }
            //}


            return view;
        }
        public async System.Threading.Tasks.Task markdataAsync()
        {
            if (con.connectivity())
            {
                string file_extension = await restService.Saveforlater(Activity, "", geolocation);
                initialtaskservicelist = JsonConvert.DeserializeObject<List<InitialTaskModel>>(file_extension);
                db.Insert_InitialTask(initialtaskservicelist);
                if (initialtaskservicelist.Count != 0)
                {
                    recyclerview_layoutmanger = new LinearLayoutManager(Activity, LinearLayoutManager.Vertical, false);
                    recyclerview.SetLayoutManager(recyclerview_layoutmanger);
                    recyclerview_adapter = new SavedTaskAdapter(Activity, initialtaskservicelist, recyclerview, FragmentManager);
                    recyclerview.SetAdapter(recyclerview_adapter);
                }
            }
            else
            {
                //dbtasklist= db.GetinitialTaskssavelist(initialtaskservicelist[0].task_id);
                if (initialTasks.Count != 0)
                {
                    recyclerview_layoutmanger = new LinearLayoutManager(Activity, LinearLayoutManager.Vertical, false);
                    recyclerview.SetLayoutManager(recyclerview_layoutmanger);
                    recyclerview_adapter = new SavedTaskAdapter(Activity, dbtasklist, recyclerview, FragmentManager);
                    recyclerview.SetAdapter(recyclerview_adapter);
                    Toast.MakeText(Activity, "you have not internet connection.. check your internet connection", ToastLength.Long).Show();
                }
            }
        }
    }
}