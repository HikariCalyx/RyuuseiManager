using Microsoft.WindowsAPICodePack.Dialogs;
using RyuuseiManager.BinaryMagic;
using RyuuseiManager.Classes;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace RyuuseiManager
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Nested Types

        public class ComboItem
        {
            public string Text { get; set; }
            public ulong Value { get; set; }
        }

        #endregion

        #region Fields

        private readonly API.MandarinKey key = new API.MandarinKey();

        #endregion

        #region Properties

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

        #endregion

        #region Constructor & Initialization

        public MainWindow()
        {
            InitializeComponent();
            LoadLanguage();
            DB.InitDatabase();
            CheckSteamAccount();
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

        private int GetLanguageID()
        {
            switch (DB.GetCurrentLanguage())
            {
                default: return 0;
                case "ja-JP": return 1;
                case "zh-CN": return 2;
                case "zh-TW": return 3;
            }
        }

        #endregion

        #region ComboBox Event Handlers

        private void ComboSteamUser_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboSteamUser.SelectedItem is ComboItem item)
            {
                ComboGameTitle.IsEnabled = true;
                ComboGameTitle.Items.Clear();
                ComboSaveName.Items.Clear();
                ComboSaveName.IsEnabled = false;
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
            ButtonCreateSave.IsEnabled = false;
            ButtonDuplicate.IsEnabled = false;
            ButtonDeleteSave.IsEnabled = false;
            ButtonRenameSave.IsEnabled = false;
            ButtonReplaceSave.IsEnabled = false;
            ButtonLoadSaveData.IsEnabled = false;
            ButtonLoadAndRun.IsEnabled = false;
            ButtonExportSave.IsEnabled = false;
            if (ComboSaveName.Items.Count > 0) ComboSaveName.SelectedIndex = 0;
        }

        private void ComboSaveName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshSaveInfo();
        }

        #endregion

        #region Button Event Handlers — Save Management

        private void ButtonImportSave_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new CommonOpenFileDialog
            {
                IsFolderPicker = false,
                Title = (string)Application.Current.Resources["Msg_ImportSave"]
            };
            dlg.Filters.Add(new CommonFileDialogFilter("", "*.bin"));
            if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
            {
                int gameGen = 0;
                byte[] saveBlob = ReadFile(dlg.FileName);
                if (saveBlob.AsSpan().StartsWith(BinaryMagic.HeaderMagic.Switch))
                {
                    saveBlob = BinaryMagic.Processor.StripSwitchSave(saveBlob);
                }
                if (!saveBlob.AsSpan().StartsWith(BinaryMagic.HeaderMagic.Raw))
                {
                    ShowInfoDialog((string)Application.Current.Resources["Msg_InvalidSave"]);
                    return;
                }
                else if (!CheckSave(saveBlob, out gameGen))
                {
                    ShowInfoDialog((string)Application.Current.Resources["Msg_InvalidSave"]);
                    return;
                }
                var namedlg = new NameDialog(
                    title: (string)Application.Current.Resources["Dlg_ImportSaveData"],
                    prompt: string.Format((string)Application.Current.Resources["Msg_SpecifyName"], AssembleGameName(gameGen))
                        .Replace("\\n", Environment.NewLine + Environment.NewLine));
                namedlg.Owner = this;
                if (namedlg.ShowDialog() == true)
                {
                    string saveName = namedlg.ResultText;
                    DB.SaveDataBlob(saveBlob, saveName, gameGen, true, out ulong saveId);
                    if (ComboGameTitle.SelectedItem != null) GetSaveDataFromDB(GameGen);
                    ComboSaveName.SelectedValue = saveId;
                }
            }
        }

        private void ButtonCreateSave_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new NameDialog(
                title: (string)Application.Current.Resources["Dlg_CreateSave"],
                prompt: (string)Application.Current.Resources["Msg_SpecifyNameCreate"]);
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

        private void ButtonDuplicate_Click(object sender, RoutedEventArgs e)
        {
            if (ComboSaveName.SelectedItem is ComboItem nameItem)
            {
                var dlg = new NameDialog(
                    title: (string)Application.Current.Resources["Dlg_Duplicate"],
                    prompt: (string)Application.Current.Resources["Msg_SpecifyNewName"]);
                dlg.Owner = this;
                dlg.ResultText = nameItem.Text;
                if (dlg.ShowDialog() == true)
                {
                    string saveName = dlg.ResultText;
                    if (saveName == nameItem.Text) return;

                    ulong resultSaveId;
                    if (SaveID == 0)
                    {
                        byte[] decSave = GetDecryptedSteamSave();
                        DB.SaveDataBlob(decSave, saveName, GameGen, true, out resultSaveId);
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

        private void ButtonRenameSave_Click(object sender, RoutedEventArgs e)
        {
            if (ComboSaveName.SelectedItem is ComboItem nameItem)
            {
                var dlg = new NameDialog(
                    title: (string)Application.Current.Resources["Dlg_Rename"],
                    prompt: (string)Application.Current.Resources["Msg_SpecifyNewName"]);
                dlg.Owner = this;
                dlg.ResultText = DB.GetSaveName(GameGen, (ulong)nameItem.Value);
                if (dlg.ShowDialog() == true)
                {
                    string saveName = dlg.ResultText;
                    if (saveName == nameItem.Text) return;

                    if (SaveID == 0)
                    {
                        byte[] decSave = GetDecryptedSteamSave();
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

        private void ButtonReplaceSave_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new ConfirmDialog(
                title: (string)Application.Current.Resources["Msg_Confirm"],
                prompt: ((string)Application.Current.Resources["Msg_ReplaceConfirm"])
                    .Replace("\\n", Environment.NewLine + Environment.NewLine));
            dlg.Owner = this;
            if (dlg.ShowDialog() == true)
            {
                byte[] decSave = GetDecryptedSteamSave();
                DB.ReplaceSaveBlob(decSave, SaveID);
                RefreshSaveInfo();
            }
        }

        private void ButtonDeleteSave_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new ConfirmDialog(
                title: (string)Application.Current.Resources["Msg_Confirm"],
                prompt: (string)Application.Current.Resources["Msg_DeleteConfirm"]);
            if (dlg.ShowDialog() == true)
            {
                DB.DeleteSaveById(SaveID);
                GetSaveDataFromDB(GameGen);
            }
        }

        private void ButtonExportSave_Click(object sender, RoutedEventArgs e)
        {
            byte[]? rawSaveData = (SaveID == 0) ? GetDecryptedSteamSave() : DB.LoadDataBlob(SaveID);
            if (rawSaveData != null)
            {
                var dlg = new CommonSaveFileDialog
                {
                    Title = (string)Application.Current.Resources["Msg_ExportSave"],
                    DefaultFileName = $"data0{GameGen}Slot.bin",
                    DefaultExtension = "bin",
                    EnsureValidNames = true,
                    EnsurePathExists = true
                };
                dlg.Filters.Add(new CommonFileDialogFilter("", "*.bin"));
                if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
                {
                    rawSaveData = BinaryMagic.Processor.RepopulateFooter(rawSaveData, GameGen);
                    if (!TrySaveFile(dlg.FileName, BinaryMagic.Processor.PopulateToSwitchSave(rawSaveData, GameGen / 10)))
                    {
                        ShowInfoDialog((string)Application.Current.Resources["Msg_UnableToSave"]);
                    }
                }
            }
        }

        private void ButtonLoadSaveData_Click(object sender, RoutedEventArgs e)
        {
            LoadSave(prompts: true);
        }

        private void ButtonLoadAndRun_Click(object sender, RoutedEventArgs e)
        {
            if (LoadSave(prompts: false)) RunGame();
        }

        #endregion

        #region Button Event Handlers — Settings & Game

        private void ButtonSettings_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new SettingsWindow();
            dlg.Owner = this;
            dlg._mainWindow = this;
            dlg.ShowDialog();
        }

        private void ButtonRunGame_Click(object sender, RoutedEventArgs e)
        {
            RunGame();
        }

        #endregion

        #region Public Methods

        public void CheckSteamAccount()
        {
            ComboSteamUser.Items.Clear();
            ComboGameTitle.Items.Clear();
            ComboSaveName.Items.Clear();
            MainTabs.Items.Clear();
            ComboGameTitle.IsEnabled = false;
            ComboSaveName.IsEnabled = false;
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

        public void ToggleVisibility()
        {
            if (MainTabs.Items.Count > 0)
            {
                int selectedIndex = MainTabs.SelectedIndex;
                RefreshSaveInfo();
                MainTabs.SelectedIndex = selectedIndex;
            }
        }

        #endregion

        #region Save Info Display

        private void RefreshSaveInfo()
        {
            if (ComboSaveName.SelectedItem is not ComboItem) // itemSave unused — checked for null only
            {
                MainTabs.Items.Clear();
                return;
            }

            var coverTabFrame = CreateHiddenFrame();
            var battleCardFrame = CreateHiddenFrame();
            var brotherFrame = CreateHiddenFrame();

            byte[] saveBlob = GetCurrentSave();
            bool showOtherLanguage = DB.GetToggleSwitch("CheckShowLanguage") == 1;
            List<Folder> folders = GetFolder(saveBlob);

            if (ComboGameTitle.SelectedItem is ComboItem item)
            {
                int gameGen = (int)item.Value;
                if (IsSF3Game(gameGen))
                {
                    BuildSF3CoverAndBattleTabs(saveBlob, coverTabFrame, battleCardFrame, showOtherLanguage, folders, gameGen);
                    BuildBrotherTab(saveBlob, brotherFrame);
                }
                else
                {
                    BuildDefaultCoverTab(saveBlob, coverTabFrame);
                }
            }

            PopulateMainTabs(coverTabFrame, battleCardFrame, brotherFrame);
            UpdateButtonStates();
        }

        private static Frame CreateHiddenFrame()
        {
            return new Frame
            {
                NavigationUIVisibility = System.Windows.Navigation.NavigationUIVisibility.Hidden
            };
        }

        private void BuildDefaultCoverTab(byte[] saveBlob, Frame coverFrame)
        {
            var page = new CoverTabPage();
            page.ImageSource = GetMugshot(saveBlob);
            page.SetMessage(GetMessage(saveBlob));
            page.SetSecret(GetSecret(saveBlob));
            coverFrame.Navigate(page);
        }

        private void BuildSF3CoverAndBattleTabs(byte[] saveBlob, Frame coverFrame, Frame battleCardFrame,
            bool showOtherLanguage, List<Folder> folders, int gameGen)
        {
            // Cover tab
            var coverPage = new CoverTabPageSF3();
            coverPage.ImageSourceMugshot = GetMugshot(saveBlob);
            coverPage.ImageSourceTeamIcon = GameResourceRetriver.GetTeamIcon(Processor.GetSF3TeamIconID(saveBlob));
            coverPage.SetMessage(GetMessage(saveBlob));
            coverPage.SetSecret(GetSecret(saveBlob));
            coverPage.SetNoiseForm("Noise Form"); // TBA
            coverPage.SetPurpose(Processor.GetSF3TeamPurpose(saveBlob)); // TBA
            coverPage.GameVersion = (gameGen - 30) / 2;
            coverPage.ProfileLanguage = GetLanguageID();
            coverPage.UpdateWarRockWeapon(Library.SF3.Weapons.weapons[5]);
            List<int> abilityList = Processor.GetAbilities(saveBlob, 3);
            coverPage.SetAbilities(abilityList);
            coverPage.SetProfileColor();
            coverFrame.Navigate(coverPage);

            // Battle card tab
            int whiteCardIndex = Processor.GetSF3SelfWhiteCard(saveBlob);
            int equippedFolder = Processor.GetSF3EquippedFolder(saveBlob);
            var whiteCardCombo = Library.SF3.WhiteCardCombo.GetWhiteCard(whiteCardIndex);

            var battleCardPage = new BattleCardPageSF3();
            battleCardPage._mainWindow = this;
            battleCardPage.WCard = whiteCardCombo;
            battleCardPage.WCardIndex = whiteCardIndex;
            battleCardPage.EquippedFolderIndex = equippedFolder;
            battleCardPage.ProfileLanguage = GetLanguageID();
            battleCardPage.ShowOtherLanguage = showOtherLanguage;
            if (folders.Count > 0) battleCardPage.Folders = folders;
            battleCardPage.SetFolderNames();
            battleCardFrame.Navigate(battleCardPage);
        }

        private void BuildBrotherTab(byte[] saveBlob, Frame brotherFrame)
        {
            var brotherPage = new BrotherPageSF3();
            brotherPage.ImageSourceMugshot = GetMugshot(saveBlob);
            brotherFrame.Navigate(brotherPage);
        }

        private void PopulateMainTabs(Frame coverFrame, Frame battleCardFrame, Frame brotherFrame)
        {
            var coverTab = new TabItem
            {
                Header = (string)Application.Current.Resources["Tab_Cover"],
                Content = coverFrame
            };

            var defaultBattleCardTab = new TabItem
            {
                Header = (string)Application.Current.Resources["Tab_BattleCard"],
                Content = new TextBlock
                {
                    Text = (string)Application.Current.Resources["Msg_TBA"],
                    Margin = new Thickness(10)
                }
            };

            var battleCardTab = new TabItem
            {
                Header = (string)Application.Current.Resources["Tab_BattleCard"],
                Content = battleCardFrame
            };

            var brotherTab = new TabItem
            {
                Header = (string)Application.Current.Resources["Tab_Brother"],
                Content = brotherFrame
            };

            MainTabs.Items.Clear();
            MainTabs.Items.Add(coverTab);

            if (ComboGameTitle.SelectedItem is ComboItem itemTitle)
            {
                if (IsSF3Game((int)itemTitle.Value))
                {
                    MainTabs.Items.Add(battleCardTab);
                    MainTabs.Items.Add(brotherTab);
                }
                else
                {
                    MainTabs.Items.Add(defaultBattleCardTab);
                    MainTabs.Items.Add(brotherTab);
                }
            }
        }

        private void UpdateButtonStates()
        {
            ButtonDuplicate.IsEnabled = true;
            ButtonRenameSave.IsEnabled = true;
            ButtonDeleteSave.IsEnabled = SaveID > 0;
            ButtonExportSave.IsEnabled = true;
            ButtonReplaceSave.IsEnabled = SaveID > 0;
            ButtonLoadSaveData.IsEnabled = SaveID > 0;
            ButtonLoadAndRun.IsEnabled = SaveID > 0;
        }

        #endregion

        #region Save Data Helpers

        private byte[] GetCurrentSave()
        {
            if (SaveID == 0)
            {
                return GetDecryptedSteamSave();
            }
            else
            {
                return DB.LoadDataBlob(SaveID);
            }
        }

        /// <summary>
        /// Reads the current Steam save file from disk and decrypts it.
        /// </summary>
        private byte[] GetDecryptedSteamSave()
        {
            string? savePath = API.SteamInterop.GetSaveDataPath(SteamID);
            byte[] steamRawSave = ReadFile(Path.Combine(savePath, $"data0{GameGen}Slot.bin"));
            return key.DecryptBlob(steamRawSave, API.SteamInterop.GetSteamID64(SteamID));
        }

        private bool LoadSave(bool prompts)
        {
            byte[]? rawSaveData = DB.LoadDataBlob(SaveID);
            if (rawSaveData == null)
                return false;

            if (!WriteSaveToSteam(rawSaveData))
                return false;

            if (prompts)
            {
                ShowInfoDialog((string)Application.Current.Resources["Msg_ImportComplete"]);
            }
            return true;
        }

        private bool WriteSaveToSteam(byte[] rawSaveData)
        {
            string? savePath = API.SteamInterop.GetSaveDataPath(SteamID);
            if (string.IsNullOrEmpty(savePath) || !CanWriteToPath(savePath))
            {
                ShowInfoDialog((string)Application.Current.Resources["Msg_RunElevate"]);
                return false;
            }

            rawSaveData = BinaryMagic.Processor.RepopulateFooter(rawSaveData, GameGen);
            byte[] signedSave = key.EncryptBlob(rawSaveData, API.SteamInterop.GetSteamID64(SteamID));
            if (!TrySaveFile(Path.Combine(savePath, $"data0{GameGen}Slot.bin"), signedSave))
            {
                ShowInfoDialog((string)Application.Current.Resources["Msg_UnableToSave"]);
                return false;
            }
            return true;
        }

        public bool ApplyWhiteCardCombo(int newCombo)
        {
            byte[] existingSave = GetCurrentSave();
            BackupExistingSave(existingSave);

            byte[] modifiedSave = (byte[])existingSave.Clone();
            if (!BinaryMagic.Processor.SetSF3SelfWhiteCard(modifiedSave, newCombo))
            {
                ShowInfoDialog((string)Application.Current.Resources["Msg_UnableToSave"]);
                return false;
            }

            if (SaveID == 0)
            {
                if (!WriteSaveToSteam(modifiedSave))
                    return false;
            }
            else
            {
                DB.ReplaceSaveBlob(modifiedSave, SaveID);
            }

            ToggleVisibility();
            return true;
        }

        private void BackupExistingSave(byte[] existingSave)
        {
            string backupDir = Path.Combine(AppContext.BaseDirectory, "backups");
            Directory.CreateDirectory(backupDir);

            string stamp = DateTime.Now.ToString("yyMMdd_HHmm");
            string plainName;
            string archivedName;
            if (SaveID == 0)
            {
                plainName = "steam_backup.bin";
                archivedName = $"steam_backup_{stamp}.bin";
            }
            else
            {
                string oldPresetName = SanitizeFileName(DB.GetSaveName(GameGen, SaveID) ?? "preset");
                plainName = $"{oldPresetName}_backup.bin";
                archivedName = $"{oldPresetName}_backup_{stamp}.bin";
            }

            string plainPath = Path.Combine(backupDir, plainName);
            string archivedPath = Path.Combine(backupDir, archivedName);
            if (File.Exists(plainPath))
            {
                File.Move(plainPath, archivedPath, true);
            }
            File.WriteAllBytes(plainPath, existingSave);
        }

        private static string SanitizeFileName(string name)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(c, '_');
            }
            return name;
        }

        private void GetSaveDataFromDB(int generation)
        {
            ComboSaveName.Items.Clear();
            ComboSaveName.Items.Add(new ComboItem
            {
                Text = (string)Application.Current.Resources["Cmb_CurrentSteamSave"],
                Value = 0
            });

            var saveDataDict = DB.GetCurrentGenerationSaves(generation);
            foreach (var i in saveDataDict.Keys)
            {
                ComboSaveName.Items.Add(new ComboItem
                {
                    Text = saveDataDict[i] + $" ({generation}-{i})",
                    Value = (ulong)i
                });
            }

            int extraGeneration = GetExtraGeneration(generation);
            if (extraGeneration != 0)
            {
                var extraSaveDataDict = DB.GetCurrentGenerationSaves(extraGeneration);
                foreach (var i in extraSaveDataDict.Keys)
                {
                    ComboSaveName.Items.Add(new ComboItem
                    {
                        Text = extraSaveDataDict[i] + $" ({extraGeneration}-{i})",
                        Value = (ulong)i
                    });
                }
            }
        }

        /// <summary>
        /// Returns the paired save-slot generation for games that share
        /// save data across two slots (SF2 Zerker ×2, SF3 ×2).
        /// Returns 0 if the generation has no paired slot.
        /// </summary>
        private static int GetExtraGeneration(int generation)
        {
            switch (generation)
            {
                case 22: return 23;
                case 23: return 22;
                case 30: return 31;
                case 31: return 30;
                case 32: return 33;
                case 33: return 32;
                default: return 0;
            }
        }

        #endregion

        #region Save Data Extraction

        private BitmapImage GetMugshot(byte[] saveBlob)
        {
            int gameID = GameGen / 10;
            int mugshotID = BinaryMagic.Processor.GetMugshotID(saveBlob, gameID);
            return GameResourceRetriver.GetMugshot(mugshotID);
        }

        private string GetMessage(byte[] saveBlob)
        {
            int gameID = GameGen / 10;
            return BinaryMagic.Processor.GetMessage(saveBlob, gameID);
        }

        private string GetSecret(byte[] saveBlob)
        {
            int gameID = GameGen / 10;
            return BinaryMagic.Processor.GetSecret(saveBlob, gameID);
        }

        private List<Folder> GetFolder(byte[] saveBlob)
        {
            int gameID = GameGen / 10;
            return BinaryMagic.Processor.GetFolders(saveBlob, gameID);
        }

        #endregion

        #region Steam & Game Helpers

        private void GetAvailableSteamSaveData(ulong steamID3)
        {
            string? saveDataDir = API.SteamInterop.GetSaveDataPath(steamID3);
            if (string.IsNullOrEmpty(saveDataDir))
            {
                MessageBox.Show((string)Application.Current.Resources["Msg_NoSaveDataFound"]);
                return;
            }

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

        private void RunGame()
        {
            if (API.WineCheck.IsRunningUnderWine())
            {
                MessageBox.Show((string)Application.Current.Resources["Msg_WineCheck"]);
            }
            else
            {
                Process.Start(new ProcessStartInfo
                {
                    UseShellExecute = true,
                    FileName = "steam://rungameid/3500390",
                });
            }
        }

        #endregion

        #region Validation Helpers

        private bool CheckSave(byte[] blob, out int gameGen)
        {
            gameGen = BinaryMagic.Processor.GetGameGen(blob);
            byte expectedNextByte;
            switch (gameGen)
            {
                case 10: case 11: case 12:
                    expectedNextByte = 0x43; break;
                case 20: case 21: case 22: case 23:
                    expectedNextByte = 0x45; break;
                case 30: case 31: case 32: case 33:
                    expectedNextByte = 0x53; break;
                default:
                    return false;
            }
            if (BinaryMagic.Processor.TryGetNextByte(blob, BinaryMagic.HeaderMagic.Raw, out byte nextByte))
            {
                return expectedNextByte == nextByte;
            }
            return false;
        }

        private static bool IsSF3Game(int gameGen)
        {
            return gameGen >= 30 && gameGen <= 33;
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

        #endregion

        #region File I/O Helpers

        private static byte[] ReadFile(string path)
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

        private static bool TrySaveFile(string path, byte[] blob)
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

        #endregion

        #region UI Helpers

        private void ShowInfoDialog(string prompt)
        {
            var infoDlg = new InfoDialog(
                title: (string)Application.Current.Resources["Msg_Info"],
                prompt: prompt);
            infoDlg.Owner = this;
            infoDlg.ShowDialog();
        }

        private string AssembleGameName(int gameGen)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append((string)Application.Current.Resources["MMSF"] + (gameGen / 10) + " ");
            switch (gameGen)
            {
                case 10:
                    sb.Append((string)Application.Current.Resources["Pegasus"]);
                    break;
                case 11:
                    sb.Append((string)Application.Current.Resources["Leo"]);
                    break;
                case 12:
                    sb.Append((string)Application.Current.Resources["Dragon"]);
                    break;
                case 20:
                    sb.Append((string)Application.Current.Resources["Ninja"]);
                    break;
                case 21:
                    sb.Append((string)Application.Current.Resources["Saurian"]);
                    break;
                case 22:
                case 23:
                    sb.Append((string)Application.Current.Resources["Zerker"]);
                    break;
                case 30:
                case 31:
                    sb.Append((string)Application.Current.Resources["BlackAce"]);
                    break;
                case 32:
                case 33:
                    sb.Append((string)Application.Current.Resources["RedJoker"]);
                    break;
            }
            return sb.ToString();
        }

        #endregion
    }
}