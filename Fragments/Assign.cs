using System;
using System.Collections.Generic;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V4.App;
using Android.Support.V4.View;
using Android.Views;
using Android.Widget;

namespace TaskAppWithLogin.Fragments
{
    public class Assign :Fragment
    {
        private TabLayout tabLayout;
        private ViewPager viewpager;
        Adapter adapter;

        //  Personal personal;


        Android.App.ProgressDialog progress;
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            View rootView = inflater.Inflate(Resource.Layout.assign, container, false);

            HasOptionsMenu = true;
            //ViewPager  
            viewpager = rootView.FindViewById<ViewPager>(Resource.Id.viewpager);

            // personal = 
            setupViewPager(viewpager);
            var tabLayout = rootView.FindViewById<TabLayout>(Resource.Id.tabs);
            tabLayout.SetupWithViewPager(viewpager);


            progress = new Android.App.ProgressDialog(Activity);
            progress.Indeterminate = true;
            progress.SetProgressStyle(Android.App.ProgressDialogStyle.Spinner);
            progress.SetCancelable(false);
            progress.SetMessage("Please wait...");
            return rootView;
        }


        void setupViewPager(Android.Support.V4.View.ViewPager viewPager)
        {
            adapter = new Adapter(FragmentManager);
            adapter.AddFragment(new Frequent(Activity), "Frequent");
            adapter.AddFragment(new Marking(Activity), "Marking");
            adapter.AddFragment(new MarkingOtherOrganization(Activity), "Other Organization");
            //adapter.AddFragment(new OrganizationOffice(Activity), "Organization Office");
            viewpager.Adapter = adapter;
            viewpager.SetCurrentItem(0, true);
            viewpager.Adapter.NotifyDataSetChanged();
        }

        class Adapter : Android.Support.V4.App.FragmentPagerAdapter
        {
            List<Fragment> fragments = new List<Fragment>();
            List<string> fragmentTitles = new List<string>();
            public Adapter(FragmentManager fm) : base(fm) { }
            public void AddFragment(Fragment fragment, String title)
            {
                fragments.Add(fragment);
                fragmentTitles.Add(title);
            }
            public override Fragment GetItem(int position)
            {
                return fragments[position];
            }
            public override int Count
            {
                get
                {
                    return fragments.Count;
                }
            }
            public override Java.Lang.ICharSequence GetPageTitleFormatted(int position)
            {
                return new Java.Lang.String(fragmentTitles[position]);
            }

        }


        public override void OnResume()
        {
            base.OnResume();

        }
    }
}