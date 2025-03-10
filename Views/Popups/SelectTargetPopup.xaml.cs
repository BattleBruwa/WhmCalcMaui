using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;
using WhmCalcMaui.Models;

namespace WhmCalcMaui.Views.Popups;

public partial class SelectTargetPopup : Popup
{

    public SelectTargetPopup(ObservableCollection<TargetModel> targets)
    {
        InitializeComponent();

        collView.ItemsSource = targets;
    }

    private async void CollectionView_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        var pickedTarget = e.CurrentSelection[0];

        

        await CloseAsync(pickedTarget);
    }
}