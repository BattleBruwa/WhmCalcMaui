namespace WhmCalcMaui.Views.CustomControls;

public class CustomEntryView : ContentView
{

    public static readonly BindableProperty TextProperty =
  BindableProperty.Create(nameof(Text), typeof(string), typeof(CustomEntryView), string.Empty);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public CustomEntryView()
    {
    }
}