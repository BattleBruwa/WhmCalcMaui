using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WhmCalcMaui.Models
{
    public sealed partial class AttackerModel : ObservableObject
    {
        [ObservableProperty]
        [property: StringLength(10)]
        private string attackerA = "0";

        [ObservableProperty]
        [property: MaxLength(1)]
        private int attackerWS;

        [ObservableProperty]
        [property: MaxLength(2)]
        private int attackerS;

        [ObservableProperty]
        [property: MaxLength(1)]
        private int attackerAP;

        [ObservableProperty]
        [property: StringLength(10)]
        private string attackerD = "0";


        public void ResetState()
        {
            AttackerA = "0";
            AttackerWS = 0;
            AttackerS = 0;
            AttackerAP = 0;
            AttackerD = "0";
        }
    }
}
