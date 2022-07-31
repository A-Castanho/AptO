using AppAptO.Models.FBData.Apoios;
using AppAptO.ViewModels.PopUps.Select;
using Firebase.Database;
using System;
using System.Collections.Generic;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms.Xaml;

namespace AppAptO.PopUps.Select
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectPedidoPopUp : Popup<string>
    {
        private readonly SelectPedidoViewModel viewModel;
        public SelectPedidoPopUp(IEnumerable<FirebaseObject<PedidoApoio>> lista)
        {
            InitializeComponent();
            viewModel = new SelectPedidoViewModel(lista);
            BindingContext = viewModel;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            if (viewModel.PedidoSelecionado != null)
                Dismiss(viewModel.PedidoSelecionado.Key);
        }

        private void Button_Clicked_1(object sender, EventArgs e)
        {
            Dismiss("");
        }
    }
}