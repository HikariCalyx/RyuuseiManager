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

        public static BitmapImage GetTeamIcon(int index)
        {
            if (index < 0 || index > 9) index = 0;
            string uri = $"pack://application:,,,/GameResource;component/Resources/TeamIcon/{index}.png";

            return new BitmapImage(new Uri(uri, UriKind.Absolute));
        }

        public static BitmapImage GetSF3CardImage(int index)
        {
            if (index < 1 || index > 1339 || (index > 397 && index < 1285) || index == 1296) index = 1;
            string fileName = $"{index:D4}.png";
            string uri = $"pack://application:,,,/GameResource;component/Resources/BattleCard/SF3/{fileName}";

            return new BitmapImage(new Uri(uri, UriKind.Absolute));
        }
    }
}
