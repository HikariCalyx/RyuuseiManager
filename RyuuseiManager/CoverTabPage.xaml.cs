using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RyuuseiManager
{
    /// <summary>
    /// CoverTabPage.xaml 的交互逻辑
    /// </summary>
    public partial class CoverTabPage : Page
    {
        public CoverTabPage()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty ImageSourceProperty =
        DependencyProperty.Register(
            nameof(ImageSource),
            typeof(ImageSource),
            typeof(CoverTabPage),
            new PropertyMetadata(null));

        public ImageSource ImageSource
        {
            get => (ImageSource)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        public void SetMessage(string message)
        {
            Message.Text = message;
        }

        public void SetSecret(string secret)
        {
            Secret.Text = secret;
        }
    }
}
