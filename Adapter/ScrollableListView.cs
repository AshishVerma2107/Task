using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace TaskAppWithLogin.Adapter
{
   public class ScrollableListView :ListView
    {
        public ScrollableListView(Context context, IAttributeSet attrs) : base(context, attrs)
        {

        }

        public override bool OnInterceptTouchEvent(MotionEvent ev)
        {
            return true;
        }

        public override bool OnTouchEvent(MotionEvent e)
        {
            if (canScrollVertically(this))
            {
                Parent.RequestDisallowInterceptTouchEvent(true);
            }
            return base.OnTouchEvent(e);
        }

        public bool canScrollVertically(AbsListView view)
        {
            bool canScroll = false;

            if (view != null && view.ChildCount > 0)
            {
                bool isOnTop = view.FirstVisiblePosition != 0 || view.GetChildAt(0).Top != 0;
                bool isAllItemsVisible = isOnTop && view.LastVisiblePosition == view.ChildCount;

                if (isOnTop || isAllItemsVisible)
                {
                    canScroll = true;
                }
            }

            return canScroll;
        }
       
    }
}