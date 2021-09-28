using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SmartMobileProject.Services
{
    public interface IDataCommand<T>
    {
        Task<bool> AddItemAsync(T item);
        Task<bool> UpdateItemAsync(T item);
        Task<bool> DeleteItemAsync(int id);
        Task<T> GetItemAsync(int id);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
    }
}
