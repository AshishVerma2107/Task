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

    public class ExpandableHeightGridView : GridView
    {

        bool expanded = false;

        public ExpandableHeightGridView(Context context): base(context)
        {
          
        }

        public ExpandableHeightGridView(Context context, IAttributeSet attrs):base(context,attrs)
        {
           
        }

        public bool isExpanded()
        {
            return expanded;
        }


        protected override void OnMeasure(int widthMeasureSpec, int heightMeasureSpec)
        {
            // HACK! TAKE THAT ANDROID!
            if (isExpanded())
            {
                int heightSpec = MeasureSpec.MakeMeasureSpec(MeasuredSizeMask, MeasureSpecMode.AtMost);
                base.OnMeasure(widthMeasureSpec, heightSpec);
            }
            else
            {
                base.OnMeasure(widthMeasureSpec, heightMeasureSpec);
            }
        }
        public void setExpanded(bool expanded)
        {
            this.expanded = expanded;
        }
    }
}
