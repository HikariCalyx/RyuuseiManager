using RyuuseiManager.Classes;
using RyuuseiManager.Library.SF3;
using System.Windows.Controls;

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
        }

        public List<Folder> Folders { get; set; }

        public void SetFolderNames()
        {
            CardFolders.Items.Clear();
            int value = 0;
            foreach (var i in Folders)
            {
                ComboItem item = new ComboItem();
                item.Text = i.FolderName;
                item.Value = value;
                CardFolders.Items.Add(item);
                value++;
            }
            CardFolders.SelectedIndex = 0;
        }

        private void CardFolders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BattleCardList.Items.Clear();
            GalaxyAdvanceList.Items.Clear();
            Folder selectedFolder = Folders[CardFolders.SelectedIndex];
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
                BattleCardList.Items.Add($"{i.Key} *{i.Value}");
            }
            Dictionary<BattleCard, int> galaxyAdvances = GaCombo.GetPossibleCombos(battleCards);
            foreach (var i in galaxyAdvances)
            {
                GalaxyAdvanceList.Items.Add($"{i.Key} *{i.Value}");
            }
        }

        public class ComboItem
        {
            public string Text { get; set; }
            public int Value { get; set; }
        }
    }
}
