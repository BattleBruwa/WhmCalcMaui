using WhmCalcMaui.ViewModels;

namespace WhmCalcMaui.Views;

public partial class MainView : ContentPage
{
    MainViewModel viewModel;

    public MainView(MainViewModel viewModel)
    {
        this.viewModel = viewModel;
        InitializeComponent();
        BindingContext = this.viewModel;
    }

    //private void PopulateModGrid()
    //{
    //    if (viewModel.ModListService.ModificatorsList is null)
    //    { return; }

    //    int count;

    //    if (viewModel.ModListService.ModificatorsList.Count >= ModGrid.Count)
    //    {
    //        count = ModGrid.Count;
    //    }
    //    else
    //    { count = viewModel.ModListService.ModificatorsList.Count; }

    //    for (int i = 0; i < count; i++)
    //    {
    //        (ModGrid.Children[i] as ContentView).BindingContext = viewModel.ModListService.ModificatorsList[i];
    //    }
    }
}