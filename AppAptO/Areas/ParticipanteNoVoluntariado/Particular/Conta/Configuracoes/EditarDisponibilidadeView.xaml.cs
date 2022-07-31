using AppAptO.Models.FBData.Utilizadores;
using AppAptO.ViewModels.Conta.Configuracoes;
using Firebase.Database;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.ViewsPartial.Conta.Configuracoes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditarDisponibilidadeView : ContentView
    {
        public EditarDisponibilidadeView()
        {
            InitializeComponent();
        }
        public EditarDisponibilidadeView(FirebaseObject<UtilizadorParticular> fbUtilizador)
        {
            InitializeComponent();
            BindingContext = new EditarDisponibilidadeViewModel(fbUtilizador);
        }
    }
}