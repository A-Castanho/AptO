using AppAptO.Areas.ParticipanteNoVoluntariado.Chats;
using AppAptO.Models.FBData.Chats;
using AppAptO.Services.Firebase;
using System.Windows.Input;
using Xamarin.Forms;

namespace AppAptO.Areas.ParticipanteNoVoluntariado.Apoios.Pedidos.Gestao
{
    public class LiderChatViewModel
    {
        public string ChatKey { get; }
        public Chat Chat { get; }
        public string PedidoKey { get; }
        public View ChatView { get; }

        public ICommand CommandGoToConfig => new Command(
        async () =>
        {
            var pedido = await FBDataBase.PedidosDS.GetByKeyAsync(PedidoKey);
            await Shell.Current.Navigation.PushAsync(new GestaoChatPage(ChatKey, Chat, pedido));
        });

        public LiderChatViewModel()
        {
        }

        public LiderChatViewModel(Chat chat, string chatKey, string pedidoKey)
        {
            this.Chat = chat;
            this.PedidoKey = pedidoKey;
            this.ChatKey = chatKey;
            this.ChatView = new ChatView(chat, chatKey);
        }

    }
}
