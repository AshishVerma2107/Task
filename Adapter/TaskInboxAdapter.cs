
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Java.Text;
using Java.Util;
using TaskAppWithLogin;
using TaskAppWithLogin.Adapter;
using TaskAppWithLogin.Fragments;
using TaskAppWithLogin.Models;
using static Android.App.ActionBar;
using Object = Java.Lang.Object;

namespace TaskAppWithLogin.Adapter
{
    public class TaskInboxAdapter : RecyclerView.Adapter,IFilterable
    {
        public List<TaskInboxModel> Mitems;
        public List<TaskInboxModel> AllItem;
        Context context;
        RecyclerView mrecycle;
        TextView task, create_by, dead_line, time_left;
        LinearLayout linear;
        TextView tv;
        ISharedPreferences prefs;
        public static long ques, ans, correct;
        Android.Support.V4.App.FragmentManager fragment;
        //List<AnswerMasterModel> answer_model = new List<AnswerMasterModel>();


        public TaskInboxAdapter(Context context, List<TaskInboxModel> Mitems, RecyclerView recyler, FragmentManager fm )
        {
            this.Mitems = Mitems;
            this.AllItem = Mitems;
            this.context = context;
            NotifyDataSetChanged();
            mrecycle = recyler;
            prefs = PreferenceManager.GetDefaultSharedPreferences(context);
            this.fragment = fm;
            //radioButton = btn;
        }
        public class MyView : RecyclerView.ViewHolder
        {
            public View mainview
            {
                get;
                set;
            }
            public TextView Task
            {
                get;
                set;
            }
            public TextView Create_by
            {
                get;
                set;
            }
            public TextView Deadline_date
            {
                get;
                set;
            }

            public TextView time_left
            {
                get;
                set;
            }
            public LinearLayout Linear
            {
                get;
                set;
            }
            public TextView Text1
            {
                get;
                set;
            }

            public MyView(View view) : base(view)
            {
                mainview = view;
            }
        }
        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            View listitem = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.taskinboxdata, parent, false);
            task = listitem.FindViewById<TextView>(Resource.Id.task_name);
            create_by = listitem.FindViewById<TextView>(Resource.Id.create_by);
            dead_line = listitem.FindViewById<TextView>(Resource.Id.deadline);
            time_left = listitem.FindViewById<TextView>(Resource.Id.time_left);
            linear = listitem.FindViewById<LinearLayout>(Resource.Id.ll);
            Filter1 = new ChemicalFilter1(this);

            
            //TextView txtnumber = listitem.FindViewById<TextView>(Resource.Id.txtnumber);
            MyView view = new MyView(listitem)
            {
                Task = task,
                Create_by = create_by,
                Deadline_date = dead_line,
                time_left = time_left,
                Linear = linear,
                Text1 = tv,
            };
            return view;
        }
        public Filter Filter1 { get; private set; }
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyView myholder = holder as MyView;
            myholder.Task.Text = Mitems[position].task_name;
            myholder.Create_by.Text = Mitems[position].task_created_by;
            myholder.Deadline_date.Text = Mitems[position].deadlineDate.ToString();
           // myholder.time_left.Text = Mitems[position].mark_to;
            var local = new LocalOnClickListener();
            myholder.Linear.SetOnClickListener(local);
            local.HandleOnClick = () =>
            {
                string id = Mitems[position].task_id;
                
                ComplainceFrag nextFrag = new ComplainceFrag();
                Android.Support.V4.App.FragmentTransaction ft = fragment.BeginTransaction();
                //ft.Replace(Resource.Id.container, nextFrag, "ComplainceFragment");
                ft.Hide(fragment.FindFragmentByTag("MainFrag"));
                ft.Add(Resource.Id.container, nextFrag, "ComplainceFragment");
                ft.AddToBackStack("TaskInboxFrag");
                ft.SetTransition(FragmentTransaction.TransitFragmentOpen);
                ft.Commit();
                // fragment.BeginTransaction().Replace(Resource.Id.container, nextFrag).Commit();
                Bundle bundle = new Bundle();
                bundle.PutString("task_id", id);
                nextFrag.Arguments = bundle;
            };

            
            myholder.time_left.Text = date_difference(DateTime.Now, Mitems[position].deadlineDate);
        }

        public string date_difference(DateTime current_date,  DateTime second_date)
        {
            SimpleDateFormat sdf = new SimpleDateFormat("dd-MM-yyyy HH:mm:ss");
            Date date1 = sdf.Parse(current_date.ToString());
            Date date2 = sdf.Parse(second_date.ToString());

            if (date1.After(date2))
            {
                return "Time out";
            }
            // before() will return true if and only if date1 is before date2
            else if (date1.Before(date2))
            {
                TimeSpan span =(second_date-current_date);
                string time_left = span.Days + "(days) " + span.Hours + "(hrs.) " + span.Minutes + "(min. )";
                return time_left;
            }
            else
            {
                return "";
            }
            

        }
        
        public void getList(int pos, MyView myholder)
        {
            tv = new TextView(context);
            linear.AddView(tv);
        }

        public override int ItemCount
        {
            get
            {
                return Mitems.Count;
            }
        }

        public Filter Filter => throw new NotImplementedException();

       
    }
}
class ChemicalFilter1 : Filter
{
    readonly TaskInboxAdapter _adapter;

    public ChemicalFilter1(TaskInboxAdapter adapter) : base()
    {
        _adapter = adapter;
    }

    protected override Filter.FilterResults PerformFiltering(Java.Lang.ICharSequence constraint)
    {
        FilterResults returnObj = new FilterResults();

        var matchList = new List<TaskInboxModel>();


        var results = new List<TaskInboxModel>();


        if (_adapter.AllItem == null)
            _adapter.AllItem = _adapter.Mitems;

        if (constraint == null) return returnObj;

        if (_adapter.AllItem != null && _adapter.AllItem.Any())
        {
            results.AddRange(
                _adapter.AllItem.Where(
                    chemical1 => chemical1.task_name.ToLower().Contains(constraint.ToString().ToLower())));
        }
        returnObj.Values = FromArray(results.Select(r => r.ToJavaObject()).ToArray());
        returnObj.Count = results.Count;

        constraint.Dispose();

        return returnObj;
    }

    protected override void PublishResults(Java.Lang.ICharSequence constraint, Filter.FilterResults results)
    {
        using (var values = results.Values)
            _adapter.Mitems = values.ToArray<Object>()
                .Select(r => r.ToNetObject<TaskInboxModel>()).ToList();

        _adapter.NotifyDataSetChanged();
    }
}