using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using TaskAppWithLogin.Adapter;
using TaskAppWithLogin.Constants;
using TaskAppWithLogin.Models;

namespace TaskAppWithLogin
{
   
    public class ReferenceAttachmentActivity : Fragment
    {
        TextView imagetext, audiotext, videotext;
        public static GridImageForRefrence ref_adapter1;
        public static GridVideoForReference ref_adapter2;
        public static GridAudioForReference ref_adapter3;
        ExpandableHeightGridView grid1, grid2, grid3;
        List<TaskFilemappingModel2> taskmappinglist;
        List<TaskFilemappingModel2> taskmappingimagelist;
        List<TaskFilemappingModel2> taskmappingvideolist;
        List<TaskFilemappingModel2> taskmappingaudiolist;
        DbHelper db;
        string taskid;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            //SetContentView(Resource.Layout.reference_attach_layout);
            // Create your application here
        }
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.reference_attach_layout, null);
            imagetext = view.FindViewById<TextView>(Resource.Id.image_no);
            audiotext = view.FindViewById<TextView>(Resource.Id.audio_no);
            videotext = view.FindViewById<TextView>(Resource.Id.video_no);
            grid1 = view.FindViewById<ExpandableHeightGridView>(Resource.Id.gridView1);
            grid2 = view.FindViewById<ExpandableHeightGridView>(Resource.Id.gridView2);
            grid3 = view.FindViewById<ExpandableHeightGridView>(Resource.Id.gridView3);
            taskmappinglist = new List<TaskFilemappingModel2>();
            taskmappingimagelist = new List<TaskFilemappingModel2>();
            taskmappingaudiolist = new List<TaskFilemappingModel2>();
            taskmappingvideolist = new List<TaskFilemappingModel2>();
            // taskid = Intent.GetStringExtra("TaskId");
            db = new DbHelper();
            taskid = Arguments.GetString("TaskId") ?? string.Empty;
            getdataofattachment();
            grid1.setExpanded(true);
            //grid1.ChoiceMode = (ChoiceMode)AbsListViewChoiceMode.MultipleModal;


            grid2.setExpanded(true);

            grid3.setExpanded(true);
            return view;
        }

            public void getdataofattachment()
        {
            //for (int i = 0; i < listmapping.Count; i++)
            //{
            //    if (listmapping[i].FileType.Equals("Image"))
            //    {
            //        image_list.Add(listmapping[i]);
            //    }
            //}
            //adapter_1 = new GridImageAdapterCreatetask(Activity, image_list);
            //Gridview_1.Adapter = adapter_1;
            //Gridview_1.setExpanded(true);
            //Gridview_1.ChoiceMode = (ChoiceMode)AbsListViewChoiceMode.MultipleModal;
            taskmappinglist = db.GetFullCreatecomplianceAttachmentData(taskid);
            for (int i = 0; i < taskmappinglist.Count; i++)
            {
                
                    if (taskmappinglist[i].FileType .Equals ("Image"))
                    {
                        taskmappingimagelist.Add(taskmappinglist[i]);
                    }
                ref_adapter1 = new GridImageForRefrence(Activity, taskmappingimagelist,FragmentManager);
                grid1.Adapter = ref_adapter1;
                grid1.setExpanded(true);
                if (taskmappinglist[i].FileType.Equals("Video"))
                {
                    taskmappingvideolist.Add(taskmappinglist[i]);
                }
                ref_adapter2 = new GridVideoForReference(Activity, taskmappingvideolist,FragmentManager);
                grid2.Adapter = ref_adapter2;
                grid2.setExpanded(true);

                if (taskmappinglist[i].FileType.Equals("Audio"))
                {
                    taskmappingaudiolist.Add(taskmappinglist[i]);
                }
                ref_adapter3 = new GridAudioForReference(Activity, taskmappingaudiolist);
                grid3.Adapter = ref_adapter3;
                grid3.setExpanded(true);
                //else
                //{

                //}
            }
        }
    }
}