using System;
using System.Collections.Generic;
using System.Dynamic;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using TaskAppWithLogin.Constants;
using TaskAppWithLogin.Models;
using static Android.App.ActionBar;

namespace TaskAppWithLogin.Fragments
{
    public class UsermarkedFragment : Fragment
    {
        TextView nametext, nametag, designationtag, designationtext, markedontext, markedtag, deadlinetext, deadlinetag;
        EditText tasknametext, taskdescription;
        Button createtask;
        ServiceHelper restService;
        InternetConnection con;
        string markto;
        string taskname = CreateTaskFrag.name ;
        string taskdescri = CreateTaskFrag.comment;
        string npname;
        string deadline = "";
        string Desingnation;
        string markedOn;
        Geolocation geo;
        string geolocation="0";
        DbHelper db;
        ISharedPreferences prefs;
        BlobFileUpload blob;
        Android.App.ProgressDialog progress;
        LinearLayout llimage, llvideo, llaudio;
        ImageView img,imgaudio,imgvideo;
        int imgheight = 50;
        int imgwidth = 50;
        List<TaskFileMapping_Model> listmapping2 = new List<TaskFileMapping_Model>();
        List<ComplianceJoinTable> lstaddcompliance = new List<ComplianceJoinTable>();

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            restService = new ServiceHelper();
            con = new InternetConnection();
            geo = new Geolocation();
            blob = new BlobFileUpload();
            db = new DbHelper();
            listmapping2 = CreateTaskFrag.listmapping;
            //lstaddcompliance = AddComplianceInCreate.modelsaddcompliance;
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.user_marked_layout, container, false);
            nametext = view.FindViewById<TextView>(Resource.Id.nametext);
            nametag = view.FindViewById<TextView>(Resource.Id.nametag);
            designationtag = view.FindViewById<TextView>(Resource.Id.designationtag);
            designationtext = view.FindViewById<TextView>(Resource.Id.designationtext);
            markedontext = view.FindViewById<TextView>(Resource.Id.markedontext);
            markedtag = view.FindViewById<TextView>(Resource.Id.markedontag);
            deadlinetag = view.FindViewById<TextView>(Resource.Id.deadlinetag);
            deadlinetext = view.FindViewById<TextView>(Resource.Id.deadlinetext);
            createtask = view.FindViewById<Button>(Resource.Id.button1);
            tasknametext = view.FindViewById<EditText>(Resource.Id.editTaskname);
            taskdescription = view.FindViewById<EditText>(Resource.Id.editTaskdescription);
            llaudio = view.FindViewById<LinearLayout>(Resource.Id.llaudio);
            llimage = view.FindViewById < LinearLayout>(Resource.Id.llimage);
            llvideo = view.FindViewById<LinearLayout>(Resource.Id.llvideo);
           
            //prefs = PreferenceManager.GetDefaultSharedPreferences(Activity);
            //npname = prefs.GetString("NPName_Marking", "");
            //markto = prefs.GetString("DesignationId_Marking", "");
            //Desingnation = prefs.GetString("DesignationName_Marking", "");


            npname = Arguments.GetString("Name");
            markto = Arguments.GetString("DesignationId");
            Desingnation = Arguments.GetString("Designation");

            deadline = CreateTaskFrag.date + " " + CreateTaskFrag.time1;
            markedOn = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
           
            tasknametext.Text = CreateTaskFrag.name;
            taskdescription.Text = CreateTaskFrag.comment;

            deadlinetext.Text = deadline.ToString();
            markedontext.Text = markedOn.ToString();

            designationtext.Text = Desingnation.ToString();
            nametext.Text = npname.ToString();
            createtask.Click += delegate
            {
                if(tasknametext.Text== null || tasknametext.Text == "" || taskdescription.Text== null || taskdescription.Text =="")
                {
                    Toast.MakeText(Activity, "Fields cannot be empty or blank.", ToastLength.Long).Show();
                }
                else
                {
                    CreateTask();
                }
            };
            for(int i=0; i < listmapping2.Count; i++)
            {
                if (listmapping2[i].FileType.Equals("Image"))
                {
                    //image_list.Add(listmapping[i]);
                    LayoutParams lparmas = new LayoutParams(200, 250);
                    
                    ImageView img = new ImageView(Activity);
                    Bitmap bitmap = BitmapFactory.DecodeFile(listmapping2[i].Path);
                    img.LayoutParameters = lparmas;
                    lparmas.LeftMargin = 30;
                   
                   // img.LayoutParameters = new LinearLayout.LayoutParams(200, 250);
                    img.SetX(10);
                    img.SetY(10);
                    img.SetImageBitmap(bitmap);
                    llimage.AddView(img);
                }
          
            }
            for (int i = 0; i < listmapping2.Count; i++)
            {
                if (listmapping2[i].FileType.Equals("Video"))
                {
                    ImageView img = new ImageView(Activity);

                    Bitmap bitmap = BitmapFactory.DecodeFile(listmapping2[i].localPath);

                    img.LayoutParameters = new LinearLayout.LayoutParams(200, 250);

                    img.SetX(10);
                    img.SetY(10);
                    img.SetImageResource(Resource.Drawable.videofile);
                    //img.SetImageBitmap(bitmap);
                    llvideo.AddView(img);
                }
            }

            for (int i = 0; i < listmapping2.Count; i++)
            {
                if (listmapping2[i].FileType.Equals("Audio"))
                {
                    ImageView img = new ImageView(Activity);

                  //  Bitmap bitmap = BitmapFactory.DecodeFile(listmapping2[i].Path);

                    img.LayoutParameters = new LinearLayout.LayoutParams(200, 250);
                   

                    img.SetX(10);
                    img.SetY(10);
                    img.SetImageResource(Resource.Drawable.audiofile);
                   // img.SetImageBitmap(bitmap);
                    llaudio.AddView(img);
                }
            }


            return view;
        }

        public void CreateTask()
        {

            Boolean connectivity = con.connectivity();
            if (connectivity)
            {
                geolocation = geo.GetGeoLocation(Context);
                CreatetaskService();
            }
            else
            {
                // db.InsertCreateTaskData(taskname, taskdescri, deadline,"mobile",markto,"no",listmapping2);
                Android.App.AlertDialog.Builder alertDiag = new Android.App.AlertDialog.Builder(Activity);
                alertDiag.SetTitle("You don't have Internet connection. Check your connection or Save Task for Later");
                alertDiag.SetMessage("press Ok to continue");
                alertDiag.SetPositiveButton("Save", (senderAlert, args) =>
                {
                    //markingtaskforlaterAsync();
                    db.InsertCreateTaskData(taskname, taskdescri, deadline, "mobile", markto, "unassign", listmapping2, CreateTaskFrag.modelsaddcompliance);
                    Toast.MakeText(Activity, "Task have been saved successfully. To assign Task manually sync the data. ", ToastLength.Long).Show();

                });
                alertDiag.SetNegativeButton("Cancel", (senderAlert, args) =>
                {
                    alertDiag.Dispose();
                });
                Android.App.Dialog diag = alertDiag.Create();
                diag.Show();

                tasknametext.Text = null;
                taskdescription.Text = null;

                deadlinetext.Text = null;
                markedontext.Text = null;

                designationtext.Text = null;
                nametext.Text = null;

                Toast.MakeText(Activity, "No Internet Connection. Data has been saved locally.", ToastLength.Long).Show();
            }
        }

        public byte[] GetStreamFromFile(string filePath)
        {
            try
            {
                Android.Net.Uri uri = Android.Net.Uri.FromFile(new Java.IO.File(filePath));
                byte[] byteArray = System.IO.File.ReadAllBytes(uri.Path);
                return byteArray;
            }
            catch (System.Exception e)
            {
                return null;
            }
        }

        public async System.Threading.Tasks.Task markingtaskforlaterAsync()
        {
           // lstcomplinace = modelsaddcompliance;
            dynamic value = new ExpandoObject();
            value.task_name = taskname;
            value.description = taskdescri;
            value.deadline_date = deadline;
            value.taskCreatethrough = "mobile";
            //value.markTo = markto;
            // value.lstTaskFileMapping = listmapping2;
            value.lstAddedCompliance = CreateTaskFrag.modelsaddcompliance;
            string json = JsonConvert.SerializeObject(value);

            try
            {

                string item = await restService.CreateTaskMethod(Activity, json, geolocation);
                if (item.Contains(""))
                {
                    //db.InsertCreateTaskData(taskname, taskdescri, deadline, "mobile", markto, "yes");


                    //Toast.MakeText(Activity, "Task Assign Successfully..", ToastLength.Long).Show();
                    Toast.MakeText(Activity, "Task Assign Successfully...", ToastLength.Long).Show();
                    // progress.Dismiss();
                }
                else
                {
                    //db.InsertCreateTaskData(taskname, taskdescri, deadline, "mobile", markto, "yes", listmapping2, CreateTaskFrag.modelsaddcompliance);

                    //tasknametext.Text = null;
                    //taskdescription.Text = null;

                    //deadlinetext.Text = null;
                    //markedontext.Text = null;

                    //designationtext.Text = null;
                    //nametext.Text = null;

                    Toast.MakeText(Activity, "Task saved Successfully.. you have not internet connection", ToastLength.Long).Show();
                    // progress.Dismiss();
                }
            }
            catch (Exception ex)
            {
                // progress.Dismiss();
            }

            // progress.Dismiss();

        }


        private async System.Threading.Tasks.Task CreatetaskService()
        {

            progress = new Android.App.ProgressDialog(Activity);
            progress.Indeterminate = true;
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            progress.SetCancelable(false);
            progress.SetMessage("Please wait...");
            progress.Show();

            //models = db.GetFullAttachmentData(task_id_to_send);

            for (int i = 0; i < listmapping2.Count; i++)
            {


                byte[] img = GetStreamFromFile(listmapping2[i].localPath);
                var url1 = await blob.UploadPhotoAsync(img, listmapping2[i].localPath.Substring(listmapping2[i].localPath.LastIndexOf("/") + 1));

                if (url1 != null)
                {
                    listmapping2[i].Path = url1;
                }
            }
            dynamic value = new ExpandoObject();
            value.task_name = taskname;
            value.description = taskdescri;
            value.deadline_date = deadline;
            value.taskCreatethrough = "mobile";
            value.markTo = markto;
            value.lstTaskFileMapping = listmapping2;
            value.lstAddedCompliance =CreateTaskFrag.modelsaddcompliance;
            string json = JsonConvert.SerializeObject(value);

            try
            {

                string item = await restService.CreateTaskMethod(Activity, json, geolocation);
                if (item.Contains("Task Created successfully"))
                {
                    //db.InsertCreateTaskData(taskname, taskdescri, deadline, "mobile", markto, "yes");


                    //Toast.MakeText(Activity, "Task Assign Successfully..", ToastLength.Long).Show();
                    Toast.MakeText(Activity, "Task Assign Successfully...", ToastLength.Long).Show();
                    CreateTaskFrag.modelsaddcompliance.Clear();
                    progress.Dismiss();
                }
                else
                {
                    //db.InsertCreateTaskData(taskname, taskdescri, deadline, "mobile", markto,"yes",listmapping2);

                    tasknametext.Text = null;
                    taskdescription.Text = null;

                    deadlinetext.Text = null;
                    markedontext.Text = null;

                    designationtext.Text = null;
                    nametext.Text = null;

                    Toast.MakeText(Activity, "Task saved Successfully.. you have not internet connection", ToastLength.Long).Show();
                    progress.Dismiss();
                }
            }
            catch (Exception ex)
            {
                progress.Dismiss();
            }

            progress.Dismiss();

        }
        
    }
}