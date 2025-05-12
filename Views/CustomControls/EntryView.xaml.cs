using System;
using System.Text.RegularExpressions;

namespace WhmCalcMaui.Views.CustomControls;

public partial class EntryView : ContentView
{
    public static readonly BindableProperty TextProperty =
  BindableProperty.Create(nameof(Text), typeof(string), typeof(EntryView), string.Empty, BindingMode.TwoWay);

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set 
        {
            if (validationRegex is not null)
            {
                if (validationRegex.IsMatch(value))
                {
                    SetValue(TextProperty, value);
                    GoToState(false);
                }
                else
                {
                    GoToState(true);
                }
            }
            else
            {
                SetValue(TextProperty, value);
            }
        }
    }

    public static readonly BindableProperty IsReadOnlyProperty =
  BindableProperty.Create(nameof(IsReadOnly), typeof(bool), typeof(EntryView), false);

    public bool IsReadOnly
    {
        get => (bool)GetValue(IsReadOnlyProperty);
        set => SetValue(IsReadOnlyProperty, value);
    }

    public static readonly BindableProperty KeyboardProperty =
  BindableProperty.Create(nameof(Keyboard), typeof(Keyboard), typeof(EntryView), Keyboard.Default, coerceValue: (o, v) => (Keyboard)v ?? Keyboard.Default);

    [System.ComponentModel.TypeConverter(typeof(Microsoft.Maui.Converters.KeyboardTypeConverter))]
    public Keyboard Keyboard
    {
        get => (Keyboard)GetValue(KeyboardProperty);
        set => SetValue(KeyboardProperty, value);
    }

    public string? ValidationExpression
    {
        get
        {
            return validationString;
        }
        set
        {
            validationString = value;

            if (!string.IsNullOrWhiteSpace(value))
            {
                validationRegex = new Regex(value);
            }
        }
    }

    private string? validationString;
    private Regex? validationRegex;

    public EntryView()
    {
        InitializeComponent();
    }

    private void GoToState(bool hasError)
    {
        string visualState = hasError ? "HasError" : "Normal";
        VisualStateManager.GoToState(this, visualState);
    }
}