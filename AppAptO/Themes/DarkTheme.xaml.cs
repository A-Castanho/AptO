
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Themes
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DarkTheme : ResourceDictionary
    {
        public DarkTheme()
        {
            InitializeComponent();
            var color = (Color)Application.Current.Resources["PrimaryDark"];
            //PrimaryColor = (Color)Application.Current.Resources["PrimaryDark"];
            //PrimaryInverseColor = (Color)Application.Current.Resources["Primary"];
            //PrimaryLightestColor = (Color)Application.Current.Resources["PrimaryDark"];
            //PrimaryInverseColor = (Color)Application.Current.Resources["Primary"];

        }
    }
}