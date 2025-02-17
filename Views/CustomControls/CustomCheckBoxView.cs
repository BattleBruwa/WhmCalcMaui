
namespace WhmCalcMaui.Views.CustomControls;

// Визуал задается через ControlTemplate в Styles
public class CustomCheckBoxView : ContentView
{
	public static readonly BindableProperty NameProperty =
  BindableProperty.Create(nameof(Name), typeof(string), typeof(CustomCheckBoxView), string.Empty);

	public string Name
	{
		get => (string)GetValue(NameProperty);
		set => SetValue(NameProperty, value);
	}

	public static readonly BindableProperty IsCheckedProperty =
  BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(CustomCheckBoxView), false, BindingMode.TwoWay, propertyChanged: OnIsCheckedChanged);


    public bool IsChecked
	{
		get => (bool)GetValue(IsCheckedProperty);
		set => SetValue(IsCheckedProperty, value);
	}

	public CustomCheckBoxView()
	{
		UpdateVisualState();
	}

    private static void OnIsCheckedChanged(BindableObject bindable, object oldValue, object newValue)
    {
		var checkbox = (CustomCheckBoxView)bindable;
		checkbox.UpdateVisualState();
    }

	private void UpdateVisualState()
	{
		VisualStateManager.GoToState(this, IsChecked ? "Checked" : "Unchecked");
	}
}