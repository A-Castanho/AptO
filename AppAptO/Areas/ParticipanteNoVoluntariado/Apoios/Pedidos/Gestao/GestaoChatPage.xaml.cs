using AppAptO.Models.FBData.Apoios;
using AppAptO.Models.FBData.Chats;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Areas.ParticipanteNoVoluntariado.Apoios.Pedidos.Gestao
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GestaoChatPage : ContentPage
    {
        public GestaoChatPage(string keyChat, Chat chat, PedidoApoio pedido)
        {
            InitializeComponent();
            BindingContext = new GestaoChatViewModel(chat, keyChat, pedido);
        }
    }
}