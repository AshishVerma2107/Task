using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Json;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Gms.Maps.Model;
using Android.Graphics;
using Android.Locations;
using Android.Media;
using Android.OS;
using Android.Preferences;
using Android.Provider;
using Android.Runtime;
using Android.Speech;
using Android.Support.Design.Widget;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using RadialProgress;
using TaskApp.Models;

namespace TaskApp
{
    [Activity(Theme = "@style/AppTheme")]
    public class complianceActivity : Activity
    {
        // "@style/Theme.Transparent"
        ImageButton camera, videobutton, location, mike, mikestop, mikeplay, mikepause;

        ImageView imageview, i;
        LinearLayout imagelayout;
        private bool isRecording;
        private readonly int VOICE = 10;
        MediaRecorder _recorder;
        MediaPlayer _player;

        string imageName, imageURL;
        Java.IO.File fileName1, fileImagePath;
        ImageView imagecaptured;
        private const int Image_Capture = 100;
        public static string filePath = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/Music/testAudio.mp3";
        MediaRecorder recorder = null;
        SeekBar seekbar;
        string AppDateTime, task_description, task_name, deadline, markto, taskcreatedby, markingDate, creationdate, taskstatus, taskcreationDate, rownum, meatingid, markby, markingtype;
        string l_id = "12345";
        string user_id = "5";
        string taskid = "9-A19-AAAB-AAA-AAA";
        string filetype = "";
        string filename = "";
        string filesize = "";
        string geolocation = "0";
        string file_format = "";
        int max_num;
        string taskoverview;
        string uploaded;
        List<Compliance> compliances;
        //List<ComplianceJoinTable> compjointable;
        ServiceHelper restService;
        //List<Compliance> comp;
        DbHelper dbHelper;
        Compliance compliance;
        Compliance comp;
        InternetConnection con;
        EditText description;
        TextView txtTimer, taskname, taskdescription, createdBy, deadlinedate, creation_date, markedto, taskdetail;
        ImageView image;
        VideoView videoView;
        Timer timer;
        Button submit, save, cancel, listen;
        int imageCount, videocount, audiocount;
        ISharedPreferences prefs1;
        List<ComplianceJoinTable> complianceJoinTables;
        LinearLayout rl1, rl2, rl3, rl4;
        int hour = 0, min = 0, sec = 0;
        Boolean retVal = true;
        RadialProgressView radialProgrssView;
        Shapes shapes1;
        Geolocation geo;
        string[] tap;
        string path = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/test.mp4";
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            // GetPermissionAsync();

            isRecording = false;
            SetContentView(Resource.Layout.compaliance_layout);
            restService = new ServiceHelper();
            dbHelper = new DbHelper();
            con = new InternetConnection();
            geo = new Geolocation();
            geolocation = geo.GetGeoLocation(ApplicationContext);
            compliance = new Compliance();
            complianceJoinTables = new List<ComplianceJoinTable>();
            comp = new Compliance();
            tap = geolocation.Split(",");
            // tap = "26.449923,80.331871".Split(",");
            getData();
            ComplianceJoinTable comjoin = new ComplianceJoinTable();
            prefs1 = PreferenceManager.GetDefaultSharedPreferences(this);

            //  checkbox1 = prefs1.GetBoolean("checkboxvalue", true);
            // string position = Intent.GetStringExtra("Position")?? "Data not available...";
            // PostServiceComplianceAsync();
            camera = FindViewById<ImageButton>(Resource.Id.imageButton1);
            videobutton = FindViewById<ImageButton>(Resource.Id.imageButton2);
            location = FindViewById<ImageButton>(Resource.Id.imageButton4);
            mike = FindViewById<ImageButton>(Resource.Id.imageButton3);
            //mikestop = FindViewById<ImageButton>(Resource.Id.imageButton5);
            //mikeplay = FindViewById<ImageButton>(Resource.Id.mikeplay);
            imagecaptured = FindViewById<ImageView>(Resource.Id.imageView1);
            description = FindViewById<EditText>(Resource.Id.ed2);
            taskdescription = FindViewById<TextView>(Resource.Id.taskdescription);
            taskname = FindViewById<TextView>(Resource.Id.taskName);
            taskdetail = FindViewById<TextView>(Resource.Id.taskDetail);
            deadlinedate = FindViewById<TextView>(Resource.Id.deadlineDate);
            createdBy = FindViewById<TextView>(Resource.Id.createdBy);
            creation_date = FindViewById<TextView>(Resource.Id.creationDate);
            //imagelayout = FindViewById<LinearLayout>(Resource.Id.linearlayout1);
            rl1 = FindViewById<LinearLayout>(Resource.Id.rl1);
            rl2 = FindViewById<LinearLayout>(Resource.Id.rl2);
            rl3 = FindViewById<LinearLayout>(Resource.Id.rl3);
            rl4 = FindViewById<LinearLayout>(Resource.Id.rl4);
            //   getData();
            // taskname.Text = comp.task_name.ToString();
            taskdescription.Text = comp.description;
            createdBy.Text = comp.task_created_by;
            creation_date.Text = comp.task_creation_date;
            //markedto.Text = comp.markTo;
            creation_date.Text = comp.task_creation_date;
            submit = FindViewById<Button>(Resource.Id.buttonSubmit);
            // taskname=

            submit.Click += SubmitCompliance;

            // seekbar = FindViewById<SeekBar>(Resource.Id.seekbar);
            //mike.Click += delegate
            //{
            //    audioplay();
            //};

            location.Click += delegate
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
                else
                {
                    // map marked
                }
                //var geoUri = Android.Net.Uri.Parse("geo:26.8467,80.9462");
                //var mapIntent = new Intent(Intent.ActionView, geoUri);
                //StartActivity(mapIntent);

            };

            //if (imagelayout != null)
            //{
            //    imagelayout.RemoveView(i);
            //}

            //mike.Click += RecordAudio;
            string path = Android.OS.Environment.ExternalStorageDirectory.AbsolutePath + "/test.mp4";
            camera.Click += BtnCamera_Click;
            videobutton.Click += VideoClick;

        }
        //public void audioplay()
        //{
        //    View view = LayoutInflater.Inflate(Resource.Layout.record, null);
        //    AlertDialog builder = new AlertDialog.Builder(this).Create();
        //    builder.SetView(view);
        //    builder.Window.SetLayout(600, 600);
        //    builder.SetCanceledOnTouchOutside(false);
        //    mikestop = view.FindViewById<ImageButton>(Resource.Id.imageButton5);
        //    mikeplay = view.FindViewById<ImageButton>(Resource.Id.mikeplay);
        //    mikepause = view.FindViewById<ImageButton>(Resource.Id.mikepause);
        //    txtTimer = view.FindViewById<TextView>(Resource.Id.txtTimer);
        //    save = view.FindViewById<Button>(Resource.Id.save);
        //    cancel = view.FindViewById<Button>(Resource.Id.cancel);
        //    listen = view.FindViewById<Button>(Resource.Id.listen);
        //    mikeplay.Click += delegate
        //    {
        //        StartRecorder();
        //        timer = new Timer();
        //        timer.Interval = 1000; // 1 second  
        //        timer.Elapsed += Timer_Elapsed;
        //        timer.Start();
        //        mikeplay.Visibility = ViewStates.Gone;
        //        mikepause.Visibility = ViewStates.Visible;
        //    };

        //    mikestop.Click += delegate
        //    {
        //        stoprecorder();
        //        txtTimer.Text = "00:00:00";
        //        timer.Dispose();
        //        timer = null;
        //    };
        //    mikepause.Click += delegate
        //    {
        //        OnPause();
        //        timer.Dispose();
        //        timer = null;
        //    };
        //    listen.Click += delegate
        //    {
        //        StartAsync();
        //    };
        //    builder.Show();
        //}

        private void SubmitCompliance(object sender, EventArgs e)
        {
            storeDataBaseAsync();

        }
        //private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        //{
        //    sec++;
        //    if (sec == 60)
        //    {
        //        min++;
        //        sec = 0;
        //    }
        //    if (min == 60)
        //    {
        //        hour++;
        //        min = 0;
        //    }
        //    RunOnUiThread(() => { txtTimer.Text = $"{hour}:{min}:{sec}"; });
        //    radialProgrssView.Value = sec;
        //}

        //private void StartAsync()
        //{
        //    try
        //    {
        //        if (_player == null)
        //        {
        //            _player = new MediaPlayer();
        //        }
        //        else
        //        {
        //            _player.Reset();
        //        }

        //        // This method works better than setting the file path in SetDataSource. Don't know why.
        //        Java.IO.File file = new Java.IO.File(complianceActivity.filePath);
        //        Java.IO.FileInputStream fis = new Java.IO.FileInputStream(file);
        //        _player.SetDataSourceAsync(fis.FD);
        //        //player.SetDataSource(filePath);
        //        _player.Prepare();
        //        _player.Start();
        //    }

        //    catch (Exception ex)
        //    {
        //        Console.Out.WriteLine(ex.StackTrace);
        //    }
        //}

        //public void stoprecorder()
        //{
        //    // _stop.Enabled = !_stop.Enabled;
        //    _recorder.Stop();
        //    _recorder.Reset();
        //    _player.SetDataSource(filePath);
        //    _player.Prepare();
        //    _player.Start();
        //    mikestop.Visibility = ViewStates.Invisible;
        //}
        public async Task getData()
        {
            dynamic value = new ExpandoObject();
            value.task_id = "9-A19-AAAB-AAA-AAA";

            string json = JsonConvert.SerializeObject(value);
            try
            {
                JsonValue item = await restService.GetComplianceTask(this, l_id, user_id, json, "0").ConfigureAwait(false);
                comp = JsonConvert.DeserializeObject<Compliance>(item);
                shapes1 = JsonConvert.DeserializeObject<Shapes>(comp.shapes);
            }
            catch (Exception ex)
            {

            }
            taskid = comp.task_id;
            task_description = comp.description;
            deadline = comp.deadline_date;
            meatingid = comp.Meeting_ID;
            rownum = comp.RowNo;
            taskcreationDate = comp.task_creation_date;
            markby = comp.task_mark_by;
            taskstatus = comp.taskStatus;
            markto = comp.markTo;
            markingtype = comp.task_marking_type;
            taskcreatedby = comp.task_created_by;
            markingDate = comp.MarkingDate;
            creationdate = comp.task_creation_date;

            int posi = prefs1.GetInt("position", 0);
            for (int i = 0; i <= posi; i++)
            {
                max_num = comp.lstAddedCompliance[i].max_numbers;
                file_format = comp.lstAddedCompliance[i].file_format;
                //  filesize = compliance.lstAddedCompliance[0].files
                filetype = comp.lstAddedCompliance[i].file_type;
                //taskid = comp.lstAddedCompliance[1].taskId;
                taskoverview = comp.lstAddedCompliance[i].task_overview;
                uploaded = compliance.lstAddedCompliance[i].Uploaded;
            }
        }
        //protected override void OnResume()
        //{
        //    base.OnResume();
        //    _recorder = new MediaRecorder();
        //    _player = new MediaPlayer();
        //    _player.Completion += (sender, e) => {
        //        _player.Reset();

        //    };
        //}
        //protected override void OnPause()
        //{
        //    base.OnPause();
        //    _player.Release();
        //    _recorder.Release();
        //    _player.Dispose();
        //    _recorder.Dispose();
        //    _player = null;
        //    _recorder = null;
        //}
        //public void StartRecorder()
        //{
        //    try
        //    {
        //        if (File.Exists(filePath))
        //        {
        //            File.Delete(filePath);
        //        }
        //        if (_recorder == null)
        //        {
        //            _recorder = new MediaRecorder();
        //        }
        //        else
        //        {
        //            _recorder.Reset();
        //            _recorder.SetAudioSource(AudioSource.Mic);
        //            _recorder.SetOutputFormat(OutputFormat.ThreeGpp);
        //            _recorder.SetAudioEncoder(AudioEncoder.AmrNb);
        //            _recorder.SetOutputFile(filePath);
        //            _recorder.Prepare();
        //            _recorder.Start();

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.Out.WriteLine(ex.StackTrace);
        //    }
        //}


        private void VideoClick(object sender, EventArgs e)
        {
            max_num = comp.lstAddedCompliance[1].max_numbers;
            CheckMedia(filetype);
            if (CheckMedia(filetype).Equals(false))
            {
                return;
            }
            else
            {
                Intent intent = new Intent(MediaStore.ActionVideoCapture);
                intent.PutExtra(MediaStore.ExtraOutput, path);
                intent.PutExtra(MediaStore.ExtraVideoQuality, 1);
                // intent.PutExtra(MediaStore.ExtraDurationLimit, 10);
                StartActivityForResult(intent, 0);
            }
        }

        private void BtnCamera_Click(object sender, EventArgs e)
        {
            // string maxnum;

            max_num = comp.lstAddedCompliance[1].max_numbers;
            uploaded = comp.lstAddedCompliance[1].Uploaded;
            CheckMedia(filetype);
            if (CheckMedia(filetype).Equals(false))
            {
                return;
            }
            else
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
        }




        public bool CheckMedia(string type)
        {
            if (type.Equals("Image"))
            {
                //int c = await database.GetMediaCount(type, taskId);
                //List<Compliance> list = dbHelper.GetCompliance(type, taskid) ;
                List<ComplianceJoinTable> list = dbHelper.GetComplianceJoinTable(taskid);
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
                        AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                        AlertDialog alert = dialog.Create();
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
                List<ComplianceJoinTable> list = dbHelper.GetComplianceJoinTable(taskid);
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
                        AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                        AlertDialog alert = dialog.Create();
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
                List<ComplianceJoinTable> list = dbHelper.GetComplianceJoinTable(taskid);
                int a = list[0].max_numbers;
                if (a > audiocount)
                {
                    return true;
                }
                else
                {
                    AlertDialog.Builder dialog = new AlertDialog.Builder(this);
                    AlertDialog alert = dialog.Create();
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
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            Bitmap bitmap = BitmapFactory.DecodeFile(imageURL);
            for (int i = 0; i < 10; i++)
            {

                image = new ImageView(this);
                image.LayoutParameters = new LinearLayout.LayoutParams(200, 250);
                image.SetX(10);
                image.SetY(10);
                image.SetImageBitmap(bitmap);

            }
            imageCount++;
            rl1.AddView(image);
            for (int i = 0; i < 10; i++)
            {

                videoView = new VideoView(this);
                LinearLayout.LayoutParams layoutParams = new LinearLayout.LayoutParams(200, 250);
                layoutParams.SetMargins(10, 10, 10, 10);
            }
            videocount++;
            rl2.AddView(videoView);

            // imagecaptured.SetImageBitmap(bitmap);

        }
        public async Task postserviceCompliance(long id)
        {
            string json = JsonConvert.SerializeObject(compliances);
            try
            {

                string item = await restService.CompliancePostServiceMethod(this, l_id, user_id, "SetCompleteTaskSubmition", json, AppDateTime, geolocation);
                if (item.Contains(""))
                {
                    dbHelper.updateComplianceStatus(id);
                    Toast.MakeText(this, "compliance post  Successfully..", ToastLength.Long).Show();
                }
            }
            catch (Exception ex)
            {

            }


        }

        public void storeDataBaseAsync()
        {

            dbHelper.ComplianceInsert(taskid, markingtype, taskstatus, taskcreatedby, creationdate, task_name, task_description, markingDate, rownum, meatingid, deadline, comp.lstAddedCompliance, geolocation, "no");
            dbHelper.InsertcompliancejoinTable(comp.lstAddedCompliance);
            if (con.connectivity())
            {
                compliances = dbHelper.GetCompliance(filetype, taskid);
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
    }


}