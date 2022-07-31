using AppAptO.Models.FBData.Apoios.Notificacoes;
using AppAptO.ViewModels.PopUps;
using System;
using Xamarin.CommunityToolkit.UI.Views;

namespace AppAptO.PopUps.Convites
{
    public partial class AceitarPedidoPopUp : Popup
    {
        public AceitarPedidoPopUp(Convite convite)
        {
            InitializeComponent();
            BindingContext = new AceitarPedidoViewModel(convite);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Dismiss("");
        }
    }
}