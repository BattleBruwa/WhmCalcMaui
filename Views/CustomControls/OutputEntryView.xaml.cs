

namespace WhmCalcMaui.Views.CustomControls;

public partial class OutputEntryView : ContentView
{
    // ��������� ������� ����������
    public static readonly BindableProperty EntryOneHeaderProperty =
  BindableProperty.Create(nameof(EntryOneHeader), typeof(string), typeof(OutputEntryView), string.Empty, BindingMode.TwoWay, propertyChanged: TextPropertyChanged);

    public string EntryOneHeader
    {
        get => (string)GetValue(EntryOneHeaderProperty);
        set => SetValue(EntryOneHeaderProperty, value);
    }

    // ����� ������� ����������
    public static readonly BindableProperty EntryOneTextProperty =
  BindableProperty.Create(nameof(EntryOneText), typeof(string), typeof(OutputEntryView), string.Empty, BindingMode.TwoWay);

    public string EntryOneText
    {
        get => (string)GetValue(EntryOneTextProperty);
        set => SetValue(EntryOneTextProperty, value);
    }

    // ��������� ������� ����������
    public static readonly BindableProperty EntryTwoHeaderProperty =
  BindableProperty.Create(nameof(EntryTwoHeader), typeof(string), typeof(OutputEntryView), string.Empty, BindingMode.TwoWay, propertyChanged: TextPropertyChanged);

    public string EntryTwoHeader
    {
        get => (string)GetValue(EntryTwoHeaderProperty);
        set => SetValue(EntryTwoHeaderProperty, value);
    }

    // ����� ������� ����������
    public static readonly BindableProperty EntryTwoTextProperty =
  BindableProperty.Create(nameof(EntryTwoText), typeof(string), typeof(OutputEntryView), string.Empty, BindingMode.TwoWay, propertyChanged: ConModPropChanged);

    public string EntryTwoText
    {
        get => (string)GetValue(EntryTwoTextProperty);
        set => SetValue(EntryTwoTextProperty, value);
    }

    // ��������
    private static readonly Easing animationEasing = Easing.Linear;
    // ����� ��������
    private const uint animationLength = 300;

    public OutputEntryView()
    {
        InitializeComponent();
        //singleStateHeader.PropertyChanged += Header_PropertyChanged;
        //doubleStateHeader1.PropertyChanged += Header_PropertyChanged;
        //doubleStateHeader2.PropertyChanged += Header_PropertyChanged;
    }

    //private void Header_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    //{
    //    if (e.PropertyName == "Text")
    //    {
    //        InvalidateMeasure();
    //    }
    //}

    private static void TextPropertyChanged(BindableObject bindable, object oldValue, object newValue)
    {
        if (!string.IsNullOrWhiteSpace(newValue as string))
        {
            var output = (OutputEntryView)bindable;
            output.doubleStateHeader1.AutoSizeFont();
            output.doubleStateHeader2.AutoSizeFont();
        }
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
                output.doubleStateHeader1.FadeTo(0, animationLength, animationEasing)

                );

            VisualStateManager.GoToState(output, "Normal");
            return;
        }

        _ = output.TranslateBoxes(output.doubleStateGrid1, 0);
        _ = output.TranslateBoxes(output.doubleStateGrid2, 0);
        _ = output.doubleStateHeader1.FadeTo(1, animationLength, animationEasing);

        VisualStateManager.GoToState(output, "Doubled");
    }

    /// <summary>
    /// ���������� ������� visualstate.
    /// </summary>
    /// <returns></returns>
    private string? GetCurrentVisualState() // ���������� ��� ��������� ������ ���������!
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