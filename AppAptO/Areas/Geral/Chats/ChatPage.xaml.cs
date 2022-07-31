using AppAptO.Models.FBData.Chats;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Areas.ParticipanteNoVoluntariado.Chats
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatPage : ContentPage
    {
        public ChatPage(Chat chat, string chatKey)
        {
            InitializeComponent();
            Title = chat.Nome;
            this.Content = new ChatView(chat, chatKey);
        }
    }
}