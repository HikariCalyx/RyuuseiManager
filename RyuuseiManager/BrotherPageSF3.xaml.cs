using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RyuuseiManager
{
    /// <summary>
    /// BrotherPageSF3.xaml 的交互逻辑
    /// </summary>
    public partial class BrotherPageSF3 : Page
    {
        public BrotherPageSF3()
        {
            InitializeComponent();
            ScalingType = GetDpiScaling();
        }

        private string ScalingType;

        private string GetDpiScaling()
        {
            DpiScale dpi = VisualTreeHelper.GetDpi(this);
            if (dpi.DpiScaleX == 1.0)
            {
                return "NearestNeighbor";
            }
            else
            {
                return "HighQuality";
            }
        }

        public static readonly DependencyProperty isp1 =
        DependencyProperty.Register(
            nameof(ImageSourceMugshot),
            typeof(ImageSource),
            typeof(BrotherPageSF3),
            new PropertyMetadata(null));

        public ImageSource ImageSourceMugshot
        {
            get => (ImageSource)GetValue(isp1);
            set => SetValue(isp1, value);
        }
    }
}
