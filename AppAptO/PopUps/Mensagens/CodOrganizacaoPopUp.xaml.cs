using System;
using Xamarin.CommunityToolkit.UI.Views;

namespace AppAptO.PopUps
{
    public partial class CodOrganizacaoPopUp : Popup
    {
        public CodOrganizacaoPopUp()
        {
            InitializeComponent();
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Dismiss("");
        }
    }
}