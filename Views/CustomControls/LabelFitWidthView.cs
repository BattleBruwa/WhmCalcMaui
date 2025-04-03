
namespace WhmCalcMaui.Views.CustomControls;

// Лэйбл с подстраивающимся под ширину размером шрифта
public class LabelFitWidthView : Label
{
    // Стандартный размер шрифта
    [System.ComponentModel.TypeConverter(typeof(FontSizeConverter))]
    public double DefaultFontSize { get; set; }

    public LabelFitWidthView()
    {
        LineBreakMode = LineBreakMode.WordWrap;
    }

    //public new static readonly BindableProperty TextProperty =
    //  BindableProperty.Create(nameof(Text), typeof(string), typeof(LabelFitWidthView), string.Empty, BindingMode.TwoWay, propertyChanged: OnTextChanged);

    //private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
    //{
    //    (bindable as LabelFitWidthView).InvalidateMeasure();
    //}

    //public new string Text
    //{
    //    get => (string)GetValue(TextProperty);
    //    set => SetValue(TextProperty, value);
    //}

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        if (width <= 0 || height <= 0 || string.IsNullOrEmpty(Text) || DefaultFontSize == 0)
        {
            return;
        }

        // Минимальный размер шрифта
        double minFontSize = 1d;

        var size = Measure(width, double.PositiveInfinity);

        // Уменьшаем размер шрифта, если не помещаемся
        while (size.Width > width && FontSize > minFontSize)
        {
            FontSize--;
            size = Measure(width, double.PositiveInfinity);
        }

        // Увеличиваем размер шрифта вплоть до стандартного, если помещаемся
        while (size.Width < width && FontSize < DefaultFontSize)
        {
            FontSize++;
            size = Measure(width, double.PositiveInfinity);
        }
    }
}