using System.Diagnostics;
using System.Windows.Input;
using System.Xml.Linq;

namespace WhmCalcMaui.Views.CustomControls;

public partial class ButtonView : ContentView
{
    public static readonly BindableProperty CommandProperty =
  BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ButtonView), null);

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }

    public static readonly BindableProperty CommandParameterProperty =
  BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ButtonView), null);

    public object CommandParameter
    {
        get => (object)GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }

    public ButtonView()
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

        this.Command?.Execute(CommandParameter);

        VisualStateManager.GoToState(this, "Normal");
#if DEBUG
        Debug.WriteLine($"{GetType().Name} is tapped!");
#endif
    }
}