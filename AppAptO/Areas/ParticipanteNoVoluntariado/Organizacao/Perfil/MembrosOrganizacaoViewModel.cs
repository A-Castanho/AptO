using AppAptO.Models.FBData.Utilizadores;
using AppAptO.Services.Firebase;
using AppAptO.Views.Conta;
using Firebase.Database;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AppAptO.Areas.ParticipanteNoVoluntariado.Organizacao.Perfil
{
    public class MembrosOrganizacaoViewModel : ObservableObject
    {
        private ObservableCollection<FirebaseObject<UtilizadorParticular>> utilizadores = new ObservableCollection<FirebaseObject<UtilizadorParticular>>();
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
        public MembrosOrganizacaoViewModel()
        {
        }
        public MembrosOrganizacaoViewModel(UtilizadorOrganizacao organizacao)
        {
            DefinirUtilizadores(organizacao);
        }

        private async void DefinirUtilizadores(UtilizadorOrganizacao organizacao)
        {
            Utilizadores = new ObservableCollection<FirebaseObject<UtilizadorParticular>>((await FBDataBase.UtilizadorDS.GetAllAsync<UtilizadorParticular>())
                .Where(u => organizacao.KeysUtilizadoresParticulares.Contains(u.Key)));
        }
    }
}
