using AppAptO.Models.FBData.Apoios;
using AppAptO.ViewModels.Ofertas;
using Firebase.Database;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Views.Ofertas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class OfertaApoioPage : ContentPage
    {
        private readonly ViewModels.Ofertas.OfertaApoioViewModel viewModel;
        public OfertaApoioPage(FirebaseObject<OfertaApoio> ofertaApoio)
        {
            InitializeComponent();
            viewModel = new OfertaApoioViewModel(ofertaApoio.Object, ofertaApoio.Key);
            BindingContext = viewModel;
        }
        public OfertaApoioPage(OfertaApoio oferta, string ofertaKey)
        {
            InitializeComponent();
            viewModel = new OfertaApoioViewModel(oferta, ofertaKey);
            BindingContext = viewModel;
        }
    }
}