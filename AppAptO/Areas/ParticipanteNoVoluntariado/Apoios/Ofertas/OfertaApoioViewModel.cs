using AppAptO.Areas.ParticipanteNoVoluntariado.Chats;
using AppAptO.Models.FBConnections;
using AppAptO.Models.FBData.Apoios;
using AppAptO.Models.FBData.Chats;
using AppAptO.Models.FBData.Utilizadores;
using AppAptO.PopUps;
using AppAptO.Services.Firebase;
using AppAptO.Views.Ofertas;
using Firebase.Database;
using System;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppAptO.ViewModels.Ofertas
{
    public class OfertaApoioViewModel : Xamarin.CommunityToolkit.ObjectModel.ObservableObject
    {
        private readonly string ofertaKey;
        private FirebaseObject<Utilizador> apoiante;
        public bool IsOfertaPropria { get; }
        public OfertaApoio OfertaApoio { get; set; }
        public FirebaseObject<Utilizador> Apoiante
        {
            get => apoiante;
            private set => SetProperty(ref apoiante, value);
        }
        public OfertaApoioViewModel(OfertaApoio ofertaApoio, string ofertaKey)
        {
            OfertaApoio = ofertaApoio;
            IsOfertaPropria = (ofertaApoio.UidApoiante == AuthHelper.UtilizadorAtual.Object.Uid);
            DefinirUtilizador(ofertaApoio);
            this.ofertaKey = ofertaKey;
        }
        public OfertaApoioViewModel() { }

        private async void DefinirUtilizador(OfertaApoio ofertaApoio)
        {
            Apoiante = (await FBDataBase.UtilizadorDS.GetByUidAsync<Utilizador>(ofertaApoio.UidApoiante));
        }
        public ICommand CommandEnviarMensagem => new Command(
        async () =>
        {
            string chatKey = await FBDataBase.UtilizadorDS.GetOrCreateChatPessoal(AuthHelper.UtilizadorAtual.Key, Apoiante.Key);
            Chat chat = await FBDataBase.ChatDS.GetByKeyAsync(chatKey);
            chat.Nome = Apoiante.Object.NomeExibicao;
            await Shell.Current.Navigation.PushAsync(new ChatPage(chat, chatKey));
        });
        public ICommand CommandContactarEmail => new Command(
        async () =>
        {
            try
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
                    subject: "Oferta de Apoio AptO do dia " + OfertaApoio.DiaPublicacao.Day + "/" + OfertaApoio.DiaPublicacao.Month,
                    body: cumprimento + ".\n O meu nome é " + AuthHelper.UtilizadorAtual.Object.NomeExibicao + " e tenho interesse na sua oferta de apoio.",
                    to: Apoiante.Object.Email
                );
                await Email.ComposeAsync(message);
            }
            catch
            {
                await App.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Erro", "Não conseguimos aceder ao envio de email\nPor favor retire o e-mail do utilizador e envie a mensagem manualmente."));
            }
        });
        public ICommand CommandGoToPerfil => new Command(
        async () =>
        {
            await Shell.Current.Navigation.PushAsync(new Views.Conta.PerfilPage(Apoiante));
        });
        public ICommand CommandGoToEditar => new Command(
            async () => await Shell.Current.Navigation.PushAsync(new CrudOfertaPage(OfertaApoio, ofertaKey)));
    }
}
