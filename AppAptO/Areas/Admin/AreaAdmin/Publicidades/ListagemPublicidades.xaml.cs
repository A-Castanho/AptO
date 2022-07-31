using AppAptO.Models.DadosAplicacao;
using AppAptO.PopUps;
using Firebase.Database;
using System;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Areas.Admin.AreaAdmin.Publicidades
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListagemPublicidades : ContentPage
    {
        ListagemPublicidadesViewModel vm;
        public ListagemPublicidades()
        {
            InitializeComponent();
            vm = new ListagemPublicidadesViewModel();
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e)
        {
            try
            {
                await Shell.AdminShell.Current.Navigation.PushAsync(new CrudPublicidade());

            }
            catch (Exception ex)
            {
                Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n" +
                "Error data: \n" + ex.Data +
                "\nMessage: \n" + ex.Message +
                "\nStackTrace: \n" + ex.StackTrace +
                "\nSource: \n" + ex.Source +
                "\nInnerException: \n" + ex.InnerException +
                "\nTargetSite: \n" + ex.TargetSite
                );
            }
        }

        protected override void OnAppearing()
        {
            vm.CommandRefresh.Execute("");
        }

        private async void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            try
            {
                var publicidadeSelecionada = (FirebaseObject<Publicidade>)e.Item;
                await Shell.AdminShell.Current.Navigation.PushAsync(new CrudPublicidade(publicidadeSelecionada.Object, publicidadeSelecionada.Key));
            }
            catch
            {
                await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Erro", "Não foi possível aceder a esta publicidade"));
            }
        }
    }
}