using AppAptO.Areas.ParticipanteNoVoluntariado.Chats;
using AppAptO.Areas.ParticipanteNoVoluntariado.Organizacao.Perfil;
using AppAptO.Areas.ParticipanteNoVoluntariado.Particular.Perfil;
using AppAptO.Models.FBConnections;
using AppAptO.Models.FBData.Apoios.Notificacoes;
using AppAptO.Models.FBData.Chats;
using AppAptO.Models.FBData.Utilizadores;
using AppAptO.PartialViews.Conta;
using AppAptO.PopUps;
using AppAptO.PopUps.Select;
using AppAptO.Services.Firebase;
using AppAptO.ViewsPartial.Conta;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppAptO.ViewModels.Conta
{
    public class PerfilViewModel : ObservableObject
    {
        private string utilizadorKey;
        private Utilizador utilizador;
        public Utilizador Utilizador
        {
            get => utilizador;
            set
            {
                SetProperty(ref utilizador, value);
            }
        }
        private bool isUtilizadorProprio;
        public bool IsUtilizadorProprio
        {
            get => isUtilizadorProprio;
            set
            {
                SetProperty(ref isUtilizadorProprio, value);
            }
        }
        private bool visibilidadePedirApoio;
        public bool VisibilidadePedirApoio
        {
            get => visibilidadePedirApoio;
            set
            {
                SetProperty(ref visibilidadePedirApoio, value);
            }
        }

        public class ElementoView
        {
            public ContentView View { get; }
            public string Nome { get; }

            public ElementoView(ContentView view, string nome)
            {
                View = view;
                Nome = nome;
            }
        }
        private ObservableRangeCollection<ElementoView> views = new ObservableRangeCollection<ElementoView>();
        public ObservableRangeCollection<ElementoView> Views
        {
            get => views;
            set
            {
                SetProperty(ref views, value);
            }
        }
        public ICommand CommandRetornar => new Command(
        async () => await Shell.Current.Navigation.PopModalAsync());

        public ICommand CommandEnviarEmail => new Command(
        async () =>
        {
            try
            {
                var message = new EmailMessage
                (
                    subject: "Contacto via AptO",
                    body: "",
                    to: Utilizador.Email
                );
                await Email.ComposeAsync(message);
            }
            catch
            {
                await App.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Erro", "Não conseguimos aceder ao envio de email\nPor favor retire o e-mail do utilizador e envie a mensagem manualmente."));
            }
        });
        public ICommand CommandEnviarMensagem => new Command(
        async () =>
        {
            string chatKey = await FBDataBase.UtilizadorDS.GetOrCreateChatPessoal(AuthHelper.UtilizadorAtual.Key, utilizadorKey);
            Chat chat = await FBDataBase.ChatDS.GetByKeyAsync(chatKey);
            chat.Nome = Utilizador.NomeExibicao;
            await Shell.Current.Navigation.PushAsync(new ChatPage(chat, chatKey));
        });
        public ICommand CommandSelectPedido => new Command(
        async () =>
        {
            try
            {
                var lista = (await FBDataBase.PedidosDS.GetAllAsync()).Where(pedido => pedido.Object.UidApoiado == AuthHelper.UtilizadorAtual.Object.Uid && !pedido.Object.KeysUtilizadoresDisponiveis.Contains(utilizadorKey));
                string pedidoKey = await Shell.Current.Navigation.ShowPopupAsync(new SelectPedidoPopUp(lista));
                if (!string.IsNullOrWhiteSpace(pedidoKey))
                {
                    var confirmacao = await Shell.Current.Navigation.ShowPopupAsync(new EscolhaPopUp("Aviso", "Pretende enviar um convite ao voluntário para participar na sua causa?"));
                    if (confirmacao)
                    {
                        Convite convite = new Convite(pedidoKey, Convite.Tipo.ConvitePorPerfil, utilizadorKey);
                        Utilizador.Convites.Add(convite);
                        await FBDataBase.UtilizadorDS.Update(Utilizador, utilizadorKey);
                        await Shell.Current.Navigation.ShowPopupAsync(new MensagemPopUp("Sucesso", "O seu pedido de ajuda foi enviado ao voluntário!", "Ok"));
                    }
                }
            }
            catch { }
        });

        public PerfilViewModel() { }
        public PerfilViewModel(Utilizador utilizador, string key)
        {
            utilizadorKey = key;
            Iniciar(utilizador);
            IsUtilizadorProprio = (utilizador.Uid == AuthHelper.UtilizadorAtual.Object.Uid);
            VisibilidadePedirApoio = ((!IsUtilizadorProprio) && utilizador.IsVoluntario && AuthHelper.UtilizadorAtual.Object.IsApoiado);
        }
        private async void Iniciar(Utilizador utilizador)
        {
            try
            {
                Utilizador = utilizador;
                await DefinirViews();
            }
            catch
            {
                await Application.Current.MainPage.Navigation.PopAsync();
                await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Erro", "Ocorreu um erro ao carregar este perfil", "Ok"));
            }
        }

        private async Task DefinirViews()
        {
            if (!Utilizador.IsOrganizacao)
            {
                var utilizador = (await FBDataBase.UtilizadorDS.GetByUidAsync<UtilizadorParticular>(Utilizador.Uid)).Object;
                Views.Add(new ElementoView(new DadosParticularView(utilizador), "Sobre"));
                Views.Add(new ElementoView(new DadosDisponibilidadeView(utilizador), "Disponibilidade"));
            }
            else
            {
                var utilizador = (await FBDataBase.UtilizadorDS.GetByUidAsync<UtilizadorOrganizacao>(Utilizador.Uid)).Object;
                Views.Add(new ElementoView(new DadosOrganizacaoView(utilizador), "Sobre"));
                Views.Add(new ElementoView(new MembrosOrganizacaoView(utilizador), "Membros"));

            }
            if (Utilizador.IsVoluntario)
            {
                Views.Add(new ElementoView(new OfertasPessoaisView(Utilizador, utilizadorKey), "Ofertas por: " + Utilizador.NomeExibicao));
            }
            else
            {
                Views.Add(new ElementoView(new PedidosPessoaisView(Utilizador, utilizadorKey), "Pedidos por: " + Utilizador.NomeExibicao));
            }




            //    Views = new ObservableRangeCollection<ElementoView>() 
            //    { 
            //        new ElementoView(),
            //        new ElementoView(),
            //        new ElementoView()
            //    };
            //    if (!Utilizador.IsOrganizacao&&!Utilizador.IsEmpresa)
            //        {
            //            ViewDisponibilidade = new DadosDisponibilidadeView((await FBDataBase.ParticularDS.GetByUidAsync(Utilizador.Uid)).Object);
            //        }
            //        if (Utilizador.IsVoluntario)
            //        {
            //            TituloDadosAdicionados = "Ofertas por: " + Utilizador.NomeExibicao;
            //            ViewDadosAdicionados = new OfertasPessoaisView(Utilizador, utilizadorKey);
            //        }
            //        else
            //        {
            //            TituloDadosAdicionados = "Pedidos por: "+Utilizador.NomeExibicao;
            //            ViewDadosAdicionados = new PedidosPessoaisView(Utilizador, utilizadorKey);
            //        }
            //        if (Utilizador.IsOrganizacao)
            //        {
            //            var utilizador= (await FBDataBase.OrganizacaoDS.GetByUidAsync(Utilizador.Uid)).Object;
            //            ViewMembros = new MembrosOrganizacaoView(utilizador);
            //            ViewDadosConta = new DadosOrganizacaoView(utilizador);
            //        }
            //        else
            //        {
            //            ViewDadosConta = new DadosParticularView((await FBDataBase.ParticularDS.GetByUidAsync(Utilizador.Uid)).Object);
            //        }
        }
    }
}
