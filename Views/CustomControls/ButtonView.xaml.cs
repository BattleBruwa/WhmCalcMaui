using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

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

    public event EventHandler? Clicked; 

    public ButtonView()
    {
        InitializeComponent();
        var tGR = new TapGestureRecognizer();
        tGR.Tapped += OnTapped;
        GestureRecognizers.Add(tGR);
    }

    protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        base.OnPropertyChanged(propertyName);

        if (string.Equals(propertyName, "Content"))
        {
            InvalidateMeasure();
        }
    }

    private async void OnTapped(object? sender, TappedEventArgs e)
    {
        if (IsEnabled)
        {
            VisualStateManager.GoToState(this, "Pressed");

            await Task.Delay(50);

            Command?.Execute(CommandParameter);
            Clicked?.Invoke(this, EventArgs.Empty);

            VisualStateManager.GoToState(this, "Normal");
        }
#if DEBUG
        Debug.WriteLine($"{GetType().Name} is tapped!");
#endif
    }
}