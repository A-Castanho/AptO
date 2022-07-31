using AppAptO.Models.FBData.Apoios;
using AppAptO.ViewModels.Conta.GestaoVoluntariado;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Views.Conta.GestaoVoluntariado
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GestaoParticipantesView : ContentView
    {
        public GestaoParticipantesView(PedidoApoio pedido, string pedidoKey, bool isModoEdicao = true)
        {
            InitializeComponent();
            BindingContext = new GestaoParticipantesViewModel(pedido, pedidoKey, isModoEdicao);
        }
    }
}