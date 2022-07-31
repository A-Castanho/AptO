using AppAptO.Models.FBData.Utilizadores;
using AppAptO.ViewModels.Conta.Configuracoes;
using Firebase.Database;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.ViewsPartial.Conta.Configuracoes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditarParticularView : ContentView
    {
        EditarParticularViewModel viewModel;
        public EditarParticularView()
        {
            InitializeComponent();
        }
        public EditarParticularView(FirebaseObject<UtilizadorParticular> fbUtilizador)
        {
            InitializeComponent();
            viewModel = new EditarParticularViewModel(fbUtilizador);
            BindingContext = viewModel;
        }
    }
}