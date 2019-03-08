using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;
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
    public class MarkingOtherOrganization : Fragment
    { 
    Context context;
        //  ProgressBar progress;
        ServiceHelper restservice;
          List<MarkingListModel> markinglist;
        List<OrgModel> orgmodel;
        DbHelper db;
        Geolocation geo;
        string location;
        Spinner selectorg;
        string selecteditem;
        MarkingListAdapter marked;
        string orgid;
        List<OrgModel> orgname;
        Android.App.ProgressDialog progress;
        ListView list;
        public MarkingOtherOrganization(Context context)
    {
        this.context = context;
    }
    public override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        geo = new Geolocation();
        restservice = new ServiceHelper();
        location = geo.GetGeoLocation(Activity);
        orgmodel = new List<OrgModel>();
        orgname = new List<OrgModel>();
        // Create your fragment here
    }

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        View view = inflater.Inflate(Resource.Layout.marking_other_org, null);
        selectorg = view.FindViewById<Spinner>(Resource.Id.spinner1);
        list = view.FindViewById<ListView>(Resource.Id.listView1);
        getOrgData();
        return view;
    }
    public async Task GetOrganizationDetail()
    {

    }
    public async Task getOrgData()
    {
        progress = new Android.App.ProgressDialog(Activity);
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

            string item = await restservice.OrgnizationList(Activity, "", location);
            orgmodel = JsonConvert.DeserializeObject<List<OrgModel>>(item);
            for (int i = 0; i < orgmodel.Count; i++)
            {
                OrgModel org = new OrgModel();
                org.organizationName = orgmodel[i].organizationName;
                orgname.Add(org);
            }
            //  db.InsertMarkingList(orgmodel);

            progress.Dismiss();
            selectorg.ItemSelected += new EventHandler<AdapterView.ItemSelectedEventArgs>(Selectorg_ItemSelected);
            ArrayAdapter adapter = new ArrayAdapter(Activity, Android.Resource.Layout.SimpleListItem1, orgmodel);
            selectorg.Adapter = adapter;
        }

        catch (Exception ex)
        {
            progress.Dismiss();
        }
    }
    public async Task getorglist(string org_id)
    {
        progress = new Android.App.ProgressDialog(Activity);
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
            string item = await restservice.MarkingList(Activity, json, location).ConfigureAwait(false);
            markinglist = JsonConvert.DeserializeObject<List<MarkingListModel>>(item);
            db.InsertMarkingList(markinglist);

            progress.Dismiss();
        }
        catch (Exception ex)
        {
            progress.Dismiss();
        }

        if (markinglist != null)
        {
            Activity.RunOnUiThread(() =>
            {
                marked = new MarkingListAdapter(Activity, markinglist);
                list.SetAdapter(marked);
            });
        }
        progress.Dismiss();



    }

    private void Selectorg_ItemSelected(object sender, AdapterView.ItemSelectedEventArgs e)
    {
        Spinner spinner = (Spinner)sender;

        orgid = orgmodel.ElementAt(e.Position).organizationId.ToString();
        selecteditem = string.Format("{0}", spinner.GetItemAtPosition(e.Position));
        getorglist(orgid);

    }
}
}
