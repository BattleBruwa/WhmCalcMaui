using System.ComponentModel;
using System.Globalization;
using WhmCalcMaui.Resources.Localization;

namespace WhmCalcMaui.Services.Localization
{
    // Решение с менеджером подсмотрено у товарища Gerald Versluis
    // https://youtu.be/cf4sXULR7os

    public class LocalizationResourceManager : INotifyPropertyChanged
    {
        public static LocalizationResourceManager Instance { get; } = new();

        public event PropertyChangedEventHandler? PropertyChanged;

        public object this[string resourceKey] 
            => AppStrings.ResourceManager.GetObject(resourceKey, AppStrings.Culture) ?? Array.Empty<byte>();

        private LocalizationResourceManager()
        {
            AppStrings.Culture = new CultureInfo("en");
        }

        public void SetCulture(CultureInfo culture)
        {
            AppStrings.Culture = culture;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        }
    }
}
