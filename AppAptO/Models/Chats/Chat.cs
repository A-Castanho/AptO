using AppAptO.Models.Chats;
using AppAptO.Models.FBConnections;
using AppAptO.Services.Firebase;
using System.Collections.ObjectModel;
using Xamarin.CommunityToolkit.ObjectModel;

namespace AppAptO.Models.FBData.Chats
{
    public class Chat : ObservableObject
    {
        //private ObservableCollection<Mensagem> mensagens = new ObservableCollection<Mensagem>();
        public ObservableCollection<string> KeysUtilizadores { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<Mensagem> Mensagens { get; set; } = new ObservableCollection<Mensagem>();
        public string Nome { get; set; }
        public string ImageSource { get; set; }
        public bool IsPessoal { get; }
        //public ObservableCollection<Mensagem> Mensagens
        //{
        //    get => mensagens; set
        //    {
        //        SetProperty(ref mensagens, value);
        //    }
        //}

        public Chat(ObservableCollection<string> keysUtilizaores, bool isPessoal, string imageSource = null)
        {
            KeysUtilizadores = keysUtilizaores;
            IsPessoal = isPessoal;
            ImageSource = imageSource ?? StorageHelper.UrlImgUtilizadorIndefinido;
        }
        //public Chat()
        //{
        //}
        public async void AdicionarMensagemEAtualizar(string keyChat, Mensagem mensagem)
        {
            Mensagens.Add(mensagem);
            await FBDataBase.ChatDS.Update(this, keyChat);
        }
    }
}
