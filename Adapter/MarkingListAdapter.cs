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
using TaskAppWithLogin;
using TaskAppWithLogin.Adapter;
using TaskAppWithLogin.Models;
using Object = Java.Lang.Object;

namespace TaskAppWithLogin.Adapter
{
    class MarkingListAdapter : BaseAdapter<MarkingListModel>, IFilterable
    {
        public List<MarkingListModel> marking;
        public List<MarkingListModel> AllItem;
        Context context;
        string img;
        string CheckStatus = "";
        ImageView Image;
        public MarkingListAdapter(Context mContext, List<MarkingListModel> marking)
        {
            this.marking = marking;
            this.AllItem = marking;
            this.context = mContext;
            //Filter = new ChemicalFilter(this);
        }

        public override MarkingListModel this[int position]
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
            Filter = new ChemicalFilter(this);
            var view = convertView;
            if (view == null)
            {
                view = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.marking_data, null);
                TextView Department = view.FindViewById<TextView>(Resource.Id.textView1);
                TextView DepartmentText = view.FindViewById<TextView>(Resource.Id.textView2);
                //TextView Name = view.FindViewById<TextView>(Resource.Id.textView3);
                TextView NameText = view.FindViewById<TextView>(Resource.Id.textView4);
                view.Tag = new OrgListAdapterViewHolder() { Department = DepartmentText, name = NameText };

            }
            var holder = (OrgListAdapterViewHolder)view.Tag;
            holder.Department.Text = marking[position].DesignationName;
            holder.name.Text = marking[position].NPName;
            string desid = marking[position].DesignationId;
            return view;

        }
        public Filter Filter { get; private set; }

    }
    class OrgListAdapterViewHolder : Java.Lang.Object
    {
        //Your adapter views to re-use
        public TextView Department { get; set; }
        public TextView name { get; set; }
    }
}
class ChemicalFilter : Filter
{
    readonly MarkingListAdapter _adapter;

    public ChemicalFilter(MarkingListAdapter adapter) : base()
    {
        _adapter = adapter;
    }

    protected override Filter.FilterResults PerformFiltering(Java.Lang.ICharSequence constraint)
    {
        FilterResults returnObj = new FilterResults();

        var matchList = new List<MarkingListModel>();


        var results = new List<MarkingListModel>();


        if (_adapter.AllItem == null)
            _adapter.AllItem = _adapter.marking;

        if (constraint == null) return returnObj;

        if (_adapter.AllItem != null && _adapter.AllItem.Any())
        {
            results.AddRange(
                _adapter.AllItem.Where(
                    chemical => chemical.NPName.ToLower().Contains(constraint.ToString().ToLower())));
        }
        returnObj.Values = FromArray(results.Select(r => r.ToJavaObject()).ToArray());
        returnObj.Count = results.Count;

        constraint.Dispose();

        return returnObj;
    }

    protected override void PublishResults(ICharSequence constraint, Filter.FilterResults results)
    {
        using (var values = results.Values)
            _adapter.marking = values.ToArray<Object>()
                .Select(r => r.ToNetObject<MarkingListModel>()).ToList();

        _adapter.NotifyDataSetChanged();
    }
}




