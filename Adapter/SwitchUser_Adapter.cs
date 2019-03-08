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
using Java.Lang;
using TaskAppWithLogin.Models;

namespace TaskAppWithLogin.Adapter
{
    public class SwitchUser_Adapter : BaseAdapter<LoginModel>
    {
        List<LoginModel> marking;
        Context context;
        string img;
        string CheckStatus = "";
        ImageView Image;
        TextView Organization, NpName, Designation;
        public SwitchUser_Adapter(Context mContext, List<LoginModel> marking)
        {
            this.marking = marking;
            this.context = mContext;
        }

        public override LoginModel this[int position]
        {
            get
            {
                return marking[position];
            }
        }

        public override int Count
        {
            get
            {
                return marking.Count;
            }
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var view = convertView;
            if (view == null)
            {
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.switchuser_listdata, null);
                Organization = view.FindViewById<TextView>(Resource.Id.org);
                NpName = view.FindViewById<TextView>(Resource.Id.npname);
                Designation = view.FindViewById<TextView>(Resource.Id.desig);
                
                view.Tag = new ViewHolder() { Org = Organization, Name = NpName, Desig=Designation };

            }
            var holder = (ViewHolder)view.Tag;
            holder.Org.Text = marking[position].Organization;
            holder.Name.Text = marking[position].UserName;
            holder.Desig.Text = marking[position].Designation;
            string desid = marking[position].DesignationId;

            return view;

        }
    }
    class ViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        public TextView Org { get; set; }
        public TextView Name { get; set; }
        public TextView Desig { get; set; }

    }
}
