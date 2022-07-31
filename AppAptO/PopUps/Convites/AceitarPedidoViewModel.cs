using AppAptO.Models.FBConnections;
using AppAptO.Models.FBData.Apoios;
using AppAptO.Models.FBData.Apoios.Notificacoes;
using AppAptO.Models.FBData.Chats;
using AppAptO.Models.FBData.Utilizadores;
using AppAptO.Services.Firebase;
using AppAptO.Views.Conta;
using AppAptO.Views.Pedidos;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AppAptO.ViewModels.PopUps
{
    //para os voluntários cuja ajuda foi aceite
    public class AceitarPedidoViewModel : ObservableObject
    {
        private bool isBusy;
        public bool IsBusy { get => isBusy; set => SetProperty(ref isBusy, value); }
        private Convite convite;
        public Convite Convite
        {
            get => convite;
            set => SetProperty(ref convite, value);
        }
        public Utilizador apoiado;
        public Utilizador Apoiado
        {
            get => apoiado;
            set => SetProperty(ref apoiado, value);
        }
        public PedidoApoio pedidoApoio;
        public PedidoApoio PedidoApoio
        {
            get => pedidoApoio;
            set => SetProperty(ref pedidoApoio, value);
        }
        public ICommand CommandGoToApoiado => new Command(
        async () =>
        {
            var utilizador = await FBDataBase.UtilizadorDS.GetByKeyAsync<Utilizador>(Convite.EmissorKey);
            await Shell.Current.Navigation.PushAsync(new PerfilPage(utilizador, Convite.EmissorKey));
        });
        public ICommand CommandAceitar => new Command(
        async () =>
        {
            PedidoApoio acaoVoluntariado = await FBDataBase.PedidosDS.GetByKeyAsync(convite.ApoioKey);
            if (acaoVoluntariado.KeysUtilizadoresDisponiveis.Contains(AuthHelper.UtilizadorAtual.Key) == false)
            {
                acaoVoluntariado.KeysUtilizadoresDisponiveis.Add(AuthHelper.UtilizadorAtual.Key);
                await FBDataBase.PedidosDS.Update(acaoVoluntariado, convite.ApoioKey);

                //Adiciona o utilizador ao chat
                Chat chat = await FBDataBase.ChatDS.GetByKeyAsync(acaoVoluntariado.ChatKey);
                chat.KeysUtilizadores.Add(AuthHelper.UtilizadorAtual.Key);
                await FBDataBase.ChatDS.Update(chat, acaoVoluntariado.ChatKey);
                //Adiciona o chat  ao utilizador
                var utilizadorAtualizado = AuthHelper.UtilizadorAtual.Object;
                utilizadorAtualizado.ChatsKeys.Add(acaoVoluntariado.ChatKey);

                await FBDataBase.UtilizadorDS.Update(utilizadorAtualizado, AuthHelper.UtilizadorAtual.Key);
                await EnviarAviso(true);
            }

            await RemoverConvite();
        });
        public ICommand CommandRecusar => new Command(
        async () =>
        {
            await RemoverConvite();
            await EnviarAviso(false);
        });
        public ICommand CommandGoToPedido => new Command(
        async () =>
        {
            await Shell.Current.Navigation.PushAsync(new PedidoApoioPage(PedidoApoio, Convite.ApoioKey));
        });
        public AceitarPedidoViewModel()
        {
        }
        public AceitarPedidoViewModel(Convite convite)
        {
            Iniciar(convite);
        }

        private async void Iniciar(Convite convite)
        {
            IsBusy = true;
            Convite = convite;
            Apoiado = await FBDataBase.UtilizadorDS.GetByKeyAsync<Utilizador>(convite.EmissorKey);
            PedidoApoio = await FBDataBase.PedidosDS.GetByKeyAsync(convite.ApoioKey);
            IsBusy = false;
        }
        private async Task RemoverConvite()
        {
            //Eliminar o convite da lista do utilizador atual
            var recetorAtualizado = AuthHelper.UtilizadorAtual.Object;
            recetorAtualizado.Convites.Remove(Convite);
            await FBDataBase.UtilizadorDS.Update(recetorAtualizado, AuthHelper.UtilizadorAtual.Key);
        }
        private async Task EnviarAviso(bool aceite)
        {
            //Enviar o aviso ao emissor do convite a dizer se a proposta foi ou não aceite
            var emissorAtualizado = (await FBDataBase.UtilizadorDS.GetByKeyAsync<Utilizador>(convite.EmissorKey));
            Aviso aviso;
            if (aceite)
                aviso = new Aviso("Pedido de Apoio Aceite!", AuthHelper.UtilizadorAtual.Object.NomeExibicao + " aceitou o seu pedido!\n O utilizador foi adicionado à sua ação: '" + PedidoApoio.Titulo + "'", "Pedido de apoio aceite!");
            else
                aviso = new Aviso("Pedido de Apoio Recusado", AuthHelper.UtilizadorAtual.Object.NomeExibicao + " recusou o seu pedido de apoio '" + PedidoApoio.Titulo + "'", "Pedido de apoio recusado");
            emissorAtualizado.Avisos.Add(aviso);
            await FBDataBase.UtilizadorDS.Update(emissorAtualizado, convite.EmissorKey);
        }
    }
}
