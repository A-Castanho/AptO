using AppAptO.Models.DadosAplicacao;
using AppAptO.Models.FBConnections;
using AppAptO.PopUps;
using AppAptO.Services.Firebase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Extensions;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AppAptO.Areas.Admin.AreaAdmin.Publicidades
{
    internal class CrudPublicidadeViewModel : ObservableObject
    {
        Publicidade publicidadeOriginal;
        private List<string> tipos = new List<string>();
        public List<string> Tipos
        {
            get { return tipos; }
            set { SetProperty(ref tipos, value); }
        }
        private bool visibilidadeEliminar = false;
        public bool VisibilidadeEliminar
        {
            get { return visibilidadeEliminar; }
            set { SetProperty(ref visibilidadeEliminar, value); }
        }
        private bool visibilidadeParams = true;
        public bool VisibilidadeParams
        {
            get { return visibilidadeParams; }
            set { SetProperty(ref visibilidadeParams, value); }
        }
        private int imageHeight = 4;
        public int ImageHeight
        {
            get { return imageHeight; }
            set { SetProperty(ref imageHeight, value); }
        }
        private int imageWidth = 4;
        public int ImageWidth
        {
            get { return imageWidth; }
            set { SetProperty(ref imageWidth, value); }
        }

        Stream FotoPerfilStream;
        private string tipo;
        public string Tipo
        {
            get { return tipo; }
            set
            {
                SetProperty(ref tipo, value);
                if (value == "Horizontal")
                {
                    ImageHeight = 60;
                    ImageWidth = (int)Application.Current.MainPage.Width;
                }
                else
                {
                    ImageHeight = (int)Application.Current.MainPage.Height;
                    ImageWidth = 60;
                }
            }
        }
        private int nivelPrioridade;
        public int NivelPrioridade
        {
            get { return nivelPrioridade; }
            set { SetProperty(ref nivelPrioridade, value); }
        }
        private string fotoSource;
        public string FotoSource
        {
            get { return fotoSource; }
            set { SetProperty(ref fotoSource, value); }
        }
        private string linkPublicidade;
        public string LinkPublicidade
        {
            get { return linkPublicidade; }
            set { SetProperty(ref linkPublicidade, value); }
        }
        private string nomeEmpresa;
        public string NomeEmpresa
        {
            get { return nomeEmpresa; }
            set { SetProperty(ref nomeEmpresa, value); }
        }
        private string tituloPublicidade;
        private string publicidadeKey;

        public string TituloPublicidade
        {
            get { return tituloPublicidade; }
            set { SetProperty(ref tituloPublicidade, value); }
        }

        public ICommand CommandAdicionarImagem => new Command(
        async () =>
        {
            try
            {
                FileResult resultado = await MediaPicker.PickPhotoAsync(new MediaPickerOptions
                {
                    Title = "Selecione uma imagem"
                });
                FotoPerfilStream = await resultado.OpenReadAsync();
                FotoSource = resultado.FullPath;
            }
            catch { }
        });

        public CrudPublicidadeViewModel(Publicidade publicidade, string key)
        {
            DefinirTipos();
            if (!string.IsNullOrEmpty(key))
            {
                visibilidadeEliminar = true;
                publicidadeOriginal = publicidade;
                FotoSource = publicidade.PathImagem;
                LinkPublicidade = publicidade.Redirecionamento;
                Tipo = publicidade.Tipo.ToString();
                TituloPublicidade = publicidade.Nome;
                NomeEmpresa = publicidade.Empresa;
                NivelPrioridade = publicidade.NivelPrioridade;
                this.publicidadeKey = key;
            }
            else
                Application.Current.MainPage.ShowPopup(new MensagemPopUp("Erro", "Não foi possível carregar a publicidade"));
        }

        public CrudPublicidadeViewModel()
        {
            Tipo = "Horizontal";
            DefinirTipos();
        }

        public ICommand CommandSubmeter => new Command(
        async () =>
        {
            try
            {
                Publicidade publicidade = new Publicidade()
                {
                    DataAdicao = DateTime.Now,
                    Empresa = NomeEmpresa,
                    Redirecionamento = linkPublicidade,
                    Nome = TituloPublicidade,
                    NivelPrioridade = NivelPrioridade,
                    Tipo = (Publicidade.TipoPublicidade)Enum.Parse(typeof(Publicidade.TipoPublicidade), Tipo)
                };
                if (string.IsNullOrEmpty(publicidadeKey))
                    await InserirPublicidade(publicidade);
                else
                    await AtualizarPublicidade(publicidade);
            }
            catch
            {

                await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Erro", "Foi impossível realizar a operação."));
            }
        });

        public ICommand CommandMudarVisParams => new Command(
        () =>
        {
            VisibilidadeParams = !VisibilidadeParams;
        });

        private async Task InserirPublicidade(Publicidade publicidade)
        {
            publicidadeKey = await FBDataBase.PublicidadeDS.Add(publicidade);
            try
            {
                var urlImagem = await StorageHelper.AdicionarStreamImagemPublicidadeAsync(FotoPerfilStream, publicidadeKey);
                publicidade.PathImagem = urlImagem;
                await FBDataBase.PublicidadeDS.Update(publicidade, publicidadeKey); ;
                await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Sucesso!", "A publicidade foi inserida na base de dados"));
                try
                {
                    await Shell.AdminShell.Current.Navigation.PopAsync();
                }
                catch { }
            }
            catch
            {
                await FBDataBase.PublicidadeDS.DeleteByKey(publicidadeKey);
                publicidadeKey = "";
                await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Erro", "Foi impossível inserir a publicidade."));
            }
        }
        private async Task AtualizarPublicidade(Publicidade publicidade)
        {
            try
            {
                if (!string.IsNullOrEmpty(FotoSource) && FotoSource != publicidadeOriginal.PathImagem)
                {
                    var urlImagem = await StorageHelper.AdicionarStreamImagemPublicidadeAsync(FotoPerfilStream, publicidadeKey);
                    publicidade.PathImagem = urlImagem;
                }
                await FBDataBase.PublicidadeDS.Update(publicidade, publicidadeKey);
                await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Sucesso!", "A publicidade foi atualizada na base de dados"));
            }
            catch
            {
                await FBDataBase.PublicidadeDS.DeleteByKey(publicidadeKey);
                await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Erro", "Foi impossível realizar a operação"));
            }
        }

        public ICommand CommandEliminar => new Command(
        async () =>
        {
            try
            {
                await FBDataBase.PublicidadeDS.DeleteByKey(publicidadeKey);
                await Shell.AdminShell.Current.Navigation.PopAsync();
            }
            catch
            {

                await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Erro", "Foi impossível eliminar a publicidade."));
            }
            try
            {

                await StorageHelper.EliminarStreamPublicidadeAsync(publicidadeKey);
            }
            catch
            {
                await Application.Current.MainPage.ShowPopupAsync(new MensagemPopUp("Erro", "Foi impossível remover a imagem da publicidade."));

            }
        });
        private void DefinirTipos()
        {
            Tipos.AddRange(Enum.GetNames(typeof(Publicidade.TipoPublicidade)));
        }
    }
}
