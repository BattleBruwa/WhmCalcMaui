using System.Globalization;

namespace WhmCalcMaui.Converters
{
    public class WeaponSkillConverter : IValueConverter
    {
        // int в string
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is null)
            {
                return string.Empty;
            }

            string str = value.ToString()!;

            return str + "+";
        }
        // string в int
        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not string str)
            {
                return string.Empty;
            }

            var res = str.Split("+", StringSplitOptions.RemoveEmptyEntries).First();

            return Int32.Parse(res);
        }
    }
}
