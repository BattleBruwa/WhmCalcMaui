using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WhmCalcMaui.Models
{
    public sealed partial class OutputModel : ObservableObject
    {
		private double attacks;
        /// <summary>
        /// Количество атак.
        /// </summary>
		public double Attacks
		{
			get { return attacks; }
			set { SetProperty(ref attacks, Math.Round(value, 2)); }
		}

        private double natHits;
        /// <summary>
        /// Количество попаданий БЕЗ доп хитов.
        /// </summary>
        public double NatHits
        {
            get { return natHits; }
            set { SetProperty(ref natHits, Math.Round(value, 2)); }
        }

        private double susHits;
        /// <summary>
        /// Количество доп хитов от правила SustainedHits.
        /// </summary>
        public double SustainedHits
        {
            get { return susHits; }
            set { SetProperty(ref susHits, Math.Round(value, 2)); }
        }

        private double natWounds;
        /// <summary>
        /// Количесвто вунд БЕЗ леталок.
        /// </summary>
        public double NatWounds
        {
            get { return natWounds; }
            set { SetProperty(ref natWounds, Math.Round(value, 2)); }
        }

        private double autoWounds;
        /// <summary>
        /// Количество доп вунд от правила LethalHits.
        /// </summary>
        public double AutoWounds
        {
            get { return autoWounds; }
            set { SetProperty(ref autoWounds, Math.Round(value, 2)); }
        }

        private double unSavedW;
        /// <summary>
        /// Количесвто проваленых спас бросков.
        /// </summary>
        public double UnSavedWounds
        {
            get { return unSavedW; }
            set { SetProperty(ref unSavedW, Math.Round(value, 2)); }
        }

        private double devW;
        /// <summary>
        /// Вунды в обход сейвов от правила DevastatingWounds.
        /// </summary>
        public double DevWounds
        {
            get { return devW; }
            set { SetProperty(ref devW, Math.Round(value, 2)); }
        }

        private double totalD;
        /// <summary>
        /// Полный урон.
        /// </summary>
        public double TotalDamage
        {
            get { return totalD; }
            set { SetProperty(ref totalD, Math.Round(value, 2)); }
        }

        private double deadModels;
        /// <summary>
        /// Количество уничтоженых моделей.
        /// </summary>
        public double DeadModels
        {
            get { return deadModels; }
            set { SetProperty(ref deadModels, Math.Round(value, 0)); }
        }
    }
}
