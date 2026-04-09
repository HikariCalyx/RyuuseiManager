using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace RyuuseiManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DB.InitDatabase();
            CheckSteamAccount();
        }

        public int GameGen
        {
            get { return (int)((ComboItem)ComboGameTitle.SelectedItem).Value; }
            private set;
        }

        public ulong SaveID
        {
            get { return ((ComboItem)ComboSaveName.SelectedItem).Value; }
            private set;
        }

        public ulong SteamID
        {
            get { return ((ComboItem)ComboSteamUser.SelectedItem).Value; }
            private set;
        }

        private API.MandarinKey key = new API.MandarinKey();

        private void ComboSteamUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboSteamUser.SelectedItem is ComboItem item)
            {
                ComboGameTitle.IsEnabled = true;
                ComboSaveName.Items.Clear();
                ComboSaveName.IsEnabled = false;
                ButtonImportSave.IsEnabled = false;
                ButtonCreateSave.IsEnabled = false;
                GetAvailableSteamSaveData(item.Value);
            }
        }

        private void ComboGameTitle_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboGameTitle.SelectedItem is ComboItem gameTitleItem)
            {
                int gen = (int)gameTitleItem.Value;
                GetSaveDataFromDB(gen);
            }
            ComboSaveName.IsEnabled = true;
            ButtonCreateSave.IsEnabled = GameGen == 11 || (GameGen >= 20) && GameGen != 30 && GameGen != 31; // perfect save data for SF1&3 to be added
            ButtonDeleteSave.IsEnabled = false;
            ButtonRenameSave.IsEnabled = false;
            ButtonLoadSaveData.IsEnabled = false;
            ButtonLoadAndRun.IsEnabled = false;
            ButtonImportSave.IsEnabled = true;
        }

        private void ComboSaveName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboSaveName.SelectedItem is ComboItem itemSave)
            {
                var coverTab = new TabItem
                {
                    Header = "Cover",
                    Content = new TextBlock
                    {
                        Text = $"This tab is intentionally left blank.",
                        Margin = new Thickness(10)
                    }
                };
                var battleCardTab = new TabItem
                {
                    Header = "Battle Card",
                    Content = new TextBlock
                    {
                        Text = $"To be added",
                        Margin = new Thickness(10)
                    }
                };
                var brotherTab = new TabItem
                {
                    Header = "Brother",
                    Content = new TextBlock
                    {
                        Text = $"To be added",
                        Margin = new Thickness(10)
                    }
                };
                var noiseModGearTab = new TabItem
                {
                    Header = "Noise Mod Gear",
                    Content = new TextBlock
                    {
                        Text = $"To be added",
                        Margin = new Thickness(10)
                    }
                };
                if (ComboGameTitle.SelectedItem is ComboItem itemTitle)
                {
                    MainTabs.Items.Clear();
                    MainTabs.Items.Add(coverTab);
                    switch (itemTitle.Value)
                    {
                        case 10:
                        case 11:
                        case 12:
                            MainTabs.Items.Add(battleCardTab);
                            MainTabs.Items.Add(brotherTab);
                            break;
                        case 20:
                        case 21:
                        case 22:
                        case 23:
                            MainTabs.Items.Add(battleCardTab);
                            MainTabs.Items.Add(brotherTab);
                            break;
                        case 30:
                        case 31:
                        case 32:
                        case 33:
                            MainTabs.Items.Add(battleCardTab);
                            MainTabs.Items.Add(brotherTab);
                            MainTabs.Items.Add(noiseModGearTab);
                            break;
                        default:
                            break;
                    }
                }
                ButtonDuplicate.IsEnabled = true;
                ButtonRenameSave.IsEnabled = true;
                ButtonDeleteSave.IsEnabled = SaveID > 0;
                ButtonExportSave.IsEnabled = true;
                ButtonLoadSaveData.IsEnabled = SaveID > 0;
                ButtonLoadAndRun.IsEnabled = SaveID > 0;
            }
            else
            {
                MainTabs.Items.Clear();
            }
        }

        private void ButtonImportSave_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new CommonOpenFileDialog
            {
                IsFolderPicker = false,
                Title = "Please select save data file."
            };
            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string loadedSaveFileName = Path.GetFileName(dlg.FileName);
                if (!GameID.ExpectedImportSources[GameGen].Contains(loadedSaveFileName))
                {
                    MessageBox.Show(this, "Selected save data is not meant for this game.", "Info");
                    return;
                }
                else
                {
                    byte[] saveBlob = File.ReadAllBytes(dlg.FileName);
                    var namedlg = new NameDialog(title: "Import Save Data", prompt: "Please specify a name.");
                    namedlg.Owner = this;
                    if (namedlg.ShowDialog() == true)
                    {
                        string saveName = namedlg.ResultText;
                        DB.SaveDataBlob(saveBlob, saveName, GameGen, true, out ulong saveId);
                        GetSaveDataFromDB(GameGen);
                        ComboSaveName.SelectedValue = saveId;
                    }
                }
            }
        }

        private void ButtonDuplicate_Click(object sender, RoutedEventArgs e)
        {
            if (ComboSaveName.SelectedItem is ComboItem nameItem)
            {
                var dlg = new NameDialog(title: "Duplicate", prompt: "Please specify a new name.");
                dlg.Owner = this;
                dlg.ResultText = nameItem.Text;
                if (dlg.ShowDialog() == true)
                {
                    string saveName = dlg.ResultText;
                    if (saveName == nameItem.Text) return;
                    if (SaveID == 0)
                    {
                        byte[] encSave = File.ReadAllBytes(Path.Combine(API.SteamInterop.GetSaveDataPath(SteamID), $"data0{GameGen}Slot.bin"));
                        byte[] decSave = key.DecryptBlob(encSave, API.SteamInterop.GetSteamID64(SteamID));
                        DB.SaveDataBlob(decSave, saveName, GameGen, true, out _);
                    }
                    else
                    {
                        byte[] currentSave = DB.LoadDataBlob(SaveID);
                        DB.SaveDataBlob(currentSave, saveName, GameGen, true, out _);
                    }
                    GetSaveDataFromDB(GameGen);
                    ComboSaveName.SelectedValue = SaveID;
                }
            }
        }

        private void ButtonCreateSave_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new NameDialog(title: "New Save Data", prompt: "Please specify a name.");
            dlg.Owner = this;
            if (dlg.ShowDialog() == true)
            {
                string saveName = dlg.ResultText;
                var uri = new Uri($"pack://application:,,,/PrebuiltBaseSaveData/{GameGen}.bin.zlib");
                using var s = Application.GetResourceStream(uri).Stream;
                using var ms = new MemoryStream();
                s.CopyTo(ms);
                DB.SaveDataBlob(ms.ToArray(), saveName, GameGen, false, out ulong saveId);
                GetSaveDataFromDB(GameGen);
                ComboSaveName.SelectedValue = saveId;
            }
        }

        private void ButtonRenameSave_Click(object sender, RoutedEventArgs e)
        {
            if (ComboSaveName.SelectedItem is ComboItem nameItem)
            {
                var dlg = new NameDialog(title: "Rename", prompt: "Please specify a new name.");
                dlg.Owner = this;
                dlg.ResultText = nameItem.Text;
                if (dlg.ShowDialog() == true)
                {
                    string saveName = dlg.ResultText;
                    if (saveName == nameItem.Text) return;
                    if (SaveID == 0)
                    {
                        byte[] encSave = File.ReadAllBytes(Path.Combine(API.SteamInterop.GetSaveDataPath(SteamID), $"data0{GameGen}Slot.bin"));
                        byte[] decSave = key.DecryptBlob(encSave, API.SteamInterop.GetSteamID64(SteamID));
                        DB.SaveDataBlob(decSave, saveName, GameGen, true, out _);
                    }
                    else
                    {
                        DB.RenameSaveBlob(saveName, SaveID);
                    }
                    GetSaveDataFromDB(GameGen);
                    ComboSaveName.SelectedValue = SaveID;
                }
            }
        }

        private void ButtonDeleteSave_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(this, "Would you like to delete this save?\r\nThis cannot be undone.", "Confirm", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                DB.DeleteSaveById(SaveID);
                GetSaveDataFromDB(GameGen);
            }
        }

        private void ButtonExportSave_Click(object sender, RoutedEventArgs e)
        {
            byte[]? rawSaveData;
            if (SaveID == 0)
            {
                byte[] encSave = File.ReadAllBytes(Path.Combine(API.SteamInterop.GetSaveDataPath(SteamID), $"data0{GameGen}Slot.bin"));
                rawSaveData = key.DecryptBlob(encSave, API.SteamInterop.GetSteamID64(SteamID));
            }
            else
            {
                rawSaveData = DB.LoadDataBlob(SaveID);
            }
            if (rawSaveData != null)
            {
                var dlg = new CommonOpenFileDialog
                {
                    IsFolderPicker = true,
                    Title = "Please select a destination directory to place exported save data."
                };
                if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    File.WriteAllBytes(Path.Combine(dlg.FileName, $"data0{GameGen}Slot.bin"), rawSaveData);
                }
            }
        }

        private void ButtonLoadSaveData_Click(object sender, RoutedEventArgs e)
        {
            LoadSave(true);
        }

        private void ButtonRunGame_Click(object sender, RoutedEventArgs e)
        {
            RunGame();
        }

        private void ButtonLoadAndRun_Click(object sender, RoutedEventArgs e)
        {
            if (LoadSave(false)) RunGame();
        }

        private bool LoadSave(bool prompts)
        {
            byte[]? rawSaveData = DB.LoadDataBlob(SaveID);
            if (rawSaveData != null)
            {
                string? savePath = API.SteamInterop.GetSaveDataPath(SteamID);
                if (CanWriteToPath(savePath))
                {
                    byte[] signedSave = key.EncryptBlob(rawSaveData, API.SteamInterop.GetSteamID64(SteamID));
                    File.WriteAllBytes(Path.Combine(savePath, $"data0{GameGen}Slot.bin"), signedSave);
                    if (prompts) MessageBox.Show(this, "Save data import complete.", "Info");
                    return true;
                }
                else
                {
                    MessageBox.Show(this, "Please run Ryuusei Manager with Administrator previleges.", "Info");
                    return false;
                }
            }
            return false;
        }

        private void RunGame()
        {
            Process.Start(new ProcessStartInfo
            {
                UseShellExecute = true,
                FileName = "steam://rungameid/3500390",
            });
        }

        private void CheckSteamAccount()
        {
            List<ulong> steamIDs = API.SteamInterop.GetAvailableSteamUsers();
            if (steamIDs.Count > 0)
            {
                foreach (var i in steamIDs)
                {
                    string nickName = (API.SteamInterop.GetLocalNickname(i) ?? "null") + $" ({i})";
                    ComboSteamUser.Items.Add(new ComboItem { Text = nickName, Value = i });
                }
            }
            else
            {
                MessageBox.Show("No valid Steam Account information has been found.\r\nPlease check if you have Steam installed, and you have the game played.");
            }
        }

        private void GetAvailableSteamSaveData(ulong steamID3)
        {
            string? saveDataDir = API.SteamInterop.GetSaveDataPath(steamID3);
            if (!string.IsNullOrEmpty(saveDataDir))
            {
                List<string> saveDataFiles = Directory.GetFiles(saveDataDir).ToList();
                foreach (var i in saveDataFiles)
                {
                    string fileName = Path.GetFileName(i);
                    switch (fileName)
                    {
                        case "data010Slot.bin":
                            ComboGameTitle.Items.Add(new ComboItem { Text = "Pegasus", Value = 10 });
                            break;
                        case "data011Slot.bin":
                            ComboGameTitle.Items.Add(new ComboItem { Text = "Leo", Value = 11 });
                            break;
                        case "data012Slot.bin":
                            ComboGameTitle.Items.Add(new ComboItem { Text = "Dragon", Value = 12 });
                            break;
                        case "data020Slot.bin":
                            ComboGameTitle.Items.Add(new ComboItem { Text = "Ninja", Value = 20 });
                            break;
                        case "data021Slot.bin":
                            ComboGameTitle.Items.Add(new ComboItem { Text = "Saurian", Value = 21 });
                            break;
                        case "data022Slot.bin":
                            ComboGameTitle.Items.Add(new ComboItem { Text = "Zerker (Ninja)", Value = 22 });
                            break;
                        case "data023Slot.bin":
                            ComboGameTitle.Items.Add(new ComboItem { Text = "Zerker (Saurian)", Value = 23 });
                            break;
                        case "data030Slot.bin":
                            ComboGameTitle.Items.Add(new ComboItem { Text = "Black Ace (Data 01)", Value = 30 });
                            break;
                        case "data031Slot.bin":
                            ComboGameTitle.Items.Add(new ComboItem { Text = "Black Ace (Data 02)", Value = 31 });
                            break;
                        case "data032Slot.bin":
                            ComboGameTitle.Items.Add(new ComboItem { Text = "Red Joker (Data 01)", Value = 32 });
                            break;
                        case "data033Slot.bin":
                            ComboGameTitle.Items.Add(new ComboItem { Text = "Red Joker (Data 02)", Value = 33 });
                            break;
                    }
                }
            }
            else
            {
                MessageBox.Show("We didn't find any save data.\r\nPlease play at least one game and try again.");
            }
        }

        private void GetSaveDataFromDB(int generation)
        {
            ComboSaveName.Items.Clear();
            ComboSaveName.Items.Add(new ComboItem { Text = "(Current Steam Save)", Value = 0 });
            var saveDataDict = DB.GetCurrentGenerationSaves(generation);
            foreach (var i in saveDataDict.Keys)
            {
                ComboSaveName.Items.Add(new ComboItem { Text = saveDataDict[i], Value = (ulong)i });
            }
        }

        private bool CanWriteToPath(string path)
        {
            try
            {
                string testFile = Path.Combine(path, Path.GetRandomFileName());
                using (FileStream fs = File.Create(testFile, 1, FileOptions.DeleteOnClose))
                {
                }
                return true;
            }
            catch
            {
                return false;
            }
        }


        public class ComboItem
        {
            public string Text { get; set; }
            public ulong Value { get; set; }
        }
    }
}