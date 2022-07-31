using AppAptO.Models.FBConnections;
using AppAptO.Models.FBData.Utilizadores;
using AppAptO.Services.Firebase;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppAptO.PopUps.Mensagens
{
    public class CodOrganizacaoViewModel : ObservableObject
    {
        private UtilizadorOrganizacao organizacao;
        private string keyorganizacao;

        public CodOrganizacaoViewModel()
        {
            DefinirValores();
        }

        private async void DefinirValores()
        {
            var fbOrganizacao = await FBDataBase.UtilizadorDS.GetByUidAsync<UtilizadorOrganizacao>(AuthHelper.UtilizadorAtual.Object.Uid);
            organizacao = fbOrganizacao.Object;
            keyorganizacao = fbOrganizacao.Key;
            Codigo = organizacao.CodEntrada;
        }

        public ICommand CommandCopiarCod => new Command(
        () =>
        {
            Clipboard.SetTextAsync(Codigo);
        });
        public ICommand CommandRecriarCod => new Command(
        async () =>
        {
            organizacao.DefinirCodEntrada();
            await FBDataBase.UtilizadorDS.Update(organizacao, keyorganizacao);
            Codigo = organizacao.CodEntrada;
        });
        private string codigo;
        public string Codigo
        {
            get { return codigo; }
            set { SetProperty(ref codigo, value); }
        }
    }
}
