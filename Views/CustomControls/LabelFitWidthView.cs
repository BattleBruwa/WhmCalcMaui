
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace WhmCalcMaui.Views.CustomControls;

// Лэйбл с подстраивающимся под ширину размером шрифта
public class LabelFitWidthView : Label
{
    // Стандартный размер шрифта
    [System.ComponentModel.TypeConverter(typeof(FontSizeConverter))]
    public double DefaultFontSize { get; set; }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (propertyName == TextProperty.PropertyName)
        {
            AutoSizeFont();
        }
    }

    public void AutoSizeFont()
    {
        if (Width <= 0 || string.IsNullOrEmpty(Text) || DefaultFontSize == 0)
        {
            return;
        }
        // ------------------------
        FontSize = DefaultFontSize;

        string text = Text;

        var splitedStrings = text.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        int maxStringLenght = splitedStrings.Max()!.Length;

        int roundedWidth = (int)Math.Round(Width, 0);

        while ((roundedWidth / maxStringLenght) + 5 < FontSize)
        {
            FontSize -= 0.2;
        }

        while ((roundedWidth / maxStringLenght) + 5 >= FontSize && FontSize < DefaultFontSize)
        {
            FontSize += 0.2;
        }
        // ------------------------
        //        // Минимальный размер шрифта
        //        double minFontSize = 1d;

        //        var size = Measure(Width, double.PositiveInfinity);

        //        // Уменьшаем размер шрифта, если не помещаемся
        //        while (size.Width >= Width && FontSize > minFontSize)
        //        {
        //            FontSize -= 0.2;
        //#if DEBUG
        //            Debug.WriteLine($"{Text}`s font size LOWERED to {FontSize}");
        //#endif
        //            size = Measure(Width, double.PositiveInfinity);
        //        }

        //        // Увеличиваем размер шрифта вплоть до стандартного, если помещаемся
        //        while (size.Width < Width && FontSize < DefaultFontSize)
        //        {
        //            FontSize += 0.2;
        //#if DEBUG
        //            Debug.WriteLine($"{Text}`s font size INCREASED to {FontSize}");
        //#endif
        //            size = Measure(Width, double.PositiveInfinity);
        //        }
    }

//    protected override void OnSizeAllocated(double width, double height)
//    {
//        base.OnSizeAllocated(width, height);
//#if DEBUG
//        Debug.WriteLine($"{Text}`s size allocation with W:{width} H:{height}");
//#endif

//        AutoSizeFont();
//    }
}