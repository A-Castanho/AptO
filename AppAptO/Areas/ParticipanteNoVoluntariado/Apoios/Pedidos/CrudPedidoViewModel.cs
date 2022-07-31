using AppAptO.Models.FBConnections;
using AppAptO.Models.FBData.Apoios;
using AppAptO.Models.FBData.Apoios.Notificacoes;
using AppAptO.Models.FBData.Chats;
using AppAptO.Models.FBData.Utilizadores;
using AppAptO.PopUps;
using AppAptO.PopUps.Mensagens;
using AppAptO.PopUps.Select;
using AppAptO.Services.Firebase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace AppAptO.ViewModels.Pedidos
{
    class CrudPedidoViewModel : Xamarin.CommunityToolkit.ObjectModel.ObservableObject
    {
        private string title;
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
        private bool visibilidadeEliminar;
        public bool VisibilidadeEliminar
        {
            get => visibilidadeEliminar;
            set => SetProperty(ref visibilidadeEliminar, value);
        }
        public ICommand SubmeterCommand => new Command(Submeter);
        public ICommand CommandEliminar { get; private set; }
        public ICommand CommandOpenSelectLocalidade => new Command(
        async () =>
        {
            try
            {
                var localidadesNomes = (await FBDataBase.LocalidadeDS.GetAllAsync())
                                        .Select(localidade => localidade.Object.Nome);
                LocalidadeSelecionada = await Application.Current.MainPage.ShowPopupAsync(new SelectAndSearchPopUp(localidadesNomes));
            }
            catch { }
        });
        private List<string> areas = new List<string>();
        public List<string> Areas
        {
            get => areas;
            set => SetProperty(ref areas, value);
        }
        private string areaSelecionada = "";
        public string AreaSelecionada
        {
            get => areaSelecionada;
            set
            {
                SetProperty(ref areaSelecionada, value);
                if (areaSelecionada == "Outro")
                {
                    VisibilidadeEntryArea = true;
                }
                else
                {
                    if (VisibilidadeEntryArea) { VisibilidadeEntryArea = false; }
                }
            }
        }
        private string areaPersonalizada;
        public string AreaPersonalizada
        {
            get => areaPersonalizada;
            set => SetProperty(ref areaPersonalizada, value);
        }
        private string localidadeSelecionada = "";
        public string LocalidadeSelecionada
        {
            get => localidadeSelecionada;
            set
            {
                SetProperty(ref localidadeSelecionada, value);
            }
        }
        public string NovaLocalidade { get; set; }
        private bool visibilidadeEntryArea = true;
        public bool VisibilidadeEntryArea
        {
            get => visibilidadeEntryArea;
            set
            {
                SetProperty(ref visibilidadeEntryArea, value);
                PedidoApoio.Area = "";
            }
        }
        //Dados do pedido de apoio
        private string pedidoApoioKey;

        private PedidoApoio pedidoApoio;
        public PedidoApoio PedidoApoio
        {
            get => pedidoApoio;
            set => SetProperty(ref pedidoApoio, value);
        }

        //Construtores
        public CrudPedidoViewModel(PedidoApoio pedido, string pedidokey)
        {
            Iniciar(pedido, pedidokey);
        }

        private void Iniciar(PedidoApoio pedido, string pedidokey)
        {
            VisibilidadeEliminar = true;
            PedidoApoio = pedido;
            pedidoApoioKey = pedidokey;
            Title = "Editar Pedido";

            AreaPersonalizada = PedidoApoio.Area;
            AreaSelecionada = Areas.Contains(PedidoApoio.Area) ? PedidoApoio.Area : "Outro";
            LocalidadeSelecionada = PedidoApoio.Localidade;

            if (AuthHelper.UtilizadorAtual.Key == "Admin")
                CommandEliminar = new Command(EliminarAdmin);
            else
                CommandEliminar = new Command(EliminarProprio);

            SetDropdowns();
        }

        private async void EliminarAdmin()
        {
            var confirmacao = await Xamarin.Forms.Shell.Current.Navigation.ShowPopupAsync(new EscolhaPopUp("Cuidado!", "De certeza que pretende eliminar esta oferta de apoio do sistema?"));
            if (confirmacao)
            {
                var mensagem = await Xamarin.Forms.Shell.Current.Navigation.ShowPopupAsync(new InserirTextoPopUp("Eliminar publicação", "Insira o motivo de eliminação desta publicação"));
                if (!string.IsNullOrEmpty(mensagem))
                {
                    var apoiado = await FBDataBase.UtilizadorDS.GetByUidAsync<Utilizador>(pedidoApoio.UidApoiado);
                    apoiado.Object.Avisos.Add(new Aviso("Pedido de Apoio Eliminado",
                        "O seu pedido de apoio '" + PedidoApoio.Titulo + "' do dia " + PedidoApoio.DiaPublicacao.Date
                        + " foi eliminado pela administração.\nMotivo dado: \n" + mensagem,
                        "Pedido de apoio eliminado pela administração"));
                    await FBDataBase.UtilizadorDS.Update(apoiado.Object, apoiado.Key);
                    EliminarPedido();
                    await Shell.Current.Navigation.ShowPopupAsync(new MensagemPopUp("Pedido eliminado", "O pedido de apoio foi eliminado com sucesso.", "Ok"));
                    await Shell.Current.Navigation.PopToRootAsync();
                }
                else
                    await Shell.Current.Navigation.ShowPopupAsync(new MensagemPopUp("Pedido não eliminado", "Não foi dado nenhum motivo para a eliminação logo a operação foi cancelada", "Ok"));
            }
        }
        public async void EliminarProprio()
        {
            bool confirmacao = await Application.Current.MainPage.ShowPopupAsync(new EscolhaPopUp("Eliminar pedido", "De certeza que pretende eliminar estae pedido?", "Sim", "Não"));
            if (confirmacao)
            {
                EliminarPedido();
                await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Sucesso!", "O seu pedido foi eliminado.", "Ok"));
            }
        }

        private async void EliminarPedido()
        {
            //Remover todos os utilizadores da ação de apoio
            foreach (var key in PedidoApoio.KeysUtilizadoresDisponiveis)
            {
                await FBDataBase.PedidosDS.RemoverParticipante(pedidoApoioKey, key, "A ação de voluntariado foi eliminada");
            }

            //Remover o acesso ao chat pelo publicador
            var apoiado = await FBDataBase.UtilizadorDS.GetByUidAsync<Utilizador>(pedidoApoio.UidApoiado);
            apoiado.Object.ChatsKeys.Remove(PedidoApoio.ChatKey);
            await FBDataBase.UtilizadorDS.Update(apoiado.Object, apoiado.Key);

            await FBDataBase.ChatDS.DeleteByKey(PedidoApoio.ChatKey);
            await FBDataBase.PedidosDS.DeleteByKey(pedidoApoioKey);
        }

        public CrudPedidoViewModel()
        {
            SetDropdowns();
            PedidoApoio = new PedidoApoio();
            Title = "Adicionar Pedido";

            AreaSelecionada = "Outro";
        }

        private async void Submeter()
        {
            PedidoApoio.DiaPublicacao = DateTime.Now.Date;
            PedidoApoio.Area = AreaSelecionada == "Outro" ? AreaPersonalizada : AreaSelecionada;
            PedidoApoio.Localidade = LocalidadeSelecionada;
            string mensagem;
            if (!string.IsNullOrEmpty(pedidoApoioKey))
            {
                await FBDataBase.PedidosDS.Update(PedidoApoio, pedidoApoioKey);
                mensagem = "O seu pedido foi atualizado.";
            }
            else
            {
                await PublicarPedido();

                mensagem = "O seu pedido foi adicionado.";
            }
            await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Sucesso!", mensagem, "Voltar"));
            await App.Iniciar();
        }

        private async Task PublicarPedido()
        {
            PedidoApoio.UidApoiado = AuthHelper.UtilizadorAtual.Object.Uid;

            //Cria e atribui um chat para o grupo da açáo
            Chat chatAcao = new Chat(
                new ObservableCollection<string>() { AuthHelper.UtilizadorAtual.Key }, false,
                AuthHelper.UtilizadorAtual.Object.FotoUrl)
            { Nome = PedidoApoio.Titulo };
            string chatKey = await FBDataBase.ChatDS.Add(chatAcao);
            PedidoApoio.ChatKey = chatKey;

            //Adiciona o pedido 
            await FBDataBase.PedidosDS.Add(PedidoApoio);

            //Adiciona o chat ao utilizador
            var utilizadorAtualizado = AuthHelper.UtilizadorAtual.Object;
            utilizadorAtualizado.ChatsKeys.Add(chatKey);
            await FBDataBase.UtilizadorDS.Update(utilizadorAtualizado, AuthHelper.UtilizadorAtual.Key);
        }

        private async void SetDropdowns()
        {
            Areas.AddRange((await FBDataBase.AreasApoioDS.GetAllAsync()).Select(fbArea => fbArea.Object.Nome));
        }
    }

}
