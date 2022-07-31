using AppAptO.Models.FBConnections;
using AppAptO.Models.FBData.Apoios;
using AppAptO.Models.FBData.Utilizadores;
using AppAptO.Services.Firebase;
using AppAptO.Views.Conta.MeuVoluntariado;
using Firebase.Database;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AppAptO.ViewModels.Conta
{
    public class MinhasAcoesViewModel : ObservableObject
    {
        public class ElementoLista : ObservableObject
        {
            private FirebaseObject<PedidoApoio> acao;
            private Utilizador utilizador;

            public FirebaseObject<PedidoApoio> Acao { get => acao; set => SetProperty(ref acao, value); }
            public Utilizador Utilizador { get => utilizador; set => SetProperty(ref utilizador, value); }
            public ElementoLista(FirebaseObject<PedidoApoio> fbPedidoApoio, Utilizador utilizador)
            {
                Utilizador = utilizador;
                Acao = fbPedidoApoio;
            }
            public ElementoLista(FirebaseObject<PedidoApoio> fbPedidoApoio, string uidUtilizador)
            {
                Acao = fbPedidoApoio;
                DefinirUtilizador(uidUtilizador);
            }

            private async void DefinirUtilizador(string uidUtilizador)
            {
                Utilizador = (await FBDataBase.UtilizadorDS.GetByUidAsync<Utilizador>(uidUtilizador)).Object;
            }

            public ElementoLista() { }
        }
        private string organizacaoKey;
        private bool conteudoIniciado = true;
        public bool ConteudoIniciado
        {
            get => conteudoIniciado;
            set => SetProperty(ref conteudoIniciado, value);
        }
        private string title = "Minhas Ações";
        public string Title
        {
            get => title;
            set => SetProperty(ref title, value);
        }
        private ElementoLista acaoSelecionada;
        public ElementoLista AcaoSelecionada
        {
            get => acaoSelecionada;
            set => SetProperty(ref acaoSelecionada, value);
        }
        private ObservableCollection<ElementoLista> lista = new ObservableCollection<ElementoLista>();
        public ObservableCollection<ElementoLista> Lista
        {
            get => lista;
            set => SetProperty(ref lista, value);
        }
        public ICommand CommandPesquisar => new Command(
            () =>
            {
                if (!string.IsNullOrEmpty(organizacaoKey))
                    PesquisarLista(organizacaoKey);
                else
                    PesquisarLista();
            });
        public ICommand CommandGoToAcao => new Command(
            async () => await Shell.Current.Navigation.PushAsync(new MinhaAcaoPage(AcaoSelecionada.Acao.Object, AcaoSelecionada.Acao.Key, organizacaoKey)));
        public MinhasAcoesViewModel()
        {
            PesquisarLista();
        }
        public MinhasAcoesViewModel(string keyOrganizacao)
        {
            Title = "Ações da sua organização";
            organizacaoKey = keyOrganizacao;
            PesquisarLista(keyOrganizacao);
        }
        public async void PesquisarLista()
        {
            ConteudoIniciado = false;
            Lista.Clear();
            Lista = new ObservableCollection<ElementoLista>
                (
                    (await FBDataBase.PedidosDS.GetAllAsync())
                    .Where(acao => acao.Object.KeysUtilizadoresDisponiveis.Contains(AuthHelper.UtilizadorAtual.Key))
                    .Select(elemento => new ElementoLista(elemento, elemento.Object.UidApoiado))
                );
            ConteudoIniciado = true;
        }
        public async void PesquisarLista(string organizacaoKey)
        {
            ConteudoIniciado = false;
            Lista.Clear();
            Lista = new ObservableCollection<ElementoLista>
                (
                    (await FBDataBase.PedidosDS.GetAllAsync())
                    .Where(acao => acao.Object.KeysUtilizadoresDisponiveis.Contains(organizacaoKey))
                    .Select(elemento => new ElementoLista(elemento, elemento.Object.UidApoiado))
                );
            ConteudoIniciado = true;
        }
    }
}
