using RyuuseiManager.Classes;
using RyuuseiManager.ImageGenerator;
using RyuuseiManager.Library.SF3;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RyuuseiManager
{
    /// <summary>
    /// BattleCardPageSF3.xaml 的交互逻辑
    /// </summary>
    public partial class BattleCardPageSF3 : Page
    {
        public BattleCardPageSF3()
        {
            InitializeComponent();
            Folders = new List<Folder>();
            ProfileLanguage = 0;
            WCard = new WhiteCard();
            ShowOtherLanguage = false;
            EquippedFolderIndex = 1;
        }

        public List<Folder> Folders { get; set; }
        public int ProfileLanguage { get; set; }
        public int EquippedFolderIndex { get; set; }
        public WhiteCard WCard { get; set; }
        public MainWindow _mainWindow { get; set; }
        public bool ShowOtherLanguage { get; set; }
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

        public void SetFolderNames()
        {
            CardFolders.Items.Clear();
            int value = 0;
            for (int i = 0; i < Folders.Count; i++)
            {
                ComboItem item = new ComboItem();
                item.Text = (i == EquippedFolderIndex ? "[E] " : "") + Folders[i].FolderName;
                item.Value = value;
                CardFolders.Items.Add(item);
                value++;
            }
            CardFolders.SelectedIndex = EquippedFolderIndex;
        }

        private void CardFolders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BattleCardList.Items.Clear();
            GalaxyAdvanceList.Items.Clear();
            Folder selectedFolder = Folders[CardFolders.SelectedIndex];
            BattleCard regCard = BattleCard.NoData;
            List<BattleCard> tagCards = new List<BattleCard>();
            if (selectedFolder.RegularCardIndex != -1)
            {
                regCard = (BattleCard)selectedFolder.Cards[selectedFolder.RegularCardIndex];
            }
            foreach (var i in selectedFolder.TagCards)
            {
                if (i < 30)
                {
                    tagCards.Add((BattleCard)selectedFolder.Cards[i]);
                }
            }
            Dictionary<BattleCard, int> battleCards = new Dictionary<BattleCard, int>();
            foreach (var i in selectedFolder.Cards)
            {
                BattleCard currentCard = (BattleCard)i;
                if (battleCards.ContainsKey(currentCard))
                {
                    battleCards[currentCard] += 1;
                }
                else
                {
                    battleCards[currentCard] = 1;
                }
            }
            foreach (var i in battleCards)
            {
                if (i.Key == BattleCard.NoData) continue;
                int damage = BattleCardAttributes.GetDamage(i.Key);
                int element = BattleCardAttributes.GetElements(i.Key);
                var entry = new ListEntry
                {
                    Image = GameResourceRetriver.GetSF3CardImage((int)i.Key),
                    DamageImage = DamageTagGenerator.GetDamageTag(damage, GetCardClass(i.Key)),
                    ElementImage = DamageTagGenerator.GetElementTag(element, (i.Key == regCard), tagCards.Contains(i.Key)),
                    Label = $"{BattleCardName.GetBattleCardName(i.Key, ProfileLanguage)}",
                    SecondaryLabel = $"{BattleCardName.GetSecondaryBattleCardName(i.Key, ProfileLanguage)}",
                    Quantity = $"x {i.Value}",
                    IsIllegal = IsIllegalCard(i.Key),
                    CardClass = GetCardClass(i.Key),
                    ShowOtherLanguage = ShowOtherLanguage,
                    ScalingType = GetDpiScaling()
                };
                BattleCardList.Items.Add(entry);
            }
            foreach (var i in WCard.ToDict())
            {
                if (i.Key == BattleCard.NoData) continue;
                int damage = BattleCardAttributes.GetDamage(i.Key);
                int element = BattleCardAttributes.GetElements(i.Key);
                var entry = new ListEntry
                {
                    Image = GameResourceRetriver.GetSF3CardImage((int)i.Key),
                    DamageImage = DamageTagGenerator.GetDamageTag(damage, 3),
                    ElementImage = DamageTagGenerator.GetElementTag(element),
                    Label = $"{BattleCardName.GetBattleCardName(i.Key, ProfileLanguage)}",
                    SecondaryLabel = $"{BattleCardName.GetSecondaryBattleCardName(i.Key, ProfileLanguage)}",
                    Quantity = $"x {i.Value}",
                    IsIllegal = IsIllegalCard(i.Key),
                    CardClass = 3,
                    ShowOtherLanguage = ShowOtherLanguage,
                    ScalingType = GetDpiScaling()
                };
                BattleCardList.Items.Add(entry);
            }
            foreach (var i in WCard.ToDict().Keys)
            {
                if (i == BattleCard.NoData) continue;
                if (battleCards.ContainsKey(i))
                {
                    battleCards[i] += 1;
                }
                else
                {
                    battleCards[i] = 1;
                }
            }
            Dictionary<BattleCard, int> galaxyAdvances = GaCombo.GetPossibleCombos(battleCards);
            foreach (var i in galaxyAdvances)
            {
                int damage = BattleCardAttributes.GetDamage(i.Key);
                int element = BattleCardAttributes.GetElements(i.Key);
                List<BattleCard> gaCombo = GaCombo.gaCombos[i.Key];
                var entry = new ListEntry
                {
                    Image = GameResourceRetriver.GetSF3CardImage((int)i.Key),
                    DamageImage = DamageTagGenerator.GetDamageTag(damage, GetCardClass(i.Key)),
                    ElementImage = DamageTagGenerator.GetElementTag(element),
                    GaPart0 = GameResourceRetriver.GetSF3CardImage((int)gaCombo[0]),
                    GaPart1 = GameResourceRetriver.GetSF3CardImage((int)gaCombo[1]),
                    GaPart2 = GameResourceRetriver.GetSF3CardImage((int)gaCombo[2]),
                    Label = $"{BattleCardName.GetBattleCardName(i.Key, ProfileLanguage)}",
                    SecondaryLabel = $"{BattleCardName.GetSecondaryBattleCardName(i.Key, ProfileLanguage)}",
                    Quantity = $"x {i.Value}",
                    CardClass = GetCardClass(i.Key),
                    ShowOtherLanguage = ShowOtherLanguage,
                    ScalingType = GetDpiScaling()
                };
                GalaxyAdvanceList.Items.Add(entry);
            }
        }

        private void BtnBattleCardEditor_Click(object sender, EventArgs e)
        {
            BattleCardEditorSF3 dlg = new BattleCardEditorSF3();
            dlg.ProfileLanguage = ProfileLanguage;
            dlg.ShowOtherLanguage = ShowOtherLanguage;
            dlg.GameVersion = 1;
            dlg.SetBattleCardList(Folders[CardFolders.SelectedIndex]);
            dlg.Owner = _mainWindow;
            dlg.ShowDialog();
        }

        public void ToggleOtherNameVisibility(bool visibility)
        {
            foreach (ListEntry i in BattleCardList.Items)
            {
                i.ShowOtherLanguage = visibility;
            }
        }

        private bool IsIllegalCard(BattleCard battleCard)
        {
            return ((int)battleCard > 207 && (int)battleCard < 1286);
        }

        private int GetCardClass(BattleCard battleCard)
        {
            if (((int)battleCard > 150 && (int)battleCard < 196) || // Library
                ((int)battleCard > 323 && (int)battleCard < 387) || // Illegal
                ((int)battleCard > 1296 && (int)battleCard < 1328)) // GA
            {
                return 1; // MEGA
            }
            else if (((int)battleCard > 195 && (int)battleCard < 208) || // Library
                ((int)battleCard > 386 && (int)battleCard < 398) || // Illegal
                ((int)battleCard > 1327 && (int)battleCard < 1336)) // GA
            {
                return 2; // GIGA
            }
            else
            {
                return 0; // STANDARD
            }
        }

        public class ComboItem
        {
            public string Text { get; set; }
            public int Value { get; set; }
        }

        public class ListEntry
        {
            public BitmapImage Image { get; set; }
            public BitmapSource ElementImage { get; set; }
            public BitmapImage GaPart0 { get; set; }
            public BitmapImage GaPart1 { get; set; }
            public BitmapImage GaPart2 { get; set; }
            public BitmapSource DamageImage { get; set; }
            public int PixelWidth => Image.PixelWidth;
            public int PixelHeight => Image.PixelHeight;
            public bool IsIllegal { get; set; }
            public string Label { get; set; }
            public string SecondaryLabel { get; set; }
            public string Quantity { get; set; }
            public int CardClass { get; set; }
            public bool ShowOtherLanguage { get; set; }
            public string ScalingType { get; set; }
        }

    }
}
