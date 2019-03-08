using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Util;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;

namespace TaskAppWithLogin.Fragments
{
    public class ImageDialogFragment : DialogFragment
    {
        private ImageView imgView;
        string Images;
        static public ImageDialogFragment newInstance()
        {
            ImageDialogFragment f = new ImageDialogFragment();
            return f;
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetStyle(DialogFragment.StyleNoTitle, Android.Resource.Style.ThemeBlackNoTitleBarFullScreen);
            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            // Use this to return your custom view for this Fragment
            // return inflater.Inflate(Resource.Layout.YourFragment, container, false);

            View v = inflater.Inflate(Resource.Layout.image_layout_dialog, container, false);

            imgView = v.FindViewById<ImageView>(Resource.Id.img);

            //Get int and set it to the ImageView
            Images = Arguments.GetString("Path");
            Glide.With(Activity).Load(Images).Into(imgView);
            return v;
        }
    }
}