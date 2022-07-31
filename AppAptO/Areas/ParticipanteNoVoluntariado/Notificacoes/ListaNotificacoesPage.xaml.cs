using AppAptO.ViewModels.Conta;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Views.Conta
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaNotificacoesPage : ContentPage
    {
        ListaNotificacoesViewModel viewModel = new ListaNotificacoesViewModel();
        public ListaNotificacoesPage()
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            viewModel.CommandOpenConvite.Execute(sender);
        }
    }
}