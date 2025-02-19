using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using WhmCalcMaui.Models;
using WhmCalcMaui.Services;
using WhmCalcMaui.Services.Calculations;

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

        public MainViewModel(ModListService modListService, IDataAccessService dataAccessService, CalcOutputService calculator)
        {
            ModListService = modListService;
            DataAccessService = dataAccessService;
            Calculator = calculator;
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

        private void Test()
        {
            while (true)
            {
                Debug.WriteLine($"Attacker:\nA: {Attacker?.AttackerA} WS: {Attacker?.AttackerWS} S: {Attacker?.AttackerS} AP: {Attacker?.AttackerAP} D: {Attacker?.AttackerD}");
                Debug.WriteLine($"Output:\nA: {Output?.Attacks} H: {Output?.AllHits} W: {Output?.AllWounds} US: {Output?.UnSavedWounds} D: {Output?.TotalDamage}");
                Debug.WriteLine("Modlist:");
                foreach(var i in ModListService.ModificatorsList)
                {
                    Debug.Write(i.Name + " ");
                }
                Debug.WriteLine("");
                Thread.Sleep(2000);
            }
        }
    }
}
