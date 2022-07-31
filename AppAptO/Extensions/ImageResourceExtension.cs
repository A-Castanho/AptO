using System;
using System.Reflection;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AppAptO.Models.AppHelpers
{
    [ContentProperty(nameof(Source))]
    public class ImageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
            {
                return null;
            }

            string imagesFolderName = "Imagens";
            string imagePath = $"{App.Current.GetType().Namespace}.{imagesFolderName}.{Source}";

            // Do your translation lookup here, using whatever method you require
            ImageSource imageSource = ImageSource.FromResource(imagePath, typeof(ImageResourceExtension).GetTypeInfo().Assembly);

            return imageSource;
        }
    }
}
