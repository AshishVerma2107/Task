using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using TaskAppWithLogin.Adapter;
using TaskAppWithLogin.Constants;
using TaskAppWithLogin.Models;

namespace TaskAppWithLogin.Fragments
{
    public class ComplainceFrag_OutBox : Fragment
    {
        ServiceHelper restService;
        Geolocation geo;
        DbHelper db;
        SeekBar seekBar;
        InternetConnection ic;
        ISharedPreferences prefs;
        BlobFileUpload blob;
        TextView descrip_text, detail_text, name_text, markby_text, deadline_text, creationdate_text, createdby_text;
        int image_max, video_max, audio_max;
        string max_num;
        string geolocation = "0", task_id_to_send;
        string task_id = "", task_description = "", task_name = "", creation = "", deadline = "", markto = "", taskcreatedby = "", markingDate = "", creationdate = "", markby = "";
        string taskstatus, rownum, meatingid, markingtype, file_format = "", filetype = "", taskoverview, uploaded, shapes_from_Comp, path, filesize, filename;
        TextView txtTimer, Timer, Image_no, Video_no, Audio_no;
        Android.App.ProgressDialog progress;

        ComplianceModel comp;
        List<ComplianceModel> datacomp;
        List<Comp_AttachmentModel> taskuploaded;
        ExpandableHeightGridView Gridview1, Gridview2, Gridview3;
        LinearLayout linear1, linear2, linear3;
        CardView referencecardview;
        ExpandableHeightGridView imagegrid, videolistview, audiolistview, attachmentlistview;
        GridImagecomplianceoutbox gridimagecomp;
        GridVideocomplianceoutbox gridvideocomp;
        GridAudiocomplianceOutbox gridaudiocomp;
        Button play, stop;
        ImageButton create_by_call, mark_by_call;
        List<Comp_AttachmentModel> imagelistoutbox = new List<Comp_AttachmentModel>();
        List<Comp_AttachmentModel> videolistoutbox = new List<Comp_AttachmentModel>();
        List<Comp_AttachmentModel> audiolistoutbox = new List<Comp_AttachmentModel>();


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
            //comp_AttachmentModels = new List<Comp_AttachmentModel>();
            blob = new BlobFileUpload();
            datacomp = new List<ComplianceModel>();
            taskuploaded = new List<Comp_AttachmentModel>();
            task_id_to_send = Arguments.GetString("task_id") ?? string.Empty;
            geolocation = geo.GetGeoLocation(Activity);
            prefs = PreferenceManager.GetDefaultSharedPreferences(Activity);
            //isRecording = false;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            View view = inflater.Inflate(Resource.Layout.layout_compOutBox, null);

            referencecardview = view.FindViewById<CardView>(Resource.Id.referncecardview);

            descrip_text = view.FindViewById<TextView>(Resource.Id.c_descrip);
            name_text = view.FindViewById<TextView>(Resource.Id.c_name);
            // detail_text = view.FindViewById<TextView>(Resource.Id.c_detail);
            markby_text = view.FindViewById<TextView>(Resource.Id.c_markby);
            deadline_text = view.FindViewById<TextView>(Resource.Id.c_deadline);
            createdby_text = view.FindViewById<TextView>(Resource.Id.c_createdby);
            creationdate_text = view.FindViewById<TextView>(Resource.Id.c_creationdate);
            imagegrid = view.FindViewById<ExpandableHeightGridView>(Resource.Id.gridView1);
            videolistview = view.FindViewById<ExpandableHeightGridView>(Resource.Id.gridView2);
            audiolistview = view.FindViewById<ExpandableHeightGridView>(Resource.Id.gridView3);
            //holder = FindViewById<ImageButton>(Resource.Id.location_btn);
            linear1 = view.FindViewById<LinearLayout>(Resource.Id.ll1);
            linear2 = view.FindViewById<LinearLayout>(Resource.Id.ll2);
            linear3 = view.FindViewById<LinearLayout>(Resource.Id.ll3);
            Image_no = view.FindViewById<TextView>(Resource.Id.image_no);
            Video_no = view.FindViewById<TextView>(Resource.Id.video_no);
            Audio_no = view.FindViewById<TextView>(Resource.Id.audio_no);
            create_by_call = view.FindViewById<ImageButton>(Resource.Id.create_by_call);
            mark_by_call = view.FindViewById<ImageButton>(Resource.Id.mark_by_call);

            getData();



            create_by_call.Click += delegate
            {
                try
                {
                    var hasTelephony = Activity.PackageManager.HasSystemFeature(PackageManager.FeatureTelephony);
                    if (hasTelephony)
                    {
                        //var uri = Android.Net.Uri.Parse("tel:" +creat_by_num);
                        var uri = Android.Net.Uri.Parse("tel:" + "9984059984");
                        var intent = new Intent(Intent.ActionDial, uri);
                        Activity.StartActivity(intent);
                    }
                }
                catch (System.Exception e) { }
            };

            mark_by_call.Click += delegate
            {
                try
                {
                    var hasTelephony = Activity.PackageManager.HasSystemFeature(PackageManager.FeatureTelephony);
                    if (hasTelephony)
                    {
                        // var uri = Android.Net.Uri.Parse("tel:" +mark_by_num);
                        var uri = Android.Net.Uri.Parse("tel:" + "9984059984");
                        var intent = new Intent(Intent.ActionDial, uri);
                        Activity.StartActivity(intent);
                    }
                }
                catch (System.Exception e) { }
            };
            // settingValues();
            return view;
        }
        public void storebase()
        {
            ///  db.ComplianceInsertforoutbox(task_id_to_send,markingtype,status,task_created_by,taskcreationdate,task_name,taskdescription,markin);
            //db.taskuploadinsert(comp.lstUploadedCompliance);
        }

        public async Task getData()
        {
            if (ic.connectivity())
            {


                progress = new Android.App.ProgressDialog(Activity);
                progress.Indeterminate = true;
                progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
                progress.SetCancelable(false);
                progress.SetMessage("Please wait...");
                progress.Show();
                dynamic value = new ExpandoObject();
                value.task_id = task_id_to_send;

                string json = JsonConvert.SerializeObject(value);
                try
                {
                    JsonValue item = await restService.GetComplianceTask(Activity, json, geolocation);
                    comp = JsonConvert.DeserializeObject<ComplianceModel>(item);
                    taskuploaded = comp.lstUploadedCompliance;
                    db.complianceoutbox_insert(comp);
                }
                catch (Exception ex)
                {
                    progress.Dismiss();
                }
            }
            else
            {
                datacomp = db.GetCompliance_for_outbox(task_id_to_send);
                db.insertuploadedtask_for_outbox(comp.lstUploadedCompliance);
                taskuploaded = db.gettaskuploaded(task_id_to_send);
                // taskuploaded = comp.lstUploadedCompliance;

                db.insertCommunicationdetail(comp.lstCommunication, task_id_to_send);
                List<CommunicationModel> lstCommunication = db.GetCommunicationModels(task_id_to_send);
            }
            //if (datacomp.Count == 0)
            //{
            //    Toast.MakeText(Activity, "Please connect to the Internet.", ToastLength.Long).Show();
            //}
            //else
            //{


            //}
            // List<CommunicationModel> lstCommunication = comp.lstCommunication;

            if (taskuploaded != null)
            {
                for (int i = 0; i < taskuploaded.Count; i++)
                {
                    if (taskuploaded[i].file_type.Equals("Image"))
                    {
                        imagelistoutbox.Add(taskuploaded[i]);

                    }
                    else if (taskuploaded[i].file_type.Equals("Video"))
                    {
                        videolistoutbox.Add(taskuploaded[i]);
                    }
                    else if (taskuploaded[i].file_type.Equals("Audio"))
                    {
                        audiolistoutbox.Add(taskuploaded[i]);
                    }
                }

                for (int i = 0; i < taskuploaded.Count; i++)
                {
                    filetype = taskuploaded[i].file_type;
                    filename = taskuploaded[i].FileName;
                    file_format = taskuploaded[i].file_format;
                    path = taskuploaded[i].Path;
                    geolocation = taskuploaded[i].GeoLocation;
                    filesize = taskuploaded[i].FileSize;
                    if (filetype == "Image")
                    {

                        gridimagecomp = new GridImagecomplianceoutbox(Activity, imagelistoutbox, FragmentManager);
                        imagegrid.Adapter = gridimagecomp;
                        imagegrid.setExpanded(true);
                    }
                    if (filetype == "Video")
                    {
                        gridvideocomp = new GridVideocomplianceoutbox(Activity, videolistoutbox, FragmentManager);
                        videolistview.Adapter = gridvideocomp;
                        videolistview.setExpanded(true);
                    }
                    if (filetype == "Audio")
                    {
                        gridaudiocomp = new GridAudiocomplianceOutbox(Activity, audiolistoutbox, FragmentManager);
                        audiolistview.Adapter = gridaudiocomp;
                        audiolistview.setExpanded(true);

                    }

                }

                //task_id = comp.task_id;
                //task_description = comp.description;
                //deadline = comp.deadline_date;
                //meatingid = comp.Meeting_ID;
                //rownum = comp.RowNo;
                ////taskcreationDate = comp.task_creation_date;
                //markby = comp.task_mark_by;
                //taskstatus = comp.taskStatus;
                //markto = comp.markTo;
                //markingtype = comp.task_marking_type;
                //taskcreatedby = comp.task_created_by;
                //markingDate = comp.MarkingDate;
                //creationdate = comp.task_creation_date;
                //shapes_from_Comp = comp.shapes;
                //task_name = comp.task_name;


                progress.Dismiss();

            }

            settingValues();

            for (int i = 0; i <= taskuploaded.Count; i++)
            {
                max_num = comp.lstUploadedCompliance[i].max_numbers;
                file_format = comp.lstUploadedCompliance[i].file_format;
                filetype = comp.lstUploadedCompliance[i].file_type;
                //  taskoverview = comp.lstUploadedCompliance[i].task_overview;
                //uploaded = comp.lstUploadedCompliance[i].Uploaded;

                if (filetype.Equals("Image"))
                {
                    image_max = Convert.ToInt32(max_num);
                }
                else if (filetype.Equals("Video"))
                {
                    video_max = Convert.ToInt32(max_num);
                }
                else if (filetype.Equals("Audio"))
                {
                    audio_max = Convert.ToInt32(max_num);
                }
                //Image_no.Text = image_max.ToString();
                //Video_no.Text = video_max.ToString();
                //Audio_no.Text = audio_max.ToString();

            }


        }

        public void settingValues()
        {
            //task_name = comp.task_name;
            //task_description = comp.description;
            //markby = comp.task_mark_by;
            //markto = comp.markTo;
            //deadline = comp.deadline_date;
            //creation = comp.task_created_by;
            //creationdate = comp.task_creation_date;
            if (ic.connectivity())
            {
                name_text.Text = comp.task_name;
                descrip_text.Text = comp.description;
                markby_text.Text = comp.task_mark_by;
                deadline_text.Text = comp.deadline_date;
                createdby_text.Text = comp.task_created_by;
                creationdate_text.Text = comp.task_creation_date;
            }
            else
            {
                for (int j = 0; j < datacomp.Count; j++)
                {
                    name_text.Text = datacomp[j].task_name;
                    descrip_text.Text = datacomp[j].description;
                    markby_text.Text = datacomp[j].task_mark_by;
                    deadline_text.Text = datacomp[j].deadline_date;
                    createdby_text.Text = datacomp[j].task_created_by;
                    creationdate_text.Text = datacomp[j].task_creation_date;
                }
            }



        }
    }
}