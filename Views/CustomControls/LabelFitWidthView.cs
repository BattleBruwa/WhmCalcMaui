namespace WhmCalcMaui.Views.CustomControls;

public class LabelFitWidthView : Label
{
    [System.ComponentModel.TypeConverter(typeof(FontSizeConverter))]
    public double DefaultFontSize { get; set; }

    protected override void OnSizeAllocated(double width, double height)
    {
        base.OnSizeAllocated(width, height);

        if (width <= 0 || height <= 0 || string.IsNullOrEmpty(Text) || DefaultFontSize == 0)
        {
            return;
        }

        double minFontSize = 1d;

        var size = Measure(width, double.PositiveInfinity);

        while (size.Width > width && FontSize > minFontSize)
        {
            FontSize--;
            size = Measure(width, double.PositiveInfinity);
        }

        while (size.Width < width && FontSize < DefaultFontSize)
        {
            FontSize++;
            size = Measure(width, double.PositiveInfinity);
        }
    }
}