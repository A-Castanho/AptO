using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Areas.Geral
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PublicidadeObj : Frame
    {
        public PublicidadeObj()
        {
            InitializeComponent();
        }
        //public async void DefinirPublicidade(Publicidade.TipoPublicidade tipo)
        //{
        //    Publicidade = await Services.Firebase.FBDataBase.PublicidadeDS.GetRndPublicidade(tipo);
        //    imagem.Source = Publicidade.Object.PathImagem;
        //}

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            try
            {
                OpenLink();
            }
            catch { }
        }

        private async void OpenLink()
        {
        }
    }
}