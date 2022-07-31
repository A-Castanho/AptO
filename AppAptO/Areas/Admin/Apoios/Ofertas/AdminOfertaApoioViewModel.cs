using AppAptO.Models.FBData.Apoios;
using AppAptO.Models.FBData.Utilizadores;
using AppAptO.Services.Firebase;
using AppAptO.Views.Ofertas;
using Firebase.Database;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppAptO.Areas.Admin.Apoios.Ofertas
{
    public class AdminOfertaApoioViewModel : Xamarin.CommunityToolkit.ObjectModel.ObservableObject
    {
        public string OfertaKey { get; }
        private FirebaseObject<Utilizador> apoiante;
        public OfertaApoio OfertaApoio { get; set; }
        public FirebaseObject<Utilizador> Apoiante
        {
            get => apoiante;
            private set => SetProperty(ref apoiante, value);
        }
        public AdminOfertaApoioViewModel(OfertaApoio ofertaApoio, string ofertaKey)
        {
            OfertaApoio = ofertaApoio;
            this.OfertaKey = ofertaKey;
            DefinirUtilizador();
        }
        public AdminOfertaApoioViewModel() { }

        private async void DefinirUtilizador()
        {
            Apoiante = (await FBDataBase.UtilizadorDS.GetByUidAsync<Utilizador>(OfertaApoio.UidApoiante));
        }
        public ICommand CommandGoToPerfil => new Command(
        async () =>
        {
            await Xamarin.Forms.Shell.Current.Navigation.PushAsync(new Views.Conta.PerfilPage(Apoiante));
        });
        public ICommand CommandGoToEditar => new Command(
            async () => await Shell.AdminShell.Current.Navigation.PushAsync(new CrudOfertaPage(OfertaApoio, OfertaKey)));
    }
}
