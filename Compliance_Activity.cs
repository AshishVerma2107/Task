using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Json;
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
using Android.Support.V4.App;
using Android.Support.V4.Content;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using RadialProgress;
using TaskAppWithLogin.Constants;
using TaskAppWithLogin.Models;

namespace TaskAppWithLogin
{
    [Activity(Label = "Compliances", WindowSoftInputMode = SoftInput.AdjustPan)]
    public class Compliance_Activity : AppCompatActivity
    {
        // "@style/Theme.Transparent"
        ImageButton camera, videobutton, location, mike, mikestop, mikeplay, mikepause;
        EditText description;
        ImageView imageview, i;
        LinearLayout imagelayout;
        private bool isRecording;
        private readonly int Camera = 10;
        private readonly int Video = 20;
        private readonly int DESC = 20;
        MediaRecorder _recorder, mediaRecorder;
        MediaPlayer _player;
        Button Stop;
        string imageName, imageURL, videoName;
        Java.IO.File fileName1, fileImagePath;
        ImageView imagecaptured;
        List<ComplianceModel> compliances;
        ComplianceModel comp;
        TextView txtTimer, Timer;

        private const int Image_Capture = 100;
        public static string filePath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/Music/testAudio.mp3";
        String AudioSavePathInDevice = null;
        SeekBar seekbar;
        ImageView image, image1;
        VideoView videoView;
        LinearLayout rl1, rl2, rl3, rl4;
        RadialProgressView radialProgrssView;
        Timer timer;
        Button save, cancel, listen;
        int hour = 0, min = 0, sec = 0;

        Button recordbtn, playbtn, stopbtn, resumebtn, pausebtn;
        string path = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/test.mp4";


        ServiceHelper restService = new ServiceHelper();
        Geolocation geo = new Geolocation();
        DbHelper db = new DbHelper();
        InternetConnection ic = new InternetConnection();
        ISharedPreferences prefs;

        string AppDateTime, task_description, task_name, deadline, markto, taskcreatedby, markingDate, creationdate, taskstatus, rownum, meatingid, markby, markingtype;

        string task_id = "0";
        string filetype = "";
        string filename = "";
        string filesize = "";
        string geolocation = "0";
        string file_format = "";
        int max_num;
        string taskoverview;
        string uploaded;


        string mark_by;
        string creation_date;
        string created_by;
        Shapes shapes1;
        Boolean retVal = true;
        string[] tap;
        int imageCount, videocount, audiocount;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // GetPermissionAsync();
            isRecording = false;
            SetContentView(Resource.Layout.compliance_layout2);
            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());
            StrictMode.ThreadPolicy.Builder builder1 = new StrictMode.ThreadPolicy.Builder().PermitAll();
            StrictMode.SetThreadPolicy(builder1.Build());

            AudioSavePathInDevice = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/AudioRecording.mp3";
            geolocation = geo.GetGeoLocation(this);
            prefs = PreferenceManager.GetDefaultSharedPreferences(this);

            tap = geolocation.Split(",");

            //task_id = Intent.GetStringExtra("task_id") ?? string.Empty;
            //task_description = Intent.GetStringExtra("task_descrip") ?? string.Empty;
            //deadline = Intent.GetStringExtra("deadline") ?? string.Empty;
            //task_name = Intent.GetStringExtra("task_name") ?? string.Empty;
            //markby = Intent.GetStringExtra("mark_by") ?? string.Empty;
            //creationdate = Intent.GetStringExtra("creation_date") ?? string.Empty;
            //taskcreatedby = Intent.GetStringExtra("created_by") ?? string.Empty;

            camera = FindViewById<ImageButton>(Resource.Id.imageButton1);
            videobutton = FindViewById<ImageButton>(Resource.Id.imageButton2);
            location = FindViewById<ImageButton>(Resource.Id.imageButton4);
            mike = FindViewById<ImageButton>(Resource.Id.imageButton3);
            imagecaptured = FindViewById<ImageView>(Resource.Id.imageView1);
            description = FindViewById<EditText>(Resource.Id.ed2);
            rl2 = FindViewById<LinearLayout>(Resource.Id.rl2);
            rl3 = FindViewById<LinearLayout>(Resource.Id.rl3);
            rl4 = FindViewById<LinearLayout>(Resource.Id.rl4);
            mike.Click += delegate
            {
                recording();

            };
            location.Click += delegate
            {
                //CheckForLocation();


            };

            string path = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/test.mp4";
            camera.Click += BtnCamera_Click;
            videobutton.Click += VideoClick;
            getData();
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
                    Toast.MakeText(this, "Latitude-Longitude is inside the circle", ToastLength.Long).Show();

                }
                else
                {
                    Toast.MakeText(this, "Latitude-Longitude is outside the circle", ToastLength.Long).Show();
                }
            }

        }
           


        public void recording()
        {
            View view = LayoutInflater.Inflate(Resource.Layout.audiorecord, null);
            Android.App.AlertDialog builder = new Android.App.AlertDialog.Builder(this).Create();
            builder.SetView(view);
            builder.Window.SetLayout(600, 600);
            builder.SetCanceledOnTouchOutside(false);
            recordbtn = view.FindViewById<Button>(Resource.Id.recordbtn);
            stopbtn = view.FindViewById<Button>(Resource.Id.stopbtn);
            playbtn = view.FindViewById<Button>(Resource.Id.playbtn);
            pausebtn = view.FindViewById<Button>(Resource.Id.pausebtn);
            resumebtn = view.FindViewById<Button>(Resource.Id.resumebtn);
            Timer = view.FindViewById<TextView>(Resource.Id.timerbtn);
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
                 mediaRecorder.Stop();

                //stoprecorder();
                Timer.Text = "0:0:0";
                timer.Dispose();
                timer = null;
                //btn2.Enabled=false;
                //buttonPlayLastRecordAudio.setEnabled(true);
                //buttonStart.setEnabled(true);
                //buttonStopPlayingRecording.setEnabled(false);

                Toast.MakeText(this, "Recording completed", ToastLength.Long).Show();
            };
            pausebtn.Click += delegate
             {
                 //OnPause();
                 timer.Dispose();
                 timer = null;
             };
            builder.Show();
        }

        public void MediaRecorderReady()
        {
            mediaRecorder = new MediaRecorder();
            mediaRecorder.SetAudioSource(AudioSource.Mic);
            mediaRecorder.SetOutputFormat(OutputFormat.ThreeGpp);
            mediaRecorder.SetAudioEncoder(AudioEncoder.AmrNb);
            mediaRecorder.SetOutputFile(AudioSavePathInDevice);
        }

        public void audioplay()
        {

            View view = LayoutInflater.Inflate(Resource.Layout.record, null);
            Android.App.AlertDialog builder = new Android.App.AlertDialog.Builder(this).Create();
            builder.SetView(view);
            builder.Window.SetLayout(600, 600);
            builder.SetCanceledOnTouchOutside(false);
            mikestop = view.FindViewById<ImageButton>(Resource.Id.imageButton5);
            mikeplay = view.FindViewById<ImageButton>(Resource.Id.mikeplay);
            mikepause = view.FindViewById<ImageButton>(Resource.Id.mikepause);
            txtTimer = view.FindViewById<TextView>(Resource.Id.txtTimer);


            save = view.FindViewById<Button>(Resource.Id.save);
            cancel = view.FindViewById<Button>(Resource.Id.cancel);
            listen = view.FindViewById<Button>(Resource.Id.listen);
            mikeplay.Click += delegate
            {
                StartRecorder();
                timer = new Timer();
                timer.Interval = 1000; // 1 second  
                timer.Elapsed += Timer_Elapsed;
                timer.Start();
                mikeplay.Visibility = ViewStates.Gone;
                mikepause.Visibility = ViewStates.Visible;
            };

            mikestop.Click += delegate
            {
                stoprecorder();
                txtTimer.Text = "00:00:00";
                timer.Dispose();
                timer = null;
            };
            mikepause.Click += delegate
            {
                OnPause();
                timer.Dispose();
                timer = null;
            };
            listen.Click += delegate
            {
                StartAsync();
            };
            builder.Show();
        }

        private void StartAsync()
        {
            try
            {
                if (_player == null)
                {
                    _player = new MediaPlayer();
                }
                else
                {
                    _player.Reset();
                }

                // This method works better than setting the file path in SetDataSource. Don't know why.
                Java.IO.File file = new Java.IO.File(Compliance_Activity.filePath);
                Java.IO.FileInputStream fis = new Java.IO.FileInputStream(file);
                //_player.SetDataSourceAsync(fis.);

                _player.SetDataSource(filePath);
                _player.Prepare();

            }

            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.StackTrace);
            }
        }


        public void stoprecorder()
        {
            // _stop.Enabled = !_stop.Enabled;
            _recorder.Stop();
            _recorder.Reset();
            _player.SetDataSource(filePath);
            _player.Prepare();
            _player.Start();
            //mikestop.Visibility = ViewStates.Invisible;
        }
       
        protected override void OnResume()
        {
            base.OnResume();
            _recorder = new MediaRecorder();
            _player = new MediaPlayer();
            _player.Completion += (sender, e) =>
            {
                _player.Reset();

            };
        }
        //protected override void OnPause()
        //{
        //    base.OnPause();
        //   // _player.Release();
        //    mediaRecorder.Release();
        //   // _player.Dispose();
        //    mediaRecorder.Dispose();
        //    //_player = null;
        //    mediaRecorder = null;
        //    //_player.Release();
        //    //_recorder.Release();
        //    //_player.Dispose();
        //    //_recorder.Dispose();
        //    //_player = null;
        //    //_recorder = null;
        //}
        public void StartRecorder()
        {

            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                }
                if (_recorder == null)
                {
                    _recorder = new MediaRecorder();
                }
                else
                {
                    _recorder.Reset();
                    _recorder.SetAudioSource(AudioSource.Mic);
                    _recorder.SetOutputFormat(OutputFormat.ThreeGpp);
                    _recorder.SetAudioEncoder(AudioEncoder.AmrNb);
                    _recorder.SetOutputFile(filePath);
                    _recorder.Prepare();
                    _recorder.Start();

                }
            }
            catch (Exception ex)
            {
                Console.Out.WriteLine(ex.StackTrace);
            }
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


        private void VideoClick(object sender, EventArgs e)
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

        private void BtnCamera_Click(object sender, EventArgs e)
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
        //public Bitmap createVideoThumbNail(String path)
        //{
        //    return ThumbnailUtils.CreateVideoThumbnail(path, Android.Provider.ThumbnailKind.MicroKind);
        //}
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            
            if (requestCode == Camera)
            {
                Bitmap bitmap = BitmapFactory.DecodeFile(imageURL);
                for (int i = 0; i < 10; i++)
                {

                    image = new ImageView(this);
                    image.LayoutParameters = new LinearLayout.LayoutParams(200, 100);
                    image.SetX(10);
                    image.SetY(10);
                    image.SetImageBitmap(bitmap);
                }
                imageCount++;
                rl1.AddView(image);
            }
            if (requestCode == Video)
            {
                //Bitmap bitmap = BitmapFactory.DecodeFile(imageURL);
                for (int i = 0; i < 10; i++)
                {
                    //Bitmap thumb = ThumbnailUtils.CreateVideoThumbnail(Resource.Drawable.videofile,
                    //Android.Provider.ThumbnailKind.MiniKind);
                    image1 = new ImageView(this);
                    image1.LayoutParameters = new LinearLayout.LayoutParams(100, 100);
                    image1.SetX(10);
                    image1.SetY(10);
                    //BitmapDrawable bitmapDrawable = new BitmapDrawable(thumb);

                    image1.SetImageResource(Resource.Drawable.videofile);
                }
                videocount++;
                rl2.AddView(image1);
            }
            // imagecaptured.SetImageBitmap(bitmap);

        }
        public async Task getData()
        {
            dynamic value = new ExpandoObject();
            value.task_id = task_id;

            string json = JsonConvert.SerializeObject(value);
            try
            {
                JsonValue item = await restService.GetComplianceTask(this, json, geolocation).ConfigureAwait(false);
                comp = JsonConvert.DeserializeObject<ComplianceModel>(item);
                shapes1 = JsonConvert.DeserializeObject<Shapes>(comp.shapes);
            }
            catch (Exception ex)
            {

            }

            //db.ComplianceInsert(comp, shapes1);


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

            int posi = prefs.GetInt("position", 0);
            for (int i = 0; i <= posi; i++)
            {
                max_num = comp.lstAddedCompliance[i].max_numbers;
                file_format = comp.lstAddedCompliance[i].file_format;
                filetype = comp.lstAddedCompliance[i].file_type;
                taskoverview = comp.lstAddedCompliance[i].task_overview;
                uploaded = comp.lstAddedCompliance[i].Uploaded;
            }
        }
        public void polygon()
        {
            retVal = retVal && isPointInPolygon(tap, shapes1.shapes[0].paths[0].path);
            if (retVal == true)
            {
                Toast.MakeText(this, "Latitude-Longitude is inside the polygon", ToastLength.Long).Show();

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
        public void storeDataBaseAsync()
        {

            db.ComplianceInsert(task_id, markingtype, taskstatus, taskcreatedby, creationdate, task_name, task_description, markingDate, rownum, meatingid, deadline, comp.lstAddedCompliance, geolocation, "no");
            db.InsertcompliancejoinTable(comp.lstAddedCompliance);
            if (ic.connectivity())
            {
                compliances = db.GetCompliance(filetype, task_id);
                //  if (geo != null && geo.Count > 0)
                if (compliances != null && compliances.Count > 0)
                {
                    long Id;
                    foreach (var val in compliances)
                    {
                        Id = val.id;
                        postserviceCompliance(Id);
                    }
                }
                // complianceJoinTables = dbHelper.GetComplianceJoinTable(taskid);
                //compliances = dbHelper.GetCompliance(filetype, taskid).Join(dbHelper.GetComplianceJoinTable(taskid).Where(m => m.taskId == taskid).Select(t => new ComplianceJoinTable
                //{
                //    taskId = t.taskId
                //}).ToList());
                // compliances=dbHelper.tab



            }

        }
        

        public bool CheckMedia(string type)
        {
            if (type.Equals("Image"))
            {
                //int c = await database.GetMediaCount(type, taskId);
                //List<Compliance> list = dbHelper.GetCompliance(type, taskid) ;
                List<ComplianceJoinTable> list = db.GetComplianceJoinTable(task_id);
                if (list.Count == 0)
                {
                    Java.IO.File path = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
                    fileName1 = new Java.IO.File(path, "OPDApp");
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
                    StartActivityForResult(intent, 0);

                }
                else
                {
                    int a = list[0].max_numbers;
                    if (a > imageCount)
                    {
                        return true;
                    }
                    else
                    {
                        Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
                        Android.App.AlertDialog alert = dialog.Create();
                        alert.SetTitle("Compliance");
                        alert.SetMessage("You have reached Maximum Limit");
                        alert.SetButton("OK", (c, ev) =>
                        {
                            // Ok button click task  
                        });
                        alert.Show();
                        // await DisplayAlert("Compliance", " You Reached Maximum Limit", "OK");
                        // return false;
                    }
                }


            }
            if (type.Equals("Video"))
            {
                //int c = await database.GetMediaCount(type, taskId);
                //List<Compliance> list = dbHelper.GetCompliance(type, taskid) ;
                List<ComplianceJoinTable> list = db.GetComplianceJoinTable(task_id);
                if (list.Count == 0)
                {
                    Intent intent = new Intent(MediaStore.ActionVideoCapture);
                    intent.PutExtra(MediaStore.ExtraOutput, path);
                    intent.PutExtra(MediaStore.ExtraVideoQuality, 1);
                    // intent.PutExtra(MediaStore.ExtraDurationLimit, 10);
                    StartActivityForResult(intent, 0);

                }
                else
                {
                    int a = list[0].max_numbers;
                    if (a > videocount)
                    {
                        return true;
                    }
                    else
                    {
                        Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
                        Android.App.AlertDialog alert = dialog.Create();
                        alert.SetTitle("Compliance");
                        alert.SetMessage("You have reached Maximum Limit");
                        alert.SetButton("OK", (c, ev) =>
                        {
                            // Ok button click task  
                        });
                        alert.Show();
                        // await DisplayAlert("Compliance", " You Reached Maximum Limit", "OK");
                        // return false;
                    }
                }
            }
            if (type.Equals("Audio"))
            {
                //int c = await database.GetMediaCount(type, taskId);
                //List<Compliance> list = dbHelper.GetCompliance(type, taskid) ;
                List<ComplianceJoinTable> list = db.GetComplianceJoinTable(task_id);
                int a = list[0].max_numbers;
                if (a > audiocount)
                {
                    return true;
                }
                else
                {
                    Android.App.AlertDialog.Builder dialog = new Android.App.AlertDialog.Builder(this);
                    Android.App.AlertDialog alert = dialog.Create();
                    alert.SetTitle("Compliance");
                    alert.SetMessage("You have reached Maximum Limit");
                    alert.SetButton("OK", (c, ev) =>
                    {
                        // Ok button click task  
                    });
                    alert.Show();
                    // await DisplayAlert("Compliance", " You Reached Maximum Limit", "OK");
                    // return false;
                }
            }
            return false;

        }
        //protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        //{
        //    base.OnActivityResult(requestCode, resultCode, data);

        //    Bitmap bitmap = BitmapFactory.DecodeFile(imageURL);
        //    for (int i = 0; i < 10; i++)
        //    {

        //        image = new ImageView(this);
        //        image.LayoutParameters = new LinearLayout.LayoutParams(200, 250);
        //        image.SetX(10);
        //        image.SetY(10);
        //        image.SetImageBitmap(bitmap);

        //    }
        //    imageCount++;
        //    rl1.AddView(image);
        //    for (int i = 0; i < 10; i++)
        //    {

        //        videoView = new VideoView(this);
        //        LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(200, 250);
        //        layoutParams.SetMargins(10, 10, 10, 10);
        //    }
        //    videocount++;
        //    rl2.AddView(videoView);

        //    // imagecaptured.SetImageBitmap(bitmap);

        //}
        public async Task postserviceCompliance(long id)
        {
            string json = JsonConvert.SerializeObject(compliances);
            try
            {

                string item = await restService.CompliancePostServiceMethod(this, "SetCompleteTaskSubmition", json,  geolocation);
                if (item.Contains(""))
                {
                    db.updateComplianceStatus(id);
                    Toast.MakeText(this, "compliance post  Successfully..", ToastLength.Long).Show();
                }
            }
            catch (Exception ex)
            {

            }


        }
        //public void playIt()
        //{
        //    // Create MediaPlayer object
        //    _player = new MediaPlayer();
        //    // set start time
        //    playTime = 0;
        //    // Reset max and progress of the SeekBar
        //    //seekbar.SetMax(recordTime);
        //    //seekbar.setProgress(0);
        //    seekbar.ProgressChanged += (object sender, SeekBar.ProgressChangedEventArgs e) => {
        //        if (e.FromUser)
        //        {
        //           // _textView.Text = string.Format("SeekBar Value is {0}", e.Progress);
        //        }
        //    };
        //    try
        //    {
        //        // Initialize the player and start playing the audio
        //        _player.SetDataSource(fileName);
        //        _player.Prepare();
        //        _player.Start();
        //        // Post the play progress
        //        handler.post(UpdatePlayTime);
        //    }
        //    catch (IOException e)
        //    {
        //        //Log.e("LOG_TAG", "prepare failed");
        //    } 
        //}

        
    }
}