using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using System.ComponentModel.DataAnnotations;

namespace WhmCalcMaui.Models
{
    public sealed partial class TargetModel : ObservableObject
    {
        [ObservableProperty]
        [property: PrimaryKey]
        //[property: StringLength(1, 20)]
        private string targetName;

        [ObservableProperty]
        //[property: MaxLength(3)]
        private int targetT;

        [ObservableProperty]
        //[property: MaxLength(1)]
        private int targetSv;

        [ObservableProperty]
        //[property: MaxLength(3)]
        private int targetW;

        [Ignore]
        public string TargetProps { get => ToString(); }

        public override string ToString()
        {
            return $"T: {TargetT} Sv: {TargetSv} W: {TargetW}";
        }
    }
}
