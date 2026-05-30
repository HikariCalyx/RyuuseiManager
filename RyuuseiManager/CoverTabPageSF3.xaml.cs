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
        }

        public int GameVersion { get; set; }
        public int ProfileLanguage { get; set; }

        public static readonly DependencyProperty ImageSourceProperty =
        DependencyProperty.Register(
            nameof(ImageSource),
            typeof(ImageSource),
            typeof(CoverTabPageSF3),
            new PropertyMetadata(null));

        public ImageSource ImageSource
        {
            get => (ImageSource)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        public void SetMessage(string message)
        {
            Message.Text = message;
        }

        public void SetSecret(string secret)
        {
            Secret.Text = secret;
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

        public class ListEntry
        {
            public string Label { get; set; }
            public string SecondaryLabel { get; set; }
            public string Quantity { get; set; }
        }
    }
}
