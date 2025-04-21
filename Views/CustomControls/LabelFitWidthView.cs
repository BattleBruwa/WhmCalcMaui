
using System.Diagnostics;

namespace WhmCalcMaui.Views.CustomControls;

// Лэйбл с подстраивающимся под ширину размером шрифта
public class LabelFitWidthView : Label
{
    // Стандартный размер шрифта
    [System.ComponentModel.TypeConverter(typeof(FontSizeConverter))]
    public double DefaultFontSize { get; set; }

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

    //protected override Size MeasureOverride(double widthConstraint, double heightConstraint)
    //{
    //    if (double.IsInfinity(widthConstraint))
    //    {
    //        widthConstraint = DeviceDisplay.MainDisplayInfo.Width;
    //    }

    //    if (double.IsInfinity(heightConstraint))
    //    {
    //        heightConstraint = DeviceDisplay.MainDisplayInfo.Height;
    //    }

    //    return base.MeasureOverride(widthConstraint, heightConstraint);
    //}

    public void AutoSizeFont()
    {
        if (Width <= 0 || string.IsNullOrEmpty(Text) || DefaultFontSize == 0)
        {
            return;
        }

        // Минимальный размер шрифта
        double minFontSize = 1d;

        var size = Measure(Width, double.PositiveInfinity);

        // Уменьшаем размер шрифта, если не помещаемся
        while (size.Width >= Width && FontSize > minFontSize)
        {
            FontSize -= 0.2;
#if DEBUG
            Debug.WriteLine($"{Text}`s font size LOWERED to {FontSize}");
#endif
            size = Measure(Width, double.PositiveInfinity);
        }

        // Увеличиваем размер шрифта вплоть до стандартного, если помещаемся
        while (size.Width < Width && FontSize < DefaultFontSize)
        {
            FontSize += 0.2;
#if DEBUG
            Debug.WriteLine($"{Text}`s font size INCREASED to {FontSize}");
#endif
            size = Measure(Width, double.PositiveInfinity);
        }
    }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);
#if DEBUG
        Debug.WriteLine($"{Text}`s size allocation with W:{width} H:{height}");
#endif

        AutoSizeFont();

//        if (width <= 0 || height <= 0 || string.IsNullOrEmpty(Text) || DefaultFontSize == 0)
//        {
//            return;
//        }

//        // Минимальный размер шрифта
//        double minFontSize = 1d;

//        var size = Measure(width, double.PositiveInfinity);

//        // Уменьшаем размер шрифта, если не помещаемся
//        while (size.Width > width && FontSize > minFontSize)
//        {
//            FontSize -= 0.2;
//#if DEBUG
//            Debug.WriteLine($"{Text}`s font size LOWERED to {FontSize}");
//#endif
//            size = Measure(width, double.PositiveInfinity);
//        }

//        // Увеличиваем размер шрифта вплоть до стандартного, если помещаемся
//        while (size.Width <= width && FontSize < DefaultFontSize)
//        {
//            FontSize += 0.2;
//#if DEBUG
//            Debug.WriteLine($"{Text}`s font size INCREASED to {FontSize}");
//#endif
//            size = Measure(width, double.PositiveInfinity);
//        }
    }
}