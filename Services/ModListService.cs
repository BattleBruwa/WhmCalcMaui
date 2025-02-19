using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using WhmCalcMaui.Models;

namespace WhmCalcMaui.Services
{
    public class ModListService
    {
        public ObservableCollection<ModificatorModel> ModificatorsList { get; private set; } = [];

        public ObservableCollection<ModificatorModel> PickedMods { get; set; } = [];

        private Task initTask;

        public ModListService()
        {
            initTask = InitAsync();
        }

        private async Task InitAsync()
        {
            await LoadModsAsync();
        }

        private async Task LoadModsAsync()
        {
            try
            {
                using Stream stream = await FileSystem.OpenAppPackageFileAsync("Modificators.json");

                var tempCollection = await JsonSerializer.DeserializeAsync<ObservableCollection<ModificatorModel>>(stream);

                foreach (var mod in tempCollection)
                {
                    ModificatorsList.Add(mod);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                Debug.WriteLine($"Ошибка при попытке прочитать данные модификаторов.\n{ex.Source}\n{ex.Message}");
#endif
            }
        }
    }
}
