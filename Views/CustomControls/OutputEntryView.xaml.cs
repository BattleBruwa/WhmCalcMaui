namespace WhmCalcMaui.Views.CustomControls;

public partial class OutputEntryView : ContentView
{
    // Заголовок первого текстбокса
    public static readonly BindableProperty EntryOneHeaderProperty =
  BindableProperty.Create(nameof(EntryOneHeader), typeof(string), typeof(OutputEntryView), string.Empty, BindingMode.TwoWay);

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
  BindableProperty.Create(nameof(EntryTwoHeader), typeof(string), typeof(OutputEntryView), string.Empty, BindingMode.TwoWay);

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
    private static readonly Easing animationEasing = Easing.Linear;
    // Длина анимации
    private const uint animationLength = 300;

    public OutputEntryView()
    {
        InitializeComponent();
    }

    private static async void ConModPropChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var output = (OutputEntryView)bindable;

        if (string.IsNullOrWhiteSpace(newValue as string) || (newValue as string) == "0")
        {
            double test = (double)output.Parent.GetValue(HeightProperty);

            await Task.WhenAll(

                output.TranslateBoxes(output.doubleStateGrid1, /*(double)output.Parent.GetValue(HeightProperty) / 6*/ 27),
                output.TranslateBoxes(output.doubleStateGrid2, /*(double)output.Parent.GetValue(HeightProperty) / -3*/ -54),
                output.upperBoxHeader.FadeTo(0, animationLength, animationEasing)

                );

            VisualStateManager.GoToState(output, "Normal");
            return;
        }

        _ = output.TranslateBoxes(output.doubleStateGrid1, 0);
        _ = output.TranslateBoxes(output.doubleStateGrid2, 0);
        _ = output.upperBoxHeader.FadeTo(1, animationLength, animationEasing);

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

    private Task<bool> TranslateBoxes(VisualElement view, double y)
    {
        var tcs = new TaskCompletionSource<bool>();

        new Animation(v => view.TranslationY = v, view.TranslationY, y)
            .Commit(view, nameof(TranslateBoxes), 8, animationLength, animationEasing, (f, a) => tcs.SetResult(a));

        return tcs.Task;
    }
}