using System.Globalization;
using System.Text.RegularExpressions;
using WhmCalcMaui.Services.Localization;

namespace WhmCalcMaui
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            SetAppPreferences();
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        private void SetAppPreferences()
        {
            // Устанавливаем локализацию
            string cultureCode = Preferences.Default.Get("appLocalization", "en-US");

            LocalizationResourceManager.Instance.SetCulture(new CultureInfo(cultureCode));

            // Устанавливаем тему
            string appThemeCode = Preferences.Default.Get("appTheme", "Dark");
            AppTheme theme;

            switch (appThemeCode)
            {
                case "Dark": theme = AppTheme.Dark; break;
                case "Light": theme = AppTheme.Light; break;
                    default: theme = AppTheme.Dark; break;
            }

            Current.UserAppTheme = theme;
        }
    }
}