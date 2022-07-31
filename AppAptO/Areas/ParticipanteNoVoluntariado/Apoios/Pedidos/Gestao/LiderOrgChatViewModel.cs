using AppAptO.Areas.ParticipanteNoVoluntariado.Chats;
using AppAptO.Models.FBData.Chats;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AppAptO.Areas.ParticipanteNoVoluntariado.Apoios.Pedidos.Gestao
{
    public class LiderOrgChatViewModel : ObservableObject
    {
        private ContentView chatiew;

        public Chat Chat { get; set; }
        public string ChatKey { get; set; }
        public ContentView ChatView
        {
            get => chatiew; set
            {
                SetProperty(ref chatiew, value);
            }
        }

        public LiderOrgChatViewModel(Chat chat, string chatKey)
        {
            Chat = chat;
            ChatKey = chatKey;
            ChatView = new ChatView(chat, chatKey);
        }

        public LiderOrgChatViewModel()
        {
        }
    }
}
