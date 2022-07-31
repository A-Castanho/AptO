using AppAptO.Models.DadosAplicacao;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Database.Streaming;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace AppAptO.Services.Firebase.DataStore.Chats
{
    public class PublicidadeDataStore : IDataStore<Publicidade>
    {
        public PublicidadeDataStore(FirebaseClient client)
        {
            DatabasePath = client.Child("DadosAplicacao").Child("Publicidade");
        }
        public ChildQuery DatabasePath { get; }
        public async Task<string> Add(Publicidade item)
        {
            return (await DatabasePath.PostAsync(JsonConvert.SerializeObject(item))).Key;
        }
        public async Task DeleteByKey(string keyChat)
        {
            await DatabasePath.Child(keyChat).DeleteAsync();
        }

        public async Task<IEnumerable<FirebaseObject<Publicidade>>> GetAllAsync()
        {
            return await DatabasePath.OnceAsync<Publicidade>();
        }

        public async Task<Publicidade> GetByKeyAsync(string key)
        {
            var b = await DatabasePath.Child(key).BuildUrlAsync();
            var a = await DatabasePath.Child(key).OnceSingleAsync<Publicidade>();
            return a;
        }

        public async Task Update(Publicidade item, string id)
        {
            await DatabasePath.Child(id).PutAsync(item);
        }
        public IObservable<FirebaseEvent<Publicidade>> GetObservable(string keyChat)
        {
            return DatabasePath.Child(keyChat).AsObservable<Publicidade>();
        }
        public async Task<FirebaseObject<Publicidade>> GetRndPublicidade(Publicidade.TipoPublicidade tipo)
        {
            IEnumerable<FirebaseObject<Publicidade>> publicidades = (await GetAllAsync()).Where(p => p.Object.Tipo == tipo);
            if (publicidades.Count() > 0)
            {
                List<FirebaseObject<Publicidade>> publicidadesFinais = new List<FirebaseObject<Publicidade>>();
                foreach (var publicidade in publicidades)
                {
                    for (int i = 0; i < publicidade.Object.NivelPrioridade; i++)
                    {
                        publicidadesFinais.Add(publicidade);
                    }
                }
                var rndIndex = new Random().Next(publicidadesFinais.Count());
                return publicidadesFinais[rndIndex];
            }
            else
                return null;
        }
    }
}
