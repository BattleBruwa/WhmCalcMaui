using System.Diagnostics;
using System.Windows.Input;
using System.Xml.Linq;

namespace WhmCalcMaui.Views.CustomControls;

public partial class TargetBoxView : ContentView
{
    public static readonly BindableProperty TextProperty =
  BindableProperty.Create(nameof(Text), typeof(string), typeof(TargetBoxView), string.Empty, BindingMode.TwoWay);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    public static readonly BindableProperty CommandProperty =
  BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(TargetBoxView), null);

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly BindableProperty CommandParameterProperty =
  BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(TargetBoxView), null);

    public object CommandParameter
    {
        get => (object)GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public TargetBoxView()
	{
		InitializeComponent();
        var tGR = new TapGestureRecognizer();
        tGR.Tapped += OnTapped;
        GestureRecognizers.Add(tGR);
    }

    private async void OnTapped(object? sender, TappedEventArgs e)
    {
        VisualStateManager.GoToState(this, "Pressed");

        await Task.Delay(50);

        VisualStateManager.GoToState(this, "Normal");

        await Task.Delay(10);

        this.Command?.Execute(CommandParameter);
#if DEBUG
        Debug.WriteLine($"{this.GetType()} is tapped!");
#endif
    }
}