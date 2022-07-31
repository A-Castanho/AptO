using AppAptO.Models.FBData.Utilizadores;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppAptO.Services.Firebase.DataStore.Utilizadores
{
    public class UtilizadorOrganizacaoDataStore : IDataStore<UtilizadorOrganizacao>
    {
        public UtilizadorOrganizacaoDataStore(FirebaseClient firebaseClient)
        {
            DatabasePath = firebaseClient.Child("Dados Utilizadores").Child("Organizacoes Empresas");
        }

        public ChildQuery DatabasePath { get; }

        public async Task<string> Add(UtilizadorOrganizacao item)
        {
            return (await DatabasePath.PostAsync(JsonConvert.SerializeObject(item))).Key;
        }
        public async Task DeleteByKey(string id)
        {
            await DatabasePath.Child(id).DeleteAsync();
        }

        public async Task<IEnumerable<FirebaseObject<UtilizadorOrganizacao>>> GetAllAsync()
        {
            return await DatabasePath.OnceAsync<UtilizadorOrganizacao>();
        }

        public async Task<UtilizadorOrganizacao> GetByKeyAsync(string id)
        {
            return await DatabasePath.Child(id).OnceSingleAsync<UtilizadorOrganizacao>();
        }
        public async Task<FirebaseObject<UtilizadorOrganizacao>> GetByUidAsync(string uid)
        {
            return (await DatabasePath
                .OnceAsync<UtilizadorOrganizacao>()).FirstOrDefault(item => item.Object.Uid == uid);
        }

        public async Task Update(UtilizadorOrganizacao item, string id)
        {
            await DatabasePath.Child(id).PutAsync(item);
        }
    }
}
