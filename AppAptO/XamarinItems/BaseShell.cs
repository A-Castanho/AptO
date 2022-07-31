using Xamarin.Forms;

namespace AppAptO.XamarinItems
{
    public class BaseShell : Shell
    {
        protected override bool OnBackButtonPressed()
        {
            if (Navigation.NavigationStack.Count == 1 && !FlyoutIsPresented)
            {
                FlyoutIsPresented = true;
                return true;
            }
            else
                return base.OnBackButtonPressed();
        }
    }
}
