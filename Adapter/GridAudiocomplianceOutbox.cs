using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using TaskAppWithLogin.Fragments;
using TaskAppWithLogin.Models;

namespace TaskAppWithLogin.Adapter
{
    class GridAudiocomplianceOutbox : BaseAdapter
    {
        MediaPlayer player;

        private Context mContext;
        public static List<Comp_AttachmentModel> myList = new List<Comp_AttachmentModel>();
        Android.Support.V4.App.FragmentManager fragment;


        public GridAudiocomplianceOutbox(Context c, List<Comp_AttachmentModel> mList, Android.Support.V4.App.FragmentManager fragment)
        {
            mContext = c;
            myList = mList;
            this.fragment = fragment;
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

            OutboxViewHolder3 holder;
            if (grid == null)
            {
                holder = new OutboxViewHolder3();
                grid = inflater.Inflate(Resource.Layout.Attachment_layout, null);
                var view = grid.FindViewById<ImageView>(Resource.Id.imageview_attach);
                grid.Tag = new OutboxViewHolder3() { View = view };
            }

            holder = (OutboxViewHolder3)grid.Tag;


            holder.View.SetImageResource(Resource.Drawable.audiofile);
            holder.View.Click += (o, e) =>
            {

                FragmentTransaction transcation = fragment.BeginTransaction();
                DialogClassFragment audiodownload = new DialogClassFragment();
                //  audiodownload.SetStyle(Convert.ToInt32(Android.App.DialogFragmentStyle.Normal), Resource.Style.FullScreenDialogStyle);


                audiodownload.Show(transcation, "Dialog Fragment");
                Bundle bundle = new Bundle();
                bundle.PutString("Path", myList[position].Path);
                audiodownload.Arguments = bundle;
            };

                //View view1 = LayoutInflater.Inflate(Resource.Layout.audio_player, null);
                //Android.App.AlertDialog builder = new Android.App.AlertDialog.Builder(mContext).Create();
                //builder.SetView(view1);
                //builder.Window.SetLayout(600, 600);
                //builder.SetCanceledOnTouchOutside(false);


                //AudioFragment1 audiofragment = new AudioFragment1();
                //// audiofragment.Download_Click();

                //FragmentTransaction ft = fragment.BeginTransaction();
                //ft.Replace(Resource.Id.container, audiofragment).Commit();

                //ft.AddToBackStack(null);
                //Bundle bundle = new Bundle();
                //bundle.PutString("Path", myList[position].Path);
                //audiofragment.Arguments = bundle;
                ////  var path = myList[position].Path;
                ////  StartPlayer(path);



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
            //myList[position].Checked = 1;
            NotifyDataSetChanged();
        }



        public void removeSelection(int position)
        {
            // myList[position].Checked = 0;
            NotifyDataSetChanged();

        }
    }

    public class OutboxViewHolder3 : Java.Lang.Object
    {

        public ImageView View { get; set; }
    }
}