using AppAptO.Models.FBData.TiposDados;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppAptO.Services.Firebase.DataStore.TiposDados
{
    public class AreasDataStore : IDataStore<AreaApoio>
    {
        public AreasDataStore(FirebaseClient firebaseClient)
        {
            DatabasePath = firebaseClient.Child("Tipos de Dados")
            .Child("Areas");
        }

        public ChildQuery DatabasePath { get; }

        public async Task<string> Add(AreaApoio item)
        {
            return (await DatabasePath.PostAsync(JsonConvert.SerializeObject(item))).Key;
        }
        Task IDataStore<AreaApoio>.DeleteByKey(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<FirebaseObject<AreaApoio>>> GetAllAsync()
        {
            return await DatabasePath.OnceAsync<AreaApoio>();
        }

        public async Task<AreaApoio> GetByKeyAsync(string id)
        {
            return await DatabasePath.Child(id).OnceSingleAsync<AreaApoio>();
        }
        public async Task<FirebaseObject<AreaApoio>> GetByNomeAsync(string nome)
        {
            return (await DatabasePath
                .OnceAsync<AreaApoio>()).FirstOrDefault(item => item.Object.Nome == nome);
        }

        public async Task Update(AreaApoio item, string id)
        {
            if (item.NOfertas < 0)
                item.NOfertas = 0;
            if (item.NPedidos < 0)
                item.NPedidos = 0;
            await DatabasePath.Child(id).PutAsync(item);
        }
        public async Task InsertInitialData()
        {
            await Add(new AreaApoio("Apoio Domiciliário"));
            await Add(new AreaApoio("Natureza/Agricultura"));
            await Add(new AreaApoio("Construção Civil"));
            await Add(new AreaApoio("Animais Domésticos"));
            await Add(new AreaApoio("Outros Animais"));
            await Add(new AreaApoio("Desporto"));
            await Add(new AreaApoio("Festividades"));
            await Add(new AreaApoio("Mecânica"));
            await Add(new AreaApoio("Entretenimento"));
            await Add(new AreaApoio("Tecnologias (Hardware)"));
            await Add(new AreaApoio("Tecnologias (Software)"));
            await Add(new AreaApoio("Estética"));
            await Add(new AreaApoio("Ensino"));
            await Add(new AreaApoio("Transportes"));
            await Add(new AreaApoio("Outro"));
            await Add(new AreaApoio("Saúde"));
        }
    }
}
