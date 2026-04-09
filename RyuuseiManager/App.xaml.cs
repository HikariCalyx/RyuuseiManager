using System.Configuration;
using System.Data;
using System.Windows;

namespace RyuuseiManager
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static void SetLanguage(string cultureName)
        {
            var dict = new ResourceDictionary
            {
                Source = new Uri($"Lang/{cultureName}.xaml", UriKind.Relative)
            };

            Current.Resources.MergedDictionaries[0] = dict;
        }
    }


}
