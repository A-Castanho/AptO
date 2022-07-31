using AppAptO.Models.FBConnections;
using AppAptO.Models.FBData.Utilizadores;
using AppAptO.PopUps;
using AppAptO.PopUps.Select;
using AppAptO.Services.Firebase;
using Firebase.Database;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AppAptO.ViewModels.Conta.Configuracoes
{
    public class EditarOrganizacaoViewModel : ObservableObject
    {
        private Stream FotoPerfilStream { get; set; }
        private string localidade;
        public string Localidade
        {
            get => localidade; private set
            {
                SetProperty(ref localidade, value);
            }
        }
        private string imageSource;
        public string ImageSource
        {
            get => imageSource;
            set => SetProperty(ref imageSource, value);
        }
        private UtilizadorOrganizacao utilizador;
        private List<string> nomesLocalidades;
        private readonly string keyUtilizador;
        public UtilizadorOrganizacao Utilizador
        {
            get { return utilizador; }
            set { SetProperty(ref utilizador, value); }
        }

        public EditarOrganizacaoViewModel()
        {
        }

        public EditarOrganizacaoViewModel(FirebaseObject<UtilizadorOrganizacao> fbutilizador)
        {
            ImageSource = fbutilizador.Object.FotoUrl;
            Utilizador = fbutilizador.Object;
            keyUtilizador = fbutilizador.Key;
            Localidade = fbutilizador.Object.Localidade;
            Iniciar();
        }
        public EditarOrganizacaoViewModel(UtilizadorOrganizacao utilizador, string keyUtilizador)
        {
            ImageSource = utilizador.FotoUrl;
            Utilizador = utilizador;
            this.keyUtilizador = keyUtilizador;
            Localidade = utilizador.Localidade;
            Iniciar();
        }
        private async void Iniciar()
        {
            nomesLocalidades = (await FBDataBase.LocalidadeDS.GetAllAsync())
                                    .Select(localidade => localidade.Object.Nome)
                                    .Where(localidade => localidade.Contains(",")).ToList();
        }

        public ICommand SubmeterDadosPessoaisCommand => new Command(
        async () =>
        {
            if (FotoPerfilStream != null)
            {
                Utilizador.FotoUrl = await StorageHelper.AtualizarStreamImagemPerfilAsync(FotoPerfilStream, Utilizador.Uid);
            }
            Utilizador.Localidade = Localidade;
            await FBDataBase.UtilizadorDS.Update(Utilizador, keyUtilizador);
            //await FBDataBase.OrganizacaoDS.Update(Utilizador, AuthHelper.UtilizadorAtual.Key);
            await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Sucesso!", "Os seus dados foram atualizados!", "Ok"));
            await App.Iniciar();

        });
        public ICommand CommandOpenSelectLocalidade => new Command(
        async () =>
        {
            try
            {
                Localidade = await Application.Current.MainPage.ShowPopupAsync(new SelectAndSearchPopUp(nomesLocalidades));
            }
            catch { }
        });
        public ICommand AlterarImagemCommand => new Command(
        async () =>
        {
            try
            {
                if (CrossMedia.Current.IsPickPhotoSupported)
                {
                    MediaFile resultado = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions()
                    {
                        CompressionQuality = 50,
                        PhotoSize = PhotoSize.MaxWidthHeight,
                        MaxWidthHeight = 600
                    });
                    FotoPerfilStream = resultado.GetStream();
                    ImageSource = resultado.Path;
                }
            }
            catch { }
        });
    }
}
