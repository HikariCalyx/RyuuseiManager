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
            LoadLanguage();
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
                ButtonExportSave.IsEnabled = false;
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
            ButtonCreateSave.IsEnabled = false; // Disable this feature until all save collected
            ButtonDuplicate.IsEnabled = false;
            ButtonDeleteSave.IsEnabled = false;
            ButtonRenameSave.IsEnabled = false;
            ButtonLoadSaveData.IsEnabled = false;
            ButtonLoadAndRun.IsEnabled = false;
            ButtonImportSave.IsEnabled = true;
            ButtonExportSave.IsEnabled = false;
        }

        private void ComboSaveName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboSaveName.SelectedItem is ComboItem itemSave)
            {
                var coverTab = new TabItem
                {
                    Header = (string)Application.Current.Resources["Tab_Cover"],
                    Content = new TextBlock
                    {
                        Text = (string)Application.Current.Resources["Msg_IntentBlank"],
                        Margin = new Thickness(10)
                    }
                };
                var battleCardTab = new TabItem
                {
                    Header = (string)Application.Current.Resources["Tab_BattleCard"],
                    Content = new TextBlock
                    {
                        Text = (string)Application.Current.Resources["Msg_TBA"],
                        Margin = new Thickness(10)
                    }
                };
                var brotherTab = new TabItem
                {
                    Header = (string)Application.Current.Resources["Tab_Brother"],
                    Content = new TextBlock
                    {
                        Text = (string)Application.Current.Resources["Msg_TBA"],
                        Margin = new Thickness(10)
                    }
                };
                var noiseModGearTab = new TabItem
                {
                    Header = (string)Application.Current.Resources["Tab_NoiseModGear"],
                    Content = new TextBlock
                    {
                        Text = (string)Application.Current.Resources["Msg_TBA"],
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
                Title = (string)Application.Current.Resources["Msg_ImportSave"]
            };
            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                string loadedSaveFileName = Path.GetFileName(dlg.FileName);
                if (!GameID.ExpectedImportSources[GameGen].Contains(loadedSaveFileName))
                {
                    MessageBox.Show(this, (string)Application.Current.Resources["Msg_UnsuitableSave"], (string)Application.Current.Resources["Msg_Info"]);
                    return;
                }
                else
                {
                    byte[] saveBlob = ReadFile(dlg.FileName);
                    if (saveBlob.AsSpan().StartsWith(BinaryMagic.HeaderMagic.Switch))
                    {
                        int gameID = (int)(GameGen / 10);
                        if (false)
                        {
                            MessageBox.Show(this, (string)Application.Current.Resources["Msg_UnsupportedSwitchSave"], (string)Application.Current.Resources["Msg_Info"]);
                            return;
                        }
                        saveBlob = BinaryMagic.Processor.StripSwitchSave(saveBlob, gameID);
                    }
                    if (!saveBlob.AsSpan().StartsWith(BinaryMagic.HeaderMagic.Raw))
                    {
                        MessageBox.Show(this, (string)Application.Current.Resources["Msg_InvalidSave"], (string)Application.Current.Resources["Msg_Info"]);
                        return;
                    }
                    else if (!CheckSave(saveBlob))
                    {
                        MessageBox.Show(this, (string)Application.Current.Resources["Msg_UnsuitableSave"], (string)Application.Current.Resources["Msg_Info"]);
                        return;
                    }
                    var namedlg = new NameDialog(title: (string)Application.Current.Resources["Dlg_ImportSaveData"], prompt: (string)Application.Current.Resources["Msg_SpecifyName"]);
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
                var dlg = new NameDialog(title: (string)Application.Current.Resources["Dlg_Duplicate"], prompt: (string)Application.Current.Resources["Msg_SpecifyNewName"]);
                dlg.Owner = this;
                dlg.ResultText = nameItem.Text;
                ulong resultSaveId;
                if (dlg.ShowDialog() == true)
                {
                    string saveName = dlg.ResultText;
                    if (saveName == nameItem.Text) return;
                    if (SaveID == 0)
                    {
                        byte[] encSave = ReadFile(Path.Combine(API.SteamInterop.GetSaveDataPath(SteamID), $"data0{GameGen}Slot.bin"));
                        byte[] decSave = key.DecryptBlob(encSave, API.SteamInterop.GetSteamID64(SteamID));
                        DB.SaveDataBlob(decSave, saveName, GameGen, true, out resultSaveId);
                        GetSaveDataFromDB(GameGen);
                    }
                    else
                    {
                        byte[] currentSave = DB.LoadDataBlob(SaveID);
                        DB.SaveDataBlob(currentSave, saveName, GameGen, true, out resultSaveId);
                    }
                    GetSaveDataFromDB(GameGen);
                    ComboSaveName.SelectedValue = resultSaveId;
                }
            }
        }

        private void ButtonCreateSave_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new NameDialog(title: (string)Application.Current.Resources["Dlg_CreateSave"], prompt: (string)Application.Current.Resources["Msg_SpecifyName"]);
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

        private void ButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SettingsWindow();
            dlg.Owner = this;
            dlg.ShowDialog();
        }

        private void ButtonRenameSave_Click(object sender, RoutedEventArgs e)
        {
            if (ComboSaveName.SelectedItem is ComboItem nameItem)
            {
                var dlg = new NameDialog(title: (string)Application.Current.Resources["Dlg_Rename"], prompt: (string)Application.Current.Resources["Msg_SpecifyNewName"]);
                dlg.Owner = this;
                dlg.ResultText = nameItem.Text;
                if (dlg.ShowDialog() == true)
                {
                    string saveName = dlg.ResultText;
                    if (saveName == nameItem.Text) return;
                    if (SaveID == 0)
                    {
                        byte[] encSave = ReadFile(Path.Combine(API.SteamInterop.GetSaveDataPath(SteamID), $"data0{GameGen}Slot.bin"));
                        byte[] decSave = key.DecryptBlob(encSave, API.SteamInterop.GetSteamID64(SteamID));
                        DB.SaveDataBlob(decSave, saveName, GameGen, true, out ulong newSaveId);
                        GetSaveDataFromDB(GameGen);
                        ComboSaveName.SelectedValue = newSaveId;
                    }
                    else
                    {
                        ulong currentSaveId = SaveID;
                        DB.RenameSaveBlob(saveName, currentSaveId);
                        GetSaveDataFromDB(GameGen);
                        ComboSaveName.SelectedValue = currentSaveId;
                    }
                }
            }
        }

        private void ButtonDeleteSave_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show(this, (string)Application.Current.Resources["Msg_DeleteConfirm"], (string)Application.Current.Resources["Msg_Confirm"], MessageBoxButton.YesNo);
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
                byte[] encSave = ReadFile(Path.Combine(API.SteamInterop.GetSaveDataPath(SteamID), $"data0{GameGen}Slot.bin"));
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
                    Title = (string)Application.Current.Resources["Msg_ExportSave"]
                };
                if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    if (!TrySaveFile(Path.Combine(dlg.FileName, $"data0{GameGen}Slot.bin"), rawSaveData))
                    {
                        MessageBox.Show(this, (string)Application.Current.Resources["Msg_UnableToSave"], (string)Application.Current.Resources["Msg_Info"]);
                    }
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
                    if (!TrySaveFile(Path.Combine(savePath, $"data0{GameGen}Slot.bin"), signedSave))
                    {
                        MessageBox.Show(this, (string)Application.Current.Resources["Msg_UnableToSave"], (string)Application.Current.Resources["Msg_Info"]);
                        return false;
                    }
                    if (prompts) MessageBox.Show(this, (string)Application.Current.Resources["Msg_ImportComplete"], (string)Application.Current.Resources["Msg_Info"]);
                    return true;
                }
                else
                {
                    MessageBox.Show(this, (string)Application.Current.Resources["Msg_RunElevate"], (string)Application.Current.Resources["Msg_Info"]);
                    return false;
                }
            }
            return false;
        }

        private bool CheckSave(byte[] blob)
        {
            byte expectedNextByte;
            switch (GameGen)
            {
                case 10:
                case 11:
                case 12:
                    expectedNextByte = 0x43; break;
                case 20:
                case 21:
                case 22:
                case 23:
                    expectedNextByte = 0x45; break;
                case 30:
                case 31:
                case 32:
                case 33:
                    expectedNextByte = 0x53; break;
                default:
                    return false;
            }
            if (BinaryMagic.Processor.TryGetNextByte(blob, BinaryMagic.HeaderMagic.Raw, out byte nextByte))
            {
                return (expectedNextByte == nextByte);
            }
            else
            {
                return false;
            }
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
                MessageBox.Show((string)Application.Current.Resources["Msg_NoSteamAccount"]);
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
                            ComboGameTitle.Items.Add(new ComboItem { Text = (string)Application.Current.Resources["Pegasus"], Value = 10 });
                            break;
                        case "data011Slot.bin":
                            ComboGameTitle.Items.Add(new ComboItem { Text = (string)Application.Current.Resources["Leo"], Value = 11 });
                            break;
                        case "data012Slot.bin":
                            ComboGameTitle.Items.Add(new ComboItem { Text = (string)Application.Current.Resources["Dragon"], Value = 12 });
                            break;
                        case "data020Slot.bin":
                            ComboGameTitle.Items.Add(new ComboItem { Text = (string)Application.Current.Resources["Ninja"], Value = 20 });
                            break;
                        case "data021Slot.bin":
                            ComboGameTitle.Items.Add(new ComboItem { Text = (string)Application.Current.Resources["Saurian"], Value = 21 });
                            break;
                        case "data022Slot.bin":
                            ComboGameTitle.Items.Add(new ComboItem { Text = (string)Application.Current.Resources["ZerkerN"], Value = 22 });
                            break;
                        case "data023Slot.bin":
                            ComboGameTitle.Items.Add(new ComboItem { Text = (string)Application.Current.Resources["ZerkerS"], Value = 23 });
                            break;
                        case "data030Slot.bin":
                            ComboGameTitle.Items.Add(new ComboItem { Text = (string)Application.Current.Resources["BlackAceSlot1"], Value = 30 });
                            break;
                        case "data031Slot.bin":
                            ComboGameTitle.Items.Add(new ComboItem { Text = (string)Application.Current.Resources["BlackAceSlot2"], Value = 31 });
                            break;
                        case "data032Slot.bin":
                            ComboGameTitle.Items.Add(new ComboItem { Text = (string)Application.Current.Resources["RedJokerSlot1"], Value = 32 });
                            break;
                        case "data033Slot.bin":
                            ComboGameTitle.Items.Add(new ComboItem { Text = (string)Application.Current.Resources["RedJokerSlot2"], Value = 33 });
                            break;
                    }
                }
            }
            else
            {
                MessageBox.Show((string)Application.Current.Resources["Msg_NoSaveDataFound"]);
            }
        }

        private void GetSaveDataFromDB(int generation)
        {
            ComboSaveName.Items.Clear();
            ComboSaveName.Items.Add(new ComboItem { Text = (string)Application.Current.Resources["Cmb_CurrentSteamSave"], Value = 0 });
            var saveDataDict = DB.GetCurrentGenerationSaves(generation);
            foreach (var i in saveDataDict.Keys)
            {
                ComboSaveName.Items.Add(new ComboItem { Text = saveDataDict[i] + $" ({generation}-{i})", Value = (ulong)i });
            }
        }

        private void LoadLanguage()
        {
            if (string.IsNullOrEmpty(DB.GetCurrentLanguage()))
            {
                DB.SetLanguage(DB.ChooseSuitableLangCode());
            }
            else
            {
                DB.SetLanguage(DB.GetCurrentLanguage());
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

        private byte[] ReadFile(string path)
        {
            using (var stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                using (var ms = new MemoryStream())
                {
                    stream.CopyTo(ms);
                    return ms.ToArray();
                }
            }
        }

        private bool TrySaveFile(string path, byte[] blob)
        {
            try
            {
                File.WriteAllBytes(path, blob);
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