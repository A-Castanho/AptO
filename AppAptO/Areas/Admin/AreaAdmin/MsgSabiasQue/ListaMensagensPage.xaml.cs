
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Areas.Admin.AreaAdmin.MsgSabiasQue
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ListaMensagensPage : ContentPage
    {
        public ListaMensagensPage()
        {
            InitializeComponent();
            ((ListaMensagensViewModel)BindingContext).CommandRefresh.Execute(null);
        }

        protected override void OnAppearing()
        {
            ((ListaMensagensViewModel)BindingContext).CommandRefresh.Execute(null);
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ((ListaMensagensViewModel)BindingContext).CommandGoToMensagem.Execute(null);
        }
    }
}