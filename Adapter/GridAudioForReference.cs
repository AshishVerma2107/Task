using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using TaskAppWithLogin.Models;

namespace TaskAppWithLogin.Adapter
{
 public  class GridAudioForReference : BaseAdapter
    {

        private Context mContext;
        public static List<TaskFilemappingModel2> myList = new List<TaskFilemappingModel2>();

        MediaPlayer player;

        public GridAudioForReference(Context c, List<TaskFilemappingModel2> mList)
        {
            mContext = c;
            myList = mList;
        }
        public override Java.Lang.Object GetItem(int position)
        {
            return position;
        }
        public override long GetItemId(int position)
        {
            return position;
        }
        public override int Count => myList.Count;
        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            var grid = convertView;
            LayoutInflater inflater = (LayoutInflater)mContext.GetSystemService(Context.LayoutInflaterService);

            ReferenceViewHolder3 holder;
            if (grid == null)
            {
                holder = new ReferenceViewHolder3();
                grid = inflater.Inflate(Resource.Layout.Attachment_layout, null);
                var view = grid.FindViewById<ImageView>(Resource.Id.imageview_attach);
                grid.Tag = new ReferenceViewHolder3() { View = view };
            }

            holder = (ReferenceViewHolder3)grid.Tag;


            holder.View.SetImageResource(Resource.Drawable.audiofile);
            holder.View.Click += (o, e) =>
            {
                var path = myList[position].Path;
                StartPlayer(path);

            };
            return grid;
        }
        public void StartPlayer(String filePath)
        {
            if (player == null)
            {
                player = new MediaPlayer();
            }
            else
            {
                player.Reset();
                player.SetDataSource(filePath);
                player.Prepare();
                player.Start();
            }
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

    public class ReferenceViewHolder3 : Java.Lang.Object
    {

        public ImageView View { get; set; }
    }
}