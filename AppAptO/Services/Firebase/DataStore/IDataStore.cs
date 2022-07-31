using Firebase.Database;
using Firebase.Database.Query;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppAptO.Services.Firebase.DataStore
{
    public interface IDataStore<T>
    {
        ChildQuery DatabasePath { get; }
        Task<string> Add(T item);
        Task Update(T item, string key);
        Task DeleteByKey(string key);
        Task<T> GetByKeyAsync(string key);
        Task<IEnumerable<FirebaseObject<T>>> GetAllAsync();
    }
}
