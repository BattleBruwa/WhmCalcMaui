using System.Diagnostics;
using System.Windows.Input;

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


    public static readonly BindableProperty CommandProperty =
  BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(CheckBoxView), null);

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }


    public static readonly BindableProperty CommandParameterProperty =
  BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(CheckBoxView), null);

    public object CommandParameter
    {
        get => (object)GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public CheckBoxView()
	{
		InitializeComponent();  
        UpdateVisualState();
        var tGR = new TapGestureRecognizer();
        tGR.Tapped += OnTapped;
        GestureRecognizers.Add(tGR);
    }

    private async void OnTapped(object? sender, TappedEventArgs e)
    {
        VisualStateManager.GoToState(this, "Pressed");
        await Task.Delay(50);

        IsChecked = !IsChecked;

        this.Command?.Execute(CommandParameter);
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