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

namespace TaskAppWithLogin.Fragments
{
    public class TaskOutboxFrag:Fragment
    {
        private RecyclerView recyclerview;
        InternetConnection con = new InternetConnection();
        RecyclerView.LayoutManager recyclerview_layoutmanger;
       public static  TaskOutboxAdapter recyclerview_adapter;
        ServiceHelper restService = new ServiceHelper();
        List<TaskOutboxModel> result = new List<TaskOutboxModel>();
        List<TaskOutboxModel> im_model;
        ISharedPreferences prefs;
        Android.App.ProgressDialog progress;
        string userid, geolocation="0";
        Geolocation geo;
        LinearLayout LinearLayout;
        Android.Widget.SearchView search;
        List<TaskOutboxModel> listoutbox = new List<TaskOutboxModel>();

        DbHelper dbHelper = new DbHelper();
        // public static List<SubmitModel> summarydata = new List<SubmitModel>();
        public override void OnCreate(Bundle savedInstanceState)
        {
            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            im_model = new List<TaskOutboxModel>();
            StrictMode.SetVmPolicy(builder.Build());
            StrictMode.ThreadPolicy.Builder builder1 = new StrictMode.ThreadPolicy.Builder().PermitAll();
            StrictMode.SetThreadPolicy(builder1.Build());
            geo = new Geolocation();
            geolocation = geo.GetGeoLocation(Activity);
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            View view = inflater.Inflate(Resource.Layout.taskin_layout, null);
            recyclerview = view.FindViewById<RecyclerView>(Resource.Id.recyclerview1);
            LinearLayout = view.FindViewById<LinearLayout>(Resource.Id.linearlayout1);
            HasOptionsMenu = true;
            //DividerItemDecoration itemDecor = new DividerItemDecoration(Activity, Orientation.Horizontal);
            //recyclerview.AddItemDecoration(itemDecor);
            DividerItemDecoration horizontalDecoration = new DividerItemDecoration(recyclerview.Context,
            DividerItemDecoration.Vertical);
            Drawable horizontalDivider = ContextCompat.GetDrawable(Activity, Resource.Drawable.divider);
            horizontalDecoration.SetDrawable(horizontalDivider);
            recyclerview.AddItemDecoration(horizontalDecoration);
            //summarydata = db.getData();
            prefs = PreferenceManager.GetDefaultSharedPreferences(Activity);
            //licenceId = prefs.GetString("LicenceId", "");
            userid = prefs.GetString("DesignationId", "");
            search = view.FindViewById<Android.Widget.SearchView>(Resource.Id.searchview);


            search.QueryTextChange += sv_QueryTextChange;
            checkInternet();
            //getData();
            return view;
        }

        void sv_QueryTextChange(object sender, Android.Widget.SearchView.QueryTextChangeEventArgs e)
        {
            //FILTER
            recyclerview_adapter.Filter2.InvokeFilter(e.NewText);
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
                List<TaskOutboxModel> orderlist = new List<TaskOutboxModel>(listoutbox.OrderBy(x => x.Task_name).ToList());

                recyclerview_layoutmanger = new LinearLayoutManager(Activity, LinearLayoutManager.Vertical, false);
                recyclerview.SetLayoutManager(recyclerview_layoutmanger);
                recyclerview_adapter = new TaskOutboxAdapter(Activity, orderlist, recyclerview, FragmentManager);
                recyclerview.SetAdapter(recyclerview_adapter);

            }

            else if (id == Resource.Id.creationdatewise)
            {
                List<TaskOutboxModel> orderlist = new List<TaskOutboxModel>(listoutbox.OrderBy(x => x.deadline_date).ToList());

                recyclerview_layoutmanger = new LinearLayoutManager(Activity, LinearLayoutManager.Vertical, false);
                recyclerview.SetLayoutManager(recyclerview_layoutmanger);
                recyclerview_adapter = new TaskOutboxAdapter(Activity, orderlist, recyclerview, FragmentManager);
                recyclerview.SetAdapter(recyclerview_adapter);

            }
            else if (id == Resource.Id.datewiseDESC)
            {
                List<TaskOutboxModel> orderlist = new List<TaskOutboxModel>(listoutbox.OrderByDescending(x => x.deadline_date).ToList());

                recyclerview_layoutmanger = new LinearLayoutManager(Activity, LinearLayoutManager.Vertical, false);
                recyclerview.SetLayoutManager(recyclerview_layoutmanger);
                recyclerview_adapter = new TaskOutboxAdapter(Activity, orderlist, recyclerview, FragmentManager);
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
        {
            progress = new Android.App.ProgressDialog(Activity);
            progress.Indeterminate = true;
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            progress.SetCancelable(false);
            progress.SetMessage("Please wait...");
            progress.Show();
            dynamic value = new ExpandoObject();
            value.UserId = userid;

            string json = JsonConvert.SerializeObject(value);
            try
            {
                JsonValue item = await restService.TaskOutbox(Activity, json, geolocation);
                List<TaskOutboxModel> taskOutbox = JsonConvert.DeserializeObject<List<TaskOutboxModel>>(item);
                dbHelper.insertdataoutbox(taskOutbox);

                for (int i = 0; i < taskOutbox.Count; i++)
                {

                    TaskOutboxModel detail = new TaskOutboxModel
                    {
                        TaskPercentage = taskOutbox[i].TaskPercentage,
                        Task_id = taskOutbox[i].Task_id,
                        Task_name = taskOutbox[i].Task_name,
                        Description = taskOutbox[i].Description,
                        deadline_date = taskOutbox[i].deadline_date,
                        mark_to = taskOutbox[i].mark_to,
                        task_status = taskOutbox[i].task_status,
                        Task_created_by = taskOutbox[i].Task_created_by,
                        Task_creation_date = taskOutbox[i].Task_creation_date,
                        task_mark_by = taskOutbox[i].task_mark_by,
                        MarkingDate = taskOutbox[i].MarkingDate,
                        task_marking_type = taskOutbox[i].task_marking_type
                    };


                    im_model.Add(detail);
                }
                if (im_model.Count != 0)
                {
                    recyclerview_layoutmanger = new LinearLayoutManager(Activity, LinearLayoutManager.Vertical, false);
                    recyclerview.SetLayoutManager(recyclerview_layoutmanger);
                    recyclerview_adapter = new TaskOutboxAdapter(Activity, im_model, recyclerview,FragmentManager);
                    recyclerview.SetAdapter(recyclerview_adapter);
                }
                else
                {
                    TextView textView = new TextView(Activity);

                    textView.Text = "Oops ! You haven't assigned any task yet";
                    LinearLayout.AddView(textView);
                }

                progress.Dismiss();
            }
            catch (Exception ex)
            {
                progress.Dismiss();
            }
            progress.Dismiss();
        }

        public void checkInternet()
        {
            Boolean connectivity = con.connectivity();
            if (connectivity)
            {
                getData();
            }
            else
            {
                listoutbox = dbHelper.Getoutboxdata();
                for (int i = 0; i < listoutbox.Count; i++)
                {

                    TaskOutboxModel detail = new TaskOutboxModel
                    {
                        TaskPercentage = listoutbox[i].TaskPercentage,
                        Task_id = listoutbox[i].Task_id,
                        Task_name = listoutbox[i].Task_name,
                        Description = listoutbox[i].Description,
                        deadline_date = listoutbox[i].deadline_date,
                        mark_to = listoutbox[i].mark_to,
                        task_status = listoutbox[i].task_status,
                        Task_created_by = listoutbox[i].Task_created_by,
                        Task_creation_date = listoutbox[i].Task_creation_date,
                        task_mark_by = listoutbox[i].task_mark_by,
                        MarkingDate = listoutbox[i].MarkingDate,
                        task_marking_type = listoutbox[i].task_marking_type
                    };


                    im_model.Add(detail);
                }
                if (im_model.Count != 0)
                {
                    recyclerview_layoutmanger = new LinearLayoutManager(Activity, LinearLayoutManager.Vertical, false);
                    recyclerview.SetLayoutManager(recyclerview_layoutmanger);
                    recyclerview_adapter = new TaskOutboxAdapter(Activity, im_model, recyclerview,FragmentManager);
                    recyclerview.SetAdapter(recyclerview_adapter);
                }
                else
                {
                    TextView textView = new TextView(Activity);

                    textView.Text = "Oops ! You haven't assigned any task yet";
                    LinearLayout.AddView(textView);
                }

            }
        }
        //public override bool OnOptionsItemSelected(IMenuItem item)
        //{
        //    int id = item.ItemId;

        //    if (id == Resource.Id.filtr)
        //    {

        //        return true;
        //    }

        //    return base.OnOptionsItemSelected(item);

        //}

        
    }
}