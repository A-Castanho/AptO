using AppAptO.Models.FBData.Chats;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Areas.ParticipanteNoVoluntariado.Chats
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChatView : ContentView
    {
        private int countInicialLista;
        public ICommand ScrollListCommand { get; set; }
        private readonly ChatViewModel viewModel;
        public ChatView(Chat chat, string chatKey)
        {
            InitializeComponent();
            viewModel = new ChatViewModel(chat, chatKey);
            BindingContext = viewModel;
            ScrollListCommand = new Command(() =>
            {
                try
                {
                    countInicialLista = viewModel.ListaMensagens.Count();
                    Device.BeginInvokeOnMainThread(() =>
                    ChatList.ScrollTo(viewModel.ListaMensagens.FirstOrDefault(), ScrollToPosition.Start, false));
                }
                catch { }
            });
            countInicialLista = viewModel.ListaMensagens.Count();
        }

        private void ListView_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            try
            {
                if (viewModel.ListaMensagens.Count > countInicialLista)
                    ScrollListCommand.Execute(null);
            }
            catch
            {
            }
        }
    }
}