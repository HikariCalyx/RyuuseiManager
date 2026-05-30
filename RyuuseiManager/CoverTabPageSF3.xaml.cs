using RyuuseiManager.Library.SF3;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RyuuseiManager
{
    /// <summary>
    /// CoverTabPageSF3.xaml 的交互逻辑
    /// </summary>
    public partial class CoverTabPageSF3 : Page
    {
        public CoverTabPageSF3()
        {
            InitializeComponent();
            GameVersion = 0;
            ProfileLanguage = 0;
            ScalingType = GetDpiScaling();
        }

        public int GameVersion { get; set; }
        public int ProfileLanguage { get; set; }

        private string ScalingType;

        private string GetDpiScaling()
        {
            DpiScale dpi = VisualTreeHelper.GetDpi(this);
            if (dpi.DpiScaleX == 1.0)
            {
                return "NearestNeighbor";
            }
            else
            {
                return "HighQuality";
            }
        }

        public static readonly DependencyProperty isp1 =
        DependencyProperty.Register(
            nameof(ImageSourceMugshot),
            typeof(ImageSource),
            typeof(CoverTabPageSF3),
            new PropertyMetadata(null));

        public ImageSource ImageSourceMugshot
        {
            get => (ImageSource)GetValue(isp1);
            set => SetValue(isp1, value);
        }

        public static readonly DependencyProperty isp2 =
        DependencyProperty.Register(
            nameof(ImageSourceTeamIcon),
            typeof(ImageSource),
            typeof(CoverTabPageSF3),
            new PropertyMetadata(null));

        public ImageSource ImageSourceTeamIcon
        {
            get => (ImageSource)GetValue(isp2);
            set => SetValue(isp2, value);
        }

        public void SetMessage(string message)
        {
            Message.Text = message;
        }

        public void SetSecret(string teamName)
        {
            TeamName.Text = teamName;
        }

        public void SetPurpose(string purpose)
        {
            TeamPurpose.Text = purpose;
        }

        public void SetNoiseForm(string noiseForm)
        {
            NoiseForm.Text = noiseForm;
        }

        public void SetProfileColor()
        {
            switch (GameVersion)
            {
                case 0:
                    ProfileRow.Background = FromArgb("#FF6A6A6A");
                    break;
                case 1:
                    ProfileRow.Background = FromArgb("#FFD60D00");
                    break;
            }
        }

        public void SetAbilities(List<int> abilityList)
        {
            AbilityList.Items.Clear();
            int totalAbilityCost = 0;
            foreach (var i in abilityList)
            {
                var ability = Abilities.GetAbility(i);
                totalAbilityCost += ability.Cost;
                ListEntry entry = new ListEntry()
                {
                    Label = AbilityName.GetAbilityName(ability.Type, ProfileLanguage, GameVersion),
                    Quantity = ability.Cost.ToString()
                };
                AbilityList.Items.Add(entry);
            }
            TotalCost.Content = totalAbilityCost;
        }

        private Brush FromArgb(string argb)
        {
            Color color;
            try
            {
                color = (Color)ColorConverter.ConvertFromString(argb);
            }
            catch
            {
                color = (Color)ColorConverter.ConvertFromString("#FF000000");
            }
            Brush brush = new SolidColorBrush(color);
            return brush;
        }

        public class ListEntry
        {
            public string Label { get; set; }
            public string SecondaryLabel { get; set; }
            public string Quantity { get; set; }
        }
    }
}
