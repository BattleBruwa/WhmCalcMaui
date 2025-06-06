using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;
using WhmCalcMaui.Models;

namespace WhmCalcMaui.Views.Popups;

public partial class SelectTargetPopup : Popup
{
    // ��������
    private static readonly Easing animationEasing = Easing.SinOut;
    // ����� ��������
    private const uint animationLength = 250;

    private bool isBusy = false;

    public SelectTargetPopup(ObservableCollection<TargetModel> targets, TargetModel? selectedTarget = null)
    {
        InitializeComponent();

        collView.ItemsSource = targets;

        if (selectedTarget is not null && targets.Any(t => t.TargetName == selectedTarget.TargetName))
        {
            collView.SelectedItem = targets.First(t => t.TargetName == selectedTarget.TargetName);
        }

    }

    // ��� ������, �������� ���� ������ � ��� ��������.
    //protected override async Task OnClosed(object? result, bool wasDismissedByTappingOutsideOfPopup, CancellationToken token = default)
    //{
    //    await FadePopup(0);

    //    await base.OnClosed(result, wasDismissedByTappingOutsideOfPopup, token);
    //}

    public void StartAnim()
    {
        _ = Task.WhenAll(FadePopup(1), SlidePopup());
    }

    public Task<bool> FadePopup(double opacityChange)
    {
        var tcs = new TaskCompletionSource<bool>();

        new Animation(v => mainElement.Opacity = v, mainElement.Opacity, opacityChange)
            .Commit(mainElement, nameof(FadePopup), 4, animationLength, animationEasing, (f, a) => tcs.SetResult(a));

        return tcs.Task;
    }

    public Task<bool> SlidePopup(bool isToOrFrom = true)
    {
        var tcs = new TaskCompletionSource<bool>();

        double start;

        double end;

        if (isToOrFrom)
        {
            start = 248;
            end = 0d;
        }
        else
        {
            start = 0d;
            end = 248;
        }

        new Animation(v => mainElement.TranslationY = v, start, end)
            .Commit(mainElement, nameof(SlidePopup), 2, animationLength, Easing.CubicInOut, (f, a) => tcs.SetResult(a));

        return tcs.Task;
    }

    private async void TapGestureRecognizer_Tapped(object sender, TappedEventArgs e)
    {
        if (isBusy)
        { return; }

        var selectedT = (sender as Grid)?.BindingContext;

        isBusy = true;
        await Task.WhenAll(FadePopup(0), SlidePopup(false));

        isBusy = false;
        await CloseAsync(selectedT);
    }
}