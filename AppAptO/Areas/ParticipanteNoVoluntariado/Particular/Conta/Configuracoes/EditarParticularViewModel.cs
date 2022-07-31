using AppAptO.Models.FBConnections;
using AppAptO.Models.FBData.Utilizadores;
using AppAptO.PopUps;
using AppAptO.PopUps.Mensagens;
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
using Xamarin.Forms;

namespace AppAptO.ViewModels.Conta.Configuracoes
{
    public class EditarParticularViewModel : Xamarin.CommunityToolkit.ObjectModel.ObservableObject
    {
        private Stream FotoPerfilStream { get; set; }
        private List<string> nomesLocalidades;
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
        private bool visibilidadeAddOrg;
        public bool VisibilidadeAddOrg
        {
            get => visibilidadeAddOrg;
            set => SetProperty(ref visibilidadeAddOrg, value);
        }
        private UtilizadorParticular utilizador = new UtilizadorParticular();
        private string localidade;
        private readonly string keyUtilizador;
        public UtilizadorParticular Utilizador
        {
            get => utilizador;
            set => SetProperty(ref utilizador, value);
        }

        public EditarParticularViewModel()
        {
        }

        public EditarParticularViewModel(FirebaseObject<UtilizadorParticular> fbutilizador)
        {
            ImageSource = fbutilizador.Object.FotoUrl;
            Utilizador = fbutilizador.Object;
            Localidade = Utilizador.Localidade;
            keyUtilizador = fbutilizador.Key;
            VisibilidadeAddOrg = string.IsNullOrEmpty(utilizador.UidGrupoPertencente);
            Iniciar();
        }

        private async void Iniciar()
        {
            nomesLocalidades = (await FBDataBase.LocalidadeDS.GetAllAsync())
                                    .Select(localidade => localidade.Object.Nome)
                                    .Where(localidade => localidade.Contains(",")).ToList();
        }

        public EditarParticularViewModel(UtilizadorParticular utilizador, string keyUtilizador)
        {
            ImageSource = utilizador.FotoUrl;
            Utilizador = utilizador;
            Localidade = Utilizador.Localidade;
            VisibilidadeAddOrg = string.IsNullOrEmpty(utilizador.UidGrupoPertencente);
            this.keyUtilizador = keyUtilizador;
            Iniciar();
        }
        public ICommand CommandOpenSelectLocalidade => new Command(
        async () =>
        {
            try
            {
                Localidade = await Application.Current.MainPage.ShowPopupAsync(new SelectAndSearchPopUp(nomesLocalidades));
            }
            catch { }
        });
        public ICommand SubmeterDadosPessoaisCommand => new Command(
            async () =>
            {
                if (FotoPerfilStream != null)
                {
                    Utilizador.FotoUrl = await StorageHelper.AtualizarStreamImagemPerfilAsync(FotoPerfilStream, Utilizador.Uid);
                }
                Utilizador.Localidade = Localidade;
                await FBDataBase.UtilizadorDS.Update(Utilizador, keyUtilizador);
                //await FBDataBase.UtilizadorDS.Update(Utilizador, AuthHelper.UtilizadorAtual.Key);
                await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Sucesso!", "Os seus dados foram atualizados!", "Ok"));
                await App.Iniciar();

            });

        public ICommand AlterarImagemCommand => new Command(
        async () =>
        {
            try
            {
                if (CrossMedia.Current.IsPickPhotoSupported)
                {
                    MediaFile resultado = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions() { CompressionQuality = 50, PhotoSize = PhotoSize.MaxWidthHeight, MaxWidthHeight = 600 });
                    FotoPerfilStream = resultado.GetStream();
                    ImageSource = resultado.Path;
                }
            }
            catch
            {
            }
        });
        public ICommand OpenOrgCodCommand => new Command(
        async () =>
        {
            try
            {
                string codOrganizacao = await Application.Current.MainPage.ShowPopupAsync(new InserirCodOrgPopUp());
                if (!string.IsNullOrEmpty(codOrganizacao))
                {
                    FirebaseObject<UtilizadorOrganizacao> organizacao = (await FBDataBase.UtilizadorDS.GetAllAsync<UtilizadorOrganizacao>())
                                            .FirstOrDefault(org => org.Object.CodEntrada == codOrganizacao);
                    if (organizacao != null)
                    {
                        if (organizacao.Object.IsApoiado && Utilizador.IsApoiado || organizacao.Object.IsVoluntario && Utilizador.IsVoluntario)
                        {
                            var confirmacao = (await Application.Current.MainPage.ShowPopupAsync(new EscolhaPopUp("Organização Encontrada", "Pretende integrar a sua conta na " + organizacao.Object.NomeExibicao)));
                            if (confirmacao)
                            {
                                Utilizador.UidGrupoPertencente = organizacao.Object.Uid;
                                organizacao.Object.KeysUtilizadoresParticulares.Add(keyUtilizador);
                                await FBDataBase.UtilizadorDS.Update(organizacao.Object, organizacao.Key);
                                await FBDataBase.UtilizadorDS.Update(Utilizador, keyUtilizador);
                            }
                        }
                    }
                    else
                        await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Erro", "O código inserido não é válido", "Ok"));
                }
            }
            catch
            {
            }
        });

    }
}
