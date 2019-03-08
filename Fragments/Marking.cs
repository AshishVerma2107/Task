using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Speech;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Text;
using Android.Util;
using Android.Views;
using Android.Widget;
using Newtonsoft.Json;
using TaskAppWithLogin.Adapter;
using TaskAppWithLogin.Constants;
using TaskAppWithLogin.Models;

namespace TaskAppWithLogin.Fragments
{
    public class Marking : Fragment
    {
        Context context;
        public Marking(Context context)
        {
            this.context = context;
        }
        ListView list;
        MarkingListAdapter marked;
        List<MarkingListModel> markinglist;
        ServiceHelper restService;
        DbHelper db;
        InternetConnection con;
        bool ic;
        public static string markto = "Arti";
        public static string markingType = "desk";
        public static string taskid = "1";
        public static string l_id = "12345";
        Fragment frag;
        SearchView search;
        ImageView audio;
        private bool isRecording;
        private readonly int VOICE = 10;
        Geolocation geo;
        string location="0";

        Android.App.ProgressDialog progress;

        ISharedPreferences prefs;
        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var view = View;
            view = inflater.Inflate(Resource.Layout.marking, container, false);
            restService = new ServiceHelper();
            con = new InternetConnection();
            geo = new Geolocation();
            db = new DbHelper();
            prefs = PreferenceManager.GetDefaultSharedPreferences(Activity);


            list = view.FindViewById<ListView>(Resource.Id.listView1);
            audio = view.FindViewById<ImageView>(Resource.Id.imageView1);
            search = view.FindViewById<SearchView>(Resource.Id.searchview);


            search.QueryTextChange += sv_QueryTextChange;
            location = geo.GetGeoLocation(Context);

            markinglist = new List<MarkingListModel>();

            list.ItemClick += ListItemSelected;
            Boolean connectivity = con.connectivity();
            if (connectivity)
            {
                getData();
            }
            else
            {
                markinglist = db.GetMarkingList();
                if(markinglist.Count != 0)
                {
                    marked = new MarkingListAdapter(Activity, markinglist);
                    list.SetAdapter(marked);
                }
                else
                {
                    Toast.MakeText(Activity, "Couldn't find data for marking. Please connect to the internet", ToastLength.Long).Show();
                }
                
            }
            
            string rec = Android.Content.PM.PackageManager.FeatureMicrophone;
            if (rec != "android.hardware.microphone")
            {
                // no microphone, no recording. Disable the button and output an alert
                var alert = new Android.App.AlertDialog.Builder(audio.Context);
                alert.SetTitle("You don't seem to have a microphone to record with");
                alert.SetPositiveButton("OK", (sender, e) =>
                {
                    //task_name.Text = "No microphone present";
                    audio.Enabled = false;

                    return;
                });

                alert.Show();
            }
            else
            {
                audio.Click += delegate
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

                        // you can specify other languages recognised here, for example
                        // voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.German);
                        // if you wish it to recognise the default Locale language and German
                        // if you do use another locale, regional dialects may not be recognised very well

                        voiceIntent.PutExtra(RecognizerIntent.ExtraLanguage, Java.Util.Locale.Default);
                        StartActivityForResult(voiceIntent, VOICE);
                    }
                };
            }
            return view;
        }
        private void ListItemSelected(object sender, AdapterView.ItemClickEventArgs e)
        {
            var t = markinglist[e.Position];

            UsermarkedFragment myFragment = new UsermarkedFragment();
            FragmentTransaction ft = FragmentManager.BeginTransaction();
            Bundle bundle = new Bundle();
            // bundle.PutString("Photo", imglistmodel[0].ImagePath);
            bundle.PutString("Designation", markinglist[e.Position].DesignationName);
            bundle.PutString("Name", markinglist[e.Position].NPName);
            bundle.PutString("DesignationId", markinglist[e.Position].DesignationId);
            myFragment.Arguments = bundle;
            ft.Replace(Resource.Id.container, myFragment);
            ft.Commit();
            // SupportFragmentManager.BeginTransaction().Replace(Resource.Id.fragment_container, new UsermarkedFragment()).Commit();
        }
        public override void OnActivityResult(int requestCode, int resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == VOICE)
            {
                if (resultCode == -1)
                {
                    var matches = data.GetStringArrayListExtra(RecognizerIntent.ExtraResults);
                    if (matches.Count != 0)
                    {
                        string textInput = search.Query + matches[0];

                        // limit the output to 500 characters
                        if (textInput.Length > 500)
                            textInput = textInput.Substring(0, 500);
                        search.Query.Contains(textInput);
                    }
                    else
                        search.Query.Contains("No speech was recognized");
                    // change the text back on the button

                }
            }
        }

        void sv_QueryTextChange(object sender, SearchView.QueryTextChangeEventArgs e)
        {
            //FILTER
            marked.Filter.InvokeFilter(e.NewText);
        }


        public async Task getData()
        {
            progress = new Android.App.ProgressDialog(Activity);
            progress.Indeterminate = true;
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            progress.SetCancelable(false);
            progress.SetMessage("Please wait...");
            progress.Show();

            dynamic value = new ExpandoObject();
            value.OrgId = "1";

            string json = JsonConvert.SerializeObject(value);
            try
            {
                string item = await restService.MarkingList(Activity, json, location).ConfigureAwait(false);
                markinglist = JsonConvert.DeserializeObject<List<MarkingListModel>>(item);
                db.InsertMarkingList(markinglist);

                progress.Dismiss();
            }
            catch (Exception ex)
            {
                progress.Dismiss();
            }

            if (markinglist != null)
            {
                Activity.RunOnUiThread(() =>
                {
                    marked = new MarkingListAdapter(Activity, markinglist);
                    list.SetAdapter(marked);
                });
            }
            progress.Dismiss();

        }


    }
}