using AppAptO.Models.FBConnections;
using AppAptO.Models.FBData.Apoios.Notificacoes;
using AppAptO.Models.FBData.Utilizadores;
using AppAptO.PopUps;
using AppAptO.PopUps.Convites;
using AppAptO.Services.Firebase;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.Forms;

namespace AppAptO.ViewModels.Conta
{
    public class ListaNotificacoesViewModel : Xamarin.CommunityToolkit.ObjectModel.ObservableObject
    {
        public Notificacao NotificacaoSelecionada { get; set; }
        private ObservableCollection<Notificacao> notificacoes;
        public ObservableCollection<Notificacao> Notificacoes
        { get => notificacoes; set => SetProperty(ref notificacoes, value); }
        public ICommand CommandOpenConvite => new Command(
        async () =>
        {
            if (NotificacaoSelecionada.GetType() == typeof(Convite))
            {
                if (AuthHelper.UtilizadorAtual.Object.IsVoluntario)
                    await Shell.Current.Navigation.ShowPopupAsync(new AceitarPedidoPopUp((Convite)NotificacaoSelecionada));
                else
                    await Shell.Current.Navigation.ShowPopupAsync(new AceitarAjudaPopUp((Convite)NotificacaoSelecionada));
            }
            else
            {
                Aviso aviso = (Aviso)NotificacaoSelecionada;
                await Shell.Current.Navigation.ShowPopupAsync(new MensagemPopUp(aviso.Titulo, aviso.Descricao, "Ok"));
                var utilizadorAtualizado = AuthHelper.UtilizadorAtual.Object;
                utilizadorAtualizado.Avisos.Remove(aviso);
                await FBDataBase.UtilizadorDS.Update(utilizadorAtualizado, AuthHelper.UtilizadorAtual.Key);
                await Shell.Current.Navigation.PopAsync();
            }
        });
        public ListaNotificacoesViewModel()
        {
            InserirNotificacoes();
            GerarDisposable();
        }

        private void InserirNotificacoes()
        {
            List<Notificacao> notificacoes = new List<Notificacao>();
            notificacoes.AddRange(AuthHelper.UtilizadorAtual.Object.Convites);
            notificacoes.AddRange(AuthHelper.UtilizadorAtual.Object.Avisos);
            notificacoes.OrderBy(c => c.DateTime.Date)
                .ThenBy(c => c.DateTime.TimeOfDay);
            Notificacoes = new ObservableCollection<Notificacao>(notificacoes);
        }

        private void GerarDisposable()
        {
            var disposable = FBDataBase.UtilizadorDS.DatabasePath
                            .AsObservable<Utilizador>()
                            .Subscribe((dbevent) =>
                            {
                                if (dbevent.Object != null)
                                {
                                    if (dbevent.Key == AuthHelper.UtilizadorAtual.Key)
                                        InserirNotificacoes();
                                }
                            });
        }
    }
}
