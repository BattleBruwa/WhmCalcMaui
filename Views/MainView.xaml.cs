using Microsoft.Maui.Controls;
using WhmCalcMaui.ViewModels;
using WhmCalcMaui.Views.CustomControls;
using System.Collections.Specialized;
using WhmCalcMaui.Resources.Localization;
using WhmCalcMaui.Services.Localization;

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
                var mod = _viewModel.ModListService.ModificatorsList[i];

                cb.BindingContext = mod;

                Binding binding = new Binding { 
                
                    Source = LocalizationResourceManager.Instance,
                    Path = $"[{mod.Name}]",
                    Mode = BindingMode.OneWay
                };

                cb.SetBinding(CheckBoxView.NameProperty, binding);

                cb.CommandParameter = cb;

                cb.Command = _viewModel.ManageSelectedModsCommand;
            }

            if (children[i] is PickerCheckBox pcb)
            {
                var mod = _viewModel.ModListService.ModificatorsList[i];

                pcb.BindingContext = mod;

                Binding binding = new Binding
                {

                    Source = LocalizationResourceManager.Instance,
                    Path = $"[{mod.Name}]",
                    Mode = BindingMode.OneWay
                };

                pcb.SetBinding(PickerCheckBox.NameProperty, binding);

                pcb.CommandParameter = pcb;

                pcb.Command = _viewModel.ManageConModsCommand;
            }
        }
    }

}