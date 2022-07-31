using AppAptO.Models.AppHelpers;
using AppAptO.Models.FBConnections;
using AppAptO.Models.FBData.Utilizadores;
using AppAptO.PopUps;
using AppAptO.PopUps.Select;
using AppAptO.Services.Firebase;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.IO;
using System.Linq;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace AppAptO.ViewModels.Conta
{
    public class AutenticacaoViewModel : ObservableObject
    {
        #region VisibilidadePaginas
        private bool isLoginVisible = true;
        public bool IsLoginVisible
        {
            get => isLoginVisible;
            set { isLoginVisible = value; OnPropertyChanged(); }
        }
        private bool isRegistoVisible;
        public bool IsRegistoVisible
        {
            get => isRegistoVisible;
            set { isRegistoVisible = value; OnPropertyChanged(); }
        }
        private bool isRecuperarVisible;
        public bool IsRecuperarVisible
        {
            get => isRecuperarVisible;
            set { isRecuperarVisible = value; OnPropertyChanged(); }
        }
        #endregion

        public bool Validated { get; set; }
        #region Campos Perfil
        private Stream FotoPerfilStream { get; set; }
        private Utilizador utilizador = new Utilizador();
        public Utilizador Utilizador
        {
            get => utilizador; set
            {
                SetProperty(ref utilizador, value);
            }
        }
        private string telemovel;
        public string Telemovel
        {
            get => telemovel; set
            {
                SetProperty(ref telemovel, value);
            }
        }
        private string linkWebsite;
        public string LinkWebsite
        {
            get => linkWebsite; set
            {
                SetProperty(ref linkWebsite, value);
            }
        }
        private string primeiroNome;
        private string ultimoNome;
        public string PrimeiroNome
        {
            get => primeiroNome; set
            {
                SetProperty(ref primeiroNome, value);
            }
        }
        public string UltimoNome
        {
            get => ultimoNome; set
            {
                SetProperty(ref ultimoNome, value);
            }
        }

        public bool isParticular;
        public bool IsParticular
        {
            get => isParticular;
            set
            {
                isParticular = value;
                if (value)
                {
                    IsOrganizacao = false;
                }
                Utilizador.IsOrganizacao = !isParticular;
                OnPropertyChanged();
            }
        }
        public bool isOrganizacao;
        public bool IsOrganizacao
        {
            get => isOrganizacao;
            set
            {
                isOrganizacao = value;
                if (value)
                {
                    IsParticular = false;
                }
                Utilizador.IsOrganizacao = isOrganizacao;
                OnPropertyChanged();
            }
        }
        private string localidade;
        public string Localidade
        {
            get => localidade;
            set
            {
                SetProperty(ref localidade, value);
            }
        }
        public bool activityIndicatorRunning;
        public bool ActivityIndicatorRunning
        {
            get => activityIndicatorRunning;
            set
            {
                SetProperty(ref activityIndicatorRunning, value);
            }
        }
        public bool isApoiado;
        public bool IsApoiado
        {
            get => isApoiado;
            set
            {
                isApoiado = value;
                if (value)
                {
                    IsVoluntario = false;
                }
                Utilizador.IsVoluntario = IsVoluntario;
                OnPropertyChanged();
            }
        }
        public bool isVoluntario;
        public bool IsVoluntario
        {
            get => isVoluntario;
            set
            {
                isVoluntario = value;
                if (value)
                {
                    IsApoiado = false;
                }
                Utilizador.IsApoiado = IsApoiado;
                OnPropertyChanged();
            }
        }
        private string fotoSouce = "Imagens/user.png";
        public string FotoSource
        {
            get => fotoSouce;
            set { fotoSouce = value; OnPropertyChanged(); }
        }
        private string password;
        public string Password
        {
            get => password; set
            {
                SetProperty(ref password, value);
            }
        }
        private string confirmPassword;
        public string ConfirmPassword
        {
            get => confirmPassword; set
            {
                SetProperty(ref confirmPassword, value);
            }
        }
        #endregion
        public ICommand CommandLogin => new Command(async () =>
        {
            ActivityIndicatorRunning = true;
            if (!string.IsNullOrWhiteSpace(Utilizador.Email) && !string.IsNullOrWhiteSpace(Password))
            {
                try
                {
                    string utilizadorKey = await AuthHelper.IniciarSessao(Utilizador.Email, Password);
                    if (!string.IsNullOrEmpty(utilizadorKey))
                    {
                        await App.Iniciar();
                        ActivityIndicatorRunning = false;
                    }
                    else
                    {
                        ActivityIndicatorRunning = false;
                        await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Autenticação falhou", "Os dados inseridos estão incorretos", "Ok"));
                    }
                }
                catch
                {
                    ActivityIndicatorRunning = false;
                    await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Erro", "Ocorreu um erro ao tentar iniciar sessão", "Ok"));
                }
            }
            else
            {
                ActivityIndicatorRunning = false;
                await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Erro", "Todos os campos devem ser preenchidos", "Ok"));
            }
        });
        public ICommand CommandRegisto => new Command(
        async () =>
        {
            ActivityIndicatorRunning = true;
            if (AppConnection.IsConnected)
            {
                try
                {
                    if (Validate())
                    {
                        if (ConfirmPassword != Password)
                        {
                            ActivityIndicatorRunning = false;
                            await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Erro", "As passwords não correspondem", "Ok"));
                        }
                        else
                        {
                            bool registoSucedido;
                            Utilizador.Localidade = Localidade;
                            if (!await AuthHelper.IsEmailUsed(Utilizador.Email))
                            {
                                if (IsOrganizacao)
                                {
                                    UtilizadorOrganizacao utilizadorFinal = new UtilizadorOrganizacao(Utilizador, LinkWebsite);
                                    await AuthHelper.Registar(utilizadorFinal, Password, FotoPerfilStream);
                                    registoSucedido = true;
                                }
                                else
                                {
                                    UtilizadorParticular utilizadorFinal = new UtilizadorParticular(
                                        utilizador: Utilizador,
                                        primeiroNome: PrimeiroNome,
                                        ultimoNome: UltimoNome, telemovel: Telemovel);
                                    await AuthHelper.Registar(utilizadorFinal, Password, FotoPerfilStream);
                                    registoSucedido = true;
                                }
                                if (registoSucedido)
                                {
                                    ActivityIndicatorRunning = false;
                                    await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Sucesso", "Nova Conta Criada", "Ok"));
                                    Password = "";
                                    Utilizador = new Utilizador();
                                    ComandoOpen.Execute("login");
                                }
                                else
                                {
                                    ActivityIndicatorRunning = false;
                                    await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Erro", "Ocorreu um erro não identificado ao criar a sua conta", "Ok"));
                                }
                            }
                            else
                            {
                                ActivityIndicatorRunning = false;
                                await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Erro", "O email inserido já está associado a uma conta", "Ok"));
                            }
                        }
                    }
                    else
                    {
                        ActivityIndicatorRunning = false;
                        await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Erro", "Todos os campos obrigatórios devem ser preenchidos", "Ok"));
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("\n\n\n\n\n\n\n\n\n\n\n\n" +
                        "Error data: \n" + e.Data +
                        "\nMessage: \n" + e.Message +
                        "\nStackTrace: \n" + e.StackTrace +
                        "\nSource: \n" + e.Source +
                        "\nInnerException: \n" + e.InnerException +
                        "\nTargetSite: \n" + e.TargetSite
                        );
                    ActivityIndicatorRunning = false;
                    await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Erro", "Ocorreu um erro não identificado", "Ok"));
                }
            }
            else
            {
                ActivityIndicatorRunning = false;
                await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Erro", "A operação foi cancelada devido à falta de conexão na internet", "Ok"));
            }
        });

        private bool Validate()
        {
            if (IsParticular)
            {
                return !(string.IsNullOrEmpty(Utilizador.Email)
                    && string.IsNullOrEmpty(Localidade)
                    && string.IsNullOrEmpty(PrimeiroNome)
                    && string.IsNullOrEmpty(UltimoNome));
            }
            else
            {
                return !(string.IsNullOrEmpty(Utilizador.Email)
                    && string.IsNullOrEmpty(Utilizador.Telefone)
                    && string.IsNullOrEmpty(Utilizador.NomeExibicao)
                    && string.IsNullOrEmpty(Localidade)
                    && string.IsNullOrEmpty(Utilizador.NomeExibicao));
            }
        }

        public ICommand CommandOpenSelectLocalidade => new Command(
        async () =>
        {
            try
            {
                var localidadesNomes = (await FBDataBase.LocalidadeDS.GetAllAsync())
                                        .Select(localidade => localidade.Object.Nome)
                                        .Where(localidade => localidade.Contains(","));
                Localidade = await Application.Current.MainPage.ShowPopupAsync(new SelectAndSearchPopUp(localidadesNomes));
            }
            catch { }
        });
        public ICommand CommandRecuperarSenha => new Command(
        async () =>
        {
            var emailEnviado = await AuthHelper.EnviarRecuperacaoPassword(Utilizador.Email);
            if (emailEnviado)
                await Application.Current.MainPage.Navigation.ShowPopupAsync(new MensagemPopUp("Sucesso!", "Verifique o seu email para recuperar a password.", "Ok"));
            else
                await Application.Current.MainPage.Navigation.ShowPopupAsync(new MensagemPopUp("Erro", "Não foi possível enviar o email de recuperação.", "Ok"));
        });
        public ICommand ComandoAdicionarImagem => new Command(
        async () =>
        {
            try
            {
                if (CrossMedia.Current.IsPickPhotoSupported)
                {
                    MediaFile resultado = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions() { CompressionQuality = 50, PhotoSize = PhotoSize.MaxWidthHeight, MaxWidthHeight = 600 });
                    FotoPerfilStream = resultado.GetStream();
                    FotoSource = resultado.Path;
                }
            }
            catch { }
        });
        public ICommand ComandoOpen => new Command<string>(
        (string paginaVisivel) =>
        {
            IsLoginVisible = paginaVisivel == "login";
            IsRegistoVisible = paginaVisivel == "registo";
            IsRecuperarVisible = paginaVisivel == "recuperar";
        });

        public AutenticacaoViewModel()
        {
            IsParticular = true;
            IsApoiado = true;
            FotoSource = StorageHelper.UrlImgUtilizadorIndefinido;
        }

    }
}

