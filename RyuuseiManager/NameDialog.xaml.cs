using System.Windows;

namespace RyuuseiManager
{
    /// <summary>
    /// CreateDialog.xaml 的交互逻辑
    /// </summary>
    public partial class NameDialog : Window
    {
        public string ResultText
        {
            get { return InputBox.Text; }
            set { InputBox.Text = value; }
        }

        public NameDialog(string title, string prompt)
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
