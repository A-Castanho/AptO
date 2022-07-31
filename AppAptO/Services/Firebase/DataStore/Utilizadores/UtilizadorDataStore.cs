using AppAptO.Models.FBData.Chats;
using AppAptO.Models.FBData.Utilizadores;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AppAptO.Services.Firebase.DataStore.Utilizadores
{
    public class UtilizadorDataStore
    {
        public UtilizadorDataStore(FirebaseClient firebaseClient)
        {
            DatabasePath = firebaseClient.Child("Dados Utilizadores").Child("Utilizadores");
        }

        public ChildQuery DatabasePath { get; }
        public async Task<string> Add(Utilizador item)
        {
            string key = (await DatabasePath.PostAsync(JsonConvert.SerializeObject(item))).Key;
            return key;
        }
        /// <summary>
        /// Atualizar o utilizador na base de dados da firebase
        /// </summary>
        /// <param name="item">O novo valor do tipo utilizador para a substituição</param>
        /// <param name="key">A chave do nódulo dos dados do utilizador</param>
        /// <returns></returns>
        public async Task Update<T>(T item, string key) where T : Utilizador
        {
            Utilizador UtilizadorAtualizado;
            if (typeof(T) == typeof(Utilizador))
            {
                if (item.IsOrganizacao)
                {
                    var utilizadorAnterior = await GetByKeyAsync<UtilizadorOrganizacao>(key);
                    UtilizadorAtualizado = new UtilizadorOrganizacao(item, utilizadorAnterior);
                }
                else
                {
                    var utilizadorAnterior = await GetByKeyAsync<UtilizadorParticular>(key);
                    UtilizadorAtualizado = new UtilizadorParticular(item, utilizadorAnterior);
                }
            }
            else
            {
                UtilizadorAtualizado = item;
            }
            await DatabasePath.Child(key).PutAsync(UtilizadorAtualizado);
        }
        public async Task DeleteByKey(string key)
        {
            await DatabasePath.Child(key).DeleteAsync();
        }
        public async Task<IEnumerable<FirebaseObject<T>>> GetAllAsync<T>() where T : Utilizador
        {
            return await DatabasePath.OnceAsync<T>();
        }

        public async Task<T> GetByKeyAsync<T>(string key) where T : Utilizador
        {
            var utilizador = await DatabasePath.Child(key).OnceSingleAsync<T>();
            return utilizador;
        }
        public async Task<FirebaseObject<T>> GetByUidAsync<T>(string uid) where T : Utilizador
        {
            var fbutilizador = (await DatabasePath
                .OnceAsync<T>())
                .FirstOrDefault(item => item.Object.Uid == uid);
            return fbutilizador;
        }
        #region Admin
        public async Task<FirebaseObject<Utilizador>> GetAdmin()
        {
            var utilizador = (await DatabasePath.OnceAsync<Utilizador>()).First(u => u.Key == "Admin");
            return utilizador;
        }
        public async Task UpdateAdmin(Utilizador adminAtualizado)
        {
            await DatabasePath.Child("Admin").PutAsync(adminAtualizado);
        }
        #endregion
        #region Chat
        public async Task<string> AdicionarChatPessoal(string key1, string key2)
        {
            //Adiciona um novo chat
            var chat = new Chat(new ObservableCollection<string>() { key1, key2 }, true);
            var chatKey = await FBDataBase.ChatDS.Add(chat);

            //Adiciona o chat à lista de chats do utilizador atual
            var utilizador1 = await GetByKeyAsync<Utilizador>(key1);
            utilizador1.ChatsKeys.Add(chatKey);
            await FBDataBase.UtilizadorDS.Update(utilizador1, key1);

            //Adiciona o chat à lista de chats do utilizador do perfil
            var utilizador2 = await GetByKeyAsync<Utilizador>(key2);
            utilizador2.ChatsKeys.Add(chatKey);
            await FBDataBase.UtilizadorDS.Update(utilizador2, key2);

            return chatKey;
        }
        public async Task<string> GetOrCreateChatPessoal(string key1, string key2)
        {
            string chatFinalKey = "";
            var utilizador1 = await GetByKeyAsync<Utilizador>(key1);
            List<string> keysToRemove = new List<string>();

            //Se existirem chats
            if (utilizador1.ChatsKeys.Count > 0)
            {
                //Busca as keys de todos os chats pessoais do utilizador atual
                foreach (var key in utilizador1.ChatsKeys)
                {
                    if (string.IsNullOrEmpty(chatFinalKey))
                    {
                        var chat = await FBDataBase.ChatDS.GetByKeyAsync(key);
                        if (chat != null)
                        {
                            if (chat.IsPessoal)
                            {
                                if (chat.KeysUtilizadores.Contains(key2))
                                    chatFinalKey = key;
                            }
                        }
                        else
                            keysToRemove.Add(key);
                    }
                }
            }
            if (keysToRemove.Count >= 0)
            {
                foreach (string key in keysToRemove)
                {
                    utilizador1.ChatsKeys.Remove(key);
                    if (key1 != "Admin")
                        await FBDataBase.UtilizadorDS.Update(utilizador1, key1);
                    else
                        await FBDataBase.UtilizadorDS.UpdateAdmin(utilizador1);
                }
            }
            if (string.IsNullOrEmpty(chatFinalKey))
            {
                chatFinalKey = await FBDataBase.UtilizadorDS.AdicionarChatPessoal(key1, key2);
            }
            return chatFinalKey;
        }
        #endregion

    }
}
