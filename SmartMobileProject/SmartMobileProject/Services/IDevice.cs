using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMobileProject.Services
{
    public interface IDevice
    {
        /// <summary>
        /// Gets Device id but it changes if you uninstall the app or factory reset the device
        /// </summary>
        /// <returns></returns>
        string GetIdentifier();
    }
}
