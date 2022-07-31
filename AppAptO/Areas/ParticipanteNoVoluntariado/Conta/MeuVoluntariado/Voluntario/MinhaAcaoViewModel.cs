using AppAptO.Areas.ParticipanteNoVoluntariado.Chats;
using AppAptO.Models.FBData.Apoios;
using AppAptO.Models.FBData.Chats;
using AppAptO.Services.Firebase;
using AppAptO.Views.Conta.MeuVoluntariado;
using System.Collections.Generic;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppAptO.ViewModels.Conta.MeuVoluntariado
{
    public class MinhaAcaoViewModel : Xamarin.CommunityToolkit.ObjectModel.ObservableObject
    {
        private readonly bool isModoEdicao;
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
        public PedidoApoio acao;
        public PedidoApoio Acao
        {
            get => acao;
            set => SetProperty(ref acao, value);
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
        private string KeyAcao { get; }
        private string KeyOrganizacao { get; }

        private ContentView viewAtual = new ContentView();
        public ContentView ViewAtual
        {
            get => viewAtual;
            set { SetProperty(ref viewAtual, value); }
        }
        public MinhaAcaoViewModel() { }
        public MinhaAcaoViewModel(PedidoApoio acaoAAnalisar, string keyAcaoAAnalisar, string keyOrganizacao = "")
        {
            Acao = acaoAAnalisar;
            KeyAcao = keyAcaoAAnalisar;
            KeyOrganizacao = keyOrganizacao;
            this.isModoEdicao = string.IsNullOrEmpty(keyOrganizacao);
            IniciarViews();
        }
        private async void IniciarViews()
        {
            Chat chat = await FBDataBase.ChatDS.GetByKeyAsync(Acao.ChatKey);
            var chatView = new ContentView
            {
                Content = new ChatPage(chat, Acao.ChatKey).Content
            };
            DictionaryViews = new Dictionary<string, ContentView>
                {
                    {"Sobre", new SobreMinhaAcaoView(Acao, isModoEdicao, KeyAcao)},
                    {"Minhas Tarefas", new TarefasMinhaAcaoView(Acao,KeyAcao,KeyOrganizacao)},
                    {"Participantes", new ParticipantesMinhaAcaoView(Acao)},
                    {"Chat", chatView}
                };
            Title = "Sobre";
            ViewAtual = DictionaryViews["Sobre"];
        }
    }
}
