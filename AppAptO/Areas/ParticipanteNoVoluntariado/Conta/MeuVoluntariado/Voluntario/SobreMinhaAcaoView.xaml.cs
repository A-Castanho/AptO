using AppAptO.Models.FBData.Apoios;
using AppAptO.ViewModels.Conta.MeuVoluntariado;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Views.Conta.MeuVoluntariado
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SobreMinhaAcaoView : ContentView
    {
        SobreMinhaAcaoViewModel viewModel;
        public SobreMinhaAcaoView()
        {
            InitializeComponent();
        }
        public SobreMinhaAcaoView(PedidoApoio acao, bool isModoEdicao, string acaoKey)
        {
            InitializeComponent();
            viewModel = new SobreMinhaAcaoViewModel(acao, isModoEdicao, acaoKey);
            BindingContext = viewModel;
        }
    }
}