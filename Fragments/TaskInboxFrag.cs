using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
using TaskAppWithLogin.Adapter;
using TaskAppWithLogin.Constants;
using TaskAppWithLogin.Models;
using static Android.App.ActionBar;

namespace TaskAppWithLogin.Fragments
{
    public class TaskInboxFrag: Fragment
    {
        private RecyclerView recyclerview;
        RecyclerView.LayoutManager recyclerview_layoutmanger;
        public static TaskInboxAdapter recyclerview_adapter;
        ServiceHelper restService = new ServiceHelper();
        List<TaskInboxModel> taskdata = new List<TaskInboxModel>();
        List<TaskInboxModel> result = new List<TaskInboxModel>();
        DbHelper dbHelper = new DbHelper();
        RecyclerAdapter<TaskInboxModel> im_model;
        public static TaskInboxModel detail;
        Android.Widget.SearchView search;
        string u_id = "";
        string l_id = "";
        Geolocation geo;
        string geolocation;
        ISharedPreferences prefs;
        Android.App.ProgressDialog progress;
        LinearLayout linearLayout;
        List<TaskInboxModel> freq;
        InternetConnection con = new InternetConnection();
        List<TaskFileMapping_Model> listmapping2 = new List<TaskFileMapping_Model>();
        //public static List<SubmitModel> summarydata = new List<SubmitModel>();
        public override void OnCreate(Bundle savedInstanceState)
        {
            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            im_model = new RecyclerAdapter<TaskInboxModel>();
            StrictMode.SetVmPolicy(builder.Build());
            StrictMode.ThreadPolicy.Builder builder1 = new StrictMode.ThreadPolicy.Builder().PermitAll();
            StrictMode.SetThreadPolicy(builder1.Build());
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            View view = inflater.Inflate(Resource.Layout.taskin_layout, null);
            recyclerview = view.FindViewById<RecyclerView>(Resource.Id.recyclerview1);
            linearLayout = view.FindViewById<LinearLayout>(Resource.Id.linearlayout1);
            prefs = PreferenceManager.GetDefaultSharedPreferences(Activity);
            u_id = prefs.GetString("DesignationId", "");

            geo = new Geolocation();
            geolocation = geo.GetGeoLocation(Activity);
            

            HasOptionsMenu = true;
            //DividerItemDecoration itemDecor = new DividerItemDecoration(Activity, Orientation.Horizontal);
            //recyclerview.AddItemDecoration(itemDecor);
            DividerItemDecoration horizontalDecoration = new DividerItemDecoration(recyclerview.Context,
            DividerItemDecoration.Vertical);
            Drawable horizontalDivider = ContextCompat.GetDrawable(Activity, Resource.Drawable.divider);
            horizontalDecoration.SetDrawable(horizontalDivider);
            recyclerview.AddItemDecoration(horizontalDecoration);
            search = view.FindViewById<Android.Widget.SearchView>(Resource.Id.searchview);


            search.QueryTextChange += sv_QueryTextChange;

            getData();
            return view;
        }
        public override void OnResume()
        {
            base.OnResume();

            taskdata = dbHelper.GetTaskInbox();
            if (taskdata.Count != 0)
            {
                recyclerview_layoutmanger = new LinearLayoutManager(Activity, LinearLayoutManager.Vertical, false);
                recyclerview.SetLayoutManager(recyclerview_layoutmanger);
                recyclerview_adapter = new TaskInboxAdapter(Activity, taskdata, recyclerview, FragmentManager);
                recyclerview.SetAdapter(recyclerview_adapter);
            }

            else
            {
                LayoutParams lparams = new LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);
                TextView textView = new TextView(Activity);
                textView.LayoutParameters = lparams;
                textView.Text = "Oops ! You haven't assigned any task yet";
                linearLayout.AddView(textView);
            }
            //if (FilterByDate_Activity.FromDateGlobal != null && FilterByDate_Activity.ToDateGlobal != null)
            //{
            //    List<TaskInboxModel> orderlist2 = dbHelper.getDataByDate(FilterByDate_Activity.FromDateGlobal, FilterByDate_Activity.ToDateGlobal);

            //    recyclerview_layoutmanger = new LinearLayoutManager(Activity, LinearLayoutManager.Vertical, false);
            //    recyclerview.SetLayoutManager(recyclerview_layoutmanger);
            //    recyclerview_adapter = new TaskInboxAdapter(Activity, orderlist2, recyclerview, FragmentManager);
            //    recyclerview.SetAdapter(recyclerview_adapter);
            //}
        }

        void sv_QueryTextChange(object sender, Android.Widget.SearchView.QueryTextChangeEventArgs e)
        {
            //FILTER
            recyclerview_adapter.Filter1.InvokeFilter(e.NewText);
        }
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.filtr)
            {
                Intent intent = new Intent(Activity, typeof(FilterByDate_Activity));

                StartActivity(intent);
            }
            else if (id == Resource.Id.namewise)
            {
                List<TaskInboxModel> orderlist = new List<TaskInboxModel>(freq.OrderBy(x => x.task_name).ToList());

                recyclerview_layoutmanger = new LinearLayoutManager(Activity, LinearLayoutManager.Vertical, false);
                recyclerview.SetLayoutManager(recyclerview_layoutmanger);
                recyclerview_adapter = new TaskInboxAdapter(Activity, orderlist, recyclerview, FragmentManager);
                recyclerview.SetAdapter(recyclerview_adapter);

            }

            else if (id == Resource.Id.creationdatewise)
            {
                List<TaskInboxModel> orderlist = new List<TaskInboxModel>(freq.OrderBy(x => x.deadlineDate).ToList());

                recyclerview_layoutmanger = new LinearLayoutManager(Activity, LinearLayoutManager.Vertical, false);
                recyclerview.SetLayoutManager(recyclerview_layoutmanger);
                recyclerview_adapter = new TaskInboxAdapter(Activity, orderlist, recyclerview, FragmentManager);
                recyclerview.SetAdapter(recyclerview_adapter);

            }
            else if (id == Resource.Id.datewiseDESC)
            {
                List<TaskInboxModel> orderlist = new List<TaskInboxModel>(freq.OrderByDescending(x => x.deadlineDate).ToList());
                
                recyclerview_layoutmanger = new LinearLayoutManager(Activity, LinearLayoutManager.Vertical, false);
                recyclerview.SetLayoutManager(recyclerview_layoutmanger);
                recyclerview_adapter = new TaskInboxAdapter(Activity, orderlist, recyclerview, FragmentManager);
                recyclerview.SetAdapter(recyclerview_adapter);

            }



            return base.OnOptionsItemSelected(item);

        }

        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);
            inflater.Inflate(Resource.Menu.menu_main, menu);
            menu.FindItem(Resource.Id.filtr).SetVisible(true);
            menu.FindItem(Resource.Id.order).SetVisible(true);

            var item = menu.FindItem(Resource.Id.filtr);
            var item1 = menu.FindItem(Resource.Id.order);
        }
        public async Task getData()
        { Boolean connectivity = con.connectivity();
            if (connectivity)
            {
                progress = new Android.App.ProgressDialog(Activity);
                progress.Indeterminate = true;
                progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
                progress.SetCancelable(false);
                progress.SetMessage("Please wait...");
                progress.Show();
                dynamic value = new ExpandoObject();
                value.UserId = u_id;

                string json = JsonConvert.SerializeObject(value);
                try
                {
                    JsonValue item = await restService.TaskInbox(Activity, json, geolocation);
                    freq = JsonConvert.DeserializeObject<List<TaskInboxModel>>(item);
                    dbHelper.insertdatainbox(freq);
                    progress.Dismiss();

                    //if (freq.Count != 0)
                    //{
                    //    recyclerview_layoutmanger = new LinearLayoutManager(Activity, LinearLayoutManager.Vertical, false);
                    //    recyclerview.SetLayoutManager(recyclerview_layoutmanger);
                    //    recyclerview_adapter = new TaskInboxAdapter(Activity, freq, recyclerview, FragmentManager);
                    //    recyclerview.SetAdapter(recyclerview_adapter);
                    //}

                    //else
                    //{
                    //    LayoutParams lparams = new LayoutParams(LayoutParams.MatchParent, LayoutParams.WrapContent);
                    //    TextView textView = new TextView(Activity);
                    //    textView.LayoutParameters = lparams;
                    //    lparams.Gravity = GravityFlags.Center;
                    //    textView.Text = "Oops ! You haven't assigned any task yet";
                    //    linearLayout.AddView(textView);
                    //}
                    //progress.Dismiss();

                }
                catch (Exception ex)
                {
                    progress.Dismiss();
                }
            }
            taskdata = dbHelper.GetTaskInbox();
            if (taskdata.Count != 0)
            {
                recyclerview_layoutmanger = new LinearLayoutManager(Activity, LinearLayoutManager.Vertical, false);
                recyclerview.SetLayoutManager(recyclerview_layoutmanger);
                recyclerview_adapter = new TaskInboxAdapter(Activity, taskdata, recyclerview, FragmentManager);
                recyclerview.SetAdapter(recyclerview_adapter);
            }

            else
            {
                LayoutParams lparams = new LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);
                TextView textView = new TextView(Activity);
                textView.LayoutParameters = lparams;
                textView.Text = "Oops ! You haven't assigned any task yet";
                linearLayout.AddView(textView);
            }

        }

        //public void LoadData()
        //{
        //    Boolean connectivity = con.connectivity();
        //    if (connectivity)
        //    {
        //        getData();
        //    }
            
        //        taskdata = dbHelper.GetTaskInbox();
        //        if (taskdata.Count != 0)
        //        {
        //            recyclerview_layoutmanger = new LinearLayoutManager(Activity, LinearLayoutManager.Vertical, false);
        //            recyclerview.SetLayoutManager(recyclerview_layoutmanger);
        //            recyclerview_adapter = new TaskInboxAdapter(Activity, taskdata, recyclerview, FragmentManager);
        //            recyclerview.SetAdapter(recyclerview_adapter);
        //        }

        //        else
        //        {
        //            LayoutParams lparams = new LayoutParams(LayoutParams.WrapContent, LayoutParams.WrapContent);
        //            TextView textView = new TextView(Activity);
        //            textView.LayoutParameters = lparams;
        //            textView.Text = "Oops ! You haven't assigned any task yet";
        //            linearLayout.AddView(textView);
        //        }
        //}

        //public override bool OnOptionsItemSelected(IMenuItem item)
        //{
        //    int id = item.ItemId;

        //    if (id == Resource.Id.filtr)
        //    {

        //        return true;
        //    }

        //    return base.OnOptionsItemSelected(item);

        //}

        //public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        //{
        //    base.OnCreateOptionsMenu(menu, inflater);
        //    inflater.Inflate(Resource.Menu.menu_main, menu);
        //    menu.FindItem(Resource.Id.filtr).SetVisible(true);
        //    menu.FindItem(Resource.Id.order).SetVisible(true);

        //    var item = menu.FindItem(Resource.Id.filtr);
        //    var item1 = menu.FindItem(Resource.Id.order);


        //}
    }
}