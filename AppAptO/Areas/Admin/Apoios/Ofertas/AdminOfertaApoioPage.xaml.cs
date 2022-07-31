using AppAptO.Models.FBData.Apoios;
using Firebase.Database;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Areas.Admin.Apoios.Ofertas
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminOfertaApoioPage : ContentPage
    {
        private readonly AdminOfertaApoioViewModel viewModel;
        public AdminOfertaApoioPage(FirebaseObject<OfertaApoio> ofertaApoio)
        {
            InitializeComponent();
            viewModel = new AdminOfertaApoioViewModel(ofertaApoio.Object, ofertaApoio.Key);
            BindingContext = viewModel;
        }
        public AdminOfertaApoioPage(OfertaApoio oferta, string ofertaKey)
        {
            InitializeComponent();
            viewModel = new AdminOfertaApoioViewModel(oferta, ofertaKey);
            BindingContext = viewModel;
        }
    }
}