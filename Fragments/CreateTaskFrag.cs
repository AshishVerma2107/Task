using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Timers;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Preferences;
using Android.Provider;
using Android.Runtime;
using Android.Speech;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Java.Text;
using Newtonsoft.Json;
using RadialProgress;
using TaskAppWithLogin.Adapter;
using TaskAppWithLogin.Constants;
using TaskAppWithLogin.Models;
using static Android.App.TimePickerDialog;

namespace TaskAppWithLogin.Fragments
{

    public class CreateTaskFrag : Fragment, IOnTimeSetListener
    {
        private readonly int Video = 20;
        private readonly int Camera = 10;
        private readonly int VOICE = 30;
        private readonly int fileattachment = 50;
        private bool isRecording;
        string Click_Type, task_id_to_send;
        int hour = 00, min = 00, sec = 00;
        ImageView _dateSelectButton, timeSelectButton;
        TextView _dateDisplay, timeDisplay, Timer;
        EditText task_name, task_comment,max_number;
        Button createbutton;
        Java.IO.File fileName1, fileImagePath, fileaudioPath, fileVideoPath, audiofile;
        // private bool isRecording;
        // private readonly int VOICE = 10;
        private readonly int DESC = 40;
        public static string date, time1;
        public static string name, comment;
        ImageView title, desc;
        DbHelper db;
        string imageName, imageURL, videoURL, videoName, audioname;
        string AudioSavePathInDevice = null;
        RadialProgressView radialProgrssView;
        private Handler mHandler = new Handler();
        ImageButton camera, video, microphone, attachment_btn;
        public static ExpandableHeightGridView Gridview_1, complianceGridview, Gridview_2, Gridview_3,Grid_attach;
        public static List<TaskFileMapping_Model> image_list;
        public static List<TaskFileMapping_Model> video_list;
        public static List<TaskFileMapping_Model> audio_list;
        public static GridImageAdapterCreatetask adapter_1;
        public static GridVideoAdapterCreateTask adapter_2;
        public static GridAudioCreateTask adapter_3;
        Action action;
        public static List<ComplianceJoinTable> modelsaddcompliance;
        public static int imageCount, videoCount, audioCount;
        MediaPlayer mediaPlayer;
        Timer timer;
        MediaRecorder mediaRecorder;
        SeekBar seekBar;
        Geolocation geo;
        // DbHelper db;
        InternetConnection ic;
        ISharedPreferences prefs;
        string geolocation;
        string licenceId;
        Button recordbtn, resumebtn, pausebtn, savebtn, Done_Btn,Saveforlater, Addtolist;
        ImageView comment_micro, playbtn, stopbtn;
        public static  List<CreateTaskLicenceIdReturnModel> licenceidmodel;
        public static   List<TaskFileMapping_Model> listmapping;
        string DesignationId;
        public static   string tempId;
        public static string taskid;
        //RadioButton checkBox1, checkBox2;
        //Spinner spinnertype, spinnerextension;
        //string compliancetype;
        //string max_num;
        //int max_numbers;
        //string filetype, file_format;
        CardView cardView, cardviewref;
        List<ComplianceJoinTable> lstcomplinace;
        public static   GridAttachmentForCreate forCreate;
        ServiceHelper restService;
        public override void OnCreate(Bundle savedInstanceState)
        {
            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());
            StrictMode.ThreadPolicy.Builder builder1 = new StrictMode.ThreadPolicy.Builder().PermitAll();
            StrictMode.SetThreadPolicy(builder1.Build());
            image_list = new List<TaskFileMapping_Model>();
            audio_list = new List<TaskFileMapping_Model>();
            video_list = new List<TaskFileMapping_Model>();
            listmapping = new List<TaskFileMapping_Model>();
            modelsaddcompliance = new List<ComplianceJoinTable>();
            licenceidmodel = new List<CreateTaskLicenceIdReturnModel>();
            lstcomplinace = new List<ComplianceJoinTable>();
            restService = new ServiceHelper();
            prefs = PreferenceManager.GetDefaultSharedPreferences(Activity);
            DesignationId = prefs.GetString("DesignationId", "");
            geo = new Geolocation();
          
            // db = new DbHelper();
            ic = new InternetConnection();
           
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            View view = inflater.Inflate(Resource.Layout.create_layout2, null);
            db = new DbHelper();
            geolocation = geo.GetGeoLocation(Activity);
            HasOptionsMenu = true;
            isRecording = false;
            prefs = PreferenceManager.GetDefaultSharedPreferences(Activity);
            licenceId = prefs.GetString("LicenceId", "");
            tempId = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + licenceId;
            _dateSelectButton = view.FindViewById<ImageView>(Resource.Id.imageView);
            timeSelectButton = view.FindViewById<ImageView>(Resource.Id.imageView2);
            max_number = view.FindViewById<EditText>(Resource.Id.maxnumberedit);
            task_name = view.FindViewById<EditText>(Resource.Id.ed1);
            task_comment = view.FindViewById<EditText>(Resource.Id.ed2);
            createbutton = view.FindViewById<Button>(Resource.Id.btn);
            Saveforlater = view.FindViewById<Button>(Resource.Id.saveforlater);
            Addtolist = view.FindViewById<Button>(Resource.Id.btn_addtolist);
            createbutton.Click += CreateButton_OnClick;
            _dateDisplay = view.FindViewById<TextView>(Resource.Id.textView1);
            timeDisplay = view.FindViewById<TextView>(Resource.Id.textView2);
            title = view.FindViewById<ImageView>(Resource.Id.imageView1);
            desc = view.FindViewById<ImageView>(Resource.Id.imageView3);
            camera = view.FindViewById<ImageButton>(Resource.Id.camera_btn);
            video = view.FindViewById<ImageButton>(Resource.Id.video_btn);
            microphone = view.FindViewById<ImageButton>(Resource.Id.micro_btn);
            cardView = view.FindViewById<CardView>(Resource.Id.cardView_addcomp);
            attachment_btn = view.FindViewById<ImageButton>(Resource.Id.attachment);
            cardviewref = view.FindViewById<CardView>(Resource.Id.cardView_addref);
            /// attachment_btn = view.FindViewById<ImageButton>(Resource.Id.attachment);
            cardviewref.Click += delegate
            {
                CreateRefAddFragment createRefAdd = new CreateRefAddFragment();
                createRefAdd.Show(FragmentManager, "CreateRefAddFragment");
                //FragmentTransaction ft = FragmentManager.BeginTransaction();
                //ft.Replace(Resource.Id.container, createRefAdd);
                ////ft.Hide(FragmentManager.FindFragmentByTag("CreateTaskFrag"));
                ////ft.Add(Resource.Id.container, createRefAdd);
                //ft.AddToBackStack(null);
                //ft.SetTransition(FragmentTransaction.TransitFragmentOpen);
                //ft.Commit();
            };
            //attachment_btn.Click += delegate
            //{
            //    attachmentClick();
            //};
            cardView.Click += delegate
            {
               AddComplianceInCreate nextFrag = new AddComplianceInCreate();
                nextFrag.Show(FragmentManager, "AddComplianceInCreate");

                //FragmentTransaction ft = FragmentManager.BeginTransaction();
                //ft.Replace(Resource.Id.container, nextFrag);
                ////ft.Hide(FragmentManager.FindFragmentByTag("CreateTaskFrag"));
                ////ft.Add(Resource.Id.container, nextFrag);
                //ft.AddToBackStack(null);
                //ft.SetTransition(FragmentTransaction.TransitFragmentOpen);
                //ft.Commit();



                // fragment.BeginTransaction().Replace(Resource.Id.container, nextFrag).Commit();
                //Bundle bundle = new Bundle();
                ////bundle.PutString("task_id", id);
                //nextFrag.Arguments = bundle;

            };
            //checkBox1 = view.FindViewById<RadioButton>(Resource.Id.mandatory);
            //checkBox2 = view.FindViewById<RadioButton>(Resource.Id.not);
            //spinnerextension = view.FindViewById<Spinner>(Resource.Id.spiner_format);
            //spinnertype = view.FindViewById<Spinner>(Resource.Id.spinner_type);
           // complianceGridview = view.FindViewById<ExpandableHeightGridView>(Resource.Id.grid_compliance);
            Gridview_1 = view.FindViewById<ExpandableHeightGridView>(Resource.Id.gridView1);
            Grid_attach = view.FindViewById<ExpandableHeightGridView>(Resource.Id.gridattachment);
            Gridview_2 = view.FindViewById<ExpandableHeightGridView>(Resource.Id.gridView2);
            Gridview_3 = view.FindViewById<ExpandableHeightGridView>(Resource.Id.gridView3);
            //checkBox1.Click += RadioButtonClick;
            //checkBox2.Click += RadioButtonClick;
            Saveforlater.Click += Saveforlater_Click;
            //max_num = max_number.Text;
            //max_numbers = Convert.ToInt32(max_num);
            //Addtolist.Click += delegate
            //{
            //    addtolistcompliance();
            //};
            //    microphone.Click += delegate
            //{
            //    recording();
            //};
            _dateSelectButton.Click += (sender, e) =>
            {
                DateSelect_OnClick(sender, e);
            };

            //camera.Click += delegate
            //{
            //    //Click_Type = "Camera";
            //    //CheckForShapeData_Camera();
            //    BtnCamera_Click();

            //};

            //video.Click += delegate
            //{
            //    //Click_Type = "Video";
            //    //CheckForShapeData_Video();
            //    VideoClick();
            //};

            //_dateSelectButton.Click += DateSelect_OnClick;
            timeSelectButton.Click += TimeSelectOnClick;
            string rec = Android.Content.PM.PackageManager.FeatureMicrophone;
            if (rec != "android.hardware.microphone")
            {
                var alert = new Android.App.AlertDialog.Builder(title.Context);
                alert.SetTitle("You don't seem to have a microphone to record with");
                alert.SetPositiveButton("OK", (sender, e) =>
                {
                    return;
                });

                alert.Show();
            }
            else
            {
                title.Click += delegate
                {
                    
                    isRecording = !isRecording;
                    if (isRecording)
                    {
                       
                        var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
                        voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);
                        voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, Android.App.Application.Context.GetString(Resource.String.messageSpeakNow));
                        voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
                        voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
                        voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
                        voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);

                        voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
                        StartActivityForResult(voiceIntent, VOICE);
                    }
                };
                desc.Click += delegate
                {
                    isRecording = !isRecording;
                    if (isRecording)
                    {
                        // create the intent and start the activity
                        var voiceIntent = new Intent(RecognizerIntent.ActionRecognizeSpeech);
                        voiceIntent.PutExtra(RecognizerIntent.ExtraLanguageModel, RecognizerIntent.LanguageModelFreeForm);

                        // put a message on the modal dialog
                        voiceIntent.PutExtra(RecognizerIntent.ExtraPrompt, Android.App.Application.Context.GetString(Resource.String.messageSpeakNow));

                        // if there is more then 1.5s of silence, consider the speech over
                        voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputCompleteSilenceLengthMillis, 1500);
                        voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputPossiblyCompleteSilenceLengthMillis, 1500);
                        voiceIntent.PutExtra(RecognizerIntent.ExtraSpeechInputMinimumLengthMillis, 15000);
                        voiceIntent.PutExtra(RecognizerIntent.ExtraMaxResults, 1);

                        // you can specify other languages recognised here, for example
                        // voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.German);
                        // if you wish it to recognise the default Locale language and German
                        // if you do use another locale, regional dialects may not be recognised very well

                        voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
                        StartActivityForResult(voiceIntent, DESC);
                    }
                };
            }
            return view;
        }

        
        private void Saveforlater_Click(object sender, EventArgs e)
        {
            db.insertCreateTasklicenceid(licenceId);
            licenceidmodel = db.Getlicneceidcreate();
            for(int i=0; i < licenceidmodel.Count; i++)
            {
                taskid = licenceidmodel[i].taskid.ToString();
            }
            markingtaskforlaterAsync();
            
            name = task_name.Text;
            comment = task_comment.Text;
            db.Insert_InitialTask(name, taskid ,comment, date, time1, listmapping, DesignationId);
            db.Insert_InitialMapping(listmapping,taskid);
            task_name.Text =  null;
            task_comment.Text = null;
            _dateDisplay.Text = null;
            timeDisplay.Text = null;
            Toast.MakeText(Activity, " Data saved for later ", ToastLength.Long).Show();
        }
        public async System.Threading.Tasks.Task markingtaskforlaterAsync()
        {
            lstcomplinace = modelsaddcompliance;
            dynamic value = new ExpandoObject();
            value.task_name = name;
            value.description = comment;
            value.deadline_date = date;
            value.taskCreatethrough = "mobile";
            //value.markTo = markto;
           // value.lstTaskFileMapping = listmapping2;
            value.lstAddedCompliance = lstcomplinace;
            string json = JsonConvert.SerializeObject(value);

            try
            {

                string item = await restService.CreateTaskMethod(Activity, json, geolocation);
                if (item.Contains("Task Created successfully"))
                {
                    //db.InsertCreateTaskData(taskname, taskdescri, deadline, "mobile", markto, "yes");


                    //Toast.MakeText(Activity, "Task Assign Successfully..", ToastLength.Long).Show();
                    Toast.MakeText(Activity, "Task Assign Successfully...", ToastLength.Long).Show();
                   // progress.Dismiss();
                }
                else
                {

                    Toast.MakeText(Activity, "An error occured", ToastLength.Long).Show();
                  
                }
            }
            catch (Exception ex)
            {
               // progress.Dismiss();
            }

            // progress.Dismiss();

        }



        //private void BtnCamera_Click()
        //    {
        //        Java.IO.File path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
        //        fileName1 = new Java.IO.File(path, "TaskApp");
        //        if (!fileName1.Exists())
        //        {
        //            fileName1.Mkdirs();
        //        }
        //        imageName = Utility.fileName();

        //        Intent intent = new Intent(MediaStore.ActionImageCapture);
        //        fileImagePath = new Java.IO.File(fileName1, string.Format(imageName, Guid.NewGuid()));
        //        imageURL = fileImagePath.AbsolutePath;
        //        intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(fileImagePath));
        //        intent.AddFlags(ActivityFlags.GrantReadUriPermission);
        //        StartActivityForResult(intent, Camera);
        //    }
        //    private void VideoClick()
        //    {
        //        Java.IO.File path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
        //        fileName1 = new Java.IO.File(path, "TaskApp");
        //        if (!fileName1.Exists())
        //        {
        //            fileName1.Mkdirs();
        //        }
        //        videoName = Utility.fileName2();
        //        Intent intent = new Intent(MediaStore.ActionVideoCapture);
        //        fileVideoPath = new Java.IO.File(fileName1, string.Format(videoName, Guid.NewGuid()));
        //        videoURL = fileVideoPath.AbsolutePath;
        //        intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(fileVideoPath));
        //        intent.PutExtra(MediaStore.ExtraVideoQuality, 1);
        //        // intent.PutExtra(MediaStore.ExtraDurationLimit, 10);
        //        StartActivityForResult(intent, Video);
        //    }
        //    public void attachmentClick()
        //    {
        //        Intent intent = new Intent(Intent.ActionGetContent);
        //        StartActivityForResult(intent, fileattachment);


        //    }
        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            //        if (requestCode == Camera)
            //        {
            //            if (!imageURL.Equals(""))
            //            {

            //                // Bitmap bitmap = BitmapFactory.DecodeFile(imageURL);
            //                Bitmap bitmap;

            //                //Converstion Image Size  
            //                int height = Resources.DisplayMetrics.HeightPixels;
            //                int width = Resources.DisplayMetrics.WidthPixels;
            //                using (bitmap = fileImagePath.Path.LoadAndResizeBitmap(width / 4, height / 4))
            //                {

            //                }
            //                //imagecaptured.SetImageBitmap(bitmap);

            //                long size1 = fileImagePath.Length() / 1024 * 1024;
            //                string imgsize = size1.ToString();

            //                //for (int i = 0; i < licenceidmodel.Count - 1; i++)
            //                //{
            //                //    str
            //                //}
            //                TaskFileMapping_Model attachmentModel = new TaskFileMapping_Model();
            //                attachmentModel.Path = imageURL;
            //                attachmentModel.FileType = "Image";
            //                attachmentModel.FileName = imageName;
            //                //attachmentModel.localtaskId = licenceidmodel[licenceidmodel.Count - 1].taskid.ToString();
            //                // attachmentModel.file_format = Utility.imagetype;
            //                attachmentModel.FileSize = imgsize;
            //                // attachmentModel.GeoLocation = geolocation;
            //                //  attachmentModel.max_numbers = image_max.ToString();
            //                listmapping.Add(attachmentModel);

            //                //comp_AttachmentModels.Add(attachmentModel);


            //                //imagelist.AddRange(comp_AttachmentModels.Where(p => p.Attachment_Type == "Image" ));
            //                //image_list = db.GetCreateAttachmentData("Image", licenceidmodel[licenceidmodel.Count - 1].taskid.ToString());
            //                //for (int i = 0; i < image_list.Count; i++)
            //                //{

            //                //}
            //                for(int i = 0; i < listmapping.Count; i++)
            //                {
            //                    if(listmapping[i].FileType.Equals("Image"))
            //                    {
            //                        image_list.Add(listmapping[i]);
            //                    }
            //                }
            //                adapter_1 = new GridImageAdapterCreatetask(Activity, image_list);
            //                Gridview_1.Adapter = adapter_1;
            //                Gridview_1.setExpanded(true);
            //                Gridview_1.ChoiceMode = (ChoiceMode)AbsListViewChoiceMode.MultipleModal;
            //                Gridview_1.SetMultiChoiceModeListener(new MultiChoiceModeListener1(Activity));
            //                imageCount++;
            //            }

            //        }
            //        if (requestCode == Video)
            //        {
            //           if (requestCode == Video && resultCode == (int)Android.App.Result.Ok)
            //                {

            //                long size2 = fileVideoPath.Length() / 1024;
            //                string videosize = size2.ToString();

            //                TaskFileMapping_Model attachmentModel = new TaskFileMapping_Model();

            //               // attachmentModel.Path = videoURL;
            //                attachmentModel.FileType = "Video";
            //                attachmentModel.FileName = videoName;
            //                attachmentModel.localtaskId = task_id_to_send;
            //                attachmentModel.localPath = videoURL;
            //                // attachmentModel.file_format = Utility.videotype;
            //                attachmentModel.FileSize = videosize;
            //                // attachmentModel.GeoLocation = geolocation;
            //                // attachmentModel.max_numbers = video_max.ToString();
            //                //  db.InsertCreateAttachData(attachmentModel);
            //                listmapping.Add(attachmentModel);
            //                //comp_AttachmentModels.Add(attachmentModel);
            //                for (int i = 0; i < listmapping.Count; i++)
            //                {
            //                    if (listmapping[i].FileType.Equals("Video"))
            //                    {
            //                        video_list.Add(listmapping[i]);
            //                    }
            //                }

            //                //imagelist.AddRange(comp_AttachmentModels.Where(p => p.Attachment_Type == "Image" ));
            //                //video_list = db.GetCreateAttachmentData("Video",licenceidmodel[0].taskid.ToString());

            //                adapter_2 = new GridVideoAdapterCreateTask(Activity, video_list);
            //                Gridview_2.Adapter = adapter_2;
            //                Gridview_2.setExpanded(true);
            //                Gridview_2.ChoiceMode = (ChoiceMode)AbsListViewChoiceMode.MultipleModal;
            //                Gridview_2.SetMultiChoiceModeListener(new MultiChoiceModeListener2(Activity));

            //                videoCount++;

            //            }

            //        }
            //        if(requestCode==fileattachment && resultCode== (int)Android.App.Result.Ok)
            //        {
            //            string filepath = data.Data.Path;
            //            String fileName = filepath.Substring(filepath.LastIndexOf("/") + 1);
            //            TaskFileMapping_Model attachmentmodel = new TaskFileMapping_Model();
            //            attachmentmodel.Path = filepath;
            //            attachmentmodel.FileType = "Attachment";
            //            attachmentmodel.FileName =fileName;
            //            listmapping.Add(attachmentmodel);
            //            forCreate = new GridAttachmentForCreate(Activity,listmapping);
            //            Grid_attach.Adapter = forCreate;
            //            Grid_attach.setExpanded(true);
            //        }
            //        //if (requestCode == Video && resultCode == (int)Android.App.Result.Ok)
            //        //{
            //        //    if (!videoURL.Equals(""))
            //        //    {
            //        //        long size2 = fileVideoPath.Length() / 1024;
            //        //        string videosize = size2.ToString();

            //        //        for (int i = 0; i < listmapping.Count; i++)
            //        //        {
            //        //            if (listmapping[i].FileType.Equals("Video"))
            //        //            {
            //        //                video_list.Add(listmapping[i]);
            //        //            }
            //        //        }
            //        //        TaskFileMapping_Model attachmentModel = new TaskFileMapping_Model();

            //        //        attachmentModel.Path = videoURL;
            //        //        attachmentModel.FileType = "Video";
            //        //        attachmentModel.FileName = audioname;
            //        //        attachmentModel.localtaskId = task_id_to_send;
            //        //        attachmentModel.localPath = videoURL;
            //        //        // attachmentModel.file_format = Utility.videotype;
            //        //        attachmentModel.FileSize = videosize;
            //        //        // attachmentModel.GeoLocation = geolocation;
            //        //        // attachmentModel.max_numbers = video_max.ToString();
            //        //        //  db.InsertCreateAttachData(attachmentModel);
            //        //        listmapping.Add(attachmentModel);
            //        //        //    //comp_AttachmentModels.Add(attachmentModel);


            //        //        //comp_AttachmentModels.Add(attachmentModel);


            //        //        //imagelist.AddRange(comp_AttachmentModels.Where(p => p.Attachment_Type == "Image" ));
            //        //        adapter_2 = new GridVideoAdapterCreateTask(Activity, video_list);
            //        //        Gridview_2.Adapter = adapter_2;
            //        //        Gridview_2.setExpanded(true);
            //        //        Gridview_2.ChoiceMode = (ChoiceMode)AbsListViewChoiceMode.MultipleModal;
            //        //        Gridview_2.SetMultiChoiceModeListener(new MultiChoiceModeListener2(Activity));

            //        //        videoCount++;
            //        //    }

            //        //}
            if (requestCode == VOICE)
            {
                if (resultCode == -1)
                {
                    var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                    if (matches.Count != 0)
                    {
                        string textInput = task_name.Text + matches[0];

                        // limit the output to 500 characters
                        if (textInput.Length > 500)
                            textInput = textInput.Substring(0, 500);
                        task_name.Text = textInput;
                    }
                    else
                        task_name.Text = "No speech was recognised";
                    // change the text back on the button

                }
            }
            if (requestCode == DESC)
            {
                if (resultCode == -1)
                {
                    var matches1 = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                    if (matches1.Count != 0)
                    {
                        string textInput1 = task_comment.Text + matches1[0];

                        // limit the output to 500 characters
                        if (textInput1.Length > 500)
                            textInput1 = textInput1.Substring(0, 500);
                        task_comment.Text = textInput1;
                    }
                    else
                        task_comment.Text = "No speech was recognised";
                    // change the text back on the button

                }
            }

        }
    //    private void recording()
    //    {
    //        View view = LayoutInflater.Inflate(Resource.Layout.audio_recorder, null);
    //        Android.App.AlertDialog builder = new Android.App.AlertDialog.Builder(Activity).Create();
    //        builder.SetView(view);
    //        builder.Window.SetLayout(600, 600);
    //        builder.SetCanceledOnTouchOutside(false);
    //        recordbtn = view.FindViewById<Button>(Resource.Id.recordbtn);
    //        stopbtn = view.FindViewById<ImageView>(Resource.Id.stopbtn);
    //        playbtn = view.FindViewById<ImageView>(Resource.Id.playbtn);
    //        Timer = view.FindViewById<TextView>(Resource.Id.timerbtn);
    //        seekBar = view.FindViewById<SeekBar>(Resource.Id.seek_bar);
    //        Done_Btn = view.FindViewById<Button>(Resource.Id.donebtn);


    //        Done_Btn.Click += delegate
    //        {
    //            TaskFileMapping_Model attachmentModel = new TaskFileMapping_Model();
    //            long size3 = fileaudioPath.Length() / 1024 * 1024;
    //            string audiosize = size3.ToString();
    //            attachmentModel.Path = AudioSavePathInDevice;
    //            attachmentModel.FileType = "Audio";
    //            attachmentModel.FileName = audioname;
    //            attachmentModel.localtaskId = task_id_to_send;
    //            // attachmentModel.file_format = Utility.audiotype;
    //            attachmentModel.FileSize = audiosize;
    //            //  attachmentModel.GeoLocation = geolocation;
    //            //  attachmentModel.max_numbers = audio_max.ToString();
    //            // db.InsertCreateAttachData(attachmentModel);

    //            //   comp_AttachmentModels.Add(attachmentModel);

    //            listmapping.Add(attachmentModel);
    //            //imagelist.AddRange(comp_AttachmentModels.Where(p => p.Attachment_Type == "Image" ));
    //            //  audio_list = db.GetCreateAttachmentData("Audio", licenceidmodel[0].taskid.ToString());
    //            for (int i = 0; i < listmapping.Count; i++)
    //            {
    //                if (listmapping[i].FileType.Equals("Audio"))
    //                {
    //                    audio_list.Add(listmapping[i]);
    //                }
    //            }
    //            adapter_3 = new GridAudioCreateTask(Activity, audio_list);
    //            Gridview_3.Adapter = adapter_3;
    //            Gridview_3.setExpanded(true);
    //            Gridview_3.ChoiceMode = (ChoiceMode)AbsListViewChoiceMode.MultipleModal;
    //            Gridview_3.SetMultiChoiceModeListener(new MultiChoiceModeListener3(Activity));
    //            audioCount++;
    //            builder.Dismiss();
    //        };
    //        recordbtn.Click += delegate
    //        {
    //            MediaRecorderReady();

    //            try
    //            {
    //                timer = new Timer();
    //                timer.Interval = 1000; // 1 second  
    //                timer.Elapsed += Timer_Elapsed;
    //                timer.Start();
    //                mediaRecorder.Prepare();
    //                mediaRecorder.Start();

    //            }
    //            catch (Exception e)
    //            {
    //                // TODO Auto-generated catch block
    //                //e.printStackTrace();
    //            }

    //            Toast.MakeText(Activity, "Recording started", ToastLength.Long).Show();

    //        };
    //        stopbtn.Click += delegate
    //        {
    //            try
    //            {
    //                mediaRecorder.Stop();
    //                Timer.Text = "00:00:00";
    //                timer.Stop();

    //                timer = null;
    //            }
    //            catch (Exception ex)
    //            {

    //            }

    //            //stoprecorder();

    //            //btn2.Enabled=false;
    //            //buttonPlayLastRecordAudio.setEnabled(true);
    //            //buttonStart.setEnabled(true);
    //            //buttonStopPlayingRecording.setEnabled(false);

    //            Toast.MakeText(Activity, "Recording completed", ToastLength.Long).Show();
    //        };
    //        //pausebtn.Click += delegate
    //        //{
    //        //    //OnPause();
    //        //    mediaRecorder.Pause();
    //        //    timer.Dispose();

    //        //};
    //        playbtn.Click += delegate
    //        {

    //            mediaPlayer = new MediaPlayer();
    //            mediaPlayer.SetDataSource(AudioSavePathInDevice);
    //            mediaPlayer.Prepare();
    //            mediaPlayer.Start();
    //            //mediaPlayer = MediaPlayer.Create(this, Resource.Raw.AudioSavePathInDevice);
    //            seekBar.Max = mediaPlayer.Duration;
    //            run();
    //        };

    //        //resumebtn.Click += delegate
    //        // {
    //        //     mediaRecorder.Resume();
    //        //     timer.Start();

    //        // };

    //        //savebtn.Click += delegate
    //        // {
    //        //     Java.IO.File path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
    //        //     audiofile = new Java.IO.File(path, "TaskApp");
    //        //     if (!audiofile.Exists())
    //        //     {
    //        //         audiofile.Mkdirs();
    //        //     }
    //        //     audioname = Utility.fileName1();
    //        //     fileImagePath = new Java.IO.File(audiofile, string.Format(audioname, Guid.NewGuid()));
    //        //     AudioSavePathInDevice = fileImagePath.AbsolutePath; 

    //        //     mediaRecorder.SetOutputFile(AudioSavePathInDevice);

    //        //     builder.Dismiss();
    //        // };
    //        builder .Show();
    //    }

    //    public void run()
    //    {
    //        action = () =>
    //        {


    //            if (mediaPlayer != null)
    //            {
    //                int mCurrentPosition = mediaPlayer.CurrentPosition / 1000;
    //                seekBar.Progress = mCurrentPosition;
    //            }
    //            mHandler.PostDelayed(action, 1000);
    //        };
    //        mHandler.Post(action);
    //    }
    //    public void MediaRecorderReady()
    //    {
    //        mediaRecorder = new MediaRecorder();
    //        mediaRecorder.SetAudioSource(AudioSource.Mic);
    //        mediaRecorder.SetOutputFormat(OutputFormat.ThreeGpp);
    //        mediaRecorder.SetAudioEncoder(AudioEncoder.AmrNb);
    //        Java.IO.File path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
    //        audiofile = new Java.IO.File(path, "TaskApp");
    //        if (!audiofile.Exists())
    //        {
    //            audiofile.Mkdirs();
    //        }
    //        audioname = Utility.fileName1();
    //        fileaudioPath = new Java.IO.File(audiofile, string.Format(audioname, Guid.NewGuid()));
    //        AudioSavePathInDevice = fileaudioPath.AbsolutePath;
    //        mediaRecorder.SetOutputFile(AudioSavePathInDevice);
    //        //mediaRecorder.SetOutputFile(AudioSavePathInDevice);
    //    }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            sec++;
            if (sec == 60)
            {
                min++;
                sec = 0;
            }
            if (min == 60)
            {
                hour++;
                min = 0;
            }
            Activity.RunOnUiThread(() => { Timer.Text = $"{hour}:{min}:{sec}"; });
           
            radialProgrssView.Value = sec;
        }


        void DateSelect_OnClick(object sender, EventArgs eventArgs)
        {
            DatePick frag = DatePick.NewInstance(delegate (DateTime time)
            {
                _dateDisplay.Text = time.ToString("dd-MM-yyyy");
                date = time.ToString("yyyy-MM-dd");
            });
            frag.Show(FragmentManager, DatePick.TAG);

        }


        void TimeSelectOnClick(object sender, EventArgs eventArgs)
        {
            DateTime currentTime = DateTime.Now;
            bool is24HourFormat = true;
            Android.App.TimePickerDialog dialog = new Android.App.TimePickerDialog
                (Activity, this, currentTime.Hour, currentTime.Minute, is24HourFormat);
            dialog.Show();
        }
        void CreateButton_OnClick(object sender, EventArgs eventArgs)
        {
            name = task_name.Text;
            comment = task_comment.Text;
            if (task_name.Text.Length >= 5 && task_comment.Text.Length >= 10 && date != null && time1 != null)
            {
              //  db.Insert_InitialTask1(name, comment, date, time1);
                Fragment frag = new Assign();
              
                FragmentTransaction ft = FragmentManager.BeginTransaction();
                ft.Replace(Resource.Id.container, frag);
                ft.AddToBackStack(null);
                ft.Commit();
               
            }
            else if (task_name.Text.Length == 0)
            {
                Toast.MakeText(Activity, "Please Fill Task name", ToastLength.Long).Show();
                return;
            }
            else if (task_name.Text.Length < 5 )
            {
                Toast.MakeText(Activity, "Task name should be more than 5 characters", ToastLength.Long).Show();
            }
            else if (task_comment.Text.Length < 10)
            {
                Toast.MakeText(Activity, "Task description should be more than 10 characters", ToastLength.Long).Show();
            }
            else if (task_comment.Text.Length == 0)
            {
                Toast.MakeText(Activity, "Please fill task description", ToastLength.Long).Show();
            }
            else if (_dateDisplay.Text.Equals("Select Date") || date == null)
            {
                Toast.MakeText(Activity, "Please Select Date", ToastLength.Long).Show();
            }
            else if (timeDisplay.Text.Equals("Select Time") || time1 == null)
            {
                Toast.MakeText(Activity, "Please Select Time", ToastLength.Long).Show();
            }
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;

            if (id == Resource.Id.filtr)
            {

                return true;
            }

            return base.OnOptionsItemSelected(item);

        }

      
        public override void OnCreateOptionsMenu(IMenu menu, MenuInflater inflater)
        {
            base.OnCreateOptionsMenu(menu, inflater);
            inflater.Inflate(Resource.Menu.menu_main, menu);
            menu.FindItem(Resource.Id.filtr).SetVisible(false);
            menu.FindItem(Resource.Id.order).SetVisible(false);
            var item = menu.FindItem(Resource.Id.filtr);
            var item1 = menu.FindItem(Resource.Id.order);


        }

        public void OnTimeSet(TimePicker view, int hourOfDay, int minute)
        {
            DateTime currentTime = DateTime.Now;
            DateTime selectedTime = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, hourOfDay, minute, 0);
            timeDisplay.Text = currentTime.ToLongTimeString();

            //string tm = time.ToLongTimeString();
            time1 = timeDisplay.Text;
        }

        //class MultiChoiceModeListener1 : Java.Lang.Object, GridView.IMultiChoiceModeListener
        //{
        //    Context self;
        //    DbHelper db = new DbHelper();
        //    public MultiChoiceModeListener1(Context s)
        //    {
        //        self = s;
        //    }

        //    public bool OnCreateActionMode(ActionMode mode, IMenu menu)
        //    {
        //        mode.Title = "Select Items";
        //        mode.Subtitle = "One item selected";

        //        new MenuInflater(self).Inflate(Resource.Menu.ContextualMenu, menu);

        //        return true;
        //    }

        //    public bool OnPrepareActionMode(ActionMode mode, IMenu menu)
        //    {
        //        return true;
        //    }

        //    public bool OnActionItemClicked(ActionMode mode, IMenuItem item)
        //    {
        //        switch (item.ItemId)
        //        {

        //            case Resource.Id.delete:
        //                {

        //                    for (int i = 0; i < image_list.Count; i++)
        //                    {
        //                        if (image_list[i].Checked == 1)
        //                        {
        //                            image_list.RemoveAt(i);
        //                            CreateTaskFrag.imageCount--;
        //                            db.DeleteRow(image_list[i].FileName);

        //                        }
        //                    }
        //                    adapter_1.NotifyDataSetChanged();
        //                    mode.Finish();
        //                    return true;
        //                }


        //        }
        //        return true;
        //    }

        //    public void OnDestroyActionMode(ActionMode mode)
        //    {
        //        for (int i = 0; i < image_list.Count; i++)
        //        {
        //            image_list[i].Checked = 0;

        //        }
        //    }

        //    public void OnItemCheckedStateChanged(ActionMode mode, int position, long id, bool isChecked)
        //    {
        //        int selectCount = Gridview_1.CheckedItemCount;
        //        if (isChecked)
        //        {
        //            image_list[position].Checked = 1;
        //            CreateTaskFrag.adapter_1.setNewSelection(position);

        //        }
        //        else
        //        {
        //            image_list[position].Checked = 0;
        //            CreateTaskFrag.adapter_1.removeSelection(position);

        //        }

        //        switch (selectCount)
        //        {
        //            case 1:
        //                mode.Subtitle = "One item selected";
        //                break;

        //            default:
        //                mode.Subtitle = "" + selectCount + " items selected";
        //                break;
        //        }
        //    }
        //}

        //class MultiChoiceModeListener2 : Java.Lang.Object, GridView.IMultiChoiceModeListener
        //{
        //    Context self;
        //    DbHelper db = new DbHelper();
        //    public MultiChoiceModeListener2(Context s)
        //    {
        //        self = s;
        //    }

        //    public bool OnCreateActionMode(ActionMode mode, IMenu menu)
        //    {
        //        mode.Title = "Select Items";
        //        mode.Subtitle = "One item selected";

        //        new MenuInflater(self).Inflate(Resource.Menu.ContextualMenu, menu);

        //        return true;
        //    }

        //    public bool OnPrepareActionMode(ActionMode mode, IMenu menu)
        //    {
        //        return true;
        //    }

        //    public bool OnActionItemClicked(ActionMode mode, IMenuItem item)
        //    {
        //        switch (item.ItemId)
        //        {

        //            case Resource.Id.delete:
        //                {

        //                    for (int i = 0; i < video_list.Count; i++)
        //                    {
        //                        if (video_list[i].Checked == 1)
        //                        {
        //                            video_list.RemoveAt(i);
        //                            CreateTaskFrag.videoCount--;
        //                            db.DeleteRow(video_list[i].FileName);

        //                        }
        //                    }
        //                    adapter_2.NotifyDataSetChanged();
        //                    mode.Finish();
        //                    return true;
        //                }


        //        }
        //        return true;
        //    }

        //    public void OnDestroyActionMode(ActionMode mode)
        //    {
        //        for (int i = 0; i < video_list.Count; i++)
        //        {
        //            video_list[i].Checked = 0;

        //        }
        //    }

        //    public void OnItemCheckedStateChanged(ActionMode mode, int position, long id, bool isChecked)
        //    {
        //        int selectCount = Gridview_2.CheckedItemCount;
        //        if (isChecked)
        //        {
        //            video_list[position].Checked = 1;
        //            CreateTaskFrag.adapter_2.setNewSelection(position);

        //        }
        //        else
        //        {
        //            video_list[position].Checked = 0;
        //            CreateTaskFrag.adapter_2.removeSelection(position);

        //        }

        //        switch (selectCount)
        //        {
        //            case 1:
        //                mode.Subtitle = "One item selected";
        //                break;

        //            default:
        //                mode.Subtitle = "" + selectCount + " items selected";
        //                break;
        //        }
        //    }
        //}

        //class MultiChoiceModeListener3 : Java.Lang.Object, GridView.IMultiChoiceModeListener
        //{
        //    Context self;
        //    DbHelper db = new DbHelper();

        //    public MultiChoiceModeListener3(Context s)
        //    {
        //        self = s;
        //    }

        //    public bool OnCreateActionMode(ActionMode mode, IMenu menu)
        //    {
        //        mode.Title = "Select Items";
        //        mode.Subtitle = "One item selected";

        //        new MenuInflater(self).Inflate(Resource.Menu.ContextualMenu, menu);

        //        return true;
        //    }

        //    public bool OnPrepareActionMode(ActionMode mode, IMenu menu)
        //    {
        //        return true;
        //    }

        //    public bool OnActionItemClicked(ActionMode mode, IMenuItem item)
        //    {
        //        switch (item.ItemId)
        //        {

        //            case Resource.Id.delete:
        //                {

        //                    for (int i = 0; i < audio_list.Count; i++)
        //                    {
        //                        if (audio_list[i].Checked == 1)
        //                        {
        //                            audio_list.RemoveAt(i);
        //                            CreateTaskFrag.audioCount--;
        //                            db.DeleteRow(audio_list[i].FileName);
        //                        }
        //                    }
        //                    adapter_3.NotifyDataSetChanged();
        //                    mode.Finish();
        //                    return true;
        //                }


        //        }
        //        return true;
        //    }

        //    public void OnDestroyActionMode(ActionMode mode)
        //    {
        //        for (int i = 0; i < audio_list.Count; i++)
        //        {
        //            audio_list[i].Checked = 0;

        //        }
        //    }

        //    public void OnItemCheckedStateChanged(ActionMode mode, int position, long id, bool isChecked)
        //    {
        //        int selectCount = Gridview_3.CheckedItemCount;
        //        if (isChecked)
        //        {
        //            audio_list[position].Checked = 1;
        //            CreateTaskFrag.adapter_3.setNewSelection(position);

        //        }
        //        else
        //        {
        //            image_list[position].Checked = 0;
        //            CreateTaskFrag.adapter_3.removeSelection(position);

        //        }

        //        switch (selectCount)
        //        {
        //            case 1:
        //                mode.Subtitle = "One item selected";
        //                break;

        //            default:
        //                mode.Subtitle = "" + selectCount + " items selected";
        //                break;
        //        }
        //    }
        //}
        public interface IBackButtonListener
        {
            void onBackPressed();
        }

    }
}