using AppAptO.Models.FBData.Chats;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Areas.ParticipanteNoVoluntariado.Apoios.Pedidos.Gestao
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LiderChatView : ContentView
    {
        public LiderChatView(Chat chat, string chatKey, string pedidoKey)
        {
            InitializeComponent();
            BindingContext = new LiderChatViewModel(chat, chatKey, pedidoKey);
        }
    }
}