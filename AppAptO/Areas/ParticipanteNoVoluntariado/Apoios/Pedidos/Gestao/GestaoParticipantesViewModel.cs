using AppAptO.Models.FBData.Apoios;
using AppAptO.Models.FBData.Utilizadores;
using AppAptO.PopUps;
using AppAptO.PopUps.Mensagens;
using AppAptO.Services.Firebase;
using AppAptO.Views.Conta;
using Firebase.Database;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AppAptO.ViewModels.Conta.GestaoVoluntariado
{
    public class GestaoParticipantesViewModel : ObservableObject
    {
        private bool isBusy = false;
        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }
        private bool isModoEdicao = true;
        public bool IsModoEdicao
        {
            get => isModoEdicao;
            set => SetProperty(ref isModoEdicao, value);
        }
        public PedidoApoio Pedido { get; set; }
        public FirebaseObject<Utilizador> UtilizadorSelecionado { get; set; }
        public string PedidoKey { get; set; }
        private ObservableCollection<FirebaseObject<Utilizador>> listaUtilizadores = new ObservableCollection<FirebaseObject<Utilizador>>();
        public ObservableCollection<FirebaseObject<Utilizador>> ListaUtilizadores
        {
            get { return listaUtilizadores; }
            set { SetProperty(ref listaUtilizadores, value); }
        }
        public ICommand CommandRemoverVoluntario => new Command(
        async () =>
        {
            //Se tiver sido selecionado um utilizador da lista de participantes 
            if (UtilizadorSelecionado != null)
            {
                var confirmacao = await Application.Current.MainPage.ShowPopupAsync(new EscolhaPopUp("Aviso", "De certeza que pretende remover este voluntário do acesso aos dados do seu pedido?", "Sim", "Não"));
                if (confirmacao)
                {
                    //Receber motivo da eliminação que será enviado para o voluntário eliminado
                    var motivo = await Shell.Current.Navigation.ShowPopupAsync(new InserirTextoPopUp("Motivo de remoção", "Insira o motivo pelo qual este utilizador foi removido do voluntariado."));
                    if (motivo == null) motivo = "";

                    //Remover o participante da ação de voluntariado
                    var resultado = FBDataBase.PedidosDS.RemoverParticipante(PedidoKey, UtilizadorSelecionado.Key, motivo);

                    //Mostrar a mensagem de sucesso
                    Application.Current.MainPage.ShowPopup(new MensagemPopUp("Sucesso", "O voluntário não faz mais parte desta ação.", "Ok"));
                }
            }
            else
            {
                Application.Current.MainPage.ShowPopup(new MensagemPopUp("Erro", "Não foi selecionado nenhum voluntário a eliminar."));
            }
        });
        public ICommand CommandGotoVoluntarios => new Command(
        async () =>
        {
            var page = new ListagemContasPage();
            ((ListagemContasViewModel)page.BindingContext).PesquisarVoluntarios = true;
            ((ListagemContasViewModel)page.BindingContext).PesquisarApoiados = false;
            await Shell.Current.Navigation.PushAsync(new ListagemContasPage());
        });
        public ICommand Refresh => new Command(
        async () =>
        {
            IsBusy = true;
            //Busca a ação à base de dados outra vez, se alguma alteração tiver sido feita é aplicada à página
            var pedidoAtualizado = await FBDataBase.PedidosDS.GetByKeyAsync(PedidoKey);
            if (Pedido.KeysUtilizadoresDisponiveis != pedidoAtualizado.KeysUtilizadoresDisponiveis)
                DefinirUtilizadores();
            Pedido = pedidoAtualizado;

            IsBusy = false;
        });

        //Construtor usado pela view
        public GestaoParticipantesViewModel() { }

        //Construtor usado pelo código atrás da view
        public GestaoParticipantesViewModel(PedidoApoio pedido, string pedidoKey, bool isModoEdicao)
        {
            Pedido = pedido;
            PedidoKey = pedidoKey;
            DefinirUtilizadores();
        }
        private async void DefinirUtilizadores()
        {
            ListaUtilizadores = new ObservableCollection<FirebaseObject<Utilizador>>((await FBDataBase.UtilizadorDS.GetAllAsync<Utilizador>())
                        .Where(utilizador => Pedido.KeysUtilizadoresDisponiveis.Contains(utilizador.Key)));
        }
    }
}
