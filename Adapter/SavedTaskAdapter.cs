
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
using TaskAppWithLogin;
using TaskAppWithLogin.Adapter;
using TaskAppWithLogin.Fragments;
using TaskAppWithLogin.Models;
using static Android.App.ActionBar;
using Object = Java.Lang.Object;

namespace TaskAppWithLogin.Adapter
{
    public class SavedTaskAdapter : RecyclerView.Adapter, IFilterable
    {
        public List<InitialTaskModel> Mitems;
        public List<InitialTaskModel> AllItem;
        Context context;
        RecyclerView mrecycle;
        TextView taskname, taskdescription, taskdeadlinedate, taskdeadlinetime;
      ///  LinearLayout linear1, linear2, linear3;
        TextView tv;
        ISharedPreferences prefs;
        public static long ques, ans, correct;
        FragmentManager fragment;
        //List<AnswerMasterModel> answer_model = new List<AnswerMasterModel>();
        public SavedTaskAdapter(Context context, List<InitialTaskModel> Mitems, RecyclerView recyler, FragmentManager fm)
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
            public TextView TaskName
            {
                get;
                set;
            }
            public TextView TaskDescription
            {
                get;
                set;
            }
            public TextView TaskDeadlineDate
            {
                get;
                set;
            }
            public TextView TaskDeadlineTime
            {
                get;
                set;
            }


            public LinearLayout Linear1
            {
                get;
                set;
            }
            public LinearLayout Linear2
            {
                get;
                set;
            }
            public LinearLayout Linear3
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
            View listitem = LayoutInflater.From(parent.Context).Inflate(Resource.Layout.savedtask_layout, parent, false);
            taskname = listitem.FindViewById<TextView>(Resource.Id.taskname_saved);
            taskdescription = listitem.FindViewById<TextView>(Resource.Id.taskdescription_saved);
            taskdeadlinedate = listitem.FindViewById<TextView>(Resource.Id.taskdeadlinedate_saved);
            taskdeadlinetime = listitem.FindViewById<TextView>(Resource.Id.taskdeadlinetime_saved);
            //linear1 = listitem.FindViewById<LinearLayout>(Resource.Id.linear1_saved);
            //linear2 = listitem.FindViewById<LinearLayout>(Resource.Id.linear2_saved);
            //linear3 = listitem.FindViewById<LinearLayout>(Resource.Id.linear3_saved);

            //TimeSpan span = (TaskInboxFrag.deadline - DateTime.Now);
            //String.Format("{0} days, {1} hours, {2} minutes, {3} seconds",

            //span.Days, span.Hours, span.Minutes, span.Seconds);
            //timeleft.Text = span.ToString();
            //TextView txtnumber = listitem.FindViewById<TextView>(Resource.Id.txtnumber);
            MyView view = new MyView(listitem)
            {
                TaskName = taskname,
                TaskDescription = taskdescription,
                TaskDeadlineTime = taskdeadlinetime,
                TaskDeadlineDate = taskdeadlinedate,
                //Linear1 = linear1,
                //Linear2 = linear2,
                //Linear3 = linear3,
            };
            return view;
        }
       
        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            MyView myholder = holder as MyView;
            myholder.TaskName.Text = Mitems[position].task_name;
            myholder.TaskDescription.Text = Mitems[position].description;
            myholder.TaskDeadlineTime.Text = Mitems[position].time.ToString();
            myholder.TaskDeadlineDate.Text = Mitems[position].task_creation_date.ToString();
           
            //myholder.Mark_by.Text = Mitems[position].mark_to;
            myholder.mainview.Click += Mainview_Click;
        }
        private void Mainview_Click(object sender, EventArgs e)
        {
            int position = mrecycle.GetChildAdapterPosition((View)sender);
           Save_Task_Ref_Fragment nextFrag = new Save_Task_Ref_Fragment();
            FragmentTransaction ft = fragment.BeginTransaction();
            ft.Replace(Resource.Id.container, nextFrag);
            ft.AddToBackStack(null);
            ft.Commit();
            Bundle bundle = new Bundle();
            bundle.PutInt("pos", position);
            bundle.PutString("taskid", Mitems[position].task_id);
            nextFrag.Arguments = bundle;

            //string id = Mitems[position].task_id;
            //string task_descrip = Mitems[position].description;
            //DateTime deadline = Mitems[position].deadlineDate;
            //string task_name = Mitems[position].task_name;
            //string mark_by = Mitems[position].task_mark_by;
            //string creation_date = Mitems[position].task_creation_date;
            //string created_by = Mitems[position].task_created_by;
            //DateTime current = DateTime.Now;

            //TimeSpan span = deadline - current;
            //String.Format("{0} days, {1} hours, {2} minutes, {3} seconds",
            //span.Days, span.Hours, span.Minutes, span.Seconds);
            ////timeleft.Text = span.ToString();
            //ISharedPreferencesEditor editor = prefs.Edit();
            //editor.PutInt("position", position);
            //editor.Apply();

            //ComplainceFrag nextFrag = new ComplainceFrag();
            //fragment.BeginTransaction().Replace(Resource.Id.container, nextFrag).Commit();
            //Bundle bundle = new Bundle();
            //bundle.PutString("task_id", id);
            //nextFrag.Arguments = bundle;

            //Intent intent = new Intent(context, typeof( Activity_Compliance));
            //intent.PutExtra("task_id", id);
            ////intent.PutExtra("task_descrip", task_descrip);
            ////intent.PutExtra("deadline", deadline);
            ////intent.PutExtra("task_name", task_name);
            ////intent.PutExtra("mark_by", mark_by);
            ////intent.PutExtra("creation_date", creation_date);
            ////intent.PutExtra("created_by", created_by);
            //context.StartActivity(intent);
        }
        public void getList(int pos, MyView myholder)
        {
            //tv = new TextView(context);
            //linear.AddView(tv);
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
