using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.IO;


namespace TaskAppWithLogin.Fragments
{
    public class VideoFragment : Fragment
    {
        MediaController media_controller;
        VideoView video;
        string path;
        Java.IO.File file,filename1;
        public static Android.Net.Uri uri;
       // string path = string.Format($"android.resource://" + Application.PackageName + "/" + Resource.Raw.yogi);
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View view = inflater.Inflate(Resource.Layout.attach_video_layout, null);
            video = view.FindViewById<VideoView>(Resource.Id.videoView1);
            path  = Arguments.GetString("Path") ?? string.Empty;
            //  path = "com.work121.taskappwithlogin" + path ;
            //Uri video1 = UriParser(path);
            //convertermethod();
            //Download_Click();
            Activity.RunOnUiThread(() =>
            {
                var videoView = view.FindViewById<VideoView>(Resource.Id.videoView1);
                var uri = Android.Net.Uri.Parse(path);
                videoView.SetVideoURI(uri);
                videoView.Start();


                //  TaskInBackground tb = new TaskInBackground(Activity, path);
                // tb.Execute().Wait();
            });

            videoplayer();
            return view;
        }
     
        public void downloadclick()
        {
            
        }
        
        public void videoplayer()
        {
          // String vidAddress = "https://archive.org/download/ksnn_compilation_master_the_internet/ksnn_compilation_master_the_internet_512kb.mp4";
            //Android.Net.Uri vidUri = Android.Net.Uri.Parse(vidAddress);
            ///Android.Net.Uri uri = Android.Net.Uri.Parse(path);
            media_controller = new Android.Widget.MediaController(Activity);
            media_controller.SetAnchorView(video);
          //  media_controller.SetMediaPlayer(video);
            video.SetMediaController(media_controller);
           // video.SetDataSource(path);
            video.SetVideoPath(path);
           //video.SetVideoURI(uri);
            video.RequestFocus();
            video.Start();
        }
        
        public class TaskInBackground : AsyncTask
        {
            Context context;
            Android.App.ProgressDialog progress;
            string path;
            public TaskInBackground(Context context, string path)
            {
                this.context = context;
                this.path = path;

                progress = new Android.App.ProgressDialog(context);
                progress.Indeterminate = true;
                progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
                progress.SetCancelable(false);
                progress.SetMessage("Please wait...");
            }

            protected override void OnPreExecute()
            {
                progress.Show();
            }

            protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] @params)
            {
                Android.App.DownloadManager dm;

                uri = Android.Net.Uri.FromFile(new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory + "/Download/xyz.mp4"));
                string lastSegment = uri.PathSegments.Last();
                string struri = uri.ToString();

                if (System.IO.File.Exists(struri))
                {
                    // string currenturi = uri + lastSegment;
                    return null;
                }
                else
                {
                    dm = Android.App.DownloadManager.FromContext(context);
                    Android.App.DownloadManager.Request request = new Android.App.DownloadManager.Request(Android.Net.Uri.Parse(path));
                    request.SetTitle("Task App").SetDescription("Task video");
                    request.SetVisibleInDownloadsUi(true);
                    request.SetNotificationVisibility(Android.App.DownloadVisibility.VisibleNotifyCompleted);
                    request.SetDestinationUri(uri);
                    var c = dm.Enqueue(request);
                }

                return null;
            }

            protected override void OnPostExecute(Java.Lang.Object result)
            {
                progress.Dismiss();
                
            }
        }
    }
}