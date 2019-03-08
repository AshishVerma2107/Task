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
    class GridImagecomplianceoutbox : BaseAdapter
    {
        public override int Count => myList.Count;
        private Context mContext;
        public static List<Comp_AttachmentModel> myList;
        FragmentManager fragment;


        public GridImagecomplianceoutbox(Context c, List<Comp_AttachmentModel> mList, FragmentManager fm)
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

            OutViewHolder1 holder;
            if (grid == null)
            {
                holder = new OutViewHolder1();
                grid = inflater.Inflate(Resource.Layout.Attachment_layout, null);
                var view = grid.FindViewById<ImageView>(Resource.Id.imageview_attach);
                grid.Tag = new OutViewHolder1() { View = view };
                Glide.With(mContext).Load(myList[position].Path).Into(view);
                view.Click += View_Click;
            }

            holder = (OutViewHolder1)grid.Tag;
            Glide.With(mContext).Load(myList[position].Path).Into(holder.View);
            
            var local = new LocalOnClickListener();
            holder.View.SetOnClickListener(local);
            local.HandleOnClick = () =>
            {
                ImageDialogFragment nextFrag = new ImageDialogFragment();
                FragmentTransaction ft = fragment.BeginTransaction();
                ft.Hide(fragment.FindFragmentByTag("ComplainceFrag_OutBox"));
                ft.Add(Resource.Id.container, nextFrag, "ImageDialogFragment");
                ft.AddToBackStack(null);
                ft.SetTransition(FragmentTransaction.TransitFragmentOpen);
                ft.Commit();
                Bundle bundle = new Bundle();
                bundle.PutString("Path", myList[position].Path);
                nextFrag.Arguments = bundle;

            };
            // holder.View.SetImageBitmap(bitmap);

            return grid;
        }

        private void View_Click(object sender, EventArgs e)
        {
            ImageDialogFragment imagedialog = new ImageDialogFragment();

        }

        public void setNewSelection(int position)
        {
            // myList[position].Checked = 1;
            NotifyDataSetChanged();
        }



        public void removeSelection(int position)
        {
            //  myList[position].Checked = 0;
            NotifyDataSetChanged();

        }
    }

    public class OutViewHolder1 : Java.Lang.Object
    {

        public ImageView View { get; set; }
    }
}