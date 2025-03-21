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
            SelectedTarget = new TargetModel();
            // Тест ----------------------
            //Task test = Task.Run(Test);
            // ---------------------------
        }

        [RelayCommand]
        private async Task TestTwoAsync()
        {
            var popup = new ConfirmationPopup("Подтверждение.", "Вы действительно хотите что-то сделать?", "Нет", "Да");

            popup.StartAnim();

            var result = await Shell.Current.ShowPopupAsync(popup) ?? false;

            string message;

            if ((bool)result)
            {
                message = "Вы нажали ДА!";
            }
            else
            {
                message = "Вы нажали НЕТ или дизмиснули!";
            }

            var popupTwo = new MessagePopup("Ахтунг!", message);

            _ = popupTwo.ShowAsync();
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
        private async Task AddTargetAsync()
        {
            if (SelectedTarget is null || String.IsNullOrWhiteSpace(SelectedTarget.TargetName))
            {
                return;
            }

            var targetToAdd = new TargetModel();
            targetToAdd.CopyProps(SelectedTarget);

            string message;

            // Если цель с таким именем уже есть:
            if (Targets.Any(x => x.TargetName == SelectedTarget.TargetName))
            {
                var popup = new ConfirmationPopup("Внимание.", "Цель с таким именем уже существует. Вы хотите изменить ее характеристики?", "Нет", "Да");

                popup.StartAnim();

                var result = await Shell.Current.ShowPopupAsync(popup) ?? false;
                // Пользователь нажал "Да":
                if ((bool)result)
                {
                    await DataAccessService.UpdateTargetAsync(targetToAdd);

                    var targetToDelList = Targets.Where(x => x.TargetName.Equals(SelectedTarget.TargetName)).ToList();

                    int i = Targets.IndexOf(targetToDelList.First());
                    var tg = Targets.ElementAt(i);
                    tg.CopyProps(targetToAdd);

                    message = "Цель была обновлена.";

                    var msgPopup = new MessagePopup("Успех.", message);

                    _ = msgPopup.ShowAsync();

                    return;
                }
                // Если пользователь нажал "Нет", возврат:
                return;
            }
            // Если цели с таким именем нет:
            else
            {
                int resultOfAdd = await DataAccessService.AddTargetAsync(targetToAdd);

                if (resultOfAdd != 0)
                {
                    Targets.Add(targetToAdd);

                    message = "Цель была добавлена.";

                    var msgPopup = new MessagePopup("Успех.", message);

                    _ = msgPopup.ShowAsync();

                    return;
                }
            }
        }

        [RelayCommand]
        private async Task DeleteTargetAsync()
        {
            if (SelectedTarget is null)
            {
                return;
            }

            var popup = new ConfirmationPopup("Подтверждение.", "Вы действительно хотите удалить эту цель?", "Нет", "Да");

            popup.StartAnim();

            var result = await Shell.Current.ShowPopupAsync(popup) ?? false;

            string message;

            if ((bool)result)
            {
                int res = await DataAccessService.RemoveTargetAsync(SelectedTarget.TargetName);

                if (res == 0)
                {
                    message = "Не удалось удалить цель.";

                    var errorMsgPopup = new MessagePopup("Ошибка.", message);

                    await errorMsgPopup.ShowAsync();

                    return;
                }

                var targetToDelList = Targets.Where(x => x.TargetName.Equals(SelectedTarget.TargetName)).ToList();
                if (targetToDelList.Count == 1)
                {
                    Targets.Remove(targetToDelList.First());
                }

                if (Targets.Count != 0)
                {
                    SelectedTarget.CopyProps(Targets.First());
                }
                else
                {
                    SelectedTarget = null;
                }

                message = "Цель удалена.";

                var successMsgPopup = new MessagePopup("Успех.", message);

                await successMsgPopup.ShowAsync();

                return;
            }
            else
            {
                return;
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
        private async Task ShowSelectTargetAsync(object? parameter)
        {
            var popup = new SelectTargetPopup(Targets, SelectedTarget);

            //_ = Task.WhenAll(popup.FadePopup(1), popup.SlidePopup());

            popup.StartAnim();

            var result = await Shell.Current.ShowPopupAsync(popup);

            if (result is TargetModel sTarget)
            {
                SelectedTarget ??= new TargetModel();

                SelectedTarget.CopyProps(sTarget);
            }
        }

        private async Task InitTargetsAsync()
        {
            var result = await DataAccessService.GetTargetsAsync();
            if (!result.Any())
            {
                using Stream stream = await FileSystem.OpenAppPackageFileAsync("DefaultTargets.json");

                var defaultTargets = await JsonSerializer.DeserializeAsync<List<TargetModel>>(stream) ?? [];

                foreach (var t in defaultTargets)
                {
                    await DataAccessService.AddTargetAsync(t);
                }

                await InitTargetsAsync();
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
