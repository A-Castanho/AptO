using AppAptO.Models.FBData.Apoios;
using AppAptO.Models.FBData.TiposDados;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AppAptO.Services.Firebase.DataStore.Apoios
{
    public class OfertasDataStore : IDataStore<OfertaApoio>
    {
        public OfertasDataStore(FirebaseClient firebaseClient)
        {
            DatabasePath = firebaseClient.Child("Apoios")
            .Child("Ofertas");
        }

        public ChildQuery DatabasePath { get; }

        /// <summary>
        /// Adiciona a oferta à BD e atualiza os dados a esta relacionados
        /// </summary>
        /// <param name="item">Oferta de apoio a adicionar</param>
        public async Task<string> Add(OfertaApoio item)
        {
            await UpdateConnectionsInAddition(item);
            return (await DatabasePath.PostAsync(JsonConvert.SerializeObject(item))).Key;
        }
        public async Task DeleteByKey(string id)
        {
            await UpdateConnectionsInDeletition(await GetByKeyAsync(id));
            await DatabasePath.Child(id).DeleteAsync();
        }
        private async Task UpdateConnectionsInDeletition(OfertaApoio item)
        {
            var localidade = await FBDataBase.LocalidadeDS.GetByNomeAsync(item.Localidade);
            await FBDataBase.LocalidadeDS.UpdateByKey(localidade.Key, nPedidos: localidade.Object.NPedidos - 1);
        }
        public async Task<IEnumerable<FirebaseObject<OfertaApoio>>> GetAllAsync()
        {
            return await DatabasePath.OnceAsync<OfertaApoio>();
        }

        public async Task<OfertaApoio> GetByKeyAsync(string id)
        {
            return await DatabasePath.Child(id).OnceSingleAsync<OfertaApoio>();
        }

        public async Task Update(OfertaApoio item, string id)
        {
            await DatabasePath.Child(id).PutAsync(item);
            await UpdateConnectionsInUpdate(item, id);
        }

        private async Task UpdateConnectionsInAddition(OfertaApoio item)
        {
            //Atualiza ou adiciona as localidades
            await AtualizarLocalidade(item);

            //Atualiza as áreas de apoio
            FirebaseObject<AreaApoio> fbObjectArea = await FBDataBase.AreasApoioDS.GetByNomeAsync(item.Area) ?? await FBDataBase.AreasApoioDS.GetByNomeAsync("Outro");
            AreaApoio area = fbObjectArea.Object;
            area.NOfertas++;
            await FBDataBase.AreasApoioDS.Update(area, fbObjectArea.Key);
        }
        private async Task UpdateConnectionsInUpdate(OfertaApoio item, string id)
        {
            OfertaApoio dadosOriginais = await GetByKeyAsync(id);
            if (dadosOriginais.Localidade != item.Localidade)
            {
                var fbLocalidadeAnterior = await FBDataBase.LocalidadeDS.GetByNomeAsync(dadosOriginais.Localidade);
                await FBDataBase.LocalidadeDS.UpdateByKey(fbLocalidadeAnterior.Key, nOfertas: fbLocalidadeAnterior.Object.NOfertas - 1);

            }
            if (dadosOriginais.Area != item.Area)
            {
                var fbAreaAnterior = await FBDataBase.AreasApoioDS.GetByNomeAsync(dadosOriginais.Area) ?? await FBDataBase.AreasApoioDS.GetByNomeAsync("Outro");
                fbAreaAnterior.Object.NOfertas--;
                await FBDataBase.AreasApoioDS.Update(fbAreaAnterior.Object, fbAreaAnterior.Key);

                var fbAreaNova = await FBDataBase.AreasApoioDS.GetByNomeAsync(item.Area) ?? await FBDataBase.AreasApoioDS.GetByNomeAsync("Outro");
                fbAreaNova.Object.NOfertas++;
                await FBDataBase.AreasApoioDS.Update(fbAreaNova.Object, fbAreaNova.Key);
            }
        }

        private async Task AtualizarLocalidade(OfertaApoio item)
        {
            FirebaseObject<Localidade> fbNovaLocalidade = await FBDataBase.LocalidadeDS.GetByNomeAsync(item.Localidade);
            await FBDataBase.LocalidadeDS.UpdateByKey(fbNovaLocalidade.Key, nOfertas: fbNovaLocalidade.Object.NOfertas + 1);
        }
    }
}
