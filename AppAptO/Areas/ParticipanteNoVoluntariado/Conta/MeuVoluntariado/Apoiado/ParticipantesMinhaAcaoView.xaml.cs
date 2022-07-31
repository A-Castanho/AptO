using AppAptO.Models.FBData.Apoios;
using AppAptO.ViewModels.Conta.MeuVoluntariado;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Views.Conta.MeuVoluntariado
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ParticipantesMinhaAcaoView : ContentView
    {
        ParticipantesMinhaAcaoViewModel viewModel;
        public ParticipantesMinhaAcaoView(PedidoApoio acao)
        {
            InitializeComponent();
            viewModel = new ParticipantesMinhaAcaoViewModel(acao);
            BindingContext = viewModel;
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            viewModel.CommandGoToPerfilParticipante.Execute(sender);
        }
    }
}