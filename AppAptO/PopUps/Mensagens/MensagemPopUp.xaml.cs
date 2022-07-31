using AppAptO.ViewModels.PopUps;
using System;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;

namespace AppAptO.PopUps
{
    public partial class MensagemPopUp : Popup
    {
        public MensagemPopUp(string titulo, string mensagem, string textoSaida = "Ok")
        {
            InitializeComponent();
            BindingContext = new MensagemPopupViewModel(titulo, mensagem, textoSaida);
            Size = new Size(gridMessage.Width + labelTitulo.Width + button.Width, gridMessage.Height + labelTitulo.Height + button.Height);
        }


        private void Button_Clicked(object sender, EventArgs e)
        {
            Dismiss("");
        }
    }
}