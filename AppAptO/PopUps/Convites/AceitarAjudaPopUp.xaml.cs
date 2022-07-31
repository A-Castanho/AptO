using AppAptO.Models.FBData.Apoios.Notificacoes;
using AppAptO.ViewModels.PopUps;
using System;
using Xamarin.CommunityToolkit.UI.Views;

namespace AppAptO.PopUps.Convites
{
    public partial class AceitarAjudaPopUp : Popup
    {
        public AceitarAjudaPopUp(Convite convite)
        {
            InitializeComponent();
            BindingContext = new AceitarAjudaViewModel(convite);
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Dismiss("");
        }
    }
}