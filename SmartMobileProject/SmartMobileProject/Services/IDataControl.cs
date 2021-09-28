using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartMobileProject.Services
{
    public interface IDataControl
    {
        Task<bool> UpdateItemAsync();
    }
}
