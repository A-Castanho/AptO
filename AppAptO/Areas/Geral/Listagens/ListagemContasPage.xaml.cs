using AppAptO.ViewModels.Conta;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Views.Conta
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListagemContasPage : ContentPage
    {
        ListagemContasViewModel viewModel = new ListagemContasViewModel();
        public ListagemContasPage()
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            viewModel.CommandGoToUtilizador.Execute(sender);
        }

        private void SearchBar_SearchButtonPressed(object sender, EventArgs e)
        {
            viewModel.CommandPesquisar.Execute(null);
        }
    }
}