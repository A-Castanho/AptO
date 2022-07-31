using System;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms.Xaml;

namespace AppAptO.PopUps.Mensagens
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InserirCodOrgPopUp : Popup<string>
    {
        InserirCodOrgViewModel viewModel;
        public InserirCodOrgPopUp()
        {
            InitializeComponent();
            viewModel = new InserirCodOrgViewModel();
            BindingContext = viewModel;
        }

        private void Button_Clicked(object sender, EventArgs e)
        {
            Dismiss(viewModel.Codigo);
        }

        private void ImageButton_Clicked(object sender, EventArgs e)
        {
            Dismiss("");
        }
    }
}