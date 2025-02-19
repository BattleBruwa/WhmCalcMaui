using System.Diagnostics;

namespace WhmCalcMaui.Views.CustomControls;

public partial class CheckBoxView : ContentView
{
    public static readonly BindableProperty NameProperty =
  BindableProperty.Create(nameof(Name), typeof(string), typeof(CheckBoxView), string.Empty);

    public string Name
    {
        get => (string)GetValue(NameProperty);
        set => SetValue(NameProperty, value);
    }

    public static readonly BindableProperty IsCheckedProperty =
  BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(CheckBoxView), false, BindingMode.TwoWay, propertyChanged: OnIsCheckedChanged);


    public bool IsChecked
    {
        get => (bool)GetValue(IsCheckedProperty);
        set => SetValue(IsCheckedProperty, value);
    }

    public CheckBoxView()
	{
		InitializeComponent();
        UpdateVisualState();
        var tGR = new TapGestureRecognizer();
        tGR.Tapped += OnTapped;
        GestureRecognizers.Add(tGR);
    }

    private void OnTapped(object? sender, TappedEventArgs e)
    {
        IsChecked = !IsChecked;
#if DEBUG
        Debug.WriteLine($"{Name} is tapped!");
#endif
    }

    private static void OnIsCheckedChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var checkbox = (CheckBoxView)bindable;
        checkbox.UpdateVisualState();
    }

    private void UpdateVisualState()
    {
        VisualStateManager.GoToState(this, IsChecked ? "Checked" : "Unchecked");
    }
}