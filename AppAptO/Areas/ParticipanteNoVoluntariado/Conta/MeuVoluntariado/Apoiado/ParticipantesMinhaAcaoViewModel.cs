using AppAptO.Models.FBData.Apoios;
using AppAptO.Models.FBData.Utilizadores;
using AppAptO.Services.Firebase;
using Firebase.Database;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AppAptO.ViewModels.Conta.MeuVoluntariado
{
    public class ParticipantesMinhaAcaoViewModel : ObservableObject
    {
        private PedidoApoio Acao { get; }
        public FirebaseObject<Utilizador> ParticipanteSelecionado { get; set; }
        private FirebaseObject<Utilizador> apoiado;
        public FirebaseObject<Utilizador> Apoiado
        {
            get { return apoiado; }
            private set { SetProperty(ref apoiado, value); }
        }
        private ObservableCollection<FirebaseObject<Utilizador>> lista;
        public ObservableCollection<FirebaseObject<Utilizador>> Lista
        {
            get { return lista; }
            private set { SetProperty(ref lista, value); }
        }
        public ICommand CommandGoToPerfilApoiado => new Command(
            async () => await Shell.Current.Navigation.PushAsync(new Views.Conta.PerfilPage(Apoiado)));
        public ICommand CommandGoToPerfilParticipante => new Command(
        async () =>
        {
            try
            {
                await Shell.Current.Navigation.PushAsync(new Views.Conta.PerfilPage(ParticipanteSelecionado));
            }
            catch { }
        });

        public ParticipantesMinhaAcaoViewModel() { }
        public ParticipantesMinhaAcaoViewModel(PedidoApoio acao)
        {
            Acao = acao;
            DefinirUtilizadores();
        }

        private async void DefinirUtilizadores()
        {
            Apoiado = await FBDataBase.UtilizadorDS.GetByUidAsync<Utilizador>(Acao.UidApoiado);
            Lista = new ObservableCollection<FirebaseObject<Utilizador>>((await FBDataBase.UtilizadorDS.GetAllAsync<Utilizador>())
                .ToList()
                .FindAll(utilizador => Acao.KeysUtilizadoresDisponiveis.Contains(utilizador.Key)));
        }
    }
}
