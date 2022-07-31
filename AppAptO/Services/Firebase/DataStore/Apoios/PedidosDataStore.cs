using AppAptO.Models.FBConnections;
using AppAptO.Models.FBData.Apoios;
using AppAptO.Models.FBData.Apoios.Notificacoes;
using AppAptO.Models.FBData.TiposDados;
using AppAptO.Models.FBData.Utilizadores;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppAptO.Services.Firebase.DataStore.Apoios
{
    public class PedidosDataStore : IDataStore<PedidoApoio>
    {
        public PedidosDataStore(FirebaseClient firebaseClient)
        {
            DatabasePath = firebaseClient.Child("Apoios")
            .Child("Pedidos");
        }

        public ChildQuery DatabasePath { get; }
        public async Task<string> Add(PedidoApoio item)
        {
            await UpdateConnectionsInAddition(item);
            return (await DatabasePath.PostAsync(JsonConvert.SerializeObject(item))).Key;
        }

        public async Task DeleteByKey(string id)
        {
            await UpdateConnectionsInDeletition(await GetByKeyAsync(id));
            await DatabasePath.Child(id).DeleteAsync();
        }

        private async Task UpdateConnectionsInDeletition(PedidoApoio item)
        {
            var localidade = await FBDataBase.LocalidadeDS.GetByNomeAsync(item.Localidade);
            await FBDataBase.LocalidadeDS.UpdateByKey(localidade.Key, nPedidos: localidade.Object.NPedidos - 1);

            await FBDataBase.ChatDS.DeleteByKey(item.ChatKey);
        }

        public async Task<IEnumerable<FirebaseObject<PedidoApoio>>> GetAllAsync()
        {
            return await DatabasePath.OnceAsync<PedidoApoio>();
        }
        public async Task<IEnumerable<FirebaseObject<PedidoApoio>>> GetAllPostVisAsync()
        {
            return (await DatabasePath.OnceAsync<PedidoApoio>()).Where(p => p.Object.VisPublicacao);
        }

        public async Task<PedidoApoio> GetByKeyAsync(string id)
        {
            return await DatabasePath.Child(id).OnceSingleAsync<PedidoApoio>();
        }

        public async Task Update(PedidoApoio item, string id)
        {
            await DatabasePath.Child(id).PutAsync(item);
            await UpdateConnectionsInUpdate(item, id);
        }

        private async Task UpdateConnectionsInAddition(PedidoApoio item)
        {
            //Atualiza as localidades
            await AtualizarLocalidade(item);

            //Atualiza as áreas de apoio
            FirebaseObject<AreaApoio> fbObjectArea = await FBDataBase.AreasApoioDS.GetByNomeAsync(item.Area) ?? await FBDataBase.AreasApoioDS.GetByNomeAsync("Outro");
            AreaApoio area = fbObjectArea.Object;
            area.NPedidos++;
            await FBDataBase.AreasApoioDS.Update(area, fbObjectArea.Key);
        }
        private async Task UpdateConnectionsInUpdate(PedidoApoio item, string id)
        {
            PedidoApoio dadosOriginais = await GetByKeyAsync(id);
            if (dadosOriginais.Localidade != item.Localidade)
            {
                var fbLocalidadeAnterior = await FBDataBase.LocalidadeDS.GetByNomeAsync(dadosOriginais.Localidade);
                await FBDataBase.LocalidadeDS.UpdateByKey(fbLocalidadeAnterior.Key, nPedidos: fbLocalidadeAnterior.Object.NPedidos - 1);

            }
            if (dadosOriginais.Area != item.Area)
            {
                var fbAreaAnterior = await FBDataBase.AreasApoioDS.GetByNomeAsync(dadosOriginais.Area) ?? await FBDataBase.AreasApoioDS.GetByNomeAsync("Outro");
                fbAreaAnterior.Object.NPedidos--;
                await FBDataBase.AreasApoioDS.Update(fbAreaAnterior.Object, fbAreaAnterior.Key);

                var fbAreaNova = await FBDataBase.AreasApoioDS.GetByNomeAsync(item.Area) ?? await FBDataBase.AreasApoioDS.GetByNomeAsync("Outro");
                fbAreaNova.Object.NPedidos++;
                await FBDataBase.AreasApoioDS.Update(fbAreaNova.Object, fbAreaNova.Key);
            }
        }

        private async Task AtualizarLocalidade(PedidoApoio item)
        {
            FirebaseObject<Localidade> fbNovaLocalidade = await FBDataBase.LocalidadeDS.GetByNomeAsync(item.Localidade);
            await FBDataBase.LocalidadeDS.UpdateByKey(fbNovaLocalidade.Key, nPedidos: fbNovaLocalidade.Object.NPedidos + 1);
        }
        public async Task<bool> RemoverParticipante(string keyPedido, string keyUtilizador, string motivo = "")
        {
            try
            {
                var pedido = await GetByKeyAsync(keyPedido);
                pedido.KeysUtilizadoresDisponiveis.Remove(keyUtilizador);

                var chatPedido = await FBDataBase.ChatDS.GetByKeyAsync(pedido.ChatKey);
                if (chatPedido != null)
                {
                    chatPedido.KeysUtilizadores.Remove(keyUtilizador);
                    await FBDataBase.ChatDS.Update(chatPedido, pedido.ChatKey);

                    var utilizador = await FBDataBase.UtilizadorDS.GetByKeyAsync<Utilizador>(keyUtilizador);
                    if (utilizador != null)
                    {
                        Aviso aviso = new Aviso
                            (
                                titulo: "Acesso a ação de voluntariado revocado",
                                descricao: AuthHelper.UtilizadorAtual.Object.NomeExibicao + " removeu-o da ação '" + pedido.Titulo +
                                "'\nMotivo dado:\n" + motivo,
                                mensagemPopUp: "Perdeu o acesso a dados de uma ação de voluntariado."

                            );
                        utilizador.Avisos.Add(aviso);
                        if (utilizador.ChatsKeys.Contains(pedido.ChatKey))
                            utilizador.ChatsKeys.Remove(pedido.ChatKey);
                        await FBDataBase.UtilizadorDS.Update(utilizador, keyUtilizador);
                    }
                }
                await Update(pedido, keyPedido);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
