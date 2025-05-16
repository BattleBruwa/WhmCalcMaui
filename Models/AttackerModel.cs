using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WhmCalcMaui.Models
{
    public partial class AttackerModel : ObservableObject
    {
        [ObservableProperty]
        private string attackerA = "0";

        [ObservableProperty]
        private int attackerWS;

        [ObservableProperty]
        private int attackerS;

        [ObservableProperty]
        private int attackerAP;

        [ObservableProperty]
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
