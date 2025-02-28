namespace WhmCalcMaui.Views.CustomControls;

public partial class OutputEntryView : ContentView
{
    // Текст первого лейбла
    public static readonly BindableProperty LabelOneTextProperty =
  BindableProperty.Create(nameof(LabelOneText), typeof(string), typeof(OutputEntryView), string.Empty, BindingMode.OneWay);

    public string LabelOneText
    {
        get => (string)GetValue(LabelOneTextProperty);
        set => SetValue(LabelOneTextProperty, value);
    }

    // Текст второго лейбла
    public static readonly BindableProperty LabelTwoTextProperty =
  BindableProperty.Create(nameof(LabelTwoText), typeof(string), typeof(OutputEntryView), string.Empty, BindingMode.OneWay);

    public string LabelTwoText
    {
        get => (string)GetValue(LabelTwoTextProperty);
        set => SetValue(LabelTwoTextProperty, value);
    }

    public OutputEntryView()
	{
		InitializeComponent();
	}
}