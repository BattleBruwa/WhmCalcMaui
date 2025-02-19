using System.Collections.ObjectModel;
using System.Globalization;
using WhmCalcMaui.Models;

namespace WhmCalcMaui.Converters
{
    class IndexToCollectionItemConverter : IValueConverter
    {
        public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            if (value is not int index)
                throw new ArgumentException("Value is not a valid integer", nameof(value));

            if (parameter is not Collection<ModificatorModel> collection)
                throw new ArgumentException("Parameter is not a valid collection", nameof(parameter));

            if (index < 0 || index >= collection.Count)
                throw new ArgumentOutOfRangeException(nameof(value), "Index was out of range");

            var result = collection[index];

            return result;
        }

        public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
