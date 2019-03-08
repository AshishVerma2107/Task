using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using TaskAppWithLogin.Fragments;
using TaskAppWithLogin.Models;

namespace TaskAppWithLogin.Adapter
{
 public   class GridImageForRefrence : BaseAdapter
    {
        public override int Count => myList.Count;
        private Context mContext;
        public static List<TaskFilemappingModel2> myList;
        FragmentManager fragment;


        public GridImageForRefrence(Context c, List<TaskFilemappingModel2> mList,FragmentManager fm)
        {
            mContext = c;
            myList = mList;
            fragment = fm;
        }
        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }
        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            var grid = convertView;
            LayoutInflater inflater = (LayoutInflater)mContext.GetSystemService(Context.LayoutInflaterService);

            ReferenceImageViewHolder1 holder;
            if (grid == null)
            {
                holder = new ReferenceImageViewHolder1();
                grid = inflater.Inflate(Resource.Layout.Attachment_layout, null);
                var view = grid.FindViewById<ImageView>(Resource.Id.imageview_attach);
                grid.Tag = new ReferenceImageViewHolder1() { View = view };
            }

            holder = (ReferenceImageViewHolder1)grid.Tag;
            Glide.With(mContext).Load(myList[position].Path).Into(holder.View);
            holder.View.Click += (o, e) =>
            {
                ImageDialogFragment nextFrag = new ImageDialogFragment();

                fragment.BeginTransaction().Replace(Resource.Id.container, nextFrag).Commit();
                //FragmentTransaction ft = Fragment.PopBackStack();
                fragment.PopBackStack();
                Bundle bundle = new Bundle();
                bundle.PutString("Path", myList[position].Path);
                nextFrag.Arguments = bundle;

            };
            //Bitmap bitmap = BitmapFactory.DecodeFile(myList[position].Path);
            //holder.View.SetImageBitmap(bitmap);

            return grid;
        }

        public void setNewSelection(int position)
        {
            myList[position].Checked = 1;
            NotifyDataSetChanged();
        }



        public void removeSelection(int position)
        {
            myList[position].Checked = 0;
            NotifyDataSetChanged();

        }
    }

    public class ReferenceImageViewHolder1 : Java.Lang.Object
    {

        public ImageView View { get; set; }
    }
}