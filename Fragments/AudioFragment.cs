using System;
using System.Collections.Generic;
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

namespace TaskAppWithLogin.Fragments
{
   public class AudioFragment: Android.Support.V4.App.Fragment
    {
        string path;
    Java.IO.File file, filename1;
    Android.Net.Uri uri;
    MediaPlayer player;
    //AVPlayer avplayer;
    Button play, stop;
    //  protected Com.Google.Android.ExoPlayer.IExoPlayer mediaPlayer;
    public override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Create your fragment here
    }

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        // Use this to return your custom view for this Fragment
        // return inflater.Inflate(Resource.Layout.YourFragment, container, false);
        View view = inflater.Inflate(Resource.Layout.audio_layout, null);
        play = view.FindViewById<Button>(Resource.Id.button1);
        stop = view.FindViewById<Button>(Resource.Id.button2);
        path = Arguments.GetString("Path") ?? string.Empty;
        play.Click += delegate
        {
            Download_Click();
        };
        stop.Click += Stop_Click;
        return view;

    }

    private void Stop_Click(object sender, EventArgs e)
    {

        player.Stop();
        player.Release();
    }

    async void Download_Click()
    {
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

    public async System.Threading.Tasks.Task StartPlayerAsync(String filePath)
    {
        //await Plugin.MediaManager.CrossMediaManager.Current.Play(filePath);
        //var exoPlayer = new ExoPlayerAudioImplementation(((Plugin.MediaManager.MediaManagerImplementation)Plugin.MediaManager.CrossMediaManager.Current).MediaSessionManager);
        //Plugin.MediaManager.CrossMediaManager.Current.AudioPlayer = exoPlayer;
        //var mediaUri = Android.Net.Uri.Parse(filePath);
        //DefaultTrackSelector trackSelector = new DefaultTrackSelector();
        //SimpleExoPlayer player = ExoPlayerFactory.NewSimpleInstance(Activity,trackSelector);
        //// playerView.setPlayer(player);
        //DefaultDataSourceFactory dataSourceFactory = new DefaultDataSourceFactory(Activity, Util.GetUserAgent(Activity, "yourApplicationName"));
        //// This is the MediaSource representing the media to be played.
        //SsMediaSource videoSource = new SsMediaSource.Factory(dataSourceFactory).CreateMediaSource(mediaUri);
        //    //.createMediaSource(filePath);
        //// Prepare the player with the source.
        //player.Prepare(videoSource);
        //SimpleExoPlayer _player;


        //var userAgent = Util.GetUserAgent(Activity, "ExoPlayerDemo");
        //var defaultHttpDataSourceFactory = new DefaultHttpDataSourceFactory(userAgent);
        //var defaultDataSourceFactory = new DefaultDataSourceFactory(Activity, null, defaultHttpDataSourceFactory);
        //var extractorMediaSource = new ExtractorMediaSource(mediaUri, defaultDataSourceFactory, new DefaultExtractorsFactory(), null, null);
        //var defaultBandwidthMeter = new DefaultBandwidthMeter();
        //var adaptiveTrackSelectionFactory = new AdaptiveTrackSelection.Factory(defaultBandwidthMeter);
        //var defaultTrackSelector = new DefaultTrackSelector(adaptiveTrackSelectionFactory);

        //_player = ExoPlayerFactory.NewSimpleInstance(Activity, defaultTrackSelector);
        //_player.Prepare(extractorMediaSource);
        //_player.PlayWhenReady = true;


        //String userAgent = "Mozilla/5.0 (Macintosh; Intel Mac OS X 10.11; rv:40.0) Gecko/20100101 Firefox/40.0";
        //String url = "http://www.sample-videos.com/video/mp4/480/big_buck_bunny_480p_5mb.mp4";
        //Allocator allocator = new DefaultAllocator(minBufferMs);
        //DataSource dataSource = new DefaultUriDataSource(this, null, userAgent);


        //ExtractorSampleSource sampleSource = new ExtractorSampleSource(Uri.parse(url), dataSource, allocator,
        //        BUFFER_SEGMENT_COUNT * BUFFER_SEGMENT_SIZE);

        //MediaCodecVideoTrackRenderer videoRenderer = new
        //        MediaCodecVideoTrackRenderer(this, sampleSource, MediaCodecSelector.DEFAULT,
        //        MediaCodec.VIDEO_SCALING_MODE_SCALE_TO_FIT);

        //MediaCodecAudioTrackRenderer audioRenderer = new MediaCodecAudioTrackRenderer(sampleSource, MediaCodecSelector.DEFAULT);

        //exoPlayer = ExoPlayer.Factory.newInstance(RENDERER_COUNT);
        //exoPlayer.prepare(videoRenderer, audioRenderer);
        //exoPlayer.sendMessage(videoRenderer,
        //        MediaCodecVideoTrackRenderer.MSG_SET_SURFACE,
        //        surfaceView.getHolder().getSurface());
        //exoPlayer.setPlayWhenReady(true);


        try
        {
            if (player == null)
            {
                player = new MediaPlayer();
            }
            else
            {
                player.Reset();
                player.SetDataSource(filePath);
                // player.SetAudioStreamType(Stream.Music);
                // await player.SetDataSourceAsync(Activity, Android.Net.Uri.Parse(filePath));
                player.Prepare();
                player.Start();
            }
        }
        catch (Exception ex)
        {

        }



    }
}
}