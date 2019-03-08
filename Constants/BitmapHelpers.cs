using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace TaskAppWithLogin.Constants
{
   public static class BitmapHelpers
    {
        public static void RecycleBitmap(this ImageView imageView)
        {
            if (imageView == null)
            {
                return;
            }
            Drawable toRecycle = imageView.Drawable;
            if (toRecycle != null)
            {
                ((BitmapDrawable)toRecycle).Bitmap.Recycle();
            }
        }
        /// Load the image from the device, and resize it to the specified dimensions.  
        public static Bitmap LoadAndResizeBitmap(this string fileName, int width, int height)
        {
            // First we get the the dimensions of the file on disk  
            BitmapFactory.Options options = new BitmapFactory.Options
            {
                InPurgeable = true,
                InJustDecodeBounds = false
            };
            BitmapFactory.DecodeFile(fileName, options);
            // Next we calculate the ratio that we need to resize the image by  
            // in order to fit the requested dimensions.  
            int outHeight = options.OutHeight;
            int outWidth = options.OutWidth;
            int inSampleSize = 1;
            if (outHeight > height || outWidth > width)
            {
                inSampleSize = outWidth > outHeight ? outHeight / height : outWidth / width;
            }
            // Now we will load the image and have BitmapFactory resize it for us.  
            options.InSampleSize = inSampleSize;
            options.InJustDecodeBounds = false;

            Bitmap resizedBitmap = BitmapFactory.DecodeFile(fileName, options);
            Stream ms = File.Create(fileName);
            resizedBitmap.Compress(Bitmap.CompressFormat.Jpeg, 50, ms);
            ms.Close();

            return resizedBitmap;
        }
    }
}