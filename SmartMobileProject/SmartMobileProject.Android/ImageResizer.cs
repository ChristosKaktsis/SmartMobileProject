using Android.Graphics;
using System.IO;
using Xamarin.Forms;
using Bitmap = Android.Graphics.Bitmap;

[assembly: Dependency(typeof(SmartMobileProject.Droid.ImageResizer))]
namespace SmartMobileProject.Droid
{
    public class ImageResizer : IResizeImageService
    {

        public byte[] ResizeImage(byte[] imageData, float width, float height)
        {
            // Load the bitmap
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);
            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)width, (int)height, false);
            using (MemoryStream ms = new MemoryStream())
            {
                resizedImage.Compress(Bitmap.CompressFormat.Jpeg, 100, ms);
                return ms.ToArray();
            }
        }
    }
}
