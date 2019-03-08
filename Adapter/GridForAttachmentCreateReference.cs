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
using TaskAppWithLogin.Models;

namespace TaskAppWithLogin.Adapter
{
    class GridForAttachmentCreateReference : BaseAdapter
    {
        public override int Count => myList.Count;
        private Context mContext;
        public static List<ComplianceJoinTable> myList;

        public GridForAttachmentCreateReference(Context c, List<ComplianceJoinTable> mList)
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

        public override View GetView(int position, View convertView, ViewGroup parent)
        {

            var grid = convertView;
            LayoutInflater inflater = (LayoutInflater)mContext.GetSystemService(Context.LayoutInflaterService);

            GridComplianceAttachViewHolder1 holder;
            if (grid == null)
            {
                holder = new GridComplianceAttachViewHolder1();
                grid = inflater.Inflate(Resource.Layout.compliance_attachment_layout, null);
                var file_type = grid.FindViewById<TextView>(Resource.Id.filetypetext);
                var file_format = grid.FindViewById<TextView>(Resource.Id.fileformat_text);
                var maxnum = grid.FindViewById<TextView>(Resource.Id.Maximum_text);
                var compliancetype = grid.FindViewById<TextView>(Resource.Id.compli_txt);

                var deletevar = grid.FindViewById<ImageButton>(Resource.Id.dust);
                grid.Tag = new GridComplianceAttachViewHolder1() { filetype = file_type, fileformat = file_format, maxi_num = maxnum, compliance_type = compliancetype, deletevar = deletevar };
                deletevar.Click += delegate {
                    myList.Remove(myList[position]);
                    NotifyDataSetChanged();

                };
                
            }

            holder = (GridComplianceAttachViewHolder1)grid.Tag;
            
                holder.filetype.Text = myList[position].file_type;
                holder.fileformat.Text = myList[position].file_format;
                holder.maxi_num.Text = myList[position].max_numbers.ToString();
                holder.compliance_type.Text = myList[position].complianceType;
            
            //Bitmap bitmap = BitmapFactory.DecodeFile(myList[position].localPath);
            //holder.View.SetImageBitmap(bitmap);

            return grid;
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

    public class GridComplianceAttachViewHolder1 : Java.Lang.Object
    {

        public TextView filetype { get; set; }
        public TextView fileformat { get; set; }
        public TextView maxi_num { get; set; }
        public TextView compliance_type { get; set; }
        public ImageButton deletevar { get; set; }

    }
}