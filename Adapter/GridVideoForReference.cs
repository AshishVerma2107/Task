using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using TaskAppWithLogin.Fragments;
using TaskAppWithLogin.Models;

namespace TaskAppWithLogin.Adapter
{
 public   class GridVideoForReference : BaseAdapter
    {
        public override int Count => myList.Count;
        private Context mContext;
        public static List<TaskFilemappingModel2> myList;
        FragmentManager fragment;
        public GridVideoForReference(Context c, List<TaskFilemappingModel2> mList,FragmentManager fm)
        {
            mContext = c;
            myList = mList;
            fragment =fm ;
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

            ReferenceViewHolder2 holder;
            if (grid == null)
            {
                holder = new ReferenceViewHolder2();
                grid = inflater.Inflate(Resource.Layout.Attachment_layout, null);
                var view = grid.FindViewById<ImageView>(Resource.Id.imageview_attach);
               
                grid.Tag = new ReferenceViewHolder2() { View = view, };
                
            }

            holder = (ReferenceViewHolder2)grid.Tag;
          
           
            holder.View.SetImageResource(Resource.Drawable.videofile);
            var local = new LocalOnClickListener();
            holder.View.SetOnClickListener(local);
            local.HandleOnClick = () =>
            {
                VideoFragment nextFrag = new VideoFragment();
                FragmentTransaction ft = fragment.BeginTransaction();
                ft.Replace(Resource.Id.container, nextFrag);
                ft.AddToBackStack(null);
                ft.Commit();
                // Fragment.BeginTransaction().Replace(Resource.Id.container, nextFrag).Commit();
                //FragmentTransaction ft = Fragment.PopBackStack();
                //   Fragment.PopBackStack();
                Bundle bundle = new Bundle();
                bundle.PutString("Path", myList[position].Path);
                nextFrag.Arguments = bundle;
            };
            //holder.View.Click += (o, e) => {
                


            //};

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

    public class ReferenceViewHolder2 : Java.Lang.Object
    {

        public ImageView View { get; set; }
    }
}