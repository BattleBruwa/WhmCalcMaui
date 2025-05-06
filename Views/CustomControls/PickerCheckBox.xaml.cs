using System.Diagnostics;
using System.Windows.Input;

namespace WhmCalcMaui.Views.CustomControls;

public partial class PickerCheckBox : ContentView
{
    // ��� ����
    public static readonly BindableProperty NameProperty =
  BindableProperty.Create(nameof(Name), typeof(string), typeof(PickerCheckBox), string.Empty, BindingMode.TwoWay, propertyChanged: NameChanged);

    public string Name
    {
        get => (string)GetValue(NameProperty);
        set => SetValue(NameProperty, value);
    }
    // ��������� ��������
    public static readonly BindableProperty SelectedValueProperty =
  BindableProperty.Create(nameof(SelectedValue), typeof(int?), typeof(PickerCheckBox), null, BindingMode.TwoWay, propertyChanged: SelectedValueChanged);

    public int? SelectedValue
    {
        get => (int?)GetValue(SelectedValueProperty);
        set => SetValue(SelectedValueProperty, value);
    }
    // �������� ��� viewmodel
    public static readonly BindableProperty CommandProperty =
  BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(PickerCheckBox), null);

    public ICommand Command
    {
        get => (ICommand)GetValue(CommandProperty);
        set => SetValue(CommandProperty, value);
    }
    // �������� ��������
    public static readonly BindableProperty CommandParameterProperty =
  BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(PickerCheckBox), null);

    public object CommandParameter
    {
        get => (object)GetValue(CommandParameterProperty);
        set => SetValue(CommandParameterProperty, value);
    }
    // ������ ���� (++ ��� ������, +++ ��� ���)
    private readonly string valueSymbol;
    // ��������
    private readonly Easing animationEasing = Easing.SinOut;
    // ����� ��������
    private const uint animationLength = 250;

    public PickerCheckBox(IList<int> nums, string symbol = "")
    {
        InitializeComponent();
        UpdateVisualState();
        valueSymbol = symbol;
        var tGR = new TapGestureRecognizer();
        tGR.Tapped += OnTapped;
        GestureRecognizers.Add(tGR);
        PopulateGrid(nums, symbol);
    }

    // ���������� ��� ����� ����� �����������
    private static void NameChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var picker = (PickerCheckBox)bindable;
        picker.NameLabel.Text = picker.Name + " " + picker.SelectedValue + picker.valueSymbol;
    }

    // ���������� ��� ������ ��������.
    private async void OnTapped(object? sender, TappedEventArgs e)
    {
        string? currentState = GetCurrentVisualState();

        switch (currentState)
        {
            case "Unchecked":

                //_ = CollectionGrid.TranslateTo(0, 0, animationLength, animationEasing);

                _ = SmoothTranslate(CollectionGrid, 0);

                VisualStateManager.GoToState(this, "Pressed");

                break;
            case "Pressed":

                //await CollectionGrid.TranslateTo(Width, 0, animationLength, animationEasing);

                await SmoothTranslate(CollectionGrid, Width);

                if (SelectedValue is null)
                {
                    VisualStateManager.GoToState(this, "Unchecked");
                }
                if (SelectedValue is not null)
                {
                    VisualStateManager.GoToState(this, "Checked");
                }

                break;
            case "Checked":

                //_ = CollectionGrid.TranslateTo(0, 0, animationLength, animationEasing);

                _ = SmoothTranslate(CollectionGrid, 0);

                VisualStateManager.GoToState(this, "Pressed");

                break;
            default:
                break;
        }
#if DEBUG
        Debug.WriteLine($"{Name} is tapped!");
#endif
    }

    /// <summary>
    /// ���������� �������� ��������.
    /// </summary>
    /// <param name="nums">�������� ��� ������.</param>
    /// <param name="symbol">������ ���� (++ ��� ������, +++ ��� ���).</param>
    private void PopulateGrid(IList<int> nums, string symbol)
    {
        int currentColumn = 0;

        foreach (int num in nums)
        {
            CollectionGrid.ColumnDefinitions.Add(new ColumnDefinition());

            Button btn = new()
            {
                Text = $"{num + symbol}",
                Style = (Style)Application.Current.Resources["PickerButton"]
            };

            btn.Clicked += Btn_Clicked;

            CollectionGrid.Add(btn, currentColumn);

            currentColumn++;
        }
        // ������ ���������� / ���������� ����
        CollectionGrid.ColumnDefinitions.Add(new ColumnDefinition());

        Button btnClose = new()
        {
            Text = "X",
            Style = (Style)Application.Current.Resources["PickerButton"],
            TextColor = Colors.Red,
            FontAttributes = FontAttributes.Bold
        };

        btnClose.Clicked += BtnClose_Clicked;
        ;

        CollectionGrid.Add(btnClose, currentColumn);
    }

    private Task<bool> SmoothTranslate(VisualElement element, double translate)
    {
        var tcs = new TaskCompletionSource<bool>();

        new Animation(v => element.TranslationX = v, element.TranslationX, translate)
            .Commit(element, nameof(SmoothTranslate), 4, animationLength, animationEasing, (f, a) => tcs.SetResult(a));

        return tcs.Task;
    }

    // ��������� ������ � ���������� ������ ��� ��������� ���������� ��������.
    private static void SelectedValueChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var picker = (PickerCheckBox)bindable;

        var buttons = picker.CollectionGrid.Children.ToList();

        if (newValue is null)
        {
            picker.NameLabel.Text = picker.Name;

            foreach (var btn in buttons)
            {
                if (btn is Button btnt)
                {
                    btnt.BorderWidth = 1;

                    btnt.FontAttributes = FontAttributes.None;
                }
            }
        }
        if (newValue is not null)
        {
            picker.NameLabel.Text = picker.Name + " " + picker.SelectedValue + picker.valueSymbol;

            foreach (var btn in buttons)
            {
                if (btn is Button btnt)
                {
                    if ((int)char.GetNumericValue(btnt.Text[0]) == (int)newValue)
                    {
                        btnt.BorderWidth = 2;

                        btnt.FontAttributes = FontAttributes.Bold;
                    }
                    else
                    {
                        btnt.BorderWidth = 1;

                        btnt.FontAttributes = FontAttributes.None;
                    }
                }
            }
        }
    }

    // ���������� ��� ������ ������.
    private async void BtnClose_Clicked(object? sender, EventArgs e)
    {
        if (sender is not Button btn)
            return;

        SelectedValue = null;

        Command?.Execute(CommandParameter);

        await CollectionGrid.TranslateTo(Width, 0, animationLength, animationEasing);

        UpdateVisualState();
    }

    // ���������� ��� ���������� ������.
    private async void Btn_Clicked(object? sender, EventArgs e)
    {
        if (sender is not Button btn)
            return;

        SelectedValue = (int)char.GetNumericValue(btn.Text[0]);

        Command?.Execute(CommandParameter);

        await CollectionGrid.TranslateTo(Width, 0, animationLength, animationEasing);

        UpdateVisualState();
    }

    private void UpdateVisualState()
    {
        VisualStateManager.GoToState(this, SelectedValue is null ? "Unchecked" : "Checked");
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
}