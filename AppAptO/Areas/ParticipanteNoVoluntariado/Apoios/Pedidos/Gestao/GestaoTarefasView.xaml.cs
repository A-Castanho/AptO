using AppAptO.Models.FBData.Apoios;
using AppAptO.ViewModels.Conta.GestaoVoluntariado;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Views.Conta.GestaoVoluntariado
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GestaoTarefasView : ContentView
    {
        private GestaoTarefasViewModel viewModel;
        public GestaoTarefasView(PedidoApoio pedido, string pedidoKey, bool isModoEdicao = true)
        {
            InitializeComponent();
            viewModel = new GestaoTarefasViewModel(pedido, pedidoKey, isModoEdicao);
            BindingContext = viewModel;
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            viewModel.VisibilidadeListaTarefas = !viewModel.VisibilidadeListaTarefas;
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            viewModel.CommandMudarEstado.Execute(sender);
        }
    }
}