using RyuuseiManager.Classes;
using RyuuseiManager.ImageGenerator;
using RyuuseiManager.Library.SF3;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using static System.Net.Mime.MediaTypeNames;

namespace RyuuseiManager
{
    public partial class BattleCardEditorSF3 : Window
    {
        private CancellationTokenSource _searchCts;

        public BattleCardEditorSF3()
        {
            InitializeComponent();
            ProfileLanguage = 0;
            ShowOtherLanguage = false;
            GameVersion = 1;
            CategoryID = 0;
        }

        public int ProfileLanguage { get; set; }
        public bool ShowOtherLanguage { get; set; }
        public int GameVersion { get; set; }
        private int CategoryID { get; set; }

        // -----------------------------
        //  DPI Scaling
        // -----------------------------
        private string GetDpiScaling()
        {
            DpiScale dpi = VisualTreeHelper.GetDpi(this);
            return dpi.DpiScaleX == 1.0 ? "NearestNeighbor" : "HighQuality";
        }

        // -----------------------------
        //  Load Folder Cards (UI thread)
        // -----------------------------
        public void SetBattleCardList(Folder folder)
        {
            TxtFolderName.Text = folder.FolderName;
            BattleCardList.Items.Clear();

            string scaling = GetDpiScaling();

            for (int i = 0; i < folder.Cards.Count; i++)
            {
                BattleCard card = (BattleCard)folder.Cards[i];
                int damage = BattleCardAttributes.GetDamage(card);
                int element = BattleCardAttributes.GetElements(card);

                var entry = new ListEntry
                {
                    Image = GameResourceRetriver.GetSF3CardImage((int)card),
                    DamageImage = DamageTagGenerator.GetDamageTag(damage, GetCardClass(card)),
                    ElementImage = DamageTagGenerator.GetElementTag(
                        element,
                        i == folder.RegularCardIndex,
                        folder.TagCards.Contains(i)
                    ),
                    Label = BattleCardName.GetBattleCardName(card, ProfileLanguage),
                    SecondaryLabel = BattleCardName.GetSecondaryBattleCardName(card, ProfileLanguage),
                    IsIllegal = IsIllegalCard(card),
                    CardClass = GetCardClass(card),
                    ShowOtherLanguage = ShowOtherLanguage,
                    ScalingType = scaling
                };

                BattleCardList.Items.Add(entry);
            }

            // Load available cards initially
            _ = RefreshAvailableCardsAsync(CategoryID, "");
        }

        private void BtnAll_Click(object sender, RoutedEventArgs e)
        {
            CategoryID = 0;
            string search = TxtSearchBox.Text;
            _ = RefreshAvailableCardsAsync(CategoryID, search);
            BtnAll.IsEnabled = false;
            BtnStandard.IsEnabled = true;
            BtnMega.IsEnabled = true;
            BtnGiga.IsEnabled = true;
            BtnGalaxyAdvance.IsEnabled = true;
        }

        private void BtnStandard_Click(object sender, RoutedEventArgs e)
        {
            CategoryID = 1;
            string search = TxtSearchBox.Text;
            _ = RefreshAvailableCardsAsync(CategoryID, search);
            BtnAll.IsEnabled = true;
            BtnStandard.IsEnabled = false;
            BtnMega.IsEnabled = true;
            BtnGiga.IsEnabled = true;
            BtnGalaxyAdvance.IsEnabled = true;
        }

        private void BtnMega_Click(object sender, RoutedEventArgs e)
        {
            CategoryID = 2;
            string search = TxtSearchBox.Text;
            _ = RefreshAvailableCardsAsync(CategoryID, search);
            BtnAll.IsEnabled = true;
            BtnStandard.IsEnabled = true;
            BtnMega.IsEnabled = false;
            BtnGiga.IsEnabled = true;
            BtnGalaxyAdvance.IsEnabled = true;
        }

        private void BtnGiga_Click(object sender, RoutedEventArgs e)
        {
            CategoryID = 3;
            string search = TxtSearchBox.Text;
            _ = RefreshAvailableCardsAsync(CategoryID, search);
            BtnAll.IsEnabled = true;
            BtnStandard.IsEnabled = true;
            BtnMega.IsEnabled = true;
            BtnGiga.IsEnabled = false;
            BtnGalaxyAdvance.IsEnabled = true;
        }

        private void BtnGalaxyAdvance_Click(object sender, RoutedEventArgs e)
        {
            CategoryID = 4;
            string search = TxtSearchBox.Text;
            _ = RefreshAvailableCardsAsync(CategoryID, search);
            BtnAll.IsEnabled = true;
            BtnStandard.IsEnabled = true;
            BtnMega.IsEnabled = true;
            BtnGiga.IsEnabled = true;
            BtnGalaxyAdvance.IsEnabled = false;
        }

        // -----------------------------
        //  Search Box (debounced + cancelable)
        // -----------------------------
        private async void TxtSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            _searchCts?.Cancel();
            _searchCts = new CancellationTokenSource();
            var token = _searchCts.Token;

            string search = TxtSearchBox.Text;

            try
            {
                await Task.Delay(300, token); // debounce
                await RefreshAvailableCardsAsync(CategoryID, search, token);
            }
            catch (OperationCanceledException)
            {
            }
        }

        // -----------------------------
        //  Public wrapper (no token)
        // -----------------------------
        private Task RefreshAvailableCardsAsync(int categoryId, string search)
        {
            _searchCts?.Cancel();
            _searchCts = new CancellationTokenSource();
            return RefreshAvailableCardsAsync(categoryId, search, _searchCts.Token);
        }

        // -----------------------------
        //  Main async loader
        // -----------------------------
        private async Task RefreshAvailableCardsAsync(int categoryId, string search, CancellationToken token)
        {
            int profileLang = ProfileLanguage;
            bool showOther = ShowOtherLanguage;
            string scaling = GetDpiScaling();

            // -----------------------------
            //  PHASE 1 — Background thread
            // -----------------------------
            var data = await Task.Run(() =>
            {
                token.ThrowIfCancellationRequested();

                List<int> allCardIndex = categoryId switch
                {
                    0 => BattleCardClass.GetAllowedBattleCardList(GameVersion),
                    1 => BattleCardClass.GetStandardCards(),
                    2 => BattleCardClass.GetMegaCards(),
                    3 => BattleCardClass.GetGigaCards(GameVersion),
                    4 => GaCombo.gaCombos.Keys.Select(k => (int)k).ToList(),
                    _ => new List<int>()
                };

                allCardIndex = allCardIndex.Distinct().OrderBy(x => x).ToList();

                List<CardRowData> rows = new();

                foreach (int id in allCardIndex)
                {
                    token.ThrowIfCancellationRequested();

                    BattleCard card = (BattleCard)id;

                    string label = BattleCardName.GetBattleCardName(card, profileLang);
                    string secondary = BattleCardName.GetSecondaryBattleCardName(card, profileLang);

                    if (!label.Replace(" ", "").Contains(StringProcessor.JP.Generalize(search).Replace(" ", ""), StringComparison.OrdinalIgnoreCase) &&
                        !secondary.Replace(" ", "").Contains(StringProcessor.JP.Generalize(search).Replace(" ", ""), StringComparison.OrdinalIgnoreCase))
                        continue;


                    rows.Add(new CardRowData
                    {
                        Card = card,
                        Damage = BattleCardAttributes.GetDamage(card),
                        Element = BattleCardAttributes.GetElements(card),
                        Label = label,
                        SecondaryLabel = secondary,
                        CardClass = GetCardClass(card),
                        IsIllegal = IsIllegalCard(card)
                    });
                }

                return rows;
            }, token);

            token.ThrowIfCancellationRequested();

            // -----------------------------
            //  PHASE 2 — UI thread
            // -----------------------------
            AvailableCardsList.Items.Clear();

            foreach (var row in data)
            {
                var entry = new ListEntry
                {
                    Image = GameResourceRetriver.GetSF3CardImage((int)row.Card),
                    DamageImage = DamageTagGenerator.GetDamageTag(row.Damage, row.CardClass),
                    ElementImage = DamageTagGenerator.GetElementTag(row.Element),
                    Label = row.Label,
                    SecondaryLabel = row.SecondaryLabel,
                    IsIllegal = row.IsIllegal,
                    CardClass = row.CardClass,
                    ShowOtherLanguage = showOther,
                    ScalingType = scaling
                };

                // GA parts
                if (categoryId == 4)
                {
                    var ga = GaCombo.gaCombos[row.Card];
                    entry.GaPart0 = GameResourceRetriver.GetSF3CardImage((int)ga[0]);
                    entry.GaPart1 = GameResourceRetriver.GetSF3CardImage((int)ga[1]);
                    entry.GaPart2 = GameResourceRetriver.GetSF3CardImage((int)ga[2]);
                }

                AvailableCardsList.Items.Add(entry);
            }
        }

        // -----------------------------
        //  Helpers
        // -----------------------------
        private bool IsIllegalCard(BattleCard battleCard)
        {
            return ((int)battleCard > 207 && (int)battleCard < 1286);
        }

        private int GetCardClass(BattleCard battleCard)
        {
            if (((int)battleCard > 150 && (int)battleCard < 196) ||
                ((int)battleCard > 323 && (int)battleCard < 387) ||
                ((int)battleCard > 1296 && (int)battleCard < 1328))
                return 1;

            if (((int)battleCard > 195 && (int)battleCard < 208) ||
                ((int)battleCard > 386 && (int)battleCard < 398) ||
                ((int)battleCard > 1327 && (int)battleCard < 1336))
                return 2;

            return 0;
        }

        // -----------------------------
        //  DTO for background thread
        // -----------------------------
        private class CardRowData
        {
            public BattleCard Card { get; set; }
            public int Damage { get; set; }
            public int Element { get; set; }
            public string Label { get; set; }
            public string SecondaryLabel { get; set; }
            public int CardClass { get; set; }
            public bool IsIllegal { get; set; }
        }

        // -----------------------------
        //  UI ListEntry (unchanged)
        // -----------------------------
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
            public int CardClass { get; set; }
            public bool ShowOtherLanguage { get; set; }
            public string ScalingType { get; set; }
        }
    }
}
