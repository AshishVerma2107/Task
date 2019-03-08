using System;
using System.Collections.Generic;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Widget;
using TaskAppWithLogin.Constants;
using TaskAppWithLogin.Models;
using Android.Support.V7.Widget;
using TaskAppWithLogin.Adapter;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Dynamic;
using System.Linq;

namespace TaskAppWithLogin
{
    [Activity(Label = "FilterByDate")]
    public class FilterByDate_Activity : AppCompatActivity
    {
        TextInputEditText FromDate, ToDate;

        Button Submit;


        // List<TaskInboxModel> taskdata = new List<TaskInboxModel>();
        //  List<TaskInboxModel> result = new List<TaskInboxModel>();

        ISharedPreferences prefs;
        ServiceHelper restservice;
        public List<OrgModel> orgmodel;
        public List<OrgModel> orgname;
        public static string selecteditem;
        public static string selecteditemdesignation;
        string orgid;
        Geolocation geo;
        string location;
        public static string FromDateGlobal;
        public static string ToDateGlobal;
        RecyclerAdapter<TaskInboxModel> im_model;
        Android.App.ProgressDialog progress;

        Spinner spinner, designationSpinner;

        //Context context;
        ////  ProgressBar progress;
        //ServiceHelper restservice;
        List<MarkingListModel> markinglist;
        List<MarkingListModel> addmarkinglist;
        //List<OrgModel> orgmodel;
        DbHelper db;
        //Geolocation geo;
        //string location;
        //Spinner selectorg;
        //string selecteditem;
        MarkingListAdapter marked;
        //string orgid;
        //List<OrgModel> orgname;
        //Android.App.ProgressDialog progress;
        ListView list;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.FilterByDate_Layout);

            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            im_model = new RecyclerAdapter<TaskInboxModel>();
            StrictMode.SetVmPolicy(builder.Build());
            StrictMode.ThreadPolicy.Builder builder1 = new StrictMode.ThreadPolicy.Builder().PermitAll();
            StrictMode.SetThreadPolicy(builder1.Build());

            FromDate = FindViewById<TextInputEditText>(Resource.Id.fromdate);
            ToDate = FindViewById<TextInputEditText>(Resource.Id.todate);
            Submit = FindViewById<Button>(Resource.Id.submit);
            spinner = FindViewById<Spinner>(Resource.Id.orgspinner);

            geo = new Geolocation();
            restservice = new ServiceHelper();
            location = geo.GetGeoLocation(this);
            orgmodel = new List<OrgModel>();
            orgname = new List<OrgModel>();
            markinglist = new List<MarkingListModel>();
            addmarkinglist = new List<MarkingListModel>();

            getOrgData();

            // spinner.ItemSelected += Selectorg_ItemSelected;

            //var adapter = ArrayAdapter.CreateFromResource(this, Resource.Array.organization_array, Android.Resource.Layout.SimpleSpinnerItem);
            //adapter.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            //spinner.Adapter = adapter;


            designationSpinner = FindViewById<Spinner>(Resource.Id.desigspinner);

            // designationSpinner.ItemSelected += spinner_ItemSelected1;

            //spinner1.ItemSelected += Selectorg_ItemSelected;

            // var adapter1 = ArrayAdapter.CreateFromResource(this, Resource.Array.designation_array, Android.Resource.Layout.SimpleSpinnerItem);
            //  adapter1.SetDropDownViewResource(Android.Resource.Layout.SimpleSpinnerDropDownItem);
            //  designationSpinner.Adapter = adapter1;

            FromDate.Click += (sender, e) =>
            {
                DateTime today = DateTime.Today;
                DatePickerDialog dialog = new DatePickerDialog(this, OnDateSetFrom, today.Year, today.Month - 1, today.Day);

                dialog.DatePicker.MinDate = today.Millisecond;
                dialog.Show();
            };

            ToDate.Click += (sender, e) =>
            {
                DateTime today = DateTime.Today;
                DatePickerDialog dialog = new DatePickerDialog(this, OnDateSetTo, today.Year, today.Month - 1, today.Day);

                dialog.DatePicker.MinDate = today.Millisecond;
                dialog.Show();
            };

            Submit.Click += delegate
            {
                Finish();
            };
        }

        private void OnDateSetFrom(object sender, DatePickerDialog.DateSetEventArgs e)
        {

            //   var test = DateTime.Now.ToString("dd-MM-yyyy");

            //   FromDate.Text = test;



            FromDate.Text = e.Date.ToString("dd-MM-yyyy");



            string date = e.Date.ToString("yyyy-MM-dd");

            FromDateGlobal = date;
        }

        private void OnDateSetTo(object sender, DatePickerDialog.DateSetEventArgs e)
        {



            ToDate.Text = e.Date.ToString("dd-MM-yyyy");
            string date1 = e.Date.ToString("yyyy-MM-dd");
            ToDateGlobal = date1;

        }
        //private void spinner_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        //{
        //    Spinner spinner = (Spinner)sender;
        //    string toast = string.Format("Selected group is {0}", spinner.GetItemAtPosition(e.Position));

        //  //  Toast.MakeText(this, toast, ToastLength.Long).Show();
        //}

        //private void spinner_ItemSelected1(object sender1, AdapterView.ItemSelectedEventArgs e1)
        //{
        //    Spinner spinner1 = (Spinner)sender1;
        //    string toast1 = string.Format("Selected group is {0}", spinner1.GetItemAtPosition(e1.Position));

        //   // Toast.MakeText(this, toast1, ToastLength.Long).Show();

        //}

        private void Selectorg_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;
            if (e.Position > 0)
            {
                getorglist(orgid);
            }
            orgid = orgmodel.ElementAt(e.Position).organizationId.ToString();
            selecteditem = string.Format("{0}", spinner.GetItemAtPosition(e.Position));



            // Toast.MakeText(this, selecteditem, ToastLength.Long).Show();
        }

        private void Selectdesignation_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
        {
            Spinner spinner = (Spinner)sender;

            // orgid = orgmodel.ElementAt(e.Position).organizationId.ToString();
            selecteditemdesignation = string.Format("{0}", spinner.GetItemAtPosition(e.Position));


            //  Toast.MakeText(this, selecteditem, ToastLength.Long).Show();
        }

        public async Task getOrgData()
        {
            progress = new Android.App.ProgressDialog(this);
            progress.Indeterminate = true;
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            progress.SetCancelable(false);
            progress.SetMessage("Please wait...");
            progress.Show();

            //dynamic value = new ExpandoObject();
            //value.OrgId = orgid;

            //   string json = JsonConvert.SerializeObject(value);
            try
            {

                string item = await restservice.OrgnizationList(this, "", location);
                //orgmodel[0].organizationName = "Select Orgnization";
                orgmodel = JsonConvert.DeserializeObject<List<OrgModel>>(item);
                orgmodel[0].organizationName = "Select Organization";
                for (int i = 0; i < orgmodel.Count; i++)
                {
                    OrgModel org = new OrgModel();
                    //org.organizationName=orgmodel[0].
                    org.organizationName = orgmodel[i].organizationName;
                    orgname.Add(org);
                }
                //  db.InsertMarkingList(orgmodel);

                progress.Dismiss();

                spinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(Selectorg_ItemSelected);
                ArrayAdapter adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, orgname);
                spinner.Adapter = adapter;

                //  getorglist()
            }

            catch (Exception ex)
            {
                progress.Dismiss();
            }
        }

        public async Task getorglist(string org_id)
        {
            progress = new Android.App.ProgressDialog(this);
            progress.Indeterminate = true;
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            progress.SetCancelable(false);
            progress.SetMessage("Please wait...");
            progress.Show();

            dynamic value = new ExpandoObject();
            value.OrgId = org_id;

            string json = JsonConvert.SerializeObject(value);
            try
            {
                string item = await restservice.MarkingList(this, json, location).ConfigureAwait(false);

                markinglist = JsonConvert.DeserializeObject<List<MarkingListModel>>(item);
                markinglist[0].DesignationName = "Select Designation";
                for (int j = 0; j < markinglist.Count; j++)
                {
                    MarkingListModel model = new MarkingListModel();
                    model.DesignationName = markinglist[j].DesignationName;
                    addmarkinglist.Add(model);

                }
                //db.InsertMarkingList(markinglist);

                progress.Dismiss();
            }
            catch (Exception ex)
            {
                progress.Dismiss();
            }

            if (markinglist != null)
            {
                this.RunOnUiThread(() =>
                {
                    designationSpinner.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(Selectdesignation_ItemSelected);
                    ArrayAdapter adapter1 = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, addmarkinglist);
                    designationSpinner.Adapter = adapter1;
                });

                //Activity.RunOnUiThread(() =>
                //{
                //    marked = new MarkingListAdapter(this, markinglist);
                //    list.SetAdapter(marked);
                //});
            }
            progress.Dismiss();



        }
    }
}