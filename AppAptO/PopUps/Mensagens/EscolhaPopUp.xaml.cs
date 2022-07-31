using AppAptO.ViewModels.PopUps;
using System;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms.Xaml;

namespace AppAptO.PopUps
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EscolhaPopUp : Popup<bool>
    {
        public EscolhaPopUp(string titulo, string mensagem, string textoSaidaPositiva = "Sim", string textoSaidaNegativa = "Não")
        {
            InitializeComponent();
            BindingContext = new EscolhaPopUpViewModel(titulo, mensagem, textoSaidaPositiva, textoSaidaNegativa);
        }

        private void NegativeButton_Clicked(object sender, EventArgs e)
        {
            Dismiss(false);
        }
        private void PositiveButton_Clicked(object sender, EventArgs e)
        {
            Dismiss(true);
        }
    }
}