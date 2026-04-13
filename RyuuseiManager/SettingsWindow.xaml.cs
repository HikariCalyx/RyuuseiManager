using Microsoft.WindowsAPICodePack.Dialogs;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;

namespace RyuuseiManager
{
    /// <summary>
    /// SettingsWindow.xaml 的交互逻辑
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public SettingsWindow()
        {
            InitializeComponent();
            ListLanguage();
            ListSteamPath();
            SetVersionString();
        }

        private Version? version = Assembly.GetExecutingAssembly().GetName().Version;
        public MainWindow _mainWindow { get; set; } = new MainWindow();
        private bool _internal;

        private void ListLanguage()
        {
            ComboLanguageList.Items.Clear();
            foreach (var i in Lang.LangName.LangList.Keys)
            {
                string langName = Lang.LangName.LangList[i];
                ComboItem langItem = new ComboItem { Text = langName, Value = i };
                _internal = true;
                ComboLanguageList.Items.Add(langItem);
                _internal = false;
            }
            ComboLanguageList.SelectedValue = DB.GetCurrentLanguage();
        }

        private void ListSteamPath()
        {
            ComboSteamPath.Items.Clear();
            ComboSteamPath.Items.Add(new ComboItem { Text = (string)Application.Current.Resources["Autodetect"], Value = "auto" });
            string customPath = DB.GetCustomSteamPath();
            if (!string.IsNullOrEmpty(customPath))
            {
                ComboSteamPath.Items.Add(new ComboItem { Text = customPath, Value = "manualpath" });
            }
            ComboSteamPath.Items.Add(new ComboItem { Text = (string)Application.Current.Resources["Manual"], Value = "manual" });
            _internal = true;
            switch (DB.GetSteamPathToggle())
            {
                case "unset":
                case "0":
                    ComboSteamPath.SelectedValue = "auto";
                    break;
                case "1":
                    ComboSteamPath.SelectedValue = "manualpath";
                    break;
            }
            _internal = false;
        }

        private void ComboLanguageList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboLanguageList.SelectedItem != null)
            {
                string selectedLangCode = ((ComboItem)ComboLanguageList.SelectedItem).Value;
                DB.SetLanguage(selectedLangCode);
                SetVersionString();
            }
        }

        private void ComboSteamPath_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_internal) return;
            if (ComboSteamPath.SelectedItem != null)
            {
                switch (((ComboItem)ComboSteamPath.SelectedItem).Value)
                {
                    case "auto":
                        DB.ToggleCustomSteamPath(false);
                        break;
                    case "manualpath":
                        DB.ToggleCustomSteamPath(true);
                        break;
                    case "manual":
                        var dlg = new CommonOpenFileDialog
                        {
                            IsFolderPicker = true,
                            Title = (string)Application.Current.Resources["LocateSteamPath"]
                        };
                        if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
                        {
                            if (Directory.Exists(Path.Combine(dlg.FileName, "userdata")))
                            {
                                DB.SetSteamPath(dlg.FileName);
                                DB.ToggleCustomSteamPath(true);
                            }
                            else
                            {
                                MessageBox.Show(this, (string)Application.Current.Resources["Msg_InvalidSteamPath"], (string)Application.Current.Resources["Msg_Info"]);
                            }
                        }
                        break;
                }
                ListSteamPath();
                _mainWindow.CheckSteamAccount();
            }
        }

        private void SetVersionString()
        {
            VersionText.Text = $"{Application.Current.Resources["Title_RyuuseiManager"]} | {Application.Current.Resources["Msg_Version"]} {version}";
        }

        public class ComboItem
        {
            public string Text { get; set; }
            public string Value { get; set; }
        }
    }
}
