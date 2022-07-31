using AppAptO.Models.FBConnections;
using AppAptO.Models.FBData.Apoios;
using AppAptO.Models.FBData.Utilizadores;
using AppAptO.Services.Firebase;
using AppAptO.Views.Conta.GestaoVoluntariado;
using Firebase.Database;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppAptO.ViewModels.Conta.MeuVoluntariado.MeusPedido
{
    public class MeusPedidosViewModel : Xamarin.CommunityToolkit.ObjectModel.ObservableObject
    {
        private UtilizadorOrganizacao organizacao;
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

        private FirebaseObject<PedidoApoio> acaoSelecionada;
        public FirebaseObject<PedidoApoio> AcaoSelecionada
        {
            get => acaoSelecionada;
            set => SetProperty(ref acaoSelecionada, value);
        }
        private bool isModoEdicao = true;
        public bool IsModoEdicao
        {
            get => isModoEdicao;
            set => SetProperty(ref isModoEdicao, value);
        }
        private ObservableCollection<FirebaseObject<PedidoApoio>> lista = new ObservableCollection<FirebaseObject<PedidoApoio>>();
        public ObservableCollection<FirebaseObject<PedidoApoio>> Lista
        {
            get => lista;
            set => SetProperty(ref lista, value);
        }
        public ICommand CommandPesquisar => new Command(
            () =>
            {
                if (organizacao == null)
                    PesquisarLista();
                else
                    PesquisarLista(organizacao);
            });
        public ICommand CommandGoToAcao => new Command(
        async () =>
        {
            try
            {
                await Shell.Current.Navigation.PushAsync(new GestaoPedidoPage(AcaoSelecionada.Object, AcaoSelecionada.Key, IsModoEdicao));
            }
            catch { }
        });
        public MeusPedidosViewModel()
        {
            PesquisarLista();
        }
        public MeusPedidosViewModel(UtilizadorOrganizacao minhaOrganizacao)
        {
            IsModoEdicao = false;
            Title = "Ações da sua organização";
            organizacao = minhaOrganizacao;
            PesquisarLista(minhaOrganizacao);
        }

        public async void PesquisarLista()
        {
            ConteudoIniciado = false;
            Lista.Clear();
            Lista = new ObservableCollection<FirebaseObject<PedidoApoio>>((await FBDataBase.PedidosDS.GetAllAsync())
                .Where(acao => acao.Object.UidApoiado == AuthHelper.UtilizadorAtual.Object.Uid));

            ConteudoIniciado = true;
        }
        public async void PesquisarLista(UtilizadorOrganizacao minhaOrganizacao)
        {
            ConteudoIniciado = false;
            Lista.Clear();
            Lista = new ObservableCollection<FirebaseObject<PedidoApoio>>((await FBDataBase.PedidosDS.GetAllAsync())
                .Where(acao => acao.Object.UidApoiado == minhaOrganizacao.Uid));

            ConteudoIniciado = true;
        }
    }
}
