using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media.Imaging;

namespace RyuuseiManager.ImageGenerator
{
    public class DamageTagGenerator
    {
        [DllImport("gdi32.dll")]
        private static extern bool DeleteObject(IntPtr hObject);

        private static BitmapSource ToBitmapSource(Bitmap bitmap)
        {
            IntPtr hBitmap = bitmap.GetHbitmap();

            try
            {
                return System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(
                    hBitmap,
                    IntPtr.Zero,
                    Int32Rect.Empty,
                    BitmapSizeOptions.FromEmptyOptions());
            }
            finally
            {
                DeleteObject(hBitmap);
            }
        }

        private static Bitmap LoadBitmap(string uriPath)
        {
            var uri = new Uri(uriPath, UriKind.Absolute);
            var streamInfo = Application.GetResourceStream(uri);

            if (streamInfo == null) return new Bitmap(1, 1);

            using var stream = streamInfo.Stream;
            Bitmap bmp = new Bitmap(stream);
            bmp.SetResolution(96, 96);
            return bmp;

        }

        public static BitmapSource GetDamageTag(int damage, int tier)
        {
            if (damage > 999) return null;
            Bitmap tierBmp;
            switch (tier)
            {
                default:
                case 0: tierBmp = LoadBitmap("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/Standard.png"); break;
                case 1: tierBmp = LoadBitmap("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/Mega.png"); break;
                case 2: tierBmp = LoadBitmap("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/Giga.png"); break;
                case 3: tierBmp = LoadBitmap("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/White.png"); break;
            }
            if (damage > 0)
            {
                Bitmap damageNumber = new Bitmap(23, 11);
                using (Graphics g = Graphics.FromImage(damageNumber))
                {
                    string num = damage.ToString().PadLeft(3, ' ');
                    for (int i = 0; i < num.Length; i++)
                    {
                        DrawNumber(g, num[i], i);
                    }
                }
                using (Graphics g = Graphics.FromImage(tierBmp))
                {
                    g.DrawImage(damageNumber, 8, 2, new Rectangle(0, 0, 23, 11), GraphicsUnit.Pixel);
                }
                return ToBitmapSource(tierBmp);
            }
            else if (damage < 0)
            {
                Bitmap damageNumber = new Bitmap(23, 11);
                using (Graphics g = Graphics.FromImage(damageNumber))
                {
                    string num = "???";
                    for (int i = 0; i < num.Length; i++)
                    {
                        DrawNumber(g, num[i], i);
                    }
                }
                using (Graphics g = Graphics.FromImage(tierBmp))
                {
                    g.DrawImage(damageNumber, 8, 2, new Rectangle(0, 0, 23, 11), GraphicsUnit.Pixel);
                }
                return ToBitmapSource(tierBmp);
            }
            else
            {
                return ToBitmapSource(new Bitmap(1, 1));
            }
        }

        public static BitmapSource GetElementTag(int elements, bool isRegCard = false)
        {
            string num = elements.ToString();
            int width = 14 * num.Length;
            if (isRegCard) width += 42;
            Bitmap elementTags = new Bitmap(width, 15);
            using (Graphics g = Graphics.FromImage(elementTags))
            {
                for (int i = 0; i < num.Length; i++)
                {
                    DrawElement(g, num[i], i);
                }
                if (isRegCard)
                {
                    Elements elem = new Elements();
                    g.DrawImage(elem.Reg, width - elem.Reg.Width, 0, new Rectangle(0, 0, elem.Reg.Width, elem.Reg.Height), GraphicsUnit.Pixel);
                }
            }
            return ToBitmapSource(elementTags);
        }

        private static void DrawNumber(Graphics g, char number, int location)
        {
            int x = location * 8;
            DamageNumber dmgNum = new DamageNumber();
            Bitmap targetNum;
            switch (number)
            {
                default: targetNum = dmgNum._q; break;
                case ' ': return;
                case '0': targetNum = dmgNum._0; break;
                case '1': targetNum = dmgNum._1; break;
                case '2': targetNum = dmgNum._2; break;
                case '3': targetNum = dmgNum._3; break;
                case '4': targetNum = dmgNum._4; break;
                case '5': targetNum = dmgNum._5; break;
                case '6': targetNum = dmgNum._6; break;
                case '7': targetNum = dmgNum._7; break;
                case '8': targetNum = dmgNum._8; break;
                case '9': targetNum = dmgNum._9; break;
            }
            g.DrawImage(targetNum, x, 0, new Rectangle(0, 0, targetNum.Width, targetNum.Height), GraphicsUnit.Pixel);
        }

        private static void DrawElement(Graphics g, char number, int location)
        {
            int x = location * 14;
            Elements elem = new Elements();
            Bitmap targetElem;
            switch (number)
            {
                default: return;
                case '1': targetElem = elem.Null; break;
                case '2': targetElem = elem.Aqua; break;
                case '3': targetElem = elem.Heat; break;
                case '4': targetElem = elem.Elec; break;
                case '5': targetElem = elem.Wood; break;
                case '6': targetElem = elem.Wind; break;
                case '7': targetElem = elem.Break; break;
                case '8': targetElem = elem.Sword; break;
            }
            g.DrawImage(targetElem, x, 0, new Rectangle(0, 0, targetElem.Width, targetElem.Height), GraphicsUnit.Pixel);
        }

        private class DamageNumber
        {
            public DamageNumber()
            {
                _0 = LoadBitmap("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/0.png");
                _1 = LoadBitmap("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/1.png");
                _2 = LoadBitmap("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/2.png");
                _3 = LoadBitmap("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/3.png");
                _4 = LoadBitmap("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/4.png");
                _5 = LoadBitmap("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/5.png");
                _6 = LoadBitmap("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/6.png");
                _7 = LoadBitmap("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/7.png");
                _8 = LoadBitmap("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/8.png");
                _9 = LoadBitmap("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/9.png");
                _q = LoadBitmap("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/q.png");
            }

            public Bitmap _0 { get; private set; }
            public Bitmap _1 { get; private set; }
            public Bitmap _2 { get; private set; }
            public Bitmap _3 { get; private set; }
            public Bitmap _4 { get; private set; }
            public Bitmap _5 { get; private set; }
            public Bitmap _6 { get; private set; }
            public Bitmap _7 { get; private set; }
            public Bitmap _8 { get; private set; }
            public Bitmap _9 { get; private set; }
            public Bitmap _q { get; private set; }
        }

        private class Elements
        {
            public Elements()
            {
                Null = LoadBitmap("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/Null.png");
                Aqua = LoadBitmap("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/Aqua.png");
                Heat = LoadBitmap("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/Heat.png");
                Elec = LoadBitmap("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/Elec.png");
                Wood = LoadBitmap("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/Wood.png");
                Wind = LoadBitmap("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/Wind.png");
                Break = LoadBitmap("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/Break.png");
                Sword = LoadBitmap("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/Sword.png");
                Reg = LoadBitmap("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/RegCard.png");
            }
            public Bitmap Null { get; private set; }
            public Bitmap Aqua { get; private set; }
            public Bitmap Heat { get; private set; }
            public Bitmap Elec { get; private set; }
            public Bitmap Wood { get; private set; }
            public Bitmap Wind { get; private set; }
            public Bitmap Break { get; private set; }
            public Bitmap Sword { get; private set; }
            public Bitmap Reg { get; private set; }
        }
    }
}
