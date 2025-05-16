using System.Collections.ObjectModel;
using System.ComponentModel;
using WhmCalcMaui.Services.Localization;
using WhmCalcMaui.ViewModels;
using WhmCalcMaui.Views.CustomControls;

namespace WhmCalcMaui.Views;

public partial class MainView : ContentPage
{
    MainViewModel _viewModel;

    private Task initTask;

    private readonly Dictionary<EntryView, int> targetEntryErrors = [];

    public MainView(MainViewModel viewModel)
    {
        _viewModel = viewModel;
        InitializeComponent();
        BindingContext = _viewModel;
        targetBox.Command = _viewModel.ShowSelectTargetCommand;
        deleteTBtn.Command = _viewModel.DeleteTargetCommand;
        addTBtn.Command = _viewModel.AddTargetCommand;
        initTask = BindingInitAsync();

        // ��������� ���������� ������ ���������� ��� ������� ��������� � EntryView ����
        foreach (var c in targetGrid.Children)
        {
            if (c is EntryView entry)
            {
                targetEntryErrors.Add(entry, 0);
                entry.PropertyChanged += Entry_PropertyChanged;
            }
        }
    }
    /// <summary>
    /// ��������� <see cref="targetEntryErrors"/> �� ������� �������� 1 (�������� 1 = ������ ��������� � ��������������� EntryView),
    /// ���� ���� ���� �� ���� 1, ��������� ������ ���������� ���� � ����.
    /// </summary>
    private void SetAddTargetBtnIsEnabled()
    {
        if (targetEntryErrors.Values.Any(v => v.Equals(1)))
        {
            if (addTBtn.IsEnabled)
            addTBtn.IsEnabled = false;
        }
        else
        {
            if (!addTBtn.IsEnabled)
            addTBtn.IsEnabled = true;
        }
    }

    private void Entry_PropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (sender is null)
        {
            return;
        }

        if (e.PropertyName == nameof(EntryView.HasError))
        {
            var entry = (EntryView)sender;

            // ���� ���� ������, ���������������� EntryView �������� �������� 1
            if (entry.HasError)
            {
                targetEntryErrors[entry] = 1;
                SetAddTargetBtnIsEnabled();
            }
            // ���� ������ ���, �������� 0
            else
            {
                targetEntryErrors[entry] = 0;
                SetAddTargetBtnIsEnabled();
            }
        }
    }

    private async Task BindingInitAsync()
    {
        // ��������, ���� ModListService �� �������� ���������
        while (_viewModel.ModListService.IsBusy)
        {
            await Task.Delay(500);
        }

        // ������� ������ �����
        List<IView> children = modGrid.Children.Where(i => i.GetType() == typeof(CheckBoxView) || i.GetType() == typeof(PickerCheckBox)).ToList();

        for (int i = 0; i < children.Count; i++)
        {
            if (children[i] is CheckBoxView cb)
            {
                var mod = _viewModel.ModListService.ModificatorsList[i];

                cb.BindingContext = mod;

                Binding bindingText = new Binding
                {
                    Source = LocalizationResourceManager.Instance,
                    Path = $"[{mod.Name}]",
                    Mode = BindingMode.OneWay
                };

                Binding bindingTooltip = new Binding
                {
                    Source = LocalizationResourceManager.Instance,
                    Path = $"[{mod.ToolTip}]",
                    Mode = BindingMode.OneWay
                };

                ToolTipProperties.SetText(cb, bindingTooltip);

                cb.SetBinding(CheckBoxView.NameProperty, bindingText);

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

                Binding bindingTooltip = new Binding
                {
                    Source = LocalizationResourceManager.Instance,
                    Path = $"[{mod.ToolTip}]",
                    Mode = BindingMode.OneWay
                };

                ToolTipProperties.SetText(pcb, bindingTooltip);

                pcb.SetBinding(PickerCheckBox.NameProperty, binding);

                pcb.CommandParameter = pcb;

                pcb.Command = _viewModel.ManageConModsCommand;
            }
        }
    }

}