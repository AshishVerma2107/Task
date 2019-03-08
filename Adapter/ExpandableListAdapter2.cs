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

namespace TaskAppWithLogin.Adapter
{
    class ExpandableListAdapter2 : BaseExpandableListAdapter
    {
        private Activity _context;

        private List<string> _listDataHeader; // header titles
                                              // child data in format of header title, child title
        private Dictionary<string, List<string>> _listDataChild;

        public ExpandableListAdapter2(Activity context, List<string> listDataHeader, Dictionary<String, List<string>> listChildData)
        {
            _context = context;
            _listDataHeader = listDataHeader;
            _listDataChild = listChildData;
        }
        //for cchild item view
        public override Java.Lang.Object GetChild(int groupPosition, int childPosition)
        {
            return _listDataChild[_listDataHeader[groupPosition]][childPosition];
        }
        public override long GetChildId(int groupPosition, int childPosition)
        {
            return childPosition;
        }

        public override View GetChildView(int groupPosition, int childPosition, bool isLastChild, View convertView, ViewGroup parent)
        {
            string childText = (string)GetChild(groupPosition, childPosition);
            if (convertView == null)
            {
                convertView = _context.LayoutInflater.Inflate(Resource.Layout.filter_by_list, null);
            }
            CheckBox checkbox = convertView.FindViewById<CheckBox>(Resource.Id.checkbox);
            checkbox.Text = childText;

            checkbox.Click += (o, e) => {
                if (checkbox.Checked)
                    Toast.MakeText(_context, "Checked", ToastLength.Short).Show();
                else
                    Toast.MakeText(_context, "Un checked", ToastLength.Short).Show();
            };
            return convertView;
        }
        public override int GetChildrenCount(int groupPosition)
        {
            return _listDataChild[_listDataHeader[groupPosition]].Count;
        }
        //For header view
        public override Java.Lang.Object GetGroup(int groupPosition)
        {
            return _listDataHeader[groupPosition];
        }
        public override int GroupCount
        {
            get
            {
                return _listDataHeader.Count;
            }
        }
        public override long GetGroupId(int groupPosition)
        {
            return groupPosition;
        }
        public override View GetGroupView(int groupPosition, bool isExpanded, View convertView, ViewGroup parent)
        {
            string headerTitle = (string)GetGroup(groupPosition);

            convertView = convertView ?? _context.LayoutInflater.Inflate(Resource.Layout.filter_by_header, null);
            var lblListHeader = (TextView)convertView.FindViewById(Resource.Id.headertext);
            lblListHeader.Text = headerTitle;
            //if (isExpanded)
            //{
            //    groupHolder.img.setImageResource(R.drawable.down-arrow);
            //}
            //else
            //{
            //    groupHolder.img.setImageResource(R.drawable.up-arrow);
            //}
            return convertView;
        }
        public override bool HasStableIds
        {
            get
            {
                return false;
            }
        }
        public override bool IsChildSelectable(int groupPosition, int childPosition)
        {
            return true;
        }

        class ViewHolderItem : Java.Lang.Object
        {
        }
    }
}