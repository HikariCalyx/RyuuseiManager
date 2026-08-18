using RyuuseiManager.Library.SF3;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RyuuseiManager
{
    /// <summary>
    /// WhiteCardEditorSF3.xaml 的交互逻辑
    /// </summary>
    public partial class WhiteCardEditorSF3 : Window
    {
        public WhiteCardEditorSF3()
        {
            InitializeComponent();
            ProfileLanguage = 0;
            SelectedCombo = 0;
        }

        public int ProfileLanguage { get; set; }
        public int SelectedCombo { get; private set; }
        public MainWindow? _mainWindow { get; set; }

        public void SetComboList(int currentCombo)
        {
            ComboList.Items.Clear();
            for (int i = 0x00; i <= 0x60; i++)
            {
                ComboList.Items.Add(new ComboItem
                {
                    Text = WhiteCardComboName.GetComboName(i, ProfileLanguage),
                    Value = i
                });
            }
            ComboList.SelectedValue = currentCombo;
            UpdateIcons(currentCombo);
        }

        private void ComboList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboList.SelectedValue is int id)
            {
                UpdateIcons(id);
            }
        }

        private void UpdateIcons(int id)
        {
            WhiteCard card = WhiteCardCombo.GetWhiteCard(id);
            BitmapScalingMode scaling = GetBitmapScalingMode();
            SetCardImage(Card1Image, card.Card1, scaling);
            SetCardImage(Card2Image, card.Card2, scaling);
            SetCardImage(Card3Image, card.Card3, scaling);
            SetCardImage(Card4Image, card.Card4, scaling);
        }

        private static void SetCardImage(Image target, BattleCard card, BitmapScalingMode scaling)
        {
            RenderOptions.SetBitmapScalingMode(target, scaling);
            target.Source = GameResourceRetriver.GetSF3CardImage((int)card);
        }

        private BitmapScalingMode GetBitmapScalingMode()
        {
            DpiScale dpi = VisualTreeHelper.GetDpi(this);
            if (dpi.DpiScaleX == 1.0)
            {
                return BitmapScalingMode.NearestNeighbor;
            }
            else
            {
                return BitmapScalingMode.HighQuality;
            }
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            if (ComboList.SelectedValue is int id)
            {
                var dlg = new ConfirmDialog(
                    title: (string)Application.Current.Resources["Msg_Confirm"],
                    prompt: ((string)Application.Current.Resources["Msg_WhiteCardConfirm"])
                        .Replace("\\n", Environment.NewLine + Environment.NewLine));
                dlg.Owner = this;
                if (dlg.ShowDialog() == true)
                {
                    if (_mainWindow != null && !_mainWindow.ApplyWhiteCardCombo(id))
                    {
                        return;
                    }
                    SelectedCombo = id;
                    DialogResult = true;
                }
            }
        }

        public class ComboItem
        {
            public string Text { get; set; }
            public int Value { get; set; }
        }
    }
}
