using System.Diagnostics;
using System.Windows.Input;

namespace WhmCalcMaui.Views.CustomControls;

public partial class PickerCheckBox : ContentView
{
    // ��� ����
    public static readonly BindableProperty NameProperty =
  BindableProperty.Create(nameof(Name), typeof(string), typeof(PickerCheckBox), string.Empty);

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
    private readonly Easing animationEasing = Easing.Linear;
    // ����� ��������
    private const uint animationLength = 250;

    public PickerCheckBox(int num, string symbol = "")
    {
        InitializeComponent();
        UpdateVisualState();
        valueSymbol = symbol;
        var tGR = new TapGestureRecognizer();
        tGR.Tapped += OnTapped;
        GestureRecognizers.Add(tGR);
        PopulateGrid(num, symbol);
    }
    // ���������� ��� ������ ��������.
    private async void OnTapped(object? sender, TappedEventArgs e)
    {
        string? currentState = GetCurrentVisualState();

        switch (currentState)
        {
            case "Unchecked":

                CollectionGrid.TranslateTo(0, 0, animationLength, animationEasing);

                VisualStateManager.GoToState(this, "Pressed");

                break;
            case "Pressed":

                await CollectionGrid.TranslateTo(Width, 0, animationLength, animationEasing);

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

                CollectionGrid.TranslateTo(0, 0, animationLength, animationEasing);

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
    /// <param name="num">���������� ������ ��������.</param>
    /// <param name="symbol">������ ���� (++ ��� ������, +++ ��� ���).</param>
    private void PopulateGrid(int num, string symbol)
    {
        for (int i = 1; i <= num; i++)
        {
            CollectionGrid.ColumnDefinitions.Add(new ColumnDefinition());

            Button btn = new()
            {
                Text = $"{i + symbol}"
            };

            btn.Clicked += Btn_Clicked;

            CollectionGrid.Add(btn, (i - 1));
        }

        CollectionGrid.ColumnDefinitions.Add(new ColumnDefinition());

        Button btnClose = new()
        {
            Text = "X"
        };

        btnClose.Clicked += BtnClose_Clicked;
        ;

        CollectionGrid.Add(btnClose, (num + 1));
    }

    // ��������� ������ ��� ��������� ���������� ��������.
    private static void SelectedValueChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var picker = (PickerCheckBox)bindable;
        if (newValue is null)
        {
            picker.NameLabel.Text = picker.Name;
        }
        if (newValue is not null)
        {
            picker.NameLabel.Text = picker.Name + " " + picker.SelectedValue + picker.valueSymbol;
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