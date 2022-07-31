using AppAptO.Areas.ParticipanteNoVoluntariado.Chats;
using AppAptO.Models.FBConnections;
using AppAptO.Models.FBData.Apoios;
using AppAptO.Models.FBData.Apoios.Notificacoes;
using AppAptO.Models.FBData.Chats;
using AppAptO.Models.FBData.Utilizadores;
using AppAptO.PopUps;
using AppAptO.Services.Firebase;
using Firebase.Database;

using System;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppAptO.ViewModels.Pedidos
{
    public class PedidoApoioViewModel : Xamarin.CommunityToolkit.ObjectModel.ObservableObject
    {
        private readonly string pedidoKey;
        private FirebaseObject<Utilizador> apoiado;
        public PedidoApoio PedidoApoio { get; set; }
        public bool IsApoioProprio { get; }
        public bool IsUtilizadorVoluntario { get; }
        public FirebaseObject<Utilizador> Apoiado
        {
            get => apoiado;
            private set => SetProperty(ref apoiado, value);
        }
        public PedidoApoioViewModel(PedidoApoio pedidoApoio, string pedidoApoioKey)
        {
            IsApoioProprio = (pedidoApoio.UidApoiado == AuthHelper.UtilizadorAtual.Object.Uid);
            IsUtilizadorVoluntario = AuthHelper.UtilizadorAtual.Object.IsVoluntario;
            PedidoApoio = pedidoApoio;
            pedidoKey = pedidoApoioKey;
            DefinirUtilizador(pedidoApoio);
        }
        public PedidoApoioViewModel() { }

        private async void DefinirUtilizador(PedidoApoio pedidoApoio)
        {
            Apoiado = (await FBDataBase.UtilizadorDS.GetByUidAsync<Utilizador>(pedidoApoio.UidApoiado));
        }
        public ICommand CommandEnviarMensagem => new Command(
        async () =>
        {
            string chatKey = await FBDataBase.UtilizadorDS.GetOrCreateChatPessoal(AuthHelper.UtilizadorAtual.Key, Apoiado.Key);
            Chat chat = await FBDataBase.ChatDS.GetByKeyAsync(chatKey);
            chat.Nome = Apoiado.Object.NomeExibicao;
            await Shell.Current.Navigation.PushAsync(new ChatPage(chat, chatKey));
        });
        public ICommand CommandEmail => new Command(
        async () =>
        {
            string cumprimento;

            if (DateTime.Now.Hour < new DateTime(1, 1, 1, 12, 1, 1).Hour)
            {
                cumprimento = "Bom dia";
            }
            else
            {
                if (DateTime.Now.Hour < new DateTime(1, 1, 1, 20, 1, 1).Hour)
                {
                    cumprimento = "Boa tarde";
                }
                else { cumprimento = "Boa noite"; }
            }
            var message = new EmailMessage
            (
                subject: "Pedido de Apoio AptO do dia " + PedidoApoio.DiaPublicacao.Day + "/" + PedidoApoio.DiaPublicacao.Month,
                body: cumprimento + ".\n O meu nome é " + AuthHelper.UtilizadorAtual.Object.NomeExibicao + " e tenho interesse em o auxiliar.",
                to: Apoiado.Object.Email
            );
            await Email.ComposeAsync(message);
        });
        public ICommand CommandAceitar => new Command(
        async () =>
        {
            bool confirmacao = await Application.Current.MainPage.ShowPopupAsync(new EscolhaPopUp("Confirmação", "Pretende notificar este utilizador que está disponível para o ajudar?", "Sim", "Não"));
            if (confirmacao)
            {
                Apoiado.Object.Convites.Add(new Convite(pedidoKey, Convite.Tipo.PedidoApoio, Apoiado.Key));
                await FBDataBase.UtilizadorDS.Update(Apoiado.Object, Apoiado.Key);
            }
        });
        public ICommand CommandGoToPerfil => new Command(
            async () => await Shell.Current.Navigation.PushAsync(new Views.Conta.PerfilPage(Apoiado)));
        public ICommand CommandGoToGerir => new Command(
            async () =>
            {
                await Shell.Current.Navigation.PushAsync(new Views.Conta.GestaoVoluntariado.GestaoPedidoPage(PedidoApoio, pedidoKey));
            });
        public ICommand CommandGoToEditar => new Command(
            async () => await Shell.Current.Navigation.PushAsync(new Views.Pedidos.CrudPedidoPage(PedidoApoio, pedidoKey)));
    }
}
