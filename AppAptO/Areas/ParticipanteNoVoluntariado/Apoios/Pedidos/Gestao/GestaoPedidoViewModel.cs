using AppAptO.Areas.ParticipanteNoVoluntariado.Apoios.Pedidos.Gestao;
using AppAptO.Models.FBData.Apoios;
using AppAptO.Models.FBData.Chats;
using AppAptO.Services.Firebase;
using AppAptO.Views.Conta.GestaoVoluntariado;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AppAptO.ViewModels.Conta.GestaoVoluntariado
{
    public class GestaoPedidoViewModel : ObservableObject
    {
        private bool isModoEdicao = true;
        public bool IsModoEdicao
        {
            get => isModoEdicao;
            set => SetProperty(ref isModoEdicao, value);
        }
        private Dictionary<string, ContentView> DictionaryViews;
        private string title;
        public string Title
        {
            get => title;
            set
            {
                SetProperty(ref title, value);
            }
        }
        private string PedidoKey { get; }
        private PedidoApoio pedido;
        public PedidoApoio Pedido
        {
            get { return pedido; }
            set { SetProperty(ref pedido, value); }
        }

        private ContentView viewAtual = new ContentView();
        public ContentView ViewAtual
        {
            get => viewAtual;
            set { SetProperty(ref viewAtual, value); }
        }
        public ICommand SelecionarViewCommand => new Command<string>(
        (string apontadorView) =>
        {
            try
            {
                Title = apontadorView;
                ViewAtual = DictionaryViews[apontadorView];
            }
            catch { }
        });
        public ICommand CommandGoToCrud
        {
            get
            {
                return new Command(
                        async () =>
                        {
                            await Shell.Current.Navigation.PushAsync(new Views.Pedidos.CrudPedidoPage(Pedido, PedidoKey));
                        });
            }
        }

        public GestaoPedidoViewModel() { }
        public GestaoPedidoViewModel(PedidoApoio pedido, string pedidoKey, bool isModoEdicao)
        {
            Pedido = pedido;
            PedidoKey = pedidoKey;
            IniciarViews();
            IsModoEdicao = isModoEdicao;
        }
        private async void IniciarViews()
        {
            Chat chat = await FBDataBase.ChatDS.GetByKeyAsync(Pedido.ChatKey);
            DictionaryViews = new Dictionary<string, ContentView>
                {
                    {"Tarefas",new Gestao_TarefasView(Pedido,PedidoKey,IsModoEdicao)},
                    {"Participantes",new GestaoParticipantesView(Pedido,PedidoKey,IsModoEdicao)},
                    {"Chat", new LiderChatView(chat, Pedido.ChatKey, PedidoKey)}
                };
            Title = "Tarefas";
            ViewAtual = DictionaryViews["Tarefas"];

        }
    }
}
