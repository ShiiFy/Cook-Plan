using Cook_Plan.Themes;
using Cook_Plan.Themes.Dark;
using Cook_Plan.Themes.Light;
using System.Windows;
using System.Windows.Media;

namespace Cook_Plan
{
    public partial class MainWindow : Window
    {
        private IThemeFactory _themeFactory;

        public MainWindow()
        {
            InitializeComponent();
            // По умолчанию светлая тема
            _themeFactory = new LightThemeFactory();
            ApplyTheme();
        }

        private void ApplyTheme()
        {
            var windowStyle = _themeFactory.CreateWindowStyle();
            var buttonStyle = _themeFactory.CreateButtonStyle();

            // Применяем стиль к окну
            RootWindow.Background = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString(windowStyle.Background));
            TitleText.Foreground = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString(windowStyle.Foreground));

            // Применяем стиль к кнопкам
            var btnBg = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString(buttonStyle.Background));
            var btnFg = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString(buttonStyle.Foreground));
            var btnBorder = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString(buttonStyle.BorderColor));

            foreach (var btn in new[] { LightBtn, DarkBtn })
            {
                btn.Background = btnBg;
                btn.Foreground = btnFg;
                btn.BorderBrush = btnBorder;
            }
        }

        private void LightBtn_Click(object sender, RoutedEventArgs e)
        {
            _themeFactory = new LightThemeFactory();
            ApplyTheme();
        }

        private void DarkBtn_Click(object sender, RoutedEventArgs e)
        {
            _themeFactory = new DarkThemeFactory();
            ApplyTheme();
        }
    }
}