using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartMobileProject.Core
{
    class ActivationCheck
    {
        public static Task<bool> CheckActivationCode()
        {
            return Task.Run(() =>
            {
                return true;
            });
        }
    }
}
