using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WhmCalcMaui.Models
{
    public sealed partial class TargetModel : ObservableObject
    {
        [ObservableProperty]
        [property: StringLength(20)]
        private string targetName = string.Empty;

        [ObservableProperty]
        [property: MaxLength(3)]
        private int targetT;

        [ObservableProperty]
        [property: MaxLength(1)]
        private int targetSv;

        [ObservableProperty]
        [property: MaxLength(3)]
        private int targetW;

        public string TargetProps { get => ToString(); }

        public override string ToString()
        {
            return $"{TargetName}\nT:{TargetT} Sv: {TargetSv} W: {TargetW}";
        }
    }
}
