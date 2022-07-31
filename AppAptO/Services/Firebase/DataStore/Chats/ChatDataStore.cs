using AppAptO.Models.FBData.Chats;
using AppAptO.Models.FBData.Utilizadores;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Database.Streaming;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace AppAptO.Services.Firebase.DataStore.Chats
{
    public class ChatDataStore
    {
        public ChatDataStore(FirebaseClient client)
        {
            DatabasePath = client.Child("Chats");
        }
        public ChildQuery DatabasePath { get; }
        public async Task<string> Add(Chat item)
        {
            return (await DatabasePath.PostAsync(JsonConvert.SerializeObject(item))).Key;
        }
        public async Task DeleteByKey(string keyChat)
        {
            var chat = await GetByKeyAsync(keyChat);
            foreach (var keyUtilizador in chat.KeysUtilizadores)
            {
                var utilizador = await FBDataBase.UtilizadorDS.GetByKeyAsync<Utilizador>(keyUtilizador);
                if (utilizador.ChatsKeys.Contains(keyChat))
                {
                    utilizador.ChatsKeys.Remove(keyChat);
                    await FBDataBase.UtilizadorDS.Update(utilizador, keyUtilizador);
                }
            }
            await DatabasePath.Child(keyChat).DeleteAsync();
        }

        public async Task<IEnumerable<FirebaseObject<Chat>>> GetAllAsync()
        {
            return await DatabasePath.OnceAsync<Chat>();
        }

        public async Task<Chat> GetByKeyAsync(string key)
        {
            var b = await DatabasePath.Child(key).BuildUrlAsync();
            var a = await DatabasePath.Child(key).OnceSingleAsync<Chat>();
            return a;
        }

        public async Task Update(Chat item, string id)
        {
            await DatabasePath.Child(id).PutAsync(item);
        }
        public IObservable<FirebaseEvent<Chat>> GetObservable(string keyChat)
        {
            return DatabasePath.Child(keyChat).AsObservable<Chat>();
        }
    }
}
