
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Views.Aplicacao
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TituloPage : ContentPage
    {
        public TituloPage()
        {
            InitializeComponent();
        }

        protected override void OnDisappearing()
        {
            this.FadeTo(0);
            base.OnDisappearing();
        }
    }
}