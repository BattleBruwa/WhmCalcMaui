namespace WhmCalcMaui.Views.CustomControls;

public partial class EntryView : ContentView
{
    public static readonly BindableProperty TextProperty =
  BindableProperty.Create(nameof(Text), typeof(string), typeof(EntryView), string.Empty, BindingMode.TwoWay);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty IsReadOnlyProperty =
  BindableProperty.Create(nameof(IsReadOnly), typeof(bool), typeof(EntryView), false);

    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    public EntryView()
	{
		InitializeComponent();
	}
}