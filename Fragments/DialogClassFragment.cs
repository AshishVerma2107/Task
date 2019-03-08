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
    class DialogClassFragment : DialogFragment
    {
        string path;
        Java.IO.File file, filename1;
        Android.Net.Uri uri;
        MediaPlayer player;
        //AVPlayer avplayer;
        ImageView play, stop, download;
        SeekBar seekbar;
        Handler seekHandler = new Handler();
        Action action;
        private const int NotificationId = 1;


        ImageButton cancel_Button;
        private Handler mHandler = new Handler();

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
            View view = inflater.Inflate(Resource.Layout.audio_player_layout, container, false);

            // SetStyle(Convert.ToInt32(Android.App.DialogFragmentStyle.Normal), Resource.Style.FullScreenDialogStyle);
            LinearLayout linear_Layout = view.FindViewById<LinearLayout>(Resource.Id.linearLayout1);

            play = view.FindViewById<ImageView>(Resource.Id.play2btn);
            stop = view.FindViewById<ImageView>(Resource.Id.pause1btn);
            download = view.FindViewById<ImageView>(Resource.Id.downbtn);
            seekbar = view.FindViewById<SeekBar>(Resource.Id.seekBar);
            path = Arguments.GetString("Path") ?? string.Empty;
            cancel_Button = view.FindViewById<ImageButton>(Resource.Id.imageButton1);

            // audioManager = (AudioManager)Activity.GetSystemService(AudioService);

            stop.Visibility = ViewStates.Invisible;
            play.Visibility = ViewStates.Invisible;

            cancel_Button.Click += delegate
            {
                Activity.OnBackPressed();
                player.Stop();
            };

            //play.Click += OnPlayClick;
            //pause.Click += OnPauseClick;
            //stop.Click += OnStopClick;

            play.Click += delegate
            {


                //  play.SetImageResource(Resource.Drawable.stop2);



                // StartPlayerAsync(path);

                player = new MediaPlayer();
                player.SetDataSource(path);
                player.Prepare();
                player.Start();
                run();








            };



            stop.Click += Stop_Click;

            download.Click += delegate
            {
                play.Visibility = ViewStates.Visible;
                stop.Visibility = ViewStates.Visible;

                Download_Click();

                //player.Pause();



                //seekbar.Max = player.Duration;

                //player.SeekTo(seekbar.);



            };

            return view;

        }



        //public void seekUpdation()
        //{
        //    seekbar.SetProgress(player.CurrentPosition, true);
        //    //seekHandler.PostDelayed( 1000);
        //}
        public void run()
        {
            action = () =>
            {


                if (player != null)
                {
                    int mCurrentPosition = player.CurrentPosition / 1000;
                    //  seekbar.Progress = mCurrentPosition;
                    seekbar.SetProgress(player.CurrentPosition, true);
                    seekbar.Max = player.Duration;
                }
                mHandler.PostDelayed(action, 1000);
            };
            mHandler.Post(action);
        }


        private void Stop_Click(object sender, EventArgs e)
        {

            player.Stop();
            player.Prepare();

            //  play.SetImageResource(Resource.Drawable.play2);


            //  player.Release();
        }



        public async void Download_Click()
        {


            //var destination = Path.Combine(
            //    System.Environment.GetFolderPath(
            //        System.Environment.SpecialFolder.ApplicationData),
            //        "music.mp3");

            //await new WebClient().DownloadFileTaskAsync(
            //    new System.Uri(path),
            //    destination);

            Android.App.DownloadManager dm;

            uri = Android.Net.Uri.FromFile(new Java.IO.File(Android.OS.Environment.ExternalStorageDirectory + "/Download/xyz.mp4"));
            string lastSegment = uri.PathSegments.Last();
            string struri = uri.ToString();

            if (System.IO.File.Exists(struri))
            {
                // string currenturi = uri + lastSegment;
                return;
            }
            else
            {
                dm = Android.App.DownloadManager.FromContext(Activity);
                Android.App.DownloadManager.Request request = new Android.App.DownloadManager.Request(Android.Net.Uri.Parse(path));
                request.SetTitle("Task App").SetDescription("Task Audio");
                request.SetVisibleInDownloadsUi(true);
                request.SetNotificationVisibility(Android.App.DownloadVisibility.VisibleNotifyCompleted);
                request.SetDestinationUri(uri);
                var c = dm.Enqueue(request);

            }


        }


        //public async void Download_AudioFile()
        //{
        //    var webClient = new WebClient();
        //    var url = new Uri(stream_url_soundcloud);
        //    byte[] bytes = null;


        //    webClient.DownloadProgressChanged += WebClient_DownloadProgressChanged;
        //    dialog = new ProgressDialog(this);
        //    dialog.SetProgressStyle(ProgressDialogStyle.Horizontal);
        //    dialog.SetTitle("Downloading...");
        //    dialog.SetCancelable(false);
        //    //dialog.SetButton("Cancel",);
        //    dialog.SetCanceledOnTouchOutside(false);
        //    dialog.Show();

        //    try
        //    {
        //        bytes = await webClient.DownloadDataTaskAsync(url);
        //    }
        //    catch (TaskCanceledException)
        //    {
        //        Toast.MakeText(this, "Task Canceled!", ToastLength.Long);
        //        return;
        //    }
        //    catch (Exception a)
        //    {
        //        Toast.MakeText(this, a.InnerException.Message, ToastLength.Long);
        //        dialog.Progress = 0;
        //        return;
        //    }

        //    Java.IO.File documentsPath = new Java.IO.File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryMusic), "MusicDownloaded");
        //    string localFilename = documentsPath + mListData[mPosition].track.title + ".mp3";
        //    //string localPath=System.IO.Path.Combine(documentsPath,localFilename);
        //    Java.IO.File localPath = new Java.IO.File(documentsPath, localFilename);

        //    dialog.SetTitle("Download Complete");

        //    //Save the Mp3 using writeAsync
        //    //FileStream fs=new FileStream(localPath,FileMode.OpenOrCreate);
        //    OutputStream fs = new FileOutputStream(localPath);
        //    await fs.WriteAsync(bytes, 0, bytes.Length);

        //    fs.Close();


        //    dialog.Progress = 0;
        //}

        //void WebClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        //{
        //    dialog.Progress = e.ProgressPercentage;
        //    if (e.ProgressPercentage == 100)
        //    {
        //        //dialog.Hide ();
        //    }


        private void IntializePlayer()
        {



            //  player.SetAudioStreamType(Stream.Music);

            //Wake mode will be partial to keep the CPU still running under lock screen
            //  player.SetWakeMode(ApplicationContext, WakeLockFlags.Partial);

            //When we have prepared the song start playback
            player.Prepared += (sender, args) => player.Start();

            //When we have reached the end of the song stop ourselves, however you could signal next track here.
            //  player.Completion += (sender, args) => Stop();

            player.Error += (sender, args) =>
            {
                //playback error
                Console.WriteLine("Error in playback resetting: " + args.What);
                //Stop();//this will clean up and reset properly.
            };

        }


    }
}
