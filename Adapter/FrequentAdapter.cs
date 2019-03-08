using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using Com.Bumptech.Glide.Request;
using TaskAppWithLogin.Models;
using Object = Java.Lang.Object;

namespace TaskAppWithLogin.Adapter
{
    class FrequentAdapter : BaseAdapter<FrequentList>
    {
        private readonly Context c;
        private readonly List<FrequentList> freq_list;
        private LayoutInflater inflater;

        public FrequentAdapter(Context c, List<FrequentList> freq_list1)
        {
            this.c = c;
            this.freq_list = freq_list1;
        }

        /*
         * RETURN SPACECRAFT
         */
        public override Object GetItem(int position)
        {
            return position;
        }

        /*
         * SPACECRAFT ID
         */
        public override long GetItemId(int position)
        {
            return position;
        }

        /*
         * RETURN INFLATED VIEW
         */
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.list_items, parent, false);
            var NameTxt = view.FindViewById<TextView>(Resource.Id.NameTxt);

            var numberTxt = view.FindViewById<TextView>(Resource.Id.numberTxt);
            var userImgview = view.FindViewById<ImageView>(Resource.Id.userImageView);

            var linear_layout = view.FindViewById<LinearLayout>(Resource.Id.linearlayout);

            view.Tag = new CustomAdapterViewHolder() { NameTxt = NameTxt, numberTxt = numberTxt, userImgview = userImgview, linearlayout = linear_layout };

            var holder = (CustomAdapterViewHolder)view.Tag;

            holder.NameTxt.Text = freq_list[position].NPName;
            //holder.DesigTxt.Text = freq_list[position].DesignationId;
            holder.numberTxt.Text = freq_list[position].Mobile;
            //holder.numberTxt.Visibility = ViewStates.Gone;

            try
            {
                if (freq_list[position].PhotoPath != null)
                {
                    Glide.With(c).Load(freq_list[position].PhotoPath).Apply(RequestOptions.CircleCropTransform()).Into(holder.userImgview);
                    //Android.Net.Uri contactPhotoUri = Android.Net.Uri.Parse(freq_list[position].PhotoPath);
                    //holder.userImgview.SetImageURI(contactPhotoUri);
                }
            }
            catch (System.Exception e)
            {

            }
            //if (freq_list[position].DesignationId != "" || freq_list[position].DesignationId != null)
            //{
            //    //holder.DesigTxt.Visibility = ViewStates.Visible;
            //    holder.DesigTxt.Text = freq_list[position].DesignationId;
            //}
            //else
            //{
            //    holder.DesigTxt.Visibility = ViewStates.Gone;
            //}

            //holder.linearlayout.Click += delegate
            //{
            //    Intent intent = new Intent(c, typeof(Overlay));
            //    intent.PutExtra("number", freq_list[position].Mobile);
            //    c.StartActivity(intent);
            //};
            //holder.dotsImgView.SetImageResource(spacecrafts[position].dotsImg);
            // holder.userImgview.SetImageResource(spacecrafts[position].userImg);

            //if (position % 4 == 1)
            //{
            //    // Set a background color for ListView regular row/item
            //    convertView.SetBackgroundColor(color: (Android.Graphics.Color.Blue));
            //}

            //else if (position % 4 == 2)
            //{

            //    convertView.SetBackgroundColor(color: (Android.Graphics.Color.DarkOrange));

            //}

            //else if (position % 4 == 3)
            //{

            //    convertView.SetBackgroundColor(color: (Android.Graphics.Color.SkyBlue));

            //}
            //else
            //{
            //    // Set the background color for alternate row/item
            //    convertView.SetBackgroundColor(color: (Android.Graphics.Color.Red));
            //}
            return view;
        }

        public void more_click()
        {

        }

        /*
         * TOTAL NUM OF SPACECRAFTS
         */
        public override int Count
        {
            get { return freq_list.Count(); }
        }

        public override FrequentList this[int position]
        {
            get
            {
                return freq_list[position];
            }
        }
    }

    public class CustomAdapterViewHolder : Java.Lang.Object
    {
        //adapter views to re-use
        public TextView NameTxt;
        public TextView DesigTxt;
        public TextView numberTxt;
        public ImageView userImgview;
        public ImageView dotsImgView;
        public LinearLayout linearlayout;
    }
}