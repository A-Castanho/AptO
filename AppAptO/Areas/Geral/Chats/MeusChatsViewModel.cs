using AppAptO.Controls;
using AppAptO.Models.FBConnections;
using AppAptO.Models.FBData.Chats;
using AppAptO.Models.FBData.Utilizadores;
using AppAptO.Services.Firebase;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AppAptO.Areas.ParticipanteNoVoluntariado.Chats
{
    public class MeusChatsViewModel : ObservableObject
    {
        private bool listaPorIniciar = true;
        public bool ListaPorIniciar
        {
            get => listaPorIniciar;
            set
            {
                SetProperty(ref listaPorIniciar, value);
            }
        }
        public string Pesquisa
        {
            get => pesquisa; set
            {
                if (value.Trim() == "")
                    Chats = originalChats;
                pesquisa = value;
            }
        }
        private KeyValuePair<string, Chat> chatSelecionado;
        public KeyValuePair<string, Chat> ChatSelecionado
        {
            get => chatSelecionado;
            set
            {
                SetProperty(ref chatSelecionado, value);
            }
        }
        private ObservableDictionary<string, Chat> originalChats = new ObservableDictionary<string, Chat>();
        private ObservableDictionary<string, Chat> chats = new ObservableDictionary<string, Chat>();
        private string pesquisa = "";

        public ObservableDictionary<string, Chat> Chats
        {
            get => chats; set
            {
                SetProperty(ref chats, value);
            }
        }
        public ICommand CommandGoToChat => new Command(
        () =>
        {
            if (ChatSelecionado.Value != null)
                Shell.Current.Navigation.PushAsync(new ChatPage(ChatSelecionado.Value, ChatSelecionado.Key));
        });
        public ICommand CommandPesquisar => new Command(
        () =>
        {
            var chatsFiltrados = originalChats.Where(c => c.Value.Nome.Trim().ToLower().Contains(Pesquisa.ToLower())).ToDictionary(v => v.Key, v => v.Value);
            Chats = new ObservableDictionary<string, Chat>(chatsFiltrados);
        });
        public ICommand CommandIniciarLista => new Command(IniciarLista);
        public MeusChatsViewModel()
        {
            IniciarLista();
        }
        private async void IniciarLista()
        {
            ListaPorIniciar = true;
            originalChats.Clear();
            foreach (var keyChat in AuthHelper.UtilizadorAtual.Object.ChatsKeys)
            {

                if (!originalChats.Keys.Contains(keyChat))
                {
                    var chat = await FBDataBase.ChatDS.GetByKeyAsync(keyChat);
                    if (chat != null)
                    {
                        if (string.IsNullOrEmpty(chat.Nome) && chat.KeysUtilizadores.Count == 2)
                        {
                            var keyOutroUtilizador = chat.KeysUtilizadores.First(key => key != AuthHelper.UtilizadorAtual.Key);
                            var outroUtilizador = (await FBDataBase.UtilizadorDS.GetByKeyAsync<Utilizador>(keyOutroUtilizador));
                            chat.ImageSource = outroUtilizador.FotoUrl;
                            chat.Nome = outroUtilizador.NomeExibicao;
                        }
                        originalChats.Add(keyChat, chat);
                    }
                }
            }
            originalChats.OrderBy
                (
                    c => c.Value.Mensagens.Last().DateTime
                );
            CommandPesquisar.Execute("");
            ListaPorIniciar = false;
        }

    }
}
