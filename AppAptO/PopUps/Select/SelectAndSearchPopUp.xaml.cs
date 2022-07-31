using System.Collections.Generic;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.PopUps.Select
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SelectAndSearchPopUp : Popup<string>
    {
        SelectAndSearchViewModel viewModel;
        public SelectAndSearchPopUp(IEnumerable<string> collection)
        {
            InitializeComponent();
            viewModel = new SelectAndSearchViewModel(collection);
            BindingContext = viewModel;
        }

        private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Dismiss(viewModel.Selecionado);
        }
    }
}