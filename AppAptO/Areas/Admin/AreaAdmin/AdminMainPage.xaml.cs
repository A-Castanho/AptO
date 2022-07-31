using AppAptO.Areas.Admin.AreaAdmin.Publicidades;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Areas.Admin.AreaAdmin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminMainPage : ContentPage
    {
        public AdminMainPage()
        {
            InitializeComponent();
        }
        private async void btnPublicidade_Clicked(object sender, EventArgs e)
        {
            await Shell.AdminShell.Current.Navigation.PushAsync(new ListagemPublicidades());
        }

        private async void btnSabiasQue_Clicked(object sender, EventArgs e)
        {
            await Shell.AdminShell.Current.Navigation.PushAsync(new MsgSabiasQue.ListaMensagensPage());
        }

        private async void btnNotificacoes_Clicked(object sender, EventArgs e)
        {
            await Shell.AdminShell.Current.Navigation.PushAsync(new Notificacoes.EnvNotificacaoPage());
        }
    }
}