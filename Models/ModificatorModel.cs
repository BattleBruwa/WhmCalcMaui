using System.Text.Json.Serialization;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WhmCalcMaui.Models
{
    public partial class ModificatorModel: ObservableObject
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Для модификаторов с выбором значения.
        /// </summary>
        private int? condition = null;
        [JsonIgnore]
        public int? Condition
        {
            get { return condition; }
            set { SetProperty(ref condition, value); } 
        }

        [JsonIgnore]
        public string ModInfo { get => ToString(); }

        public override string ToString()
        {
            return $"{Name}:\n{Description}";
        }
    }
}
