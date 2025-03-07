using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using WhmCalcMaui.Models;

namespace WhmCalcMaui.Views.Popups;

public partial class SelectTargetPopup : Popup
{
    public ObservableCollection<TargetModel> Targets { get; private set; }

    public SelectTargetPopup(ObservableCollection<TargetModel> targets)
    {
        InitializeComponent();
        Targets = targets;
    }

    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var pickedTarget = e.CurrentSelection[0];

        await CloseAsync(pickedTarget);
    }
}