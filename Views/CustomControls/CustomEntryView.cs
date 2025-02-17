namespace WhmCalcMaui.Views.CustomControls;

// Визуал задается через ControlTemplate в Styles
public class CustomEntryView : ContentView
{

    public static readonly BindableProperty TextProperty =
  BindableProperty.Create(nameof(Text), typeof(string), typeof(CustomEntryView), string.Empty);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }


    public static readonly BindableProperty IsReadOnlyProperty =
  BindableProperty.Create(nameof(IsReadOnly), typeof(bool), typeof(CustomEntryView), false);

    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    public CustomEntryView()
    {
    }
}