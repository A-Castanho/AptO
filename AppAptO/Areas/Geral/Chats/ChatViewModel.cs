using AppAptO.Helpers.Chat;
using AppAptO.Models.Chats;
using AppAptO.Models.FBData.Chats;
using AppAptO.Models.FBData.Utilizadores;
using AppAptO.Services.Firebase;
using Firebase.Database.Query;
using Firebase.Database.Streaming;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AppAptO.Areas.ParticipanteNoVoluntariado.Chats
{

    public class ChatViewModel : ObservableObject
    {
        public int NMensagensOcultas { get; set; } = 0;
        private ObservableCollection<MensagemFormatada> listaMensagens = new ObservableCollection<MensagemFormatada>();
        private readonly List<Mensagem> mensagensPorAdicionar = new List<Mensagem>();
        private readonly IEnumerable<string> keysUtilizadores;
        private Dictionary<string, Utilizador> chatUtilizadores = new Dictionary<string, Utilizador>();
        public Chat Chat { get; set; }
        public string KeyChat { get; set; }
        private string novaMensagem;
        public string NovaMensagem
        {
            get => novaMensagem;
            set
            {
                SetProperty(ref novaMensagem, value);
            }
        }
        public ObservableCollection<MensagemFormatada> ListaMensagens
        {
            get => listaMensagens; set
            {
                SetProperty(ref listaMensagens, value);
            }
        }

        public ChatViewModel()
        {
        }

        public ChatViewModel(Chat chat, string keyChat)
        {
            Chat = chat;
            this.KeyChat = keyChat;
            keysUtilizadores = Chat.KeysUtilizadores;
            DefinirMensagens(keyChat);

            DefinirUtilizadores();
            foreach (var mensagem in Chat.Mensagens)
            {
                mensagensPorAdicionar.Add(mensagem);
            }
        }

        private async void DefinirMensagens(string keyChat)
        {
            foreach (var mensagem in Chat.Mensagens)
            {
                var formatada = await GerarMensagem(mensagem);
                ListaMensagens.Insert(0, formatada);
            }

            var disposable = FBDataBase.ChatDS.DatabasePath.Child(keyChat).Child("Mensagens")
                .AsObservable<Mensagem>()
                .Subscribe(async (dbevent) =>
                {
                    if (dbevent.Object != null)
                    {
                        if (int.Parse(dbevent.Key) >= Chat.Mensagens.Count)
                        {
                            if (dbevent.EventType == FirebaseEventType.InsertOrUpdate)
                            {
                                Chat.Mensagens.Add(dbevent.Object);
                                ListaMensagens.Insert(0, await GerarMensagem(dbevent.Object));
                            }
                        }
                        if (dbevent.EventType == FirebaseEventType.Delete)
                        {
                            Chat.Mensagens.Remove(dbevent.Object);
                            ListaMensagens.RemoveAt(int.Parse(dbevent.Key));
                        }
                    }
                });
        }

        private async void DefinirUtilizadores()
        {
            chatUtilizadores = (await FBDataBase.UtilizadorDS.GetAllAsync<Utilizador>())
                .Where(utilizador => keysUtilizadores.Contains(utilizador.Key)).ToDictionary(key => key.Key, obj => obj.Object);
        }

        private void DefinirMensagens()
        {
            if (Chat.Mensagens.Count > 0 && ListaMensagens.Count == 0)
            {
                if (chatUtilizadores.Count == 0)
                {
                    DefinirUtilizadores();
                }
                ListaMensagens.Clear();
                var ultimasMensagens = Chat.Mensagens;
                ultimasMensagens.OrderBy(mensagem => mensagem.DateTime);
                InserirNovasMensagensEmLista(ultimasMensagens);
            }
            else
            {
                var countMensagens = Chat.Mensagens.Count;
                var countLista = (ListaMensagens.Count + NMensagensOcultas);
                var novasMensagens = Chat.Mensagens.Skip((ListaMensagens.Count + NMensagensOcultas));
                novasMensagens.OrderBy(mensagem => mensagem.DateTime);
                InserirNovasMensagensEmLista(novasMensagens);
            }
        }

        private async void InserirNovasMensagensEmLista(IEnumerable<Mensagem> novasMensagens)
        {
            foreach (Mensagem mensagem in novasMensagens)
            {
                var novaMensagem = await GerarMensagem(mensagem);
                ListaMensagens.Insert(0, novaMensagem);
                mensagensPorAdicionar.Remove(novaMensagem.Mensagem);
            }
        }
        private async Task<MensagemFormatada> GerarMensagem(Mensagem mensagem)
        {
            string nome = "Utilizador Nulo", key = "";
            var emissor = await FBDataBase.UtilizadorDS.GetByKeyAsync<Utilizador>(mensagem.KeyEmissor);
            if (emissor != null)
            {
                nome = emissor.NomeExibicao;
                key = mensagem.KeyEmissor;
            }
            var novaMensagem = new MensagemFormatada(mensagem, nome, key, mensagem.DateTime);
            return novaMensagem;
        }

        public ICommand CommandEnviarMensagem => new Command(
        async () =>
        {
            var mensagem = new Mensagem(NovaMensagem);
            Chat.Mensagens.Add(mensagem);
            ListaMensagens.Insert(0, await GerarMensagem(mensagem));
            var chat = Chat;
            if (chat.IsPessoal)
                chat.Nome = null;
            await FBDataBase.ChatDS.Update(Chat, KeyChat);
            NovaMensagem = "";
        });
        public ICommand CommandReiniciar => new Command(
        () =>
        {
            DefinirMensagens();
        });
    }
}
