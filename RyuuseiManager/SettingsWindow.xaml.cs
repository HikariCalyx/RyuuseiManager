using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

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
            SetVersionString();
        }

        private Version? version = Assembly.GetExecutingAssembly().GetName().Version;

        private void ListLanguage()
        {
            ComboLanguageList.Items.Clear();
            foreach (var i in Lang.LangName.LangList.Keys)
            {
                string langName = Lang.LangName.LangList[i];
                ComboItem langItem = new ComboItem { Text = langName, Value = i };
                ComboLanguageList.Items.Add(langItem);
            }
            ComboLanguageList.SelectedValue = DB.GetCurrentLanguage();
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
