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
using Xamarin.Forms;

namespace AppAptO.ViewModels.PopUps
{
    //para os apoiados que tiveram algum voluntario a querer ajudar
    public class AceitarAjudaViewModel : Xamarin.CommunityToolkit.ObjectModel.ObservableObject
    {
        private bool isBusy;
        public bool IsBusy { get => isBusy; set => SetProperty(ref isBusy, value); }
        
        private Convite convite;
        public Convite Convite
        {
            get => convite;
            set => SetProperty(ref convite, value);
        }
        public Utilizador voluntario;
        public Utilizador Voluntario
        {
            get => voluntario;
            set => SetProperty(ref voluntario, value);
        }
        public PedidoApoio pedidoApoio;
        public PedidoApoio PedidoApoio
        {
            get => pedidoApoio;
            set => SetProperty(ref pedidoApoio, value);
        }
        public ICommand CommandGoToVoluntario => new Command(
        async () =>
        {
            var utilizador = await FBDataBase.UtilizadorDS.GetByKeyAsync<Utilizador>(Convite.EmissorKey);
            await Shell.Current.Navigation.PushAsync(new PerfilPage(utilizador, Convite.EmissorKey));
        });
        public ICommand CommandAceitar => new Command(
        async () =>
        {
            PedidoApoio acaoVoluntariado = await FBDataBase.PedidosDS.GetByKeyAsync(convite.ApoioKey);
            if (acaoVoluntariado.KeysUtilizadoresDisponiveis.Contains(convite.EmissorKey) == false)
            {
                acaoVoluntariado.KeysUtilizadoresDisponiveis.Add(convite.EmissorKey);
                await FBDataBase.PedidosDS.Update(acaoVoluntariado, convite.ApoioKey);
                //Adiciona o utilizador ao chat
                Chat chat = await FBDataBase.ChatDS.GetByKeyAsync(acaoVoluntariado.ChatKey);
                chat.KeysUtilizadores.Add(convite.EmissorKey);
                await FBDataBase.ChatDS.Update(chat, acaoVoluntariado.ChatKey);
                //Adiciona o chat  ao utilizador
                Voluntario.ChatsKeys.Add(acaoVoluntariado.ChatKey);
                await FBDataBase.UtilizadorDS.Update(Voluntario, convite.EmissorKey);
                await EnviarAviso(true);
            }

            //Remover a notificação
            await RemoverConvite();
        });
        public ICommand CommandRecusar => new Command(
        async () =>
        {
            await RemoverConvite();
            await EnviarAviso(false);
        });
        public ICommand CommandGoToPedido => new Command(
        async () => await Shell.Current.Navigation.PushAsync(new PedidoApoioPage(PedidoApoio, Convite.ApoioKey)));
        public AceitarAjudaViewModel() { }
        public AceitarAjudaViewModel(Convite convite)
        {
            Iniciar(convite);
        }
        private async void Iniciar(Convite convite)
        {
            IsBusy = true;
            Convite = convite;
            Voluntario = await FBDataBase.UtilizadorDS.GetByKeyAsync<Utilizador>(convite.EmissorKey);
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

        /// <summary>
        /// Enviar aviso da decisão ao emissor do convite
        /// </summary>
        /// <param name="aceite">Se a proposta foi aceite ou recusada</param>
        private async Task EnviarAviso(bool aceite)
        {
            //Enviar o aviso ao emissor do convite a dizer se a proposta foi ou não aceite
            var emissorAtualizado = (await FBDataBase.UtilizadorDS.GetByKeyAsync<Utilizador>(convite.EmissorKey));
            Aviso aviso;
            if (aceite)
                aviso = new Aviso("Oferta de Apoio Aceite!", AuthHelper.UtilizadorAtual.Object.NomeExibicao + " aceitou a sua oferta!\n A nova ação foi adicionada à sua área!", "Nova ação adicionada à sua área.");
            else
                aviso = new Aviso("Oferta de Apoio Recusada", AuthHelper.UtilizadorAtual.Object.NomeExibicao + " recusou a sua oferta de apoio para o pedido '" + PedidoApoio.Titulo + "'", "Oferta de apoio recusada");
            emissorAtualizado.Avisos.Add(aviso);
            await FBDataBase.UtilizadorDS.Update(emissorAtualizado, convite.EmissorKey);
        }
    }
}
