using System.Windows.Media.Imaging;

namespace RyuuseiManager
{
    internal class GameResourceRetriver
    {
        public static BitmapImage GetMugshot(int index)
        {
            if (index < 0 || index > 348) index = 226;
            string fileName = $"{index:D3}.png";
            string uri = $"pack://application:,,,/GameResource;component/Resources/Mugshots/{fileName}";

            return new BitmapImage(new Uri(uri, UriKind.Absolute));
        }
    }
}
