using AppAptO.Models.DadosAplicacao;
using AppAptO.PopUps;
using AppAptO.Services.Firebase;
using Firebase.Database;
using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AppAptO.Areas.Admin.AreaAdmin.MsgSabiasQue
{
    internal class CrudMensagemViewModel : ObservableObject
    {
        private string anotacao;
        public string Anotacao { get => anotacao; set => SetProperty(ref anotacao, value); }
        private string texto;
        public string Texto { get => texto; set => SetProperty(ref texto, value); }
        public MensagemInicial Mensagem { get; }
        private string MensagemKey { get; set; }
        public bool VisibilidadeEliminar { get; }
        public ICommand CommandSubmeter => new Command(
        async () =>
        {
            try
            {
                MensagemInicial mensagem = new MensagemInicial()
                {
                    DataAdicao = DateTime.Now,
                    Anotacao = Anotacao,
                    Texto = Texto
                };
                if (string.IsNullOrEmpty(MensagemKey))
                    await InserirPublicidade(mensagem);
                else
                    await AtualizarPublicidade(mensagem);
            }
            catch
            {

                await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Erro", "Foi impossível realizar a operação."));
            }
        });
        public ICommand CommandEliminar => new Command(
        async () =>
        {
            bool confirmacao = await Shell.AdminShell.Current.Navigation.ShowPopupAsync(new EscolhaPopUp("Aviso", "De certeza que pretende elminar esta mensagem?"));
            if (confirmacao)
            {
                try
                {
                    await FBDataBase.MsgSabiasDS.DeleteByKey(MensagemKey);
                    await Shell.AdminShell.Current.Navigation.PopAsync();
                }
                catch
                {

                    await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Erro", "Foi impossível eliminar a mensagem."));
                }
            }
        });
        public CrudMensagemViewModel(FirebaseObject<MensagemInicial> mensagemSelecionada)
        {
            Mensagem = mensagemSelecionada.Object;
            MensagemKey = mensagemSelecionada.Key;
            Texto = Mensagem.Texto;
            Anotacao = Mensagem.Anotacao;
            VisibilidadeEliminar = true;
        }
        public CrudMensagemViewModel()
        {
            Mensagem = new MensagemInicial();
            MensagemKey = "";
        }
        private async Task InserirPublicidade(MensagemInicial publicidade)
        {
            MensagemKey = await FBDataBase.MsgSabiasDS.Add(publicidade);
            try
            {
                await FBDataBase.MsgSabiasDS.Update(publicidade, MensagemKey); ;
                await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Sucesso!", "A mensagem foi inserida na base de dados"));
                try
                {
                    await Shell.AdminShell.Current.Navigation.PopAsync();
                }
                catch { }
            }
            catch
            {
                await FBDataBase.PublicidadeDS.DeleteByKey(MensagemKey);
                MensagemKey = "";
                await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Erro", "Foi impossível inserir a mensagem."));
            }
        }
        private async Task AtualizarPublicidade(MensagemInicial mensagem)
        {
            try
            {
                await FBDataBase.MsgSabiasDS.Update(mensagem, MensagemKey);
                await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Sucesso!", "A mensagem foi atualizada na base de dados"));
            }
            catch
            {
                await FBDataBase.MsgSabiasDS.DeleteByKey(MensagemKey);
                await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Erro", "Foi impossível realizar a operação"));
            }
        }

    }
}
