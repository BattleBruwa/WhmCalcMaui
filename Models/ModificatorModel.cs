using System.Text.Json.Serialization;

namespace WhmCalcMaui.Models
{
    public class ModificatorModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Для модификаторов с выбором значения.
        /// </summary>
        [JsonIgnore]
        public int? Condition { get; set; } = null;

        [JsonIgnore]
        public string ModInfo { get => ToString(); }

        public override string ToString()
        {
            return $"{Name}:\n{Description}";
        }
    }
}
