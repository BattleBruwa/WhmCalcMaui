using CommunityToolkit.Mvvm.ComponentModel;
using SQLite;
using System.ComponentModel.DataAnnotations;

namespace WhmCalcMaui.Models
{
    public sealed partial class TargetModel : ObservableObject
    {
        [ObservableProperty]
        [property: PrimaryKey]
        private string targetName;

        [ObservableProperty]
        private int targetT;

        [ObservableProperty]
        private int targetSv;

        [ObservableProperty]
        private int targetW;

        [Ignore]
        public string TargetProps { get => ToString(); }

        public override string ToString()
        {
            return $"T: {TargetT} Sv: {TargetSv} W: {TargetW}";
        }

        public void CopyProps(TargetModel target)
        {
            TargetName = target.TargetName;
            TargetT = target.TargetT;
            TargetSv = target.TargetSv;
            TargetW = target.TargetW;
        }
    }
}
