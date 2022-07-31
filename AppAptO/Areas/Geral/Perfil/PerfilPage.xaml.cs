using AppAptO.Models.FBData.Utilizadores;
using AppAptO.ViewModels.Conta;
using Firebase.Database;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Views.Conta
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PerfilPage : ContentPage
    {
        PerfilViewModel viewModel;
        public PerfilPage(FirebaseObject<Utilizador> utilizador)
        {
            InitializeComponent();
            viewModel = new PerfilViewModel(utilizador.Object, utilizador.Key);
            BindingContext = viewModel;
        }
        public PerfilPage(Utilizador utilizador, string key)
        {
            InitializeComponent();
            viewModel = new PerfilViewModel(utilizador, key);
            BindingContext = viewModel;
        }

    }
}