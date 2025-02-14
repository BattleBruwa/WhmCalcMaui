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
        ModListService ModListService { get; set; }

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

        public OutputModel Output { get; set; }

        public MainViewModel(ModListService modListService, IDataAccessService dataAccessService, CalcOutputService calculator)
        {
            ModListService = modListService;
            DataAccessService = dataAccessService;
            Calculator = calculator;
            Attacker.PropertyChanged += Attacker_PropertyChanged;
            ModListService.PickedMods.CollectionChanged += PickedMods_CollectionChanged;
            Task test = Task.Run(Test);
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
                Debug.WriteLine($"Attacker: {Attacker.AttackerA}");
                Thread.Sleep(2000);
            }
        }
    }
}
