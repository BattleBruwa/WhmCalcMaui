using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;
using WhmCalcMaui.Models;

namespace WhmCalcMaui.Services
{
    public class ModListService
    {
        public List<ModificatorModel> ModificatorsList { get; private set; }

        public ObservableCollection<ModificatorModel> PickedMods { get; set; } = [];

        private Task initTask;

        public ModListService()
        {
            initTask = LoadModsAsync();
        }

        private async Task LoadModsAsync()
        {
            try
            {
            using Stream stream = await FileSystem.OpenAppPackageFileAsync("Modificators.json");

            ModificatorsList = await JsonSerializer.DeserializeAsync<List<ModificatorModel>>(stream);

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
