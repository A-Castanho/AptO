using AppAptO.Models.FBData.Apoios;
using AppAptO.ViewModels.Conta.MeuVoluntariado;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Views.Conta.MeuVoluntariado
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MinhaAcaoPage : ContentPage
    {
        public MinhaAcaoPage(PedidoApoio acao, string acaoKey, string organizacaoKey = "")
        {
            InitializeComponent();
            BindingContext = new MinhaAcaoViewModel(acao, acaoKey, organizacaoKey);
        }
    }
}