using AppAptO.Models.FBData.Chats;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Areas.ParticipanteNoVoluntariado.Apoios.Pedidos.Gestao
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LiderOrgChat : ContentPage
    {
        public LiderOrgChat(Chat chat, string chatKey)
        {
            InitializeComponent();
            Title = chat.Nome;
            BindingContext = new LiderOrgChatViewModel(chat, chatKey);
        }
    }
}