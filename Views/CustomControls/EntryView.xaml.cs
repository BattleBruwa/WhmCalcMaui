using System;
using System.Text.RegularExpressions;

namespace WhmCalcMaui.Views.CustomControls;

public partial class EntryView : ContentView
{
    public static readonly BindableProperty InternalTextProperty =
  BindableProperty.Create(nameof(InternalText), typeof(string), typeof(EntryView), string.Empty, BindingMode.TwoWay, propertyChanged:OnInternalTextChanged);

    private static void OnInternalTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        EntryView entry = (EntryView)bindable;

        if (entry.validationRegex is not null)
        {
            if (entry.validationRegex.IsMatch((string)newValue))
            {
                if (entry.HasError)
                    entry.HasError = false;
                entry.Text = (string)newValue;
                entry.GoToState(entry.HasError);
            }
            else
            {
                if (!entry.HasError)
                    entry.HasError = true;
                entry.GoToState(entry.HasError);
            }
        }
        else
        {
            if (entry.HasError)
                entry.HasError = false;
            entry.Text = (string)newValue;
        }
    }

    public string InternalText
    {
        get => (string)GetValue(InternalTextProperty);
        set => SetValue(InternalTextProperty, value);
    }

    public static readonly BindableProperty TextProperty =
  BindableProperty.Create(nameof(Text), typeof(string), typeof(EntryView), string.Empty, BindingMode.TwoWay, propertyChanged:OnTextChanged);

    private static void OnTextChanged(BindableObject bindable, object oldValue, object newValue)
    {
        EntryView entry = (EntryView)bindable;

        if(entry.InternalText != (string)newValue)
        {
            entry.InternalText = (string)newValue;
        }
    }

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    private bool hasError;
    public bool HasError
    {
        get => hasError;
        set
        {
            if (hasError != value)
            {
                hasError = value;
                OnPropertyChanged(nameof(HasError));
            }
        }
    }

    public static readonly BindableProperty TooltipTextProperty =
  BindableProperty.Create(nameof(TooltipText), typeof(string), typeof(EntryView), null);

    public string TooltipText
    {
        get => (string)GetValue(TooltipTextProperty);
        set => SetValue(TooltipTextProperty, value);
    }


    public static readonly BindableProperty ValidationErrorTextProperty =
  BindableProperty.Create(nameof(ValidationErrorText), typeof(string), typeof(EntryView), null);

    public string ValidationErrorText
    {
        get => (string)GetValue(ValidationErrorTextProperty);
        set => SetValue(ValidationErrorTextProperty, value);
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

        if (TooltipText is not null)
        {
            ToolTipProperties.SetText(this, TooltipText);
        }
    }

    private void GoToState(bool hasError)
    {
        string visualState = hasError ? "HasError" : "Normal";
        VisualStateManager.GoToState(this, visualState);

        // Если тултип не установлен, менять нечего
        if (TooltipText is null)
        {
            return;
        }
        
        if (hasError)
        {
            if ((string)ToolTipProperties.GetText(this) != ValidationErrorText)
            {
                ToolTipProperties.SetText(this, ValidationErrorText);
            }
        }
        else
        {
            if ((string)ToolTipProperties.GetText(this) != TooltipText)
            {
                ToolTipProperties.SetText(this, TooltipText);
            }
        }
    }
}