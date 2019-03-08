using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TaskAppWithLogin.Models;

namespace TaskAppWithLogin.Adapter
{
  public  class GridImageAdapterCreatetask : BaseAdapter
    {
       
        private Context mContext;
        public static List<TaskFileMapping_Model> myList;



        public GridImageAdapterCreatetask(Context c, List<TaskFileMapping_Model> mList)
        {
            mContext = c;
            myList = mList;
        }
        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }
        public override int Count => myList.Count;
        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            var grid = convertView;
            LayoutInflater inflater = (LayoutInflater)mContext.GetSystemService(Context.LayoutInflaterService);

            CreateImageViewHolder holder;
            if (grid == null)
            {
                holder = new CreateImageViewHolder();
                grid = inflater.Inflate(Resource.Layout.Attachment_layout, null);
                var view = grid.FindViewById<ImageView>(Resource.Id.imageview_attach);
                grid.Tag = new CreateImageViewHolder() { View = view };
            }

            holder = (CreateImageViewHolder)grid.Tag;

            Bitmap bitmap = BitmapFactory.DecodeFile(myList[position].Path);
           // Bitmap bitmap;

            //Converstion Image Size  
            //int height = Resources.DisplayMetrics.HeightPixels;
            //int width = Resources.DisplayMetrics.WidthPixels;
            //using (bitmap = fileImagePath.Path.LoadAndResizeBitmap(width / 4, height / 4))
            //{

            //}
            holder.View.SetImageBitmap(bitmap);

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

    public class CreateImageViewHolder : Java.Lang.Object
    {

        public ImageView View { get; set; }
    }
}