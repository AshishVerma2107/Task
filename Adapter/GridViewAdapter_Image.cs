using System.Collections.Generic;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using Com.Bumptech.Glide;
using Com.Bumptech.Glide.Request;
using TaskAppWithLogin.Fragments;
using TaskAppWithLogin.Models;

namespace TaskAppWithLogin.Adapter
{
   public class GridViewAdapter_Image : BaseAdapter
    {
        public override int Count => myList.Count;
        private Context mContext;
        public static List<Comp_AttachmentModel> myList;
        FragmentManager fm;


        public GridViewAdapter_Image(Context c, List<Comp_AttachmentModel> mList, FragmentManager fm)
        {
            mContext = c;
            myList = mList;
            this.fm = fm;
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
            var local = new LocalOnClickListener();
            ViewHolder1 holder;
            if (grid == null)
            {
                holder = new ViewHolder1();
                grid = inflater.Inflate(Resource.Layout.Attachment_layout, null);
                var view = grid.FindViewById<ImageView>(Resource.Id.imageview_attach);
                grid.Tag = new ViewHolder1() { View = view };
            }

            holder = (ViewHolder1)grid.Tag;
            if(myList[position].Path!="" && myList[position].Path != null)
            {
                Glide.With(mContext).Load(myList[position].Path).Into(holder.View);
            }
            else if(myList[position].localPath != "" && myList[position].localPath != null)
            {
                Glide.With(mContext).Load(myList[position].localPath).Into(holder.View);
            }

            local.HandleOnClick = () =>
            {
                ImageDialogFragment nextFrag = new ImageDialogFragment();
                FragmentTransaction ft = fm.BeginTransaction();
                ft.Hide(fm.FindFragmentByTag("ComplainceFragment"));
                ft.Add(Resource.Id.container, nextFrag);
                ft.AddToBackStack(null);
                ft.SetTransition(FragmentTransaction.TransitFragmentOpen);
                ft.Commit();
                Bundle bundle = new Bundle();
                bundle.PutString("Path", myList[position].Path);
                nextFrag.Arguments = bundle;
            };
            holder.View.SetOnClickListener(local);

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

    public class ViewHolder1 : Java.Lang.Object
    {
        
        public ImageView View { get; set; }
    }
}