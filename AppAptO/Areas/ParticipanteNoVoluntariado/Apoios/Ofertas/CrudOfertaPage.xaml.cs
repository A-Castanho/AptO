using AppAptO.Models.AppHelpers;
using AppAptO.Models.FBData.Apoios;
using AppAptO.PopUps;
using AppAptO.ViewModels.Ofertas;
using AppAptO.Views.Erro;
using Firebase.Database;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Views.Ofertas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CrudOfertaPage : ContentPage
    {
        private readonly CrudOfertaViewModel viewModel;
        public CrudOfertaPage()
        {
            if (AppConnection.IsConnected)
            {

                InitializeComponent();
                viewModel = new CrudOfertaViewModel();
                BindingContext = viewModel;
            }
            else
            {
                OnNoInternet();
            }
        }
        private async void OnNoInternet()
        {
            await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Erro", "A operação foi cancelada devido à falta de conexão na internet", "Ok"));
            await Navigation.PushAsync(new SemInternetPage());
        }
        public CrudOfertaPage(FirebaseObject<OfertaApoio> fbOferta)
        {

            if (AppConnection.IsConnected)
            {

                InitializeComponent();
                viewModel = new CrudOfertaViewModel(fbOferta);
                // viewModel.IniciarParaEdicao();
                BindingContext = viewModel;
            }
            else
            {
                OnNoInternet();
            }
        }
        public CrudOfertaPage(OfertaApoio oferta, string ofertaKey)
        {
            if (AppConnection.IsConnected)
            {
                InitializeComponent();
                viewModel = new CrudOfertaViewModel(oferta, ofertaKey);
                // viewModel.IniciarParaEdicao();
                BindingContext = viewModel;
            }
            else
            {
                OnNoInternet();
            }
        }
    }
}