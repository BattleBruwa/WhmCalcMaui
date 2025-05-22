using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace WhmCalcMaui.Models
{
    public partial class OutputModel : ObservableObject
    {
        private const int roundingPrecision = 1;

		private double attacks;
        /// <summary>
        /// Количество атак.
        /// </summary>
		public double Attacks
		{
			get { return attacks; }
			set { SetProperty(ref attacks, Math.Round(value, roundingPrecision)); }
		}

        private double natHits;
        /// <summary>
        /// Количество попаданий БЕЗ доп хитов.
        /// </summary>
        public double NatHits
        {
            get { return natHits; }
            set { SetProperty(ref natHits, Math.Round(value, roundingPrecision)); }
        }

        private double susHits;
        /// <summary>
        /// Количество доп хитов от правила SustainedHits.
        /// </summary>
        public double SustainedHits
        {
            get { return susHits; }
            set { SetProperty(ref susHits, Math.Round(value, roundingPrecision)); }
        }

        /// <summary>
        /// Полное количество хитов.
        /// </summary>
        public double AllHits
        {
            get { return NatHits + SustainedHits; }
        }

        private double natWounds;
        /// <summary>
        /// Количесвто вунд БЕЗ леталок.
        /// </summary>
        public double NatWounds
        {
            get { return natWounds; }
            set { SetProperty(ref natWounds, Math.Round(value, roundingPrecision)); }
        }

        private double autoWounds;
        /// <summary>
        /// Количество доп вунд от правила LethalHits.
        /// </summary>
        public double AutoWounds
        {
            get { return autoWounds; }
            set { SetProperty(ref autoWounds, Math.Round(value, roundingPrecision)); }
        }

        /// <summary>
        /// Полное количество вунд.
        /// </summary>
        public double AllWounds
        {
            get { return NatWounds + AutoWounds; }
        }

        private double unSavedW;
        /// <summary>
        /// Количесвто проваленых спас бросков.
        /// </summary>
        public double UnSavedWounds
        {
            get { return unSavedW; }
            set { SetProperty(ref unSavedW, Math.Round(value, roundingPrecision)); }
        }

        private double devW;
        /// <summary>
        /// Вунды в обход сейвов от правила DevastatingWounds.
        /// </summary>
        public double DevWounds
        {
            get { return devW; }
            set { SetProperty(ref devW, Math.Round(value, roundingPrecision)); }
        }

        private double dmgPerWound;
        /// <summary>
        /// Урон за каждую вунду.
        /// </summary>
        public double DmgPerWound
        {
            get { return dmgPerWound; }
            set { if (value != dmgPerWound) dmgPerWound = value; }
        }

        private double totalD;
        /// <summary>
        /// Полный урон.
        /// </summary>
        public double TotalDamage
        {
            get { return totalD; }
            set { SetProperty(ref totalD, Math.Round(value, roundingPrecision)); }
        }

        private int deadModels;
        /// <summary>
        /// Количество уничтоженых моделей.
        /// </summary>
        public int DeadModels
        {
            get { return deadModels; }
            set { SetProperty(ref deadModels, value); }
        }

    }
}
