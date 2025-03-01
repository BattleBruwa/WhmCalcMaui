
namespace WhmCalcMaui.Views.CustomControls;

public partial class OutputEntryView : ContentView
{
    // Заголовок первого текстбокса
    public static readonly BindableProperty EntryOneHeaderProperty =
  BindableProperty.Create(nameof(EntryOneHeader), typeof(string), typeof(OutputEntryView), string.Empty);

    public string EntryOneHeader
    {
        get => (string)GetValue(EntryOneHeaderProperty);    
        set => SetValue(EntryOneHeaderProperty, value);
    }

    // Текст первого текстбокса
    public static readonly BindableProperty EntryOneTextProperty =
  BindableProperty.Create(nameof(EntryOneText), typeof(string), typeof(OutputEntryView), string.Empty, BindingMode.TwoWay);

    public string EntryOneText
    {
        get => (string)GetValue(EntryOneTextProperty);
        set => SetValue(EntryOneTextProperty, value);
    }

    // Заголовок второго текстбокса
    public static readonly BindableProperty EntryTwoHeaderProperty =
  BindableProperty.Create(nameof(EntryTwoHeader), typeof(string), typeof(OutputEntryView), string.Empty);

    public string EntryTwoHeader
    {   
        get => (string)GetValue(EntryTwoHeaderProperty);
        set => SetValue(EntryTwoHeaderProperty, value);
    }

    // Текст второго текстбокса
    public static readonly BindableProperty EntryTwoTextProperty =
  BindableProperty.Create(nameof(EntryTwoText), typeof(string), typeof(OutputEntryView), string.Empty, BindingMode.TwoWay, propertyChanged: ConModPropChanged);

    public string EntryTwoText
    {
        get => (string)GetValue(EntryTwoTextProperty);
        set => SetValue(EntryTwoTextProperty, value);
    }

    // Анимация
    private static readonly Easing animationEasing = Easing.CubicInOut;
    // Длина анимации
    private const uint animationLength = 250;

    public OutputEntryView()
	{
		InitializeComponent();
	}

    //public async void ChangeState()
    //{
    //    string? currentState = GetCurrentVisualState();

    //    switch (currentState)
    //    {
    //        case "Normal":

    //            _ = doubleStateGrid1.TranslateTo(0, 0, animationLength, animationEasing);

    //            _ = doubleStateGrid2.TranslateTo(0, 0, animationLength, animationEasing);

    //            VisualStateManager.GoToState(this, "Doubled");

    //            break;
    //        case "Doubled":

    //            Task a = doubleStateGrid1.TranslateTo(0, -30, animationLength, animationEasing);

    //            Task b = doubleStateGrid2.TranslateTo(0, 30, animationLength, animationEasing);

    //            await Task.WhenAll(b, a);

    //            //await Task.WhenAll(doubleStateGrid1.TranslateTo(0, -30, animationLength, animationEasing), doubleStateGrid2.TranslateTo(0, 30, animationLength, animationEasing));

    //            VisualStateManager.GoToState(this, "Normal");

    //            break;
    //        default:
    //            break;
    //    }
    //}

    private static async void ConModPropChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var output = (OutputEntryView)bindable;

        if (string.IsNullOrWhiteSpace(newValue as string) || (newValue as string) == "0")
        {
            //Task taskA = output.doubleStateGrid1.TranslateTo(0, 30, animationLength, animationEasing);

            //Task taskB = output.doubleStateGrid2.TranslateTo(0, -30, animationLength, animationEasing);

            await Task.WhenAll(

                output.doubleStateGrid1.TranslateTo(0, 30, animationLength, animationEasing),

                output.doubleStateGrid2.TranslateTo(0, -30, animationLength, animationEasing)

                );

            VisualStateManager.GoToState(output, "Normal");
            return;
        }

        _ = output.doubleStateGrid1.TranslateTo(0, 0, animationLength, animationEasing);

        _ = output.doubleStateGrid2.TranslateTo(0, 0, animationLength, animationEasing);

        VisualStateManager.GoToState(output, "Doubled");
    }

    /// <summary>
    /// Возвращает текущий visualstate.
    /// </summary>
    /// <returns></returns>
    private string? GetCurrentVisualState() // Переписать при изменении группы состояний!
    {
        var states = VisualStateManager.GetVisualStateGroups(this);

        var group = states[0];

        return group?.CurrentState.Name;
    }
}