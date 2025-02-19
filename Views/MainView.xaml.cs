using Microsoft.Maui.Controls;
using WhmCalcMaui.ViewModels;
using WhmCalcMaui.Views.CustomControls;

namespace WhmCalcMaui.Views;

public partial class MainView : ContentPage
{
    MainViewModel _viewModel;

    private Task initTask;

    public MainView(MainViewModel viewModel)
    {
        _viewModel = viewModel;
        InitializeComponent();
        BindingContext = _viewModel;
        //_viewModel.PropertyChanged += _viewModel_PropertyChanged;

        initTask = BindingInitAsync();
    }

    //private void _viewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    //{
    //    if (e.PropertyName == nameof(_viewModel.ModListService.IsBusy))
    //    {
    //        List<IView> children = modGrid.Children.Where(i => i.GetType() == typeof(CheckBoxView)).ToList();

    //        for (int i = 0; i < _viewModel.ModListService.ModificatorsList.Count; i++)
    //        {
    //            CheckBoxView checkBox = children[i] as CheckBoxView;

    //            checkBox.BindingContext = _viewModel.ModListService.ModificatorsList[i];
    //        }
    //    }
    //}

    private async Task BindingInitAsync()
    {
        while (_viewModel.ModListService.IsBusy)
        {
            await Task.Delay(500);
        }

        List<IView> children = modGrid.Children.Where(i => i.GetType() == typeof(CheckBoxView)).ToList();

        for (int i = 0; i < children.Count; i++)
        {
            (children[i] as CheckBoxView).BindingContext = _viewModel.ModListService.ModificatorsList[i];
        }
    }

}