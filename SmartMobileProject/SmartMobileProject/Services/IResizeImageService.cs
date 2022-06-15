using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMobileProject
{
    public interface IResizeImageService
    {
        byte[] ResizeImage(byte[] imageData, float width, float height);
    }
}
