﻿using System;
using System.Drawing;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(SmartMobileProject.iOS.ImageResizer))]
namespace SmartMobileProject.iOS
{
    public class ImageResizer : IResizeImageService
    {

        //public  async Task<byte[]> ResizeImage(byte[] imageData, float width, float height)
        //{
        //    return ResizeImageIOS(imageData, width, height);
        //}

        public byte[] ResizeImage(byte[] imageData, float width, float height)
        {
            UIImage originalImage = ImageFromByteArray(imageData);
            UIImageOrientation orientation = originalImage.Orientation;
            //create a 24bit RGB image
            using (CoreGraphics.CGBitmapContext context = new CoreGraphics.CGBitmapContext(IntPtr.Zero,
                                                 (int)width, (int)height, 8,
                                                 4 * (int)width, CoreGraphics.CGColorSpace.CreateDeviceRGB(),
                                                 CoreGraphics.CGImageAlphaInfo.PremultipliedFirst))
            {
                RectangleF imageRect = new RectangleF(0, 0, width, height);
                // draw the image
                context.DrawImage(imageRect, originalImage.CGImage);
                UIKit.UIImage resizedImage = UIKit.UIImage.FromImage(context.ToImage(), 0, orientation);
                // save the image as a jpeg
                return resizedImage.AsJPEG().ToArray();
            }
        }
        public static UIKit.UIImage ImageFromByteArray(byte[] data)
        {
            if (data == null)
            {
                return null;
            }
            UIKit.UIImage image;
            try
            {
                image = new UIKit.UIImage(Foundation.NSData.FromArray(data));
            }
            catch (Exception e)
            {
                Console.WriteLine("Image load failed: " + e.Message);
                return null;
            }
            return image;
        }

    }
}