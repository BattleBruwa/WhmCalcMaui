using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WhmCalcMaui.Models;
using WhmCalcMaui.Services;
using WhmCalcMaui.Services.Calculations;
using WhmCalcMaui.Views.CustomControls;
using WhmCalcMaui.Views.Popups;
using static SQLite.SQLite3;

namespace WhmCalcMaui.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public ModListService ModListService { get; private set; }

        IDataAccessService DataAccessService { get; set; }

        CalcOutputService Calculator;

        [ObservableProperty]
        AttackerModel attacker = new();

        TargetModel? selectedTarget;
        public TargetModel? SelectedTarget
        {
            get { return selectedTarget; }
            set
            {
                SetProperty(ref selectedTarget, value);
                Recalculate();
            }
        }

        public ObservableCollection<TargetModel> Targets { get; set; }

        public OutputModel Output { get; set; } = new();

        private Task initTask;

        public MainViewModel(ModListService modListService, IDataAccessService dataAccessService, CalcOutputService calculator)
        {
            ModListService = modListService;
            DataAccessService = dataAccessService;
            Calculator = calculator;
            Targets = [];
            initTask = InitTargetsAsync();
            Attacker.PropertyChanged += Attacker_PropertyChanged;
            ModListService.PickedMods.CollectionChanged += PickedMods_CollectionChanged;
            // Тест ----------------------
            Task test = Task.Run(Test);
            SelectedTarget = new TargetModel { TargetName = "Space Marine", TargetT = 4, TargetSv = 3, TargetW = 2 };
            // ---------------------------
        }

        private void Attacker_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Recalculate();
        }

        private void PickedMods_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            Recalculate();
        }

        private void Recalculate()
        {
            if (Attacker is not null && SelectedTarget is not null && Output is not null)
            {
                Calculator.CalculateOutput(Attacker, SelectedTarget, Output, ModListService.PickedMods);
            }
        }

        [RelayCommand]
        private void ManageSelectedMods(object obj)
        {
            if (obj is null || obj is not CheckBoxView cb)
                return;

            var mod = cb.BindingContext as ModificatorModel;

            if (mod is not null)
            {
                if (cb.IsChecked)
                {
                    ModListService.PickedMods.Add(mod);
                }
                if (!cb.IsChecked)
                {
                    ModListService.PickedMods.Remove(mod);
                }
            }
        }

        [RelayCommand]
        private void ManageConMods(object obj)
        {
            if (obj is null || obj is not PickerCheckBox cb)
                return;

            var mod = cb.BindingContext as ModificatorModel;

            if (mod is not null)
            {
                mod.Condition = cb.SelectedValue;

                if (cb.SelectedValue is not null && !ModListService.PickedMods.Contains(mod))
                {
                    ModListService.PickedMods.Add(mod);
                }
                if (cb.SelectedValue is null)
                {
                    ModListService.PickedMods.Remove(mod);
                }

                Recalculate();
            }
        }

        [RelayCommand]
        private async Task ShowSelectTargetAsync()
        {
            var popup = new SelectTargetPopup(Targets);

            var result = await Shell.Current.ShowPopupAsync(popup);

            if (result is TargetModel sTarget)
            {
                SelectedTarget = sTarget;
            }
        }
        
        private async Task InitTargetsAsync()
        {
            var result = await DataAccessService.GetTargetsAsync();
            if (!result.Any())
            {
                using Stream stream = await FileSystem.OpenAppPackageFileAsync("DefaultTargets.json");

                result = await JsonSerializer.DeserializeAsync<List<TargetModel>>(stream);
            }

            if (result is not null)
            {
                foreach (var t in result)
                {
                    Targets.Add(t);
                }
            }
        }

        private void Test()
        {
            while (true)
            {
                Debug.WriteLine($"Attacker:\nA: {Attacker?.AttackerA} WS: {Attacker?.AttackerWS} S: {Attacker?.AttackerS} AP: {Attacker?.AttackerAP} D: {Attacker?.AttackerD}");
                Debug.WriteLine($"Output:\nA: {Output?.Attacks} H: {Output?.NatHits} Sus: {Output?.SustainedHits} W: {Output?.AllWounds} US: {Output?.UnSavedWounds} D: {Output?.TotalDamage}");
                Debug.WriteLine("Modlist:");
                foreach (var i in ModListService.PickedMods)
                {
                    Debug.Write(i.Name + " ");

                    if (i.Condition is not null)
                    {
                        Debug.Write(i.Condition + " ");
                    }
                }
                Debug.WriteLine("Targets:");
                    foreach (var t in Targets)
                    {
                        Debug.Write(t.TargetName + " ");
                    }
                Debug.WriteLine("");
                Thread.Sleep(2000);
            }
        }
    }
}
