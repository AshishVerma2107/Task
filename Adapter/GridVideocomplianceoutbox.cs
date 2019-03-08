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
        class GridVideocomplianceoutbox : BaseAdapter
        {
            public override int Count => myList.Count;
            private Context mContext;
            public static List<Comp_AttachmentModel> myList;

            FragmentManager fragment;

            public GridVideocomplianceoutbox(Context c, List<Comp_AttachmentModel> mList, FragmentManager fm)
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

                OutnboxViewHolder2 holder;
                if (grid == null)
                {
                    holder = new OutnboxViewHolder2();
                    grid = inflater.Inflate(Resource.Layout.Attachment_layout, null);
                    var view = grid.FindViewById<ImageView>(Resource.Id.imageview_attach);
                    grid.Tag = new OutnboxViewHolder2() { View = view, };
                }

                holder = (OutnboxViewHolder2)grid.Tag;

                holder.View.SetImageResource(Resource.Drawable.videofile);
                var local = new LocalOnClickListener();
                holder.View.SetOnClickListener(local);
                local.HandleOnClick = () =>
                {


                    FragmentTransaction transcation = fragment.BeginTransaction();
                    DialogFragmentVideo videodownload = new DialogFragmentVideo();
                   


                    videodownload.Show(transcation, "Dialog Fragment");

                    Toast.MakeText(mContext, "Please wait until download is completed !!!", ToastLength.Long).Show();
                    Bundle bundle = new Bundle();
                    bundle.PutString("Path", myList[position].Path);
                    videodownload.Arguments = bundle;

                    //VideoFragment nextFrag = new VideoFragment();
                    //FragmentTransaction ft = fragment.BeginTransaction();
                    //ft.Hide(fragment.FindFragmentByTag("ComplainceFrag_OutBox"));
                    //ft.Add(Resource.Id.container, nextFrag, "VideoFragment");
                    //ft.AddToBackStack(null);
                    //ft.SetTransition(FragmentTransaction.TransitFragmentOpen);
                    //ft.Commit();
                    //// Fragment.BeginTransaction().Replace(Resource.Id.container, nextFrag).Commit();
                    ////FragmentTransaction ft = Fragment.PopBackStack();
                    ////   Fragment.PopBackStack();
                    //Bundle bundle = new Bundle();
                    //bundle.PutString("Path", myList[position].Path);
                    //nextFrag.Arguments = bundle;
                };

            return grid;
            }
            public void setNewSelection(int position)
            {
                // myList[position].Checked = 1;
                NotifyDataSetChanged();
            }



            public void removeSelection(int position)
            {
                // myList[position].Checked = 0;
                NotifyDataSetChanged();

            }

        }

        public class OutnboxViewHolder2 : Java.Lang.Object
        {

            public ImageView View { get; set; }
        }
    }