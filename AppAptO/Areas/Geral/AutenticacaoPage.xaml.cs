
using AppAptO.Models.DadosAplicacao;
using AppAptO.Themes;
using AppAptO.ViewModels.Conta;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Views.Conta
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AutenticacaoPage : ContentPage
    {
        private Publicidade publicidade;
        public Publicidade Publicidade
        {
            get => publicidade; set
            {
                publicidade = value;
                imgPublicidade.Source = Publicidade.PathImagem;
            }
        }
        public AutenticacaoPage()
        {
            Application.Current.Resources.MergedDictionaries.Add(new VoluntarioTheme());
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            DefinirPublicidade();
        }

        private async void DefinirPublicidade()
        {
            var fbPublicidade = (await Services.Firebase.FBDataBase.PublicidadeDS.GetRndPublicidade(
                (Publicidade.TipoPublicidade)Enum.Parse(typeof(Publicidade.TipoPublicidade), "Horizontal"))
                );
            Publicidade = fbPublicidade != null ? fbPublicidade.Object : new Publicidade();
        }

        private void RadioButton_Checked(object sender, System.EventArgs e)
        {

        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            OpenPublicidade();
        }

        private async void OpenPublicidade()
        {
            try
            {
                await Browser.OpenAsync(Publicidade.Redirecionamento, BrowserLaunchMode.SystemPreferred);
            }
            catch
            {
            }
        }

        protected override bool OnBackButtonPressed()
        {
            if (!(this.BindingContext as AutenticacaoViewModel).IsLoginVisible)
            {
                (this.BindingContext as AutenticacaoViewModel).ComandoOpen.Execute("login");
                return true;
            }
            else
                return base.OnBackButtonPressed();
        }
    }
}