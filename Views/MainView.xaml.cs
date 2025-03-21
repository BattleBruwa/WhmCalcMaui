using Microsoft.Maui.Controls;
using WhmCalcMaui.ViewModels;
using WhmCalcMaui.Views.CustomControls;
using System.Collections.Specialized;

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
        targetBox.Command = _viewModel.ShowSelectTargetCommand;
        deleteTBtn.Command = _viewModel.DeleteTargetCommand;
        addTBtn.Command = _viewModel.AddTargetCommand;
        initTask = BindingInitAsync();
    }

    private async Task BindingInitAsync()
    {
        while (_viewModel.ModListService.IsBusy)
        {
            await Task.Delay(500);
        }

        List<IView> children = modGrid.Children.Where(i => i.GetType() == typeof(CheckBoxView) || i.GetType() == typeof(PickerCheckBox)).ToList();

        for (int i = 0; i < children.Count; i++)
        {
            if (children[i] is CheckBoxView cb)
            {
                cb.BindingContext = _viewModel.ModListService.ModificatorsList[i];

                cb.CommandParameter = cb;

                cb.Command = _viewModel.ManageSelectedModsCommand;
            }

            if (children[i] is PickerCheckBox pcb)
            {
                pcb.BindingContext = _viewModel.ModListService.ModificatorsList[i];

                pcb.CommandParameter = pcb;

                pcb.Command = _viewModel.ManageConModsCommand;
            }
        }
    }

}