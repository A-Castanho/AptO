using AppAptO.Models.DadosAplicacao;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppAptO.Services.Firebase.DataStore.Chats
{
    public class MsgSabiasDataStore : IDataStore<MensagemInicial>
    {
        public MsgSabiasDataStore(FirebaseClient client)
        {
            DatabasePath = client.Child("DadosAplicacao").Child("Mensagens Sabias");
        }
        public ChildQuery DatabasePath { get; }
        public async Task<string> Add(MensagemInicial item)
        {
            return (await DatabasePath.PostAsync(JsonConvert.SerializeObject(item))).Key;
        }
        public async Task DeleteByKey(string keyChat)
        {
            await DatabasePath.Child(keyChat).DeleteAsync();
        }
        public async Task<IEnumerable<FirebaseObject<MensagemInicial>>> GetAllAsync()
        {
            return await DatabasePath.OnceAsync<MensagemInicial>();
        }
        public async Task<MensagemInicial> GetByKeyAsync(string key)
        {
            var b = await DatabasePath.Child(key).BuildUrlAsync();
            var a = await DatabasePath.Child(key).OnceSingleAsync<MensagemInicial>();
            return a;
        }
        public async Task Update(MensagemInicial item, string id)
        {
            await DatabasePath.Child(id).PutAsync(item);
        }
    }
}
