using System;
using System.Collections.Generic;

using System.Linq;
using System.Text;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;

namespace TaskAppWithLogin.Fragments
{

   
    public class DialogFragmentVideo : DialogFragment
    {
        string path;
       // MediaController media_controller;
      
        Android.App.ProgressDialog progress;

        public static VideoView video;

        private Handler mHandler = new Handler();

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            StrictMode.VmPolicy.Builder builder = new StrictMode.VmPolicy.Builder();
            StrictMode.SetVmPolicy(builder.Build());
            StrictMode.ThreadPolicy.Builder builder1 = new StrictMode.ThreadPolicy.Builder().PermitAll();
            StrictMode.SetThreadPolicy(builder1.Build());


            View view = inflater.Inflate(Resource.Layout.videoplayer, container, false);

            path = Arguments.GetString("Path") ?? string.Empty;

            //  player = MediaPlayer.Create(this, Resource.Raw.Neruppu);

           // Button downloadbutton = view.FindViewById<Button>(Resource.Id.downloadbtn);

            video = view.FindViewById<VideoView>(Resource.Id.videoView1);

            video.Visibility = ViewStates.Invisible;

            //downloadbutton.Click += delegate 
            //{

            //    progress = new Android.App.ProgressDialog(Activity);
            //    progress.Indeterminate = true;
            //    progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            //    progress.SetCancelable(false);
            //    progress.SetMessage("Loading is Progress...");
            //    progress.Show();

            //    video.Visibility = ViewStates.Visible;

            //    // player.Start();
            //};




            //Activity.RunOnUiThread(() =>
            //{

            TaskInBackground tb = new TaskInBackground(Activity, path);
                  tb.Execute();

                //progress = new Android.App.ProgressDialog(Activity);
                //progress.Indeterminate = true;
                //progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
                //progress.SetCancelable(false);
                //progress.SetMessage("Loading is Progress...");
                //progress.Show();


                //video = view.FindViewById<VideoView>(Resource.Id.videoView1);

                //video.Visibility = ViewStates.Invisible;

                //var uri = Android.Net.Uri.Parse(path);
                //video.SetVideoURI(uri);

                //video.Start();

            //});

           // progress.Dismiss();

            //  videoplayer();

            return view;
        }

       

        //public async void Download_Click()
        //{


        //    //var destination = Path.Combine(
        //    //    System.Environment.GetFolderPath(
        //    //        System.Environment.SpecialFolder.ApplicationData),
        //    //        "music.mp3");

        //    //await new WebClient().DownloadFileTaskAsync(
        //    //    new System.Uri(path),
        //    //    destination);

        //    Android.App.DownloadManager dm;

        //   var download_uri = Android.Net.Uri.FromFile(new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory + "/Download/xyz.mp4"));
        //    string lastSegment = download_uri.PathSegments.Last();
        //    string struri = download_uri.ToString();

        //    if (System.IO.File.Exists(struri))
        //    {
        //        // string currenturi = uri + lastSegment;
        //        return;
        //    }
        //    else
        //    {
        //        dm = Android.App.DownloadManager.FromContext(Activity);
        //        Android.App.DownloadManager.Request request = new Android.App.DownloadManager.Request(Android.Net.Uri.Parse(path));
        //        request.SetTitle("Task App").SetDescription("Task Audio");
        //        request.SetVisibleInDownloadsUi(true);
        //        request.SetNotificationVisibility(Android.App.DownloadVisibility.VisibleNotifyCompleted);
        //        request.SetDestinationUri(download_uri);
        //        var c = dm.Enqueue(request);

        //    }


        //}


       

        //public void videoplayer()
        //{

        //    media_controller = new Android.Widget.MediaController(Activity);
        //    media_controller.SetAnchorView(video);
        //    //  media_controller.SetMediaPlayer(video);
        //    video.SetMediaController(media_controller);
        //    // video.SetDataSource(path);
        //    video.SetVideoPath(path);
        //    //video.SetVideoURI(uri);
        //    video.RequestFocus();
        //    video.Start();
        //}
    }

    public class TaskInBackground : AsyncTask
    {
        Context context;
        Android.App.ProgressDialog progress;
        string path;
        public static Android.Net.Uri downloaded_uri;

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

            string file;

            string filename = path.Substring(path.LastIndexOf("/") + 1);

            if (filename.IndexOf(".") > 0)
            {
                file = filename.Substring(0, filename.LastIndexOf("."));
            }
            else
            {
                file = filename;
            }



            //  downloaded_uri = Android.Net.Uri.FromFile(new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory + "/Download/xyz.mp4"));

            downloaded_uri = Android.Net.Uri.FromFile(new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory + "/" + filename));

            string lastSegment = downloaded_uri.PathSegments.Last();
            string struri = downloaded_uri.ToString();



            

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
                request.SetDestinationUri(downloaded_uri);
                var c = dm.Enqueue(request);
            }

            return null;
        }

        protected override void OnPostExecute(Java.Lang.Object result)
        {
            progress.Dismiss();

           


            DialogFragmentVideo.video.Visibility = ViewStates.Visible;
            // var uri = Android.Net.Uri.Parse(path);
           

            DialogFragmentVideo.video.SetVideoURI(downloaded_uri);

           

            DialogFragmentVideo.video.Touch += delegate
                {
                   
                   DialogFragmentVideo.video.Start();
                };

           // DialogFragmentVideo.video.Start();

        }
    }


}