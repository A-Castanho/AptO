using AppAptO.Models.FBConnections;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Areas.Geral.Configuracoes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppSobrePage : ContentPage
    {
        public AppSobrePage()
        {
            InitializeComponent();
        }
        private async void GoToInstrucoes()
        {
            if (AuthHelper.UtilizadorAtual.Object.IsApoiado)
                await Shell.Current.Navigation.PushAsync(new InstrucoesApoiadoPage());
            else
                await Shell.Current.Navigation.PushAsync(new InstrucoesVoluntarioPage());
        }
        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            GoToInstrucoes();
        }
    }
}