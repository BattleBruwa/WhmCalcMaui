using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WhmCalcMaui.Models;
using WhmCalcMaui.Resources.Localization;
using WhmCalcMaui.Services;
using WhmCalcMaui.Services.Calculations;
using WhmCalcMaui.Services.Localization;
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
            Task test = Task.Run(Test);
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
        private void ChangeAppTheme()
        {
            if (Application.Current is null)
            {
                return;
            }

            if (Application.Current.RequestedTheme is AppTheme.Dark)
            {
                Application.Current.UserAppTheme = AppTheme.Light;
                return;
            }

            Application.Current.UserAppTheme = AppTheme.Dark;
        }

        [RelayCommand]
        private void ChangeLanguage()
        {
            var culture = AppStrings.Culture.TwoLetterISOLanguageName
                .Equals("ru", StringComparison.InvariantCultureIgnoreCase) ? new CultureInfo("en-US") : new CultureInfo("ru-RU");

            LocalizationResourceManager.Instance.SetCulture(culture);
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

            // Если цель с таким именем уже есть:
            if (Targets.Any(x => x.TargetName == SelectedTarget.TargetName))
            {
                var popup = new ConfirmationPopup(AppStrings.popup_type_warning, AppStrings.popup_confirm_targetupdate, AppStrings.btn_no, AppStrings.btn_yes);
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

                    var msgPopup = new MessagePopup(AppStrings.popup_type_success, AppStrings.popup_message_targetupdated);

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

                    var msgPopup = new MessagePopup(AppStrings.popup_type_success, AppStrings.popup_message_targetadded);

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

            var popup = new ConfirmationPopup(AppStrings.popup_type_confirmation, AppStrings.popup_confirm_targetdelete, AppStrings.btn_no, AppStrings.btn_yes);

            popup.StartAnim();

            var result = await Shell.Current.ShowPopupAsync(popup) ?? false;

            if ((bool)result)
            {
                int res = await DataAccessService.RemoveTargetAsync(SelectedTarget.TargetName);

                if (res == 0)
                {
                    var errorMsgPopup = new MessagePopup(AppStrings.popup_type_error, AppStrings.popup_error_couldnotdelete);

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

                var successMsgPopup = new MessagePopup(AppStrings.popup_type_success, AppStrings.popup_message_targetremoved);

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
                //Debug.WriteLine("Targets:");
                //foreach (var t in Targets)
                //{
                //    Debug.Write(t.TargetName + " ");
                //}
                Debug.WriteLine("");
                Thread.Sleep(2000);
            }
        }
    }
}
