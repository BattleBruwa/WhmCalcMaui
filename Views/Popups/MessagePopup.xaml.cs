using CommunityToolkit.Maui.Views;

namespace WhmCalcMaui.Views.Popups;

public partial class MessagePopup : Popup
{
	public static readonly BindableProperty MessageProperty =
  BindableProperty.Create(nameof(Message), typeof(string), typeof(MessagePopup), String.Empty);

	public string Message
	{
		get => (string)GetValue(MessageProperty);
		set => SetValue(MessageProperty, value);
	}

	public static readonly BindableProperty TitleProperty =
  BindableProperty.Create(nameof(Title), typeof(string), typeof(MessagePopup), String.Empty);

	public string Title
	{
		get => (string)GetValue(TitleProperty);
		set => SetValue(TitleProperty, value);
	}

    private bool isDismissed = false;
    // Анимация
    private static readonly Easing animationEasing = Easing.Linear;
    // Длина анимации
    private const uint animationLength = 500;

    public MessagePopup(string title, string message)
	{
		InitializeComponent();
		Title = title;
		Message = message;
	}

    protected override Task OnDismissedByTappingOutsideOfPopup(CancellationToken token = default)
    {
        isDismissed = true;

        return base.OnDismissedByTappingOutsideOfPopup(token);
    }

    public async Task ShowAsync()
    {
        Shell.Current.ShowPopup(this);

        await Task.WhenAll(FadePopup(1), SlidePopup());

        await Task.Delay(2000);

        await Task.WhenAll(FadePopup(0), SlidePopup(false));

        if (!isDismissed)
        { Close(); }
    }

    public Task<bool> FadePopup(double opacityChange)
    {
        var tcs = new TaskCompletionSource<bool>();
        new Animation(v => mainBorder.Opacity = v, mainBorder.Opacity, opacityChange)
            .Commit(mainBorder, nameof(FadePopup), 4, animationLength, animationEasing, (f, a) => tcs.SetResult(a));

        return tcs.Task;
    }

    public Task<bool> SlidePopup(bool isToOrFrom = true)
    {
        var tcs = new TaskCompletionSource<bool>();
        
        double start;

        double end;

        if (isToOrFrom)
        {
            start = -248d;
            end = 0d;
        }
        else
        {
            start = 0d;
            end = -248d;
        }

        new Animation(v => mainBorder.TranslationY = v, start, end)
            .Commit(mainBorder, nameof(SlidePopup), 2, animationLength, Easing.CubicInOut, (f, a) => tcs.SetResult(a));

        return tcs.Task;
    }
}