using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RyuuseiManager.ImageGenerator
{
    public static class DamageTagGenerator
    {
        private static readonly DamageNumberAtlas damageAtlas = new DamageNumberAtlas();
        private static readonly ElementAtlas elementAtlas = new ElementAtlas();

        private static BitmapSource LoadBitmapSource(string uriPath)
        {
            var uri = new Uri(uriPath, UriKind.Absolute);
            var streamInfo = Application.GetResourceStream(uri);
            if (streamInfo == null)
                return new RenderTargetBitmap(1, 1, 96, 96, PixelFormats.Pbgra32);

            using var stream = streamInfo.Stream;
            var bmp = new BitmapImage();
            bmp.BeginInit();
            bmp.StreamSource = stream;
            bmp.CacheOption = BitmapCacheOption.OnLoad;
            bmp.CreateOptions = BitmapCreateOptions.PreservePixelFormat;
            bmp.EndInit();
            bmp.Freeze();
            return bmp;
        }

        private static RenderTargetBitmap CreateRTB(int width, int height)
        {
            return new RenderTargetBitmap(width, height, 96, 96, PixelFormats.Pbgra32);
        }

        public static BitmapSource GetDamageTag(int damage, int tier)
        {
            if (damage > 999)
                return CreateRTB(1, 1);

            BitmapSource tierBmp = tier switch
            {
                1 => LoadBitmapSource("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/Mega.png"),
                2 => LoadBitmapSource("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/Giga.png"),
                3 => LoadBitmapSource("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/White.png"),
                _ => LoadBitmapSource("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/Standard.png")
            };

            if (damage == 0)
                return CreateRTB(1, 1);

            string num = damage > 0
                ? damage.ToString().PadLeft(3, ' ')
                : "???";

            int w = tierBmp.PixelWidth;
            int h = tierBmp.PixelHeight;

            var dv = new DrawingVisual();
            using (var dc = dv.RenderOpen())
            {
                // background
                dc.DrawImage(tierBmp, new Rect(0, 0, w, h));

                // numbers at (8,2)
                double baseX = 8;
                double baseY = 2;

                for (int i = 0; i < num.Length; i++)
                    DrawNumber(dc, num[i], i, baseX, baseY);
            }

            var rtb = CreateRTB(w, h);
            rtb.Render(dv);
            rtb.Freeze();
            return rtb;
        }

        public static BitmapSource GetElementTag(int elements, bool isRegCard = false, bool isTagCard = false)
        {
            string num = elements.ToString();
            int width = 14 * num.Length + (isRegCard ? 42 : 0) + (isTagCard ? 42 : 0);
            int height = 15;

            var dv = new DrawingVisual();
            using (var dc = dv.RenderOpen())
            {
                // draw element icons
                for (int i = 0; i < num.Length; i++)
                    DrawElement(dc, num[i], i);

                // draw Reg icon
                if (isRegCard)
                {
                    var reg = elementAtlas.Reg;
                    double x = width - reg.PixelWidth - (isTagCard ? reg.PixelWidth : 0);
                    dc.DrawImage(reg, new Rect(x, 0, reg.PixelWidth, reg.PixelHeight));
                }

                // draw Tag icon
                if (isTagCard)
                {
                    var tag = elementAtlas.Tag;
                    double x = width - tag.PixelWidth;
                    dc.DrawImage(tag, new Rect(x, 0, tag.PixelWidth, tag.PixelHeight));
                }
            }

            var rtb = CreateRTB(width, height);
            rtb.Render(dv);
            rtb.Freeze();
            return rtb;
        }

        private static void DrawNumber(DrawingContext dc, char number, int index, double offsetX, double offsetY)
        {
            BitmapSource? src = number switch
            {
                '0' => damageAtlas._0,
                '1' => damageAtlas._1,
                '2' => damageAtlas._2,
                '3' => damageAtlas._3,
                '4' => damageAtlas._4,
                '5' => damageAtlas._5,
                '6' => damageAtlas._6,
                '7' => damageAtlas._7,
                '8' => damageAtlas._8,
                '9' => damageAtlas._9,
                ' ' => null,
                _ => damageAtlas._q
            };

            if (src == null)
                return;

            double x = offsetX + index * 8;
            dc.DrawImage(src, new Rect(x, offsetY, src.PixelWidth, src.PixelHeight));
        }

        private static void DrawElement(DrawingContext dc, char number, int index)
        {
            BitmapSource? src = number switch
            {
                '1' => elementAtlas.Null,
                '2' => elementAtlas.Aqua,
                '3' => elementAtlas.Heat,
                '4' => elementAtlas.Elec,
                '5' => elementAtlas.Wood,
                '6' => elementAtlas.Wind,
                '7' => elementAtlas.Break,
                '8' => elementAtlas.Sword,
                _ => null
            };

            if (src == null)
                return;

            double x = index * 14;
            dc.DrawImage(src, new Rect(x, 0, src.PixelWidth, src.PixelHeight));
        }

        private class DamageNumberAtlas
        {
            public BitmapSource _0 { get; } = LoadBitmapSource("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/0.png");
            public BitmapSource _1 { get; } = LoadBitmapSource("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/1.png");
            public BitmapSource _2 { get; } = LoadBitmapSource("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/2.png");
            public BitmapSource _3 { get; } = LoadBitmapSource("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/3.png");
            public BitmapSource _4 { get; } = LoadBitmapSource("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/4.png");
            public BitmapSource _5 { get; } = LoadBitmapSource("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/5.png");
            public BitmapSource _6 { get; } = LoadBitmapSource("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/6.png");
            public BitmapSource _7 { get; } = LoadBitmapSource("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/7.png");
            public BitmapSource _8 { get; } = LoadBitmapSource("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/8.png");
            public BitmapSource _9 { get; } = LoadBitmapSource("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/9.png");
            public BitmapSource _q { get; } = LoadBitmapSource("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/q.png");
        }

        private class ElementAtlas
        {
            public BitmapSource Null { get; } = LoadBitmapSource("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/Null.png");
            public BitmapSource Aqua { get; } = LoadBitmapSource("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/Aqua.png");
            public BitmapSource Heat { get; } = LoadBitmapSource("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/Heat.png");
            public BitmapSource Elec { get; } = LoadBitmapSource("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/Elec.png");
            public BitmapSource Wood { get; } = LoadBitmapSource("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/Wood.png");
            public BitmapSource Wind { get; } = LoadBitmapSource("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/Wind.png");
            public BitmapSource Break { get; } = LoadBitmapSource("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/Break.png");
            public BitmapSource Sword { get; } = LoadBitmapSource("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/Sword.png");
            public BitmapSource Reg { get; } = LoadBitmapSource("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/RegCard.png");
            public BitmapSource Tag { get; } = LoadBitmapSource("pack://application:,,,/GameResource;component/Resources/BattleCard/Common/TagCard.png");
        }
    }
}
