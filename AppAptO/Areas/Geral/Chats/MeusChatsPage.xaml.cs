
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Areas.ParticipanteNoVoluntariado.Chats
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MeusChatsPage : ContentPage
    {
        MeusChatsViewModel viewmodel;
        public MeusChatsPage()
        {
            InitializeComponent();
            viewmodel = new MeusChatsViewModel();
            BindingContext = viewmodel;
        }

        private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            viewmodel.CommandGoToChat.Execute(null);
        }
    }
}