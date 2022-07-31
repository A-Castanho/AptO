using AppAptO.Models.FBData.Utilizadores;
using AppAptO.ViewModels.Conta;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.ViewsPartial.Conta
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DadosDisponibilidadeView : ContentView
    {
        DisponibilidadeViewModel viewModel;
        public DadosDisponibilidadeView(UtilizadorParticular utilizador)
        {
            InitializeComponent();
            viewModel = new DisponibilidadeViewModel(utilizador);
            BindingContext = viewModel;
        }
    }
}