using System;
using System.Collections.Generic;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;
using Android.Locations;
using Android.Media;
using Android.Preferences;
using Android.Provider;
using Android.Speech;
using Newtonsoft.Json;
using RadialProgress;
using TaskAppWithLogin.Adapter;
using TaskAppWithLogin.Constants;
using TaskAppWithLogin.Models;

using System.Threading.Tasks;
using System.Dynamic;
using System.Json;
using Android.Gms.Maps.Model;
using Android.Graphics;
using System.Timers;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.App;
using Android.Support.Design.Widget;
using Android.Content.PM;
using FragmentTransaction = Android.Support.V4.App.FragmentTransaction;

namespace TaskAppWithLogin.Fragments
{
    [Activity(Theme = "@android:style/Theme.Dialog")]
    public class ComplainceFrag : BottomSheetDialogFragment
    {
        ServiceHelper restService;
        Geolocation geo;
        DbHelper db;
        InternetConnection ic;
        ISharedPreferences prefs;
        BlobFileUpload blob;

        private readonly int Video = 20;
        private readonly int Camera = 10;
        private readonly int VOICE = 30;
        private bool isRecording;
        string geolocation;
        string[] tap;
        string task_id = "", task_description = "", task_name = "", deadline = "", markto = "", taskcreatedby = "", markingDate = "", creationdate = "", markby = "", status="";
        string taskstatus, rownum, meatingid, markingtype, file_format = "", filetype = "", taskoverview, uploaded, shapes_from_Comp;
        string filename = "", filesize = "";
        int max_num;
        string imageName, imageURL, videoName, audioname, videoURL;
        string AudioSavePathInDevice = null;
        int hour = 00, min = 00, sec = 00;
        string Click_Type, task_id_to_send;

        private Handler mHandler = new Handler();
        Java.IO.File fileName1, fileImagePath, audiofile, fileVideoPath, fileaudioPath;

        EditText Description;
        TextView descrip_text, detail_text, name_text, markby_text, deadline_text, creationdate_text, createdby_text;
        ImageButton camera, video, microphone, create_by_call, mark_by_call;
        LinearLayout linear1, linear2, linear3, ll_task_desc;
        Button recordbtn, resumebtn, pausebtn, savebtn;
        TextView txtTimer, Timer, Image_no, Video_no, Audio_no, task_desc;
        RadialProgressView radialProgrssView;
        Timer timer;
        Button Submit_Btn, Done_Btn;
        MediaRecorder _recorder, mediaRecorder;
        Android.App.ProgressDialog progress;
        SeekBar seekBar;
        ImageView comment_micro, playbtn, stopbtn;
        public static ExpandableHeightGridView Gridview1, Gridview2, Gridview3;
        public static GridViewAdapter_Image adapter1;
        public static GridViewAdapter_Video adapter2;
        public static GridViewAdapter_Audio adapter3;
        Action action;
        MediaPlayer mediaPlayer;
        ComplianceModel comp;
        CardView referencecardview;
        string filenametaskmapping, filesizetaskmapping, pathtaskmapping;
        TextView uploadimage, uploadvideo, uploadaudio;
        
        List<ComplianceJoinTable> audio_lst;
        List<ComplianceJoinTable> video_lst;
        List<ComplianceJoinTable> image_lst;
        string creat_by_num, mark_by_num;
        public static List<Comp_AttachmentModel> audio_comp_lst;
        public static List<Comp_AttachmentModel> video_comp_lst;
        public static List<Comp_AttachmentModel> image_comp_lst;
        int img_max = 0, vdo_max = 0, aud_max = 0, img_min = 0, vdo_min = 0, aud_min = 0;
        Shapes shapes1;
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
            blob = new BlobFileUpload();

            geolocation = geo.GetGeoLocation(Activity);
            tap = geolocation.Split(',');
            prefs = PreferenceManager.GetDefaultSharedPreferences(Activity);
            isRecording = false;
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {

            View view = inflater.Inflate(Resource.Layout.layout_complaince, null);
            task_id_to_send = Arguments.GetString("task_id") ?? string.Empty;

            referencecardview = view.FindViewById<CardView>(Resource.Id.referncecardview);
            Description = view.FindViewById<EditText>(Resource.Id.comment);
            descrip_text = view.FindViewById<TextView>(Resource.Id.c_descrip);
            name_text = view.FindViewById<TextView>(Resource.Id.c_name);
            detail_text = view.FindViewById<TextView>(Resource.Id.c_detail);
            markby_text = view.FindViewById<TextView>(Resource.Id.c_markby);
            deadline_text = view.FindViewById<TextView>(Resource.Id.c_deadline);
            createdby_text = view.FindViewById<TextView>(Resource.Id.c_createdby);
            creationdate_text = view.FindViewById<TextView>(Resource.Id.c_creationdate);
            camera = view.FindViewById<ImageButton>(Resource.Id.camera_btn);
            video = view.FindViewById<ImageButton>(Resource.Id.video_btn);
            create_by_call = view.FindViewById<ImageButton>(Resource.Id.create_by_call);
            mark_by_call = view.FindViewById<ImageButton>(Resource.Id.mark_by_call);
            microphone = view.FindViewById<ImageButton>(Resource.Id.micro_btn);
            linear1 = view.FindViewById<LinearLayout>(Resource.Id.ll1);
            linear2 = view.FindViewById<LinearLayout>(Resource.Id.ll2);
            linear3 = view.FindViewById<LinearLayout>(Resource.Id.ll3);
            ll_task_desc = view.FindViewById<LinearLayout>(Resource.Id.ll_task_desc);
            task_desc = view.FindViewById<TextView>(Resource.Id.task_desc);
            Image_no = view.FindViewById<TextView>(Resource.Id.image_no);
            Video_no = view.FindViewById<TextView>(Resource.Id.video_no);
            Audio_no = view.FindViewById<TextView>(Resource.Id.audio_no);
            comment_micro = view.FindViewById<ImageView>(Resource.Id.comment_micro);
            Submit_Btn = view.FindViewById<Button>(Resource.Id.submit);
            uploadimage = view.FindViewById<TextView>(Resource.Id.uploaded_no1);
            uploadvideo = view.FindViewById<TextView>(Resource.Id.uploaded_no2);
            uploadaudio = view.FindViewById<TextView>(Resource.Id.uploaded_no3);
            Gridview1 = view.FindViewById<ExpandableHeightGridView>(Resource.Id.gridView1);
            Gridview2 = view.FindViewById<ExpandableHeightGridView>(Resource.Id.gridView2);
            Gridview3 = view.FindViewById<ExpandableHeightGridView>(Resource.Id.gridView3);

            progress = new Android.App.ProgressDialog(Activity);
            progress.Indeterminate = true;
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            progress.SetCancelable(false);
            progress.SetMessage("Please wait...");

            referencecardview.Click += delegate
            {
                ReferenceAttachmentActivity reference = new ReferenceAttachmentActivity();
                Android.Support.V4.App.FragmentTransaction ft = FragmentManager.BeginTransaction();
                //ft.Replace(Resource.Id.container, reference);
                ft.Hide(FragmentManager.FindFragmentByTag("ComplainceFragment"));
                ft.Add(Resource.Id.container, reference);
                ft.AddToBackStack(null);
                ft.SetTransition(FragmentTransaction.TransitFragmentOpen);
                ft.Commit();
                Bundle bundle = new Bundle();
                bundle.PutString("TaskId", task_id_to_send);
                reference.Arguments = bundle;
            };

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

            Gridview1.setExpanded(true);
            //Gridview1.ChoiceMode = (ChoiceMode)AbsListViewChoiceMode.MultipleModal;
            //Gridview1.SetMultiChoiceModeListener(new MultiChoiceModeListener1(Activity));

            Gridview2.setExpanded(true);
            //Gridview2.ChoiceMode = (ChoiceMode)AbsListViewChoiceMode.MultipleModal;
            //Gridview2.SetMultiChoiceModeListener(new MultiChoiceModeListener2(Activity));

            Gridview3.setExpanded(true);
            //Gridview3.ChoiceMode = (ChoiceMode)AbsListViewChoiceMode.MultipleModal;
            //Gridview3.SetMultiChoiceModeListener(new MultiChoiceModeListener3(Activity));


            //if (ic.connectivity())
            //{
                getDataFromServer();
            //}
            //else
           // {

            //}
            //attachmentData = db.GetComp_Attachments(task_id_to_send);
            //if (attachmentData != null)
            //{
            //    for (int i = 0; i < attachmentData.Count; i++)
            //    {
            //        if (attachmentData[i].file_type.Equals("Image"))
            //        {
            //            imagelist.Add(attachmentData[i]);

            //        }
            //        else if (attachmentData[i].file_type.Equals("Video"))
            //        {
            //            videolist.Add(attachmentData[i]);
            //        }
            //        else if (attachmentData[i].file_type.Equals("Audio"))
            //        {
            //            audiolist.Add(attachmentData[i]);
            //        }
            //    }
            //    adapter1 = new GridViewAdapter_Image(Activity, imagelist);
            //    Gridview1.Adapter = adapter1;

            //    adapter2 = new GridViewAdapter_Video(Activity, videolist);
            //    Gridview2.Adapter = adapter2;

            //    adapter3 = new GridViewAdapter_Audio(Activity, audiolist);
            //    Gridview3.Adapter = adapter3;

            //}




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
                if (audio_comp_lst.Count < aud_max)
                {
                    if (shapes1 != null)
                    {
                        if (CheckForShape())
                        {
                            recording();
                        }
                        else
                        {
                            Toast.MakeText(Activity, "you are outside marked area", ToastLength.Long).Show();
                        }
                    }
                    else
                    {
                        recording();
                    }

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

            return view;
        }

        public async Task getDataFromServer()
        {
            //if (ic.connectivity())
            //{
                progress.Show();
                dynamic value = new ExpandoObject();
                value.task_id = task_id_to_send;

                string json = JsonConvert.SerializeObject(value);
                try
                {
                    JsonValue item = await restService.GetComplianceTask(Activity, json, geolocation);
                     comp = JsonConvert.DeserializeObject<ComplianceModel>(item);
                    

                    //db.ComplianceInsert(compliance);
                    progress.Dismiss();
                }
                catch (Exception e) { progress.Dismiss(); }
            //}

              // List< ComplianceModel>comp= db.GetCompliance(task_id_to_send);
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

                List<ComplianceJoinTable> lstAddedCompliance = comp.lstAddedCompliance;
                List<CommunicationModel> lstCommunication = comp.lstCommunication;
                List<TaskFilemappingModel2> lstTaskFileMapping = comp.lstTaskFileMapping;
                List<Comp_AttachmentModel> lstUploadedCompliance = comp.lstUploadedCompliance;

                image_lst = new List<ComplianceJoinTable>();
                audio_lst = new List<ComplianceJoinTable>();
                video_lst = new List<ComplianceJoinTable>();


                for (int i = 0; i < lstAddedCompliance.Count; i++)
                {
                    if (lstAddedCompliance[i].file_type.Equals("Image"))
                    {
                        image_lst.Add(lstAddedCompliance[i]);
                        img_max += lstAddedCompliance[i].max_numbers;
                        try
                        {
                            if (lstAddedCompliance[i].complianceType.ToLower().Equals("mandatory"))
                                img_min += lstAddedCompliance[i].max_numbers;
                        }
                        catch (Exception e) { }
                    }
                    else if (lstAddedCompliance[i].file_type.Equals("Audio"))
                    {
                        audio_lst.Add(lstAddedCompliance[i]);
                        aud_max += lstAddedCompliance[i].max_numbers;
                        try
                        {
                            if (lstAddedCompliance[i].complianceType.ToLower().Equals("mandatory"))
                                aud_min += lstAddedCompliance[i].max_numbers;
                        }
                        catch (Exception e) { }
                    }
                    else if (lstAddedCompliance[i].file_type.Equals("Video"))
                    {
                        video_lst.Add(lstAddedCompliance[i]);
                        vdo_max += lstAddedCompliance[i].max_numbers;
                        try { 
                            if (lstAddedCompliance[i].complianceType.ToLower().Equals("mandatory"))
                                vdo_min += lstAddedCompliance[i].max_numbers;
                        }
                        catch (Exception e)
                        { progress.Dismiss(); }
                    }
                }

                for (int j = 0; j < lstCommunication.Count; j++)
                {
                    if (lstCommunication[j].role.Equals("Assigner"))
                        mark_by_num = lstCommunication[j].mobile;
                    if (lstCommunication[j].role.Equals("Creator"))
                        creat_by_num = lstCommunication[j].mobile;
                }

                
               // db.InsertCreatecomplianceAttachData(lstTaskFileMapping, task_id_to_send);
                
                audio_comp_lst = new List<Comp_AttachmentModel>();
                video_comp_lst = new List<Comp_AttachmentModel>();
                image_comp_lst = new List<Comp_AttachmentModel>();
                for (int l = 0; l < lstUploadedCompliance.Count; l++)
                {
                    if (lstUploadedCompliance[l].file_type.Equals("Video"))
                    {
                        video_comp_lst.Add(lstUploadedCompliance[l]);
                    }
                    if (lstUploadedCompliance[l].file_type.Equals("Audio"))
                    {
                        audio_comp_lst.Add(lstUploadedCompliance[l]);
                    }

                    if (lstUploadedCompliance[l].file_type.Equals("Image"))
                    {
                        image_comp_lst.Add(lstUploadedCompliance[l]);
                    }
                }

                descrip_text.Text = task_description;
                createdby_text.Text = taskcreatedby;
                markby_text.Text = markby;
                creationdate_text.Text = creationdate;
                deadline_text.Text = deadline;
                name_text.Text = task_name;
                uploadimage.Text = image_comp_lst.Count.ToString();
                uploadaudio.Text = audio_comp_lst.Count.ToString();
                uploadvideo.Text = video_comp_lst.Count.ToString();
                //if(!task_description.Equals("") && task_description != null)
                //{
                //    ll_task_desc.Visibility = ViewStates.Visible;
                //    task_desc.Text = task_description;
                //}
                //else
                //{
                //    ll_task_desc.Visibility = ViewStates.Gone;
                //}

                Image_no.Text = img_max.ToString();
                Video_no.Text = vdo_max.ToString();
                Audio_no.Text = aud_max.ToString();

                adapter1 = new GridViewAdapter_Image(Activity, image_comp_lst, FragmentManager);
                Gridview1.Adapter = adapter1;

                adapter2 = new GridViewAdapter_Video(Activity, video_comp_lst, FragmentManager);
                Gridview2.Adapter = adapter2;

                adapter3 = new GridViewAdapter_Audio(Activity, audio_comp_lst, FragmentManager);
                Gridview3.Adapter = adapter3;

                progress.Dismiss();
        }
           

            //db.ComplianceInsert(comp, shapes1);

            //storeDataBaseAsync();



            //int posi = prefs.GetInt("position", 0);
            //for (int i = 0; i <= comp.lstAddedCompliance.Count; i++)
            //{
            //    max_num = comp.lstAddedCompliance[i].max_numbers;
            //    file_format = comp.lstAddedCompliance[i].file_format;
            //    filetype = comp.lstAddedCompliance[i].file_type;
            //    taskoverview = comp.lstAddedCompliance[i].task_overview;
            //    uploaded = comp.lstAddedCompliance[i].Uploaded;

            //    if (filetype.Equals("Image"))
            //    {
            //        image_max = max_num;
            //    }
            //    else if (filetype.Equals("Video"))
            //    {
            //        video_max = max_num;
            //    }
            //    else if (filetype.Equals("Audio"))
            //    {
            //        audio_max = max_num;
            //    }
            //    Image_no.Text = image_max.ToString();
            //    Video_no.Text = video_max.ToString();
            //    Audio_no.Text = audio_max.ToString();

        

        //progress.Dismiss();
        //}

        //public void storeDataBaseAsync()
        //{
        //    db.ComplianceInsert(task_id, markingtype, taskstatus, taskcreatedby, creationdate, task_name, task_description, markingDate, rownum, meatingid, deadline, comp.lstAddedCompliance, geolocation, "no", shapes_from_Comp);
        //    db.InsertcompliancejoinTable(comp.lstAddedCompliance,"no");
        //    taskmappinglist = db.GetFullCreatecomplianceAttachmentData(task_id_to_send);
        //    for(int i =0; i < taskmappinglist.Count; i++)
        //    {
        //        if (taskmappinglist[i].taskId == task_id_to_send)
        //        {

        //        }
        //        else
        //        {
        //            db.InsertCreatecomplianceAttachData(comp.lstTaskFileMapping, task_id_to_send);
        //        }
        //    }
        //    if (taskmappinglist.Count == 0)
        //    {
        //        db.InsertCreatecomplianceAttachData(comp.lstTaskFileMapping, task_id_to_send);
        //    }


        //}



        public bool CheckForShape()
        {
            if (shapes1.shapes[0].type == "polygon")
            {
               return isPointInPolygon(tap, shapes1.shapes[0].paths[0].path);
            }
            else if (shapes1.shapes[0].type == "circle")
            {
                if (isMarkerOutsideCircle())
                {
                    return true;
                }
                else
                {
                    Toast.MakeText(Activity, "Latitude-Longitude is outside the circle", ToastLength.Long).Show();
                    return false;
                }
            }
            return false;

        }

        public void CheckForShapeData_Camera()
        {
                if (image_comp_lst.Count < img_max)
                {
                    if (shapes1 != null)
                    {
                       if(CheckForShape())
                       {
                          BtnCamera_Click();
                       }
                       else
                       {
                          Toast.MakeText(Activity, "you are outside marked area", ToastLength.Long).Show();
                       }
                    }
                    else
                    {
                       BtnCamera_Click();
                    }
                }
                else if (image_comp_lst.Count == img_max)
                {
                    Toast.MakeText(Activity, "Reached Maximum Limit", ToastLength.Long).Show();
                }
        }
        public void CheckForShapeData_Video()
        {

            if (video_comp_lst.Count < vdo_max)
            {
                if (shapes1 != null)
                {
                    if (CheckForShape())
                    {
                        VideoClick();
                    }
                    else
                    {
                        Toast.MakeText(Activity, "you are outside marked area", ToastLength.Long).Show();
                    }
                }
                else
                {
                    VideoClick();
                }

            }
            else if (video_comp_lst.Count == vdo_max)
            {
                Toast.MakeText(Activity, "Reached Maximum Limit", ToastLength.Long).Show();
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
            videoName = Utility.fileName2();
            Intent intent = new Intent(MediaStore.ActionVideoCapture);
            fileVideoPath = new Java.IO.File(fileName1, string.Format(videoName, Guid.NewGuid()));
            videoURL = fileVideoPath.AbsolutePath;
            intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(fileVideoPath));
            intent.PutExtra(MediaStore.ExtraVideoQuality, 1);
            // intent.PutExtra(MediaStore.ExtraDurationLimit, 10);
            StartActivityForResult(intent, Video);
        }
        public async override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (requestCode == Camera && resultCode == (int)Android.App.Result.Ok)
            {
                Bitmap bitmap;

                //Converstion Image Size  
                int height = Resources.DisplayMetrics.HeightPixels;
                int width = Resources.DisplayMetrics.WidthPixels;
                using (bitmap = fileImagePath.Path.LoadAndResizeBitmap(width / 4, height / 4))
                {

                }
                long size1 = fileImagePath.Length() / 1024;
                string imgsize = size1.ToString();

                Comp_AttachmentModel attachmentModel = new Comp_AttachmentModel();
                attachmentModel.localPath = imageURL;
                attachmentModel.file_type = "Image";
                attachmentModel.FileName = imageName;
                attachmentModel.taskId = task_id_to_send;
                attachmentModel.GeoLocation = geolocation;
                attachmentModel.FileSize = imgsize;
                attachmentModel.file_format = ".jpg";
                //attachmentModel.max_numbers = image_max.ToString();

               

                Android.App.AlertDialog.Builder alertDiag = new Android.App.AlertDialog.Builder(Activity);
                alertDiag.SetTitle("Upload Compliance");
                alertDiag.SetMessage("press upload to continue");
                alertDiag.SetPositiveButton("Upload", (senderAlert, args) =>
                {
                    db.InsertAttachmentData(attachmentModel, "no");
                    image_comp_lst.AddRange(db.GetAttachmentData(imageName));

                    adapter1 = new GridViewAdapter_Image(Activity, image_comp_lst, FragmentManager);
                    Gridview1.Adapter = adapter1;

                    if (ic.connectivity())
                    {
                        postattachmentcomplianceAsync(attachmentModel);
                        //db.updateComplianceattachmentstatus("yes");
                        //uploadcountimage++;
                        //uploadimage.Text = uploaded;
                       
                    }

                });
                alertDiag.SetNegativeButton("Cancel", (senderAlert, args) =>
                {
                    alertDiag.Dispose();
                });
                Dialog diag = alertDiag.Create();
                diag.Show();
                


            }
            if (requestCode == Video && resultCode == (int)Android.App.Result.Ok)
            {
                long size2 = fileVideoPath.Length() / 1024;
                string videosize = size2.ToString();

                Comp_AttachmentModel attachmentModel = new Comp_AttachmentModel();
                attachmentModel.localPath = videoURL;
                attachmentModel.file_type = "Video";
                attachmentModel.FileName = videoName;
                attachmentModel.taskId = task_id_to_send;
                attachmentModel.GeoLocation = geolocation;
                attachmentModel.FileSize = videosize;
                attachmentModel.file_format = ".mp4";
                //attachmentModel.max_numbers = video_max.ToString();

                db.InsertAttachmentData(attachmentModel, "no");

                //comp_AttachmentModels.Add(attachmentModel);


                //imagelist.AddRange(comp_AttachmentModels.Where(p => p.Attachment_Type == "Image" ));
                video_comp_lst.AddRange(db.GetAttachmentData(videoName));
                // postattachmentcomplianceAsync(attachmentModel);
                adapter2 = new GridViewAdapter_Video(Activity, video_comp_lst, FragmentManager);
                Gridview2.Adapter = adapter2;
               
                if (ic.connectivity())
                {
                    postattachmentcomplianceAsync(attachmentModel);
                    //db.updateComplianceattachmentstatus("yes");
                    //uploadcountvideo++;
                    //uploadvideo.Text = uploaded;

                }

            }

            if (requestCode == VOICE && resultCode == (int)Android.App.Result.Ok)
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

            uploadimage.Text = image_comp_lst.Count.ToString();
            uploadaudio.Text = audio_comp_lst.Count.ToString();
            uploadvideo.Text = video_comp_lst.Count.ToString();
        }

        public void recording()
        {
            View view = LayoutInflater.Inflate(Resource.Layout.audio_recorder, null);
            Android.App.AlertDialog builder = new Android.App.AlertDialog.Builder(Activity).Create();
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
                long size3 = fileaudioPath.Length() / 1024;
                string audiosize = size3.ToString();
                Comp_AttachmentModel attachmentModel = new Comp_AttachmentModel();
                attachmentModel.localPath = AudioSavePathInDevice;
                attachmentModel.file_type = "Audio";
                attachmentModel.FileName = audioname;
                attachmentModel.taskId = task_id_to_send;
                attachmentModel.GeoLocation = geolocation;
                attachmentModel.FileSize = audiosize;
                attachmentModel.file_format = ".mp3";
               // attachmentModel.max_numbers = audio_max.ToString();
                db.InsertAttachmentData(attachmentModel, "no");
                //comp_AttachmentModels.Add(attachmentModel);
                //imagelist.AddRange(comp_AttachmentModels.Where(p => p.Attachment_Type == "Image" ));
                audio_comp_lst.AddRange(db.GetAttachmentData(audioname));

                // postattachmentcomplianceAsync(attachmentModel);
                adapter3 = new GridViewAdapter_Audio(Activity, audio_comp_lst, FragmentManager);
                Gridview3.Adapter = adapter3;

                
                if (ic.connectivity())
                {
                    postattachmentcomplianceAsync(attachmentModel);
                   // db.updateComplianceattachmentstatus("yes");
                }
                
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
                    mediaRecorder.Stop();
                    mediaRecorder.Prepare();
                    mediaRecorder.Start();

                }
                catch (Exception e)
                {
                    // TODO Auto-generated catch block
                    //e.printStackTrace();
                }

                Toast.MakeText(Activity, "Recording started", ToastLength.Long).Show();

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
                catch (Exception ex)
                {

                }

                //stoprecorder();

                //btn2.Enabled=false;
                //buttonPlayLastRecordAudio.setEnabled(true);
                //buttonStart.setEnabled(true);
                //buttonStopPlayingRecording.setEnabled(false);

                Toast.MakeText(Activity, "Recording completed", ToastLength.Long).Show();
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
            fileaudioPath = new Java.IO.File(audiofile, string.Format(audioname, Guid.NewGuid()));
            AudioSavePathInDevice = fileaudioPath.AbsolutePath;
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
            Activity.RunOnUiThread(() => { Timer.Text = $"{hour}:{min}:{sec}"; });
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
            if (ic.connectivity())
            {

                compliancemarkascompleted();
            }
            else
            {
                Toast.MakeText(Activity, "Please connect to the internet", ToastLength.Long).Show();
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
        ////public async Task postuploadedcompliance()
        ////{


        ////    List<Comp_AttachmentModel> models = new List<Comp_AttachmentModel>();
        ////    models = db.GetFullAttachmentData(task_id_to_send);




        ////    string json = JsonConvert.SerializeObject(models);
        ////    try
        ////    {

        ////        string item = await restService.CompliancePostServiceMethod(Activity, "UpoadTaskCompliance", json, "completed");
        ////        if (item.Contains("Data Submitted Sucessfully"))
        ////        {
        ////            //db.updateComplianceStatus(id);
        ////            Toast.MakeText(Activity, "compliance post  Successfully..", ToastLength.Long).Show();
        ////            //progress.Dismiss();
        ////        }
        ////        else
        ////        {
        ////            Toast.MakeText(Activity, "Oops! Something Went Wrong.", ToastLength.Long).Show();
        ////            //progress.Dismiss();
        ////        }
        ////    }
        ////    catch (Exception ex)
        ////    {
        ////        //progress.Dismiss();
        ////    }
        ////    //progress.Dismiss();

        ////}
        public async Task compliancemarkascompleted()
        {
            if(image_comp_lst.Count>=img_min && video_comp_lst.Count>=vdo_min && audio_comp_lst.Count >= aud_min)
            {
                dynamic value = new ExpandoObject();
                value.Remark = "ok";
                value.task_id = task_id_to_send;

                string json = JsonConvert.SerializeObject(value);
                try
                {
                    string item = await restService.ComplianceTaskMarkCompleted(Activity, "SetCompleteTaskSubmition", json, "completed");
                    if (item.Contains("Task completed successfully"))
                    {
                        //db.updateComplianceStatus(id);
                        Toast.MakeText(Activity, "Task completed successfully..", ToastLength.Long).Show();
                        db.UpdateInboxTaskStatus(task_id_to_send);
                        Android.Support.V4.App.FragmentManager fm = Activity.SupportFragmentManager;
                        fm.PopBackStack();
                        //progress.Dismiss();
                    }
                    else
                    {
                        Toast.MakeText(Activity, "Oops! Something Went Wrong.", ToastLength.Long).Show();
                        //progress.Dismiss();
                    }
                }
                catch (Exception ex)
                {
                    //progress.Dismiss();
                }
            }
            else
            {
                Toast.MakeText(Activity, "Please attach all mandatory compliances", ToastLength.Long).Show();
            }
            
            //progress.Dismiss();

        }

        public async Task postattachmentcomplianceAsync(Comp_AttachmentModel compmodel)
        {
            progress.Show();
            List<Comp_AttachmentModel> models = new List<Comp_AttachmentModel>();
            //for(int i=0; i < models.Count; i++)
            //{
            models.Add(compmodel);
            //  }

            if (compmodel.file_type == "Image")
            {
                byte[] img = GetStreamFromFile(compmodel.localPath);
                var url1 = await blob.UploadPhotoAsync(img, compmodel.localPath.Substring(compmodel.localPath.LastIndexOf("/") + 1));

                if (url1 != null)
                {
                    compmodel.Path = url1;
                }

            }
            if (compmodel.file_type == "Video")
            {
                byte[] img = GetStreamFromFile(compmodel.localPath);
                var url1 = await blob.UploadPhotoAsync(img, compmodel.localPath.Substring(compmodel.localPath.LastIndexOf("/") + 1));

                if (url1 != null)
                {
                    compmodel.Path = url1;
                }
            }
            if (compmodel.file_type == "Audio")
            {
                byte[] img = GetStreamFromFile(compmodel.localPath);
                var url1 = await blob.UploadPhotoAsync(img, compmodel.localPath.Substring(compmodel.localPath.LastIndexOf("/") + 1));

                if (url1 != null)
                {
                    compmodel.Path = url1;
                }
            }

            string json = JsonConvert.SerializeObject(models);
            try
            {

                string item = await restService.CompliancePostServiceMethod(Activity, "UpoadTaskCompliance", json, Description.Text);
                if (item.Contains("Compliance Upload Sucessfully"))
                {
                    db.updateComplianceStatus(compmodel.taskId);
                    Toast.MakeText(Activity, "attachment post  Successfully..", ToastLength.Long).Show();
                    //progress.Dismiss();
                }
                else
                {
                    Toast.MakeText(Activity, "Oops! Something Went Wrong.", ToastLength.Long).Show();
                    //progress.Dismiss();
                }
            }
            catch (Exception ex)
            {
                //progress.Dismiss();
            }
            progress.Dismiss();

        }

    }
}
