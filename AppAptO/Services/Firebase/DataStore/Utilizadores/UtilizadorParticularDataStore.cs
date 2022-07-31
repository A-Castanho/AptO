using AppAptO.Models.FBData.Utilizadores;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppAptO.Services.Firebase.DataStore.Utilizadores
{
    public class UtilizadorParticularDataStore : IDataStore<UtilizadorParticular>
    {
        public UtilizadorParticularDataStore(FirebaseClient firebaseClient)
        {
            DatabasePath = firebaseClient.Child("Dados Utilizadores").Child("Utilizadores Particulares");
        }

        public ChildQuery DatabasePath { get; }

        public async Task<string> Add(UtilizadorParticular item)
        {
            string key = (await DatabasePath.PostAsync(JsonConvert.SerializeObject(item))).Key;
            return key;
        }
        public async Task DeleteByKey(string id)
        {
            await DatabasePath.Child(id).DeleteAsync();
        }

        public async Task<IEnumerable<FirebaseObject<UtilizadorParticular>>> GetAllAsync()
        {
            return await DatabasePath.OnceAsync<UtilizadorParticular>();
        }

        public async Task<UtilizadorParticular> GetByKeyAsync(string id)
        {
            return await DatabasePath.Child(id).OnceSingleAsync<UtilizadorParticular>();
        }

        public async Task<FirebaseObject<UtilizadorParticular>> GetByUidAsync(string uid)
        {
            return (await DatabasePath
                .OnceAsync<UtilizadorParticular>()).FirstOrDefault(item => item.Object.Uid == uid);
        }

        public async Task Update(UtilizadorParticular item, string id)
        {
            await DatabasePath.Child(id).PutAsync(JsonConvert.SerializeObject(item));
        }
    }
}
