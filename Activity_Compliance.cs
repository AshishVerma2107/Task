using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Json;
using System.Linq;
using System.Threading.Tasks;
using System.Timers;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Locations;
using Android.Media;
using Android.OS;
using Android.Preferences;
using Android.Provider;
using Android.Runtime;
using Android.Speech;
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using RadialProgress;
using TaskAppWithLogin.Adapter;
using TaskAppWithLogin.Constants;
using TaskAppWithLogin.Models;

namespace TaskAppWithLogin
{
    [Activity(Label = "Compliances", WindowSoftInputMode = SoftInput.AdjustPan)]
    public class Activity_Compliance : AppCompatActivity
    {
        ServiceHelper restService;
        Geolocation geo;
        DbHelper db;
        InternetConnection ic;
        ISharedPreferences prefs;

        private readonly int Video = 20;
        private readonly int Camera = 10;
        private readonly int VOICE = 30;
        private bool isRecording;
        string geolocation;
        bool retVal = true;
        string[] tap;
        string task_id = "", task_description="", task_name = "", deadline = "", markto = "", taskcreatedby = "", markingDate = "", creationdate = "", markby = "";
        string  taskstatus, rownum, meatingid, markingtype, file_format = "", filetype = "", taskoverview, uploaded, shapes_from_Comp;
        string filename = "", filesize = "";
        int max_num;
        string imageName, imageURL, videoName, audioname;
        public static int imageCount, videoCount, audioCount;
        string AudioSavePathInDevice = null;
        int hour = 00, min = 00, sec = 00;
        int image_max, video_max, audio_max;
        string Click_Type, task_id_to_send;

        private Handler mHandler = new Handler();
        Java.IO.File fileName1, fileImagePath,audiofile;

        EditText Description;
        TextView descrip_text, detail_text, name_text, markby_text, deadline_text, creationdate_text, createdby_text;
        ImageButton camera, video, microphone;
        ImageView image, videoimage;
        LinearLayout linear1, linear2, linear3;
        Button recordbtn,  resumebtn, pausebtn, savebtn;
        TextView txtTimer, Timer, Image_no, Video_no, Audio_no;
        RadialProgressView radialProgrssView;
        Timer timer;
        Button Submit_Btn, Done_Btn;
        MediaRecorder _recorder, mediaRecorder;
        ProgressDialog progress;
        SeekBar seekBar;
        ImageView comment_micro, playbtn, stopbtn;
        ExpandableHeightGridView Gridview1, Gridview2, Gridview3;
        public static List<Comp_AttachmentModel> imagelist = new List<Comp_AttachmentModel>();
        public static List<Comp_AttachmentModel> videolist = new List<Comp_AttachmentModel>();
        public static List<Comp_AttachmentModel> audiolist = new List<Comp_AttachmentModel>();
        //public static List<Comp_AttachmentModel> imagelist = new List<Comp_AttachmentModel>();
        public static GridViewAdapter_Image adapter1;
        public static GridViewAdapter_Video adapter2;
        public static GridViewAdapter_Audio adapter3;
        List<Comp_AttachmentModel> comp_AttachmentModels;
        Action action;
        MediaPlayer mediaPlayer;

        Shapes shapes1;
        List<ComplianceModel> compliances;
        ComplianceModel comp;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layout_complaince);
            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());
            StrictMode.ThreadPolicy.Builder builder1 = new StrictMode.ThreadPolicy.Builder().PermitAll();
            StrictMode.SetThreadPolicy(builder1.Build());
        
            restService = new ServiceHelper();
            geo = new Geolocation();
            db = new DbHelper();
            ic = new InternetConnection();
            comp_AttachmentModels = new List<Comp_AttachmentModel>();

            geolocation = geo.GetGeoLocation(this);
            prefs = PreferenceManager.GetDefaultSharedPreferences(this);
            isRecording = false;
            //AudioSavePathInDevice = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/AudioRecording.mp3";

            task_id_to_send = Intent.GetStringExtra("task_id") ?? string.Empty;
            //task_description = Intent.GetStringExtra("task_descrip") ?? string.Empty;
            //deadline = Intent.GetStringExtra("deadline") ?? string.Empty;
            //task_name = Intent.GetStringExtra("task_name") ?? string.Empty;
            //markby = Intent.GetStringExtra("mark_by") ?? string.Empty;
            //creationdate = Intent.GetStringExtra("creation_date") ?? string.Empty;
            //taskcreatedby = Intent.GetStringExtra("created_by") ?? string.Empty;
            tap = geolocation.Split(",");

            Description = FindViewById<EditText>(Resource.Id.comment);
            descrip_text = FindViewById<TextView>(Resource.Id.c_descrip);
            name_text = FindViewById<TextView>(Resource.Id.c_name);
            detail_text = FindViewById<TextView>(Resource.Id.c_detail);
            markby_text = FindViewById<TextView>(Resource.Id.c_markby);
            deadline_text = FindViewById<TextView>(Resource.Id.c_deadline);
            createdby_text = FindViewById<TextView>(Resource.Id.c_createdby);
            creationdate_text = FindViewById<TextView>(Resource.Id.c_creationdate);
            camera = FindViewById<ImageButton>(Resource.Id.camera_btn);
            video = FindViewById<ImageButton>(Resource.Id.video_btn);
            microphone = FindViewById<ImageButton>(Resource.Id.micro_btn);
            //holder = FindViewById<ImageButton>(Resource.Id.location_btn);
            linear1 = FindViewById<LinearLayout>(Resource.Id.ll1);
            linear2 = FindViewById<LinearLayout>(Resource.Id.ll2);
            linear3 = FindViewById<LinearLayout>(Resource.Id.ll3);
            Image_no = FindViewById<TextView>(Resource.Id.image_no);
            Video_no = FindViewById<TextView>(Resource.Id.video_no);
            Audio_no = FindViewById<TextView>(Resource.Id.audio_no);
            comment_micro = FindViewById<ImageView>(Resource.Id.comment_micro);
            Submit_Btn = FindViewById<Button>(Resource.Id.submit);
            Gridview1 = FindViewById<ExpandableHeightGridView>(Resource.Id.gridView1);
            Gridview2 = FindViewById<ExpandableHeightGridView>(Resource.Id.gridView2);
            Gridview3 = FindViewById<ExpandableHeightGridView>(Resource.Id.gridView3);

            getData();
            camera.Click += delegate
            {
                Click_Type = "Camera";
                CheckForShapeData_Camera();

            };

            video.Click += delegate
             {
                 Click_Type = "Video";
                 CheckForShapeData_Video();
             };

            microphone.Click += delegate
             {
                 if(audioCount == audio_max)
                 {
                     Toast.MakeText(this, "Reached Maximum Point", ToastLength.Long).Show();
                 }
                 else
                 {
                     recording();
                 }
                 
             };

            Submit_Btn.Click += delegate
             {
                 Submit_Method();
             };
            comment_micro.Click += delegate
            {
                CheckMicrophone();
            };
            // Create your application here
        }

        public async Task getData()
        {
            progress = new ProgressDialog(this);
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
                JsonValue item = await restService.GetComplianceTask(this, json, geolocation);
                comp = JsonConvert.DeserializeObject<ComplianceModel>(item);
                shapes1 = JsonConvert.DeserializeObject<Shapes>(comp.shapes);

                task_id = comp.task_id;
                task_description = comp.description;
                deadline = comp.deadline_date;
                meatingid = comp.Meeting_ID;
                rownum = comp.RowNo;
                //taskcreationDate = comp.task_creation_date;
                markby = comp.task_mark_by;
                taskstatus = comp.taskStatus;
                markto = comp.markTo;
                markingtype = comp.task_marking_type;
                taskcreatedby = comp.task_created_by;
                markingDate = comp.MarkingDate;
                creationdate = comp.task_creation_date;
                shapes_from_Comp = comp.shapes;
                task_name = comp.task_name;

                SettingValues();

                progress.Dismiss();
            }
            catch (Exception ex)
            {
                progress.Dismiss();
            }

            //db.ComplianceInsert(comp, shapes1);

            storeDataBaseAsync();

            

            int posi = prefs.GetInt("position", 0);
            for (int i = 0; i <= comp.lstAddedCompliance.Count; i++)
            {
                max_num = comp.lstAddedCompliance[i].max_numbers;
                file_format = comp.lstAddedCompliance[i].file_format;
                filetype = comp.lstAddedCompliance[i].file_type;
                taskoverview = comp.lstAddedCompliance[i].task_overview;
                uploaded = comp.lstAddedCompliance[i].Uploaded;

                if(filetype.Equals("Image"))
                {
                    image_max = max_num;
                }
                else if (filetype.Equals("Video"))
                {
                    video_max = max_num;
                }
                else if (filetype.Equals("Audio"))
                {
                    audio_max = max_num;
                }
                Image_no.Text = image_max.ToString();
                Video_no.Text = video_max.ToString();
                Audio_no.Text = audio_max.ToString();

            }

            
            
           

            //progress.Dismiss();
        }
        public void SettingValues()
        {
            descrip_text.Text = task_description;
            createdby_text.Text = taskcreatedby;
            markby_text.Text = markby;
            creationdate_text.Text = creationdate;
            deadline_text.Text = deadline;
            name_text.Text = task_name;
        }
        public void storeDataBaseAsync()
        {
            db.ComplianceInsert(task_id, markingtype, taskstatus, taskcreatedby, creationdate, task_name, task_description, markingDate, rownum, meatingid, deadline, comp.lstAddedCompliance, geolocation, "no", shapes_from_Comp);
            db.InsertcompliancejoinTable(comp.lstAddedCompliance);
        }

       

        public void CheckForShape()
        {
            if (shapes1.shapes[0].type == "polygon")
            {
                polygon();
            }
            else if (shapes1.shapes[0].type == "circle")
            {
                isMarkerOutsideCircle();
                if (isMarkerOutsideCircle() == true)
                {
                    //Toast.MakeText(this, "Latitude-Longitude is inside the circle", ToastLength.Long).Show();
                    if(Click_Type.Equals("Camera"))
                    {
                        if (imageCount < image_max)
                        {
                            BtnCamera_Click();
                        }
                        else if (imageCount == image_max)
                        {
                            Toast.MakeText(this, "Reached Maximum Point", ToastLength.Long).Show();
                        }
                    }
                    else if (Click_Type.Equals("Video"))
                    {
                        if (videoCount < video_max)
                        {
                            VideoClick();
                        }
                        else if (videoCount== video_max)
                        {
                            Toast.MakeText(this, "Reached Maximum Point", ToastLength.Long).Show();
                        }
                    }

                    
                    //BtnCamera_Click();
                }
                else
                {
                    Toast.MakeText(this, "Latitude-Longitude is outside the circle", ToastLength.Long).Show();
                }
            }

        }

        public void CheckForShapeData_Camera()
        {
            if (shapes1 != null)
            {
                CheckForShape(); 
            }
            else
            {
                if (imageCount < image_max)
                {
                    BtnCamera_Click();
                }
                else if(imageCount == image_max)
                {
                    Toast.MakeText(this, "Reached Maximum Point", ToastLength.Long).Show();
                }
                
            }
        }
        public void CheckForShapeData_Video()
        {
            if (shapes1 != null)
            {
                CheckForShape(); 
            }
            else
            {
                if (videoCount < video_max)
                {
                    VideoClick();
                }
                else if (videoCount == video_max)
                {
                    Toast.MakeText(this, "Reached Maximum Point", ToastLength.Long).Show();
                }
            }
        }
        public void polygon()
        {
            retVal = retVal && isPointInPolygon(tap, shapes1.shapes[0].paths[0].path);
            if (retVal == true)
            {
                //Toast.MakeText(this, "Latitude-Longitude is inside the polygon", ToastLength.Long).Show();
                if (Click_Type.Equals("Camera"))
                {
                    if (imageCount < image_max)
                    {
                        BtnCamera_Click();
                    }
                    else if (imageCount == image_max)
                    {
                        Toast.MakeText(this, "Reached Maximum Point", ToastLength.Long).Show();
                    }
                }
                else if (Click_Type.Equals("Video"))
                {
                    if (videoCount < video_max)
                    {
                        VideoClick();
                    }
                    else if (videoCount == video_max)
                    {
                        Toast.MakeText(this, "Reached Maximum Point", ToastLength.Long).Show();
                    }
                }

            }
            else
            {
                Toast.MakeText(this, "Latitude-Longitude is outside the polygon.", ToastLength.Long).Show();
            }
        }
        private Boolean isPointInPolygon(string[] tap, List<ComplianceLatlngPath> vertices)
        {
            int intersectCount = 0;
            //  String[] latLng = vertices[0].lat + vertices[0].lon.Split(",");

            for (int j = 0; j < vertices.Count - 1; j++)
            {
                // double latitude = Double.parseDouble(lat[0]);
                double latitude = Convert.ToDouble(vertices[j].lat);
                double longitude = Convert.ToDouble(vertices[j].lon);
                LatLng location = new LatLng(latitude, longitude);
                double lat1 = Convert.ToDouble(vertices[j + 1].lat);
                double lon1 = Convert.ToDouble(vertices[j + 1].lon);
                LatLng location1 = new LatLng(lat1, lon1);
                if (rayCastIntersect(tap, location, location1))
                {
                    intersectCount++;
                }

            }

            return ((intersectCount % 2) == 1); // odd = inside, even = outside;
        }

        private Boolean rayCastIntersect(string[] tap, LatLng vertA, LatLng vertB)
        {

            double aY = vertA.Latitude;
            double bY = vertB.Latitude;
            double aX = vertA.Longitude;
            double bX = vertB.Longitude;
            double pY = Convert.ToDouble(tap[0]);
            double pX = Convert.ToDouble(tap[1]);

            if ((aY > pY && bY > pY) || (aY < pY && bY < pY)
                    || (aX < pX && bX < pX))
            {
                return false; // a and b can't both be above or below pt.y, and a or
                              // b must be east of pt.x
            }

            double m = (aY - bY) / (aX - bX); // Rise over run
            double bee = (-aX) * m + aY; // y = mx + b
            double x = (pY - bee) / m; // algebra is neat!

            return x > pX;
        }

        private bool isMarkerOutsideCircle()
        {
            float[] results = new float[1];
            //float[] array = Array.ConvertAll<string, float[]>(shapes1.shapes[0].radius, float.Parse);
            // float[] radius1 = float.Parse(shapes1.shapes[0].radius);
            float vari = float.Parse(shapes1.shapes[0].radius);
            //  Location.DistanceBetween(26.863812, 80.983006, 26.864257, 80.981879, results);
            double lati = Convert.ToDouble(shapes1.shapes[0].center.lat);
            double longi = Convert.ToDouble(shapes1.shapes[0].center.lon);
            double currentlat = Convert.ToDouble(tap[0]);
            double currentlng = Convert.ToDouble(tap[1]);
            Location.DistanceBetween(lati, longi, currentlat, currentlng, results);
            float distanceInMeters = results[0];
            bool isWithin100m = distanceInMeters < vari;
            return isWithin100m;
        }

        private void BtnCamera_Click()
        {
            Java.IO.File path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
            fileName1 = new Java.IO.File(path, "TaskApp");
            if (!fileName1.Exists())
            {
                fileName1.Mkdirs();
            }
            imageName = Utility.fileName();
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            fileImagePath = new Java.IO.File(fileName1, string.Format(imageName, Guid.NewGuid()));
            imageURL = fileImagePath.AbsolutePath;
            intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(fileImagePath));
            intent.AddFlags(ActivityFlags.GrantReadUriPermission);
            StartActivityForResult(intent, Camera);
        }
        private void VideoClick()
        {
            Java.IO.File path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
            fileName1 = new Java.IO.File(path, "TaskApp");
            if (!fileName1.Exists())
            {
                fileName1.Mkdirs();
            }
            videoName = Utility.fileName();
            Intent intent = new Intent(MediaStore.ActionVideoCapture);
            fileImagePath = new Java.IO.File(fileName1, string.Format(videoName, Guid.NewGuid()));
            imageURL = fileImagePath.AbsolutePath;
            intent.PutExtra(MediaStore.ExtraOutput, path);
            intent.PutExtra(MediaStore.ExtraVideoQuality, 1);
            // intent.PutExtra(MediaStore.ExtraDurationLimit, 10);
            StartActivityForResult(intent, Video);
        }
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == Camera)
            {
                Bitmap bitmap = BitmapFactory.DecodeFile(imageURL);

                Comp_AttachmentModel attachmentModel = new Comp_AttachmentModel();
                attachmentModel.Attachment_Path = AudioSavePathInDevice;
                attachmentModel.Attachment_Type = "Image";
                attachmentModel.Attachment_Name = audioname;
                attachmentModel.Task_id = task_id_to_send;

                db.InsertAttachmentData(attachmentModel);

                //comp_AttachmentModels.Add(attachmentModel);


                //imagelist.AddRange(comp_AttachmentModels.Where(p => p.Attachment_Type == "Image" ));
                imagelist = db.GetAttachmentData("Image");
                adapter1 = new GridViewAdapter_Image(this, imagelist);
                Gridview1.Adapter = adapter1;
                Gridview1.setExpanded(true);
                Gridview1.ChoiceMode = (ChoiceMode)AbsListViewChoiceMode.MultipleModal;
                Gridview1.SetMultiChoiceModeListener(new MultiChoiceModeListener1(this));
                imageCount++;
                
            }
            if (requestCode == Video)
            {
                Comp_AttachmentModel attachmentModel = new Comp_AttachmentModel();
                attachmentModel.Attachment_Path = AudioSavePathInDevice;
                attachmentModel.Attachment_Type = "Video";
                attachmentModel.Attachment_Name = audioname;
                attachmentModel.Task_id = task_id_to_send;

                db.InsertAttachmentData(attachmentModel);

                //comp_AttachmentModels.Add(attachmentModel);


                //imagelist.AddRange(comp_AttachmentModels.Where(p => p.Attachment_Type == "Image" ));
                videolist = db.GetAttachmentData("Video");

                adapter2 = new GridViewAdapter_Video(this, videolist);
                Gridview2.Adapter = adapter2;
                Gridview2.setExpanded(true);
                Gridview2.ChoiceMode = (ChoiceMode)AbsListViewChoiceMode.MultipleModal;
                Gridview2.SetMultiChoiceModeListener(new MultiChoiceModeListener2(this));

                videoCount++;
                
            }

            if (requestCode == VOICE)
            {
                var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                if (matches.Count != 0)
                {
                    string textInput = Description.Text + matches[0];

                    // limit the output to 500 characters
                    if (textInput.Length > 500)
                        textInput = textInput.Substring(0, 500);
                    Description.Text = textInput;
                }
                else
                    Description.Text = "No speech was recognised";

            }
            

        }

        public void recording()
        {
            View view = LayoutInflater.Inflate(Resource.Layout.audio_recorder, null);
            Android.App.AlertDialog builder = new Android.App.AlertDialog.Builder(this).Create();
            builder.SetView(view);
            builder.Window.SetLayout(600, 600);
            builder.SetCanceledOnTouchOutside(false);
            recordbtn = view.FindViewById<Button>(Resource.Id.recordbtn);
            stopbtn = view.FindViewById<ImageView>(Resource.Id.stopbtn);
            playbtn = view.FindViewById<ImageView>(Resource.Id.playbtn);
            Timer = view.FindViewById<TextView>(Resource.Id.timerbtn);
            seekBar = view.FindViewById<SeekBar>(Resource.Id.seek_bar);
            Done_Btn = view.FindViewById<Button>(Resource.Id.donebtn);
            

            Done_Btn.Click += delegate
             {
                 Comp_AttachmentModel attachmentModel = new Comp_AttachmentModel();
                 attachmentModel.Attachment_Path = AudioSavePathInDevice;
                 attachmentModel.Attachment_Type = "Audio";
                 attachmentModel.Attachment_Name = audioname;
                 attachmentModel.Task_id = task_id_to_send;

                 db.InsertAttachmentData(attachmentModel);

                 //comp_AttachmentModels.Add(attachmentModel);


                 //imagelist.AddRange(comp_AttachmentModels.Where(p => p.Attachment_Type == "Image" ));
                 audiolist = db.GetAttachmentData("Audio");

                 adapter3 = new GridViewAdapter_Audio(this, audiolist);
                 Gridview3.Adapter = adapter3;
                 Gridview3.setExpanded(true);
                 Gridview3.ChoiceMode = (ChoiceMode)AbsListViewChoiceMode.MultipleModal;
                 Gridview3.SetMultiChoiceModeListener(new MultiChoiceModeListener3(this));
                 audioCount++;
                 builder.Dismiss();
             };
            recordbtn.Click += delegate
            {
                MediaRecorderReady();

                try
                {
                    timer = new Timer();
                    timer.Interval = 1000; // 1 second  
                    timer.Elapsed += Timer_Elapsed;
                    timer.Start();
                    mediaRecorder.Prepare();
                    mediaRecorder.Start();
                    
                }
                catch (Exception e)
                {
                    // TODO Auto-generated catch block
                    //e.printStackTrace();
                }

                Toast.MakeText(this, "Recording started", ToastLength.Long).Show();

            };
            stopbtn.Click += delegate
            {
                try
                {
                    mediaRecorder.Stop();
                    Timer.Text = "00:00:00";
                    timer.Stop();
                    
                    timer = null;
                }
                catch(Exception ex)
                {

                }

                //stoprecorder();
                
                //btn2.Enabled=false;
                //buttonPlayLastRecordAudio.setEnabled(true);
                //buttonStart.setEnabled(true);
                //buttonStopPlayingRecording.setEnabled(false);

                Toast.MakeText(this, "Recording completed", ToastLength.Long).Show();
            };
            //pausebtn.Click += delegate
            //{
            //    //OnPause();
            //    mediaRecorder.Pause();
            //    timer.Dispose();
                
            //};
            playbtn.Click += delegate
             {

                 mediaPlayer = new MediaPlayer();
                 mediaPlayer.SetDataSource(AudioSavePathInDevice);
                 mediaPlayer.Prepare();
                 mediaPlayer.Start();

                 //mediaPlayer = MediaPlayer.Create(this, Resource.Raw.AudioSavePathInDevice);
                 seekBar.Max = mediaPlayer.Duration;
                 run();
             };

            //resumebtn.Click += delegate
            // {
            //     mediaRecorder.Resume();
            //     timer.Start();
                 
            // };

            //savebtn.Click += delegate
            // {
            //     Java.IO.File path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
            //     audiofile = new Java.IO.File(path, "TaskApp");
            //     if (!audiofile.Exists())
            //     {
            //         audiofile.Mkdirs();
            //     }
            //     audioname = Utility.fileName1();
            //     fileImagePath = new Java.IO.File(audiofile, string.Format(audioname, Guid.NewGuid()));
            //     AudioSavePathInDevice = fileImagePath.AbsolutePath; 
                 
            //     mediaRecorder.SetOutputFile(AudioSavePathInDevice);
                 
            //     builder.Dismiss();
            // };
            builder.Show();
        }
        public void run()
        {
            action = () =>
            {


                if (mediaPlayer != null)
                {
                    int mCurrentPosition = mediaPlayer.CurrentPosition / 1000;
                    seekBar.Progress = mCurrentPosition;
                }
                mHandler.PostDelayed(action, 1000);
            };
            mHandler.Post(action);
        }
        public void MediaRecorderReady()
        {
            mediaRecorder = new MediaRecorder();
            mediaRecorder.SetAudioSource(AudioSource.Mic);
            mediaRecorder.SetOutputFormat(OutputFormat.ThreeGpp);
            mediaRecorder.SetAudioEncoder(AudioEncoder.AmrNb);
            Java.IO.File path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
            audiofile = new Java.IO.File(path, "TaskApp");
            if (!audiofile.Exists())
            {
                audiofile.Mkdirs();
            }
            audioname = Utility.fileName1();
            fileImagePath = new Java.IO.File(audiofile, string.Format(audioname, Guid.NewGuid()));
            AudioSavePathInDevice = fileImagePath.AbsolutePath;
            mediaRecorder.SetOutputFile(AudioSavePathInDevice);
            //mediaRecorder.SetOutputFile(AudioSavePathInDevice);
        }

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
            RunOnUiThread(() => { Timer.Text = $"{hour}:{min}:{sec}"; });
            //RunOnUiThread(() => { txtTimer.Text = $"{hour}:{min}:{sec}"; });
            radialProgrssView.Value = sec;
        }
        public void CheckMicrophone()
        {
            string rec = Android.Content.PM.PackageManager.FeatureMicrophone;
            if (rec != "android.hardware.microphone")
            {
                // no microphone, no recording. Disable the button and output an alert
                var alert = new Android.App.AlertDialog.Builder(comment_micro.Context);
                alert.SetTitle("You don't seem to have a microphone to record with");
                alert.SetPositiveButton("OK", (sender, e) =>
                {
                    Description.Text = "No microphone present";
                    comment_micro.Enabled = false;

                    return;
                });


                alert.Show();
            }
            else
            {
                comment_micro.Click += delegate
                {
                    // change the text on the button

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


                        voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
                        StartActivityForResult(voiceIntent, VOICE);
                    }
                };
            }
        }
        public void Submit_Method()
        {
            if(imageCount == image_max && videoCount == video_max && audioCount == audio_max )
            {
                
                if(ic.connectivity())
                {
                    //postserviceCompliance();
                }
                else
                {

                }
            }
            else if(imageCount == image_max || videoCount == video_max)
            {
                Toast.MakeText(this, "Please fill all Asked details", ToastLength.Long).Show();
            }
        }

        public async Task postserviceCompliance(long id)
        {
            progress = new ProgressDialog(this);
            progress.Indeterminate = true;
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            progress.SetCancelable(false);
            progress.SetMessage("Submitting Data...");
            progress.Show();

            dynamic value = new ExpandoObject();
            value.task_id = task_id_to_send;

            string json = JsonConvert.SerializeObject(value);
            try
            {

                string item = await restService.CompliancePostServiceMethod(this, "SetCompleteTaskSubmition", json, geolocation);
                if (item.Contains(""))
                {
                    db.updateComplianceStatus(id);
                    Toast.MakeText(this, "compliance post  Successfully..", ToastLength.Long).Show();
                    progress.Dismiss();
                }
                else
                {
                    Toast.MakeText(this, "Oops! Something Went Wrong.", ToastLength.Long).Show();
                    progress.Dismiss();
                }
            }
            catch (Exception ex)
            {
                progress.Dismiss();
            }
            progress.Dismiss();

        }



        //-------------------------------------------------------------------MultiChoiceClasses-------------------------------------------------------


        class MultiChoiceModeListener1 : Java.Lang.Object, GridView.IMultiChoiceModeListener
        {
            Context self;
            DbHelper db = new DbHelper();
            public MultiChoiceModeListener1(Context s)
            {
                self = s;
            }

            public bool OnCreateActionMode(ActionMode mode, IMenu menu)
            {
                mode.Title = "Select Items";
                mode.Subtitle = "One item selected";

                new MenuInflater(self).Inflate(Resource.Menu.ContextualMenu, menu);

                return true;
            }

            public bool OnPrepareActionMode(ActionMode mode, IMenu menu)
            {
                return true;
            }

            public bool OnActionItemClicked(ActionMode mode, IMenuItem item)
            {
                switch (item.ItemId)
                {

                    case Resource.Id.delete:
                        {

                            for (int i = 0; i < imagelist.Count; i++)
                            {
                                if (imagelist[i].Checked == 1)
                                {
                                    imagelist.RemoveAt(i);
                                    Activity_Compliance.imageCount --;
                                    db.DeleteRow(imagelist[i].Attachment_Name);

                                }
                            }
                            adapter1.NotifyDataSetChanged();
                            mode.Finish();
                            return true;
                        }


                }
                return true;
            }

            public void OnDestroyActionMode(ActionMode mode)
            {
                for (int i = 0; i < imagelist.Count; i++)
                {
                    imagelist[i].Checked = 0;

                }
            }

            public void OnItemCheckedStateChanged(ActionMode mode, int position, long id, bool isChecked)
            {
                int selectCount = ((Activity_Compliance)self).Gridview1.CheckedItemCount;
                if (isChecked)
                {
                    imagelist[position].Checked = 1;
                    Activity_Compliance.adapter1.setNewSelection(position);

                }
                else
                {
                    imagelist[position].Checked = 0;
                    Activity_Compliance.adapter1.removeSelection(position);

                }

                switch (selectCount)
                {
                    case 1:
                        mode.Subtitle = "One item selected";
                        break;

                    default:
                        mode.Subtitle = "" + selectCount + " items selected";
                        break;
                }
            }
        }

        class MultiChoiceModeListener2 : Java.Lang.Object, GridView.IMultiChoiceModeListener
        {
            Context self;
            DbHelper db = new DbHelper();
            public MultiChoiceModeListener2(Context s)
            {
                self = s;
            }

            public bool OnCreateActionMode(ActionMode mode, IMenu menu)
            {
                mode.Title = "Select Items";
                mode.Subtitle = "One item selected";

                new MenuInflater(self).Inflate(Resource.Menu.ContextualMenu, menu);

                return true;
            }

            public bool OnPrepareActionMode(ActionMode mode, IMenu menu)
            {
                return true;
            }

            public bool OnActionItemClicked(ActionMode mode, IMenuItem item)
            {
                switch (item.ItemId)
                {

                    case Resource.Id.delete:
                        {
                                    
                            for (int i = 0; i < videolist.Count; i++)
                            {
                                if (videolist[i].Checked == 1)
                                {
                                    videolist.RemoveAt(i);
                                    Activity_Compliance.videoCount--;
                                    db.DeleteRow(videolist[i].Attachment_Name);

                                }
                            }
                            adapter2.NotifyDataSetChanged();
                            mode.Finish();
                            return true;
                        }


                }
                return true;
            }

            public void OnDestroyActionMode(ActionMode mode)
            {
                for (int i = 0; i < videolist.Count; i++)
                {
                    videolist[i].Checked = 0;

                }
            }

            public void OnItemCheckedStateChanged(ActionMode mode, int position, long id, bool isChecked)
            {
                int selectCount = ((Activity_Compliance)self).Gridview2.CheckedItemCount;
                if (isChecked)
                {
                    videolist[position].Checked = 1;
                    Activity_Compliance.adapter2.setNewSelection(position);

                }
                else
                {
                    videolist[position].Checked = 0;
                    Activity_Compliance.adapter2.removeSelection(position);

                }

                switch (selectCount)
                {
                    case 1:
                        mode.Subtitle = "One item selected";
                        break;

                    default:
                        mode.Subtitle = "" + selectCount + " items selected";
                        break;
                }
            }
        }

        class MultiChoiceModeListener3 : Java.Lang.Object, GridView.IMultiChoiceModeListener
        {
            Context self;
            DbHelper db = new DbHelper();

            public MultiChoiceModeListener3(Context s)
            {
                self = s;
            }

            public bool OnCreateActionMode(ActionMode mode, IMenu menu)
            {
                mode.Title = "Select Items";
                mode.Subtitle = "One item selected";

                new MenuInflater(self).Inflate(Resource.Menu.ContextualMenu, menu);

                return true;
            }

            public bool OnPrepareActionMode(ActionMode mode, IMenu menu)
            {
                return true;
            }

            public bool OnActionItemClicked(ActionMode mode, IMenuItem item)
            {
                switch (item.ItemId)
                {

                    case Resource.Id.delete:
                        {

                            for (int i = 0; i < audiolist.Count; i++)
                            {
                                if (audiolist[i].Checked == 1)
                                {
                                    audiolist.RemoveAt(i);
                                    Activity_Compliance.audioCount--;
                                    db.DeleteRow(audiolist[i].Attachment_Name);
                                }
                            }
                            adapter3.NotifyDataSetChanged();
                            mode.Finish();
                            return true;
                        }


                }
                return true;
            }

            public void OnDestroyActionMode(ActionMode mode)
            {
                for (int i = 0; i < audiolist.Count; i++)
                {
                    audiolist[i].Checked = 0;

                }
            }

            public void OnItemCheckedStateChanged(ActionMode mode, int position, long id, bool isChecked)
            {
                int selectCount = ((Activity_Compliance)self).Gridview3.CheckedItemCount;
                if (isChecked)
                {
                    audiolist[position].Checked = 1;
                    Activity_Compliance.adapter3.setNewSelection(position);

                }
                else
                {
                    imagelist[position].Checked = 0;
                    Activity_Compliance.adapter3.removeSelection(position);

                }

                switch (selectCount)
                {
                    case 1:
                        mode.Subtitle = "One item selected";
                        break;

                    default:
                        mode.Subtitle = "" + selectCount + " items selected";
                        break;
                }
            }
        }

    }
}