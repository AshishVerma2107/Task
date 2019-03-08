using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using TaskAppWithLogin.Adapter;
using TaskAppWithLogin.Fragments;

namespace TaskAppWithLogin
{
    [Activity(Label = "FilterActivity")]
    public class FilterActivity : AppCompatActivity
    {
        ExpandableListAdapter2 listAdapter;
        ExpandableListView expListView;
        List<string> listDataHeader;
        Dictionary<string, List<string>> listDataChild;
        int previousGroup = -1;
        Button done;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.filter);
            expListView = FindViewById<ExpandableListView>(Resource.Id.lvExp);
            done = FindViewById<Button>(Resource.Id.btn);
            done.Click += delegate
            {
                Finish();
            };

            //if(TaskInboxAdapter.dead_line1.CompareTo(DateTime date))
            // Prepare list data
            FnGetListData();
            //Bind list
            listAdapter = new ExpandableListAdapter2(this, listDataHeader, listDataChild);
            expListView.SetAdapter(listAdapter);

            FnClickEvents();

            // Create your application here
        }
        void FnClickEvents()
        {
            expListView.ChildClick += delegate (object sender, ExpandableListView.ChildClickEventArgs e) {

                Toast.MakeText(this, "child clicked", ToastLength.Short).Show();
            };
            expListView.GroupExpand += delegate (object sender, ExpandableListView.GroupExpandEventArgs e) {

                if (e.GroupPosition != previousGroup)
                    expListView.CollapseGroup(previousGroup);
                previousGroup = e.GroupPosition;
            };
            expListView.GroupCollapse += delegate (object sender, ExpandableListView.GroupCollapseEventArgs e) {
                Toast.MakeText(this, "group collapsed", ToastLength.Short).Show();
            };

        }
        void FnGetListData()
        {
            listDataHeader = new List<string>();
            listDataChild = new Dictionary<string, List<string>>();

            // Adding child data
            //listDataHeader.Add("Task Details");
            listDataHeader.Add("Deadline");
            listDataHeader.Add("Organization");
            listDataHeader.Add("Designation");



            // Adding child data
            // Adding child data
            //var details = new List<string>();
            //details.Add("Task Name");
            //details.Add("Created By");


            var date = new List<string>();

            date.Add("Between 01/01/19-31/01/19");
            date.Add("Between 01/02/19-28/02/19");
            date.Add("Between 01/03/19-31/03/19");

            var org = new List<string>();
            org.Add("Organization");

            var desig = new List<string>();
            desig.Add("Designation");
            //listDataChild.Add(listDataHeader[0], details);
            listDataChild.Add(listDataHeader[0], date);
            listDataChild.Add(listDataHeader[1], org);
            listDataChild.Add(listDataHeader[2], desig);

        }
        //void Done_OnClick(object sender, EventArgs eventArgs)
        //{
        //    //Android.Support.V4.App.Fragment frag = new TaskInboxFrag();
        //    SupportFragmentManager.BeginTransaction().Replace(Resource.Id.container, new TaskInboxFrag()).Commit();

        //}
    }
}