using AppAptO.Models.FBData.Apoios;
using AppAptO.ViewModels.Conta.MeuVoluntariado;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Views.Conta.MeuVoluntariado
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MinhaAcaoTarefaPage : ContentPage
    {
        private MinhaAcaoTarefaViewModel viewModel;
        public MinhaAcaoTarefaPage()
        {
            InitializeComponent();
        }
        public MinhaAcaoTarefaPage(Tarefa tarefa, int tarefaIndex, string keyAcao)
        {
            InitializeComponent();
            viewModel = new MinhaAcaoTarefaViewModel(tarefa, tarefaIndex, keyAcao);
            BindingContext = viewModel;
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            viewModel.CommandGoToPerfilParticipante.Execute(sender);
        }

        private void Switch_Toggled(object sender, ToggledEventArgs e)
        {
            viewModel.CommandMudarEstado.Execute("");
        }
    }
}