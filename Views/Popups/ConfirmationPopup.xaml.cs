using CommunityToolkit.Maui.Views;

namespace WhmCalcMaui.Views.Popups;

public partial class ConfirmationPopup : Popup
{
    private bool isDismissed = false;
    // Анимация
    private static readonly Easing animationEasing = Easing.Linear;
    // Длина анимации
    private const uint animationLength = 2500;

    public ConfirmationPopup(string title, string message, string leftBtnText, string rightBtnText)
	{
		InitializeComponent();
        ResultWhenUserTapsOutsideOfPopup = false;
		titleLabel.Text = title;
		messageLabel.Text = message;
        leftBtnLabel.Text = leftBtnText;
		rightBtnLabel.Text = rightBtnText;
	}

    protected override Task OnDismissedByTappingOutsideOfPopup(CancellationToken token = default)
    {
        isDismissed = true;
        return base.OnDismissedByTappingOutsideOfPopup(token);
    }

    public void StartAnim()
    {
        _ = Task.WhenAll(FadePopup(1), SlidePopup());
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
            start = -354d;
            end = 0d;
        }
        else
        {
            start = 0d;
            end = -354d;
        }

        new Animation(v => mainBorder.TranslationX = v, start, end)
            .Commit(mainBorder, nameof(SlidePopup), 2, animationLength, Easing.CubicInOut, (f, a) => tcs.SetResult(a));

        return tcs.Task;
    }

    private async void leftBtn_Clicked(object sender, EventArgs e)
    {
        await Task.WhenAll(FadePopup(0), SlidePopup(false));

        if (!isDismissed)
        { Close(false); }
    }

    private async void rightBtn_Clicked(object sender, EventArgs e)
    {
        ResultWhenUserTapsOutsideOfPopup = true;

        await Task.WhenAll(FadePopup(0), SlidePopup(false));

        if (!isDismissed)
        { Close(true); }
    }
}