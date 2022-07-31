using AppAptO.Areas.ParticipanteNoVoluntariado.Chats;
using AppAptO.Models.FBConnections;
using AppAptO.Models.FBData.Chats;
using AppAptO.Models.FBData.Utilizadores;
using AppAptO.Services.Firebase;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AppAptO.Areas.Geral.Configuracoes
{
    public class AppContactosViewModel : ObservableObject
    {
        private Utilizador admin;
        public Utilizador Admin
        {
            get { return admin; }
            set { SetProperty(ref admin, value); }
        }
        public ICommand CommandChat => new Command(
        async () =>
        {
            string chatKey = await FBDataBase.UtilizadorDS.GetOrCreateChatPessoal(AuthHelper.UtilizadorAtual.Key, "Admin");
            Chat chat = await FBDataBase.ChatDS.GetByKeyAsync(chatKey);
            await Shell.Current.Navigation.PushAsync(new ChatPage(chat, chatKey));
        });
        public AppContactosViewModel()
        {
            DefinirAdmin();
        }
        private async void DefinirAdmin()
        {
            try { Admin = await FBDataBase.UtilizadorDS.GetByKeyAsync<Utilizador>("Admin"); }
            catch { await Shell.Current.Navigation.PopModalAsync(); }
        }
    }
}
