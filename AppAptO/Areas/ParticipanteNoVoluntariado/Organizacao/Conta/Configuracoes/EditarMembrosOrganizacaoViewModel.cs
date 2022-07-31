using AppAptO.Models.FBConnections;
using AppAptO.Models.FBData.Utilizadores;
using AppAptO.PopUps;
using AppAptO.Services.Firebase;
using AppAptO.Views.Conta;
using Firebase.Database;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AppAptO.Areas.ParticipanteNoVoluntariado.Organizacao.Conta.Configuracoes
{
    public class EditarMembrosOrganizacaoViewModel : ObservableObject
    {
        private bool isRefreshing;
        public bool IsRefreshing
        {
            get { return isRefreshing; }
            set { SetProperty(ref isRefreshing, value); }
        }
        private ObservableCollection<FirebaseObject<UtilizadorParticular>> utilizadores;
        public ObservableCollection<FirebaseObject<UtilizadorParticular>> Utilizadores
        {
            get { return utilizadores; }
            set { SetProperty(ref utilizadores, value); }
        }
        public FirebaseObject<UtilizadorParticular> UtilizadorSelecionado { get; set; }
        public ICommand CommandGoToPerfil => new Command(
        async () =>
        {
            await Shell.Current.Navigation.PushAsync(new PerfilPage(UtilizadorSelecionado.Object, UtilizadorSelecionado.Key));
        });
        public ICommand CommandOpenCod => new Command(
        () =>
        {
            Shell.Current.Navigation.ShowPopup(new CodOrganizacaoPopUp());
        });
        public ICommand CommandRefresh => new Command(
        () =>
        {
            IsRefreshing = true;
            DefinirUtilizadores();
            IsRefreshing = false;
        });
        public ICommand CommandRemoverVoluntario => new Command(
        async () =>
        {
            var utilizadorfb = await FBDataBase.UtilizadorDS.GetByUidAsync<UtilizadorOrganizacao>(AuthHelper.UtilizadorAtual.Object.Uid);
            var utilizador = utilizadorfb.Object;
            var confirmacao = await Shell.Current.Navigation.ShowPopupAsync(new EscolhaPopUp("Aviso", "De certeza que pretende remover este utilizador?"));
            if (confirmacao)
            {
                utilizador.KeysUtilizadoresParticulares.Remove(UtilizadorSelecionado.Key);
                await FBDataBase.UtilizadorDS.Update(utilizador, utilizadorfb.Key);
            }
        });

        public EditarMembrosOrganizacaoViewModel()
        {
            DefinirUtilizadores();
        }

        private async void DefinirUtilizadores()
        {
            var utilizador = await FBDataBase.UtilizadorDS.GetByUidAsync<UtilizadorOrganizacao>(AuthHelper.UtilizadorAtual.Object.Uid);
            var utilizadores = (await FBDataBase.UtilizadorDS.GetAllAsync<UtilizadorParticular>())
                .Where(u => utilizador.Object.KeysUtilizadoresParticulares.Contains(u.Key));
            Utilizadores = new ObservableCollection<FirebaseObject<UtilizadorParticular>>(utilizadores);
        }
    }
}
