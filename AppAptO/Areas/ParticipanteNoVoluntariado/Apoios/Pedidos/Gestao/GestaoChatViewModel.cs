using AppAptO.Controls;
using AppAptO.Models.FBConnections;
using AppAptO.Models.FBData.Apoios;
using AppAptO.Models.FBData.Chats;
using AppAptO.Models.FBData.Utilizadores;
using AppAptO.PopUps;
using AppAptO.PopUps.Select;
using AppAptO.Services.Firebase;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.IO;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AppAptO.Areas.ParticipanteNoVoluntariado.Apoios.Pedidos.Gestao
{
    public class GestaoChatViewModel : ObservableObject
    {
        private ObservableDictionary<string, Utilizador> utilizadoresChat = new ObservableDictionary<string, Utilizador>();
        private ObservableDictionary<string, Utilizador> utilizadoresTodos = new ObservableDictionary<string, Utilizador>();
        private Stream fotoChatStream;
        private string imagemSource;
        public string ImagemSource { get => imagemSource; private set => SetProperty(ref imagemSource, value); }
        public ObservableDictionary<string, Utilizador> UtilizadoresChat { get => utilizadoresChat; set => SetProperty(ref utilizadoresChat, value); }
        public ObservableDictionary<string, Utilizador> UtilizadoresTodos { get => utilizadoresTodos; set => SetProperty(ref utilizadoresTodos, value); }
        private string ChatKey { get; }
        public string ChatTitulo { get; set; }
        public ICommand CommandMudarTitulo => new Command(
        async () =>
        {
            var confirm = await Shell.Current.Navigation.ShowPopupAsync(new EscolhaPopUp("Aviso", "Pertende alterar o nome do chat para '" + ChatTitulo + "'?"));
            if (confirm)
            {
                var chat = await FBDataBase.ChatDS.GetByKeyAsync(ChatKey);
                chat.Nome = ChatTitulo;
                await FBDataBase.ChatDS.Update(chat, ChatKey);

                await Shell.Current.Navigation.ShowPopupAsync(new MensagemPopUp("Sucesso", "O nome do chat foi atualizado."));
            }
        });
        public ICommand CommandSelectImagem => new Command(
        async () =>
        {
            try
            {
                if (CrossMedia.Current.IsPickPhotoSupported)
                {
                    MediaFile resultado = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions() { CompressionQuality = 50, PhotoSize = PhotoSize.MaxWidthHeight, MaxWidthHeight = 600 });
                    fotoChatStream = resultado.GetStream();
                    ImagemSource = resultado.Path;
                }
            }
            catch
            {
            }
        });
        public ICommand CommandMudarImagem => new Command(
        async () =>
        {
            var confirm = await Shell.Current.Navigation.ShowPopupAsync(new EscolhaPopUp("Aviso", "Pertende alterar a imagem do chat?"));
            if (confirm)
            {
                if (fotoChatStream != null)
                {
                    var chat = await FBDataBase.ChatDS.GetByKeyAsync(ChatKey);
                    chat.ImageSource = await StorageHelper.AtualizarStreamImagemChatAsync(fotoChatStream, ChatKey);
                    await FBDataBase.ChatDS.Update(chat, ChatKey);

                    await Shell.Current.Navigation.ShowPopupAsync(new MensagemPopUp("Sucesso", "A imagem do chat foi atualizada."));

                    fotoChatStream = null;
                }
                else
                    await Shell.Current.Navigation.ShowPopupAsync(new MensagemPopUp("Erro", "A imagem não foi selecionada"));
            }
        });
        public ICommand CommandAddUtilizador => new Command(
        async () =>
        {
            string keyUtilizador = await Shell.Current.Navigation.ShowPopupAsync(new SelectUtilizadorPopUp(UtilizadoresTodos.TrueDictionary));
            if (keyUtilizador != null)
            {
                var confirm = await Shell.Current.Navigation.ShowPopupAsync(new EscolhaPopUp("Aviso", "Pertende devolver o acesso ao chat pelo utilizador?"));
                if (confirm)
                {
                    if (!utilizadoresChat.ContainsKey(keyUtilizador))
                    {
                        var utilizador = await FBDataBase.UtilizadorDS.GetByKeyAsync<Utilizador>(keyUtilizador);
                        utilizador.ChatsKeys.Add(ChatKey);
                        await FBDataBase.UtilizadorDS.Update(utilizador, keyUtilizador);

                        var chat = await FBDataBase.ChatDS.GetByKeyAsync(ChatKey);
                        chat.KeysUtilizadores.Add(keyUtilizador);
                        await FBDataBase.ChatDS.Update(chat, ChatKey);

                        UtilizadoresTodos[keyUtilizador] = utilizador;
                        UtilizadoresChat.Add(keyUtilizador, utilizador);

                        await Shell.Current.Navigation.ShowPopupAsync(new MensagemPopUp("Sucesso", "O utilizador tem agora acesso ao chat."));
                    }
                    else
                        await Shell.Current.Navigation.ShowPopupAsync(new MensagemPopUp("Erro", "O utilizador já tem acesso ao chat."));
                }
            }
        });
        public ICommand CommandRemoveUtilizador => new Command(
        async () =>
        {
            string keyUtilizador = await Shell.Current.Navigation.ShowPopupAsync(new SelectUtilizadorPopUp(UtilizadoresChat.TrueDictionary));
            if (keyUtilizador != null)
            {
                var confirm = await Shell.Current.Navigation.ShowPopupAsync(new EscolhaPopUp("Aviso", "Pertende remover o acesso do utilizador ao chat?"));
                if (confirm)
                {
                    var utilizador = await FBDataBase.UtilizadorDS.GetByKeyAsync<Utilizador>(keyUtilizador);
                    utilizador.ChatsKeys.Remove(ChatKey);
                    await FBDataBase.UtilizadorDS.Update(utilizador, keyUtilizador);

                    var chat = await FBDataBase.ChatDS.GetByKeyAsync(ChatKey);
                    chat.KeysUtilizadores.Remove(keyUtilizador);
                    await FBDataBase.ChatDS.Update(chat, ChatKey);

                    UtilizadoresTodos[keyUtilizador] = utilizador;
                    UtilizadoresChat.Remove(keyUtilizador);

                    await Shell.Current.Navigation.ShowPopupAsync(new MensagemPopUp("Sucesso", "O acesso ao chat foi removido do utilizador."));
                }
            }
        });

        public GestaoChatViewModel() { }
        public GestaoChatViewModel(Chat chat, string chatKey, PedidoApoio pedido)
        {
            ChatKey = chatKey;
            ChatTitulo = chat.Nome;
            ImagemSource = chat.ImageSource;
            DefineUtilizadores(chat, pedido);
        }


        private async void DefineUtilizadores(Chat chat, PedidoApoio pedido)
        {
            UtilizadoresChat.Clear(); UtilizadoresTodos.Clear();
            //Definir os utilizadores que estão na ação de voluntariado
            foreach (var key in pedido.KeysUtilizadoresDisponiveis)
            {
                var utilizador = await FBDataBase.UtilizadorDS.GetByKeyAsync<Utilizador>(key);
                UtilizadoresTodos.Add(key, utilizador);
            }
            //Definir os utilizadores que estão no chat da ação
            foreach (var key in chat.KeysUtilizadores)
            {
                if (key != AuthHelper.UtilizadorAtual.Key)
                {
                    try
                    {
                        UtilizadoresTodos.TryGetValue(key, out Utilizador utilizador);
                        if (utilizador == null && key != AuthHelper.UtilizadorAtual.Key)
                            utilizador = await FBDataBase.UtilizadorDS.GetByKeyAsync<Utilizador>(key);
                        if (!UtilizadoresChat.ContainsKey(key))
                            UtilizadoresChat.Add(key, utilizador);
                    }
                    catch { }
                }
            }
        }
    }
}
