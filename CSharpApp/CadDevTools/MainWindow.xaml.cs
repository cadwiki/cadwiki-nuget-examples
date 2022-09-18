using System.Windows;
using cadwiki.NetUtils;
namespace CadDevTools
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonTest_Click(object sender, RoutedEventArgs e)
        {
            var window = new cadwiki.CadDevTools.MainWindow();
            window.Show();
        }
    }
}
