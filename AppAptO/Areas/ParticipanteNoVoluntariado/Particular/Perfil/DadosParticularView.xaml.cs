using AppAptO.Models.FBData.Utilizadores;
using AppAptO.ViewModels.Conta;
using System.Collections.Specialized;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.PartialViews.Conta
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DadosParticularView : ContentView
    {
        DadosParticularViewModel viewModel;
        public DadosParticularView(UtilizadorParticular utilizador)
        {
            InitializeComponent();
            viewModel = new DadosParticularViewModel(utilizador);
            BindingContext = viewModel;
            viewModel.Aptidoes.CollectionChanged += Aptidoes_CollectionChanged;
        }

        void Aptidoes_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                stackAptidoes.Children.Add
                   (
                        new Label()
                        {
                            Text = " - " + e.NewItems[0].ToString()
                        }
                   );
            }
        }
    }
}