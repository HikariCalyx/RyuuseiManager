using System.Windows;

namespace RyuuseiManager
{
    /// <summary>
    /// InfoDialog.xaml 的交互逻辑
    /// </summary>
    public partial class InfoDialog : Window
    {
        public InfoDialog(string title, string prompt)
        {
            InitializeComponent();
            Title = title;
            PromptText.Text = prompt;
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
