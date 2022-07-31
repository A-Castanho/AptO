using AppAptO.Models.FBConnections;
using AppAptO.Models.FBData.Apoios;
using AppAptO.PopUps;
using AppAptO.Services.Firebase;
using System.Linq;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AppAptO.ViewModels.Conta.MeuVoluntariado
{
    public class SobreMinhaAcaoViewModel : ObservableObject
    {
        private bool isModoEdicao = true;
        public bool IsModoEdicao
        {
            get => isModoEdicao;
            set => SetProperty(ref isModoEdicao, value);
        }
        public string AcaoKey { get; }

        public PedidoApoio acao;
        public PedidoApoio Acao
        {
            get { return acao; }
            private set
            {
                SetProperty(ref acao, value);
            }
        }
        public ObservableRangeCollection<Tarefa> tarefas;
        public ObservableRangeCollection<Tarefa> Tarefas
        {
            get { return tarefas; }
            private set
            {
                SetProperty(ref tarefas, value);
            }
        }

        public SobreMinhaAcaoViewModel() { }
        public SobreMinhaAcaoViewModel(PedidoApoio acao, bool isModoEdicao, string acaoKey)
        {
            Acao = acao;
            IsModoEdicao = isModoEdicao;
            AcaoKey = acaoKey;
        }
        public ICommand SairAcaoCommand => new Command(
        async () =>
        {
            if (await Shell.Current.Navigation.ShowPopupAsync(new EscolhaPopUp("Aviso", "De certeza que pretende deixar esta ação de voluntariado?")))
            {
                //Remover o utilizador das tarefas em que este está
                var pedido = await FBDataBase.PedidosDS.GetByKeyAsync(AcaoKey);
                pedido.KeysUtilizadoresDisponiveis.Remove(AuthHelper.UtilizadorAtual.Key);
                foreach (var tarefa in pedido.Tarefas.Where(t => t.KeysVoluntariosEnvolvidos.Contains(AuthHelper.UtilizadorAtual.Key)))
                {
                    pedido.Tarefas.First(t => t == tarefa).KeysVoluntariosEnvolvidos.Remove(AuthHelper.UtilizadorAtual.Key);
                }
                await FBDataBase.PedidosDS.Update(pedido, AcaoKey);

                //Remover o chat do utilizador
                var utilizadorAtualizado = AuthHelper.UtilizadorAtual.Object;
                utilizadorAtualizado.ChatsKeys.Remove(Acao.ChatKey);
                await FBDataBase.UtilizadorDS.Update(utilizadorAtualizado, AuthHelper.UtilizadorAtual.Key);

                //Remover o utilizador do chat
                var chat = await FBDataBase.ChatDS.GetByKeyAsync(Acao.ChatKey);
                chat.KeysUtilizadores.Remove(AuthHelper.UtilizadorAtual.Key);
                await FBDataBase.ChatDS.Update(chat, Acao.ChatKey);

                await Shell.Current.Navigation.ShowPopupAsync(new MensagemPopUp("Sucesso", "A sua saída da ação de voluntariado foi realizada com sucesso."));
                await App.Iniciar();
            }
        });
    }
}
