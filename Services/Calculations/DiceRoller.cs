namespace WhmCalcMaui.Services.Calculations
{
    public static class DiceRoller
    {
        // Примерно 0.17
        private const float SixPlus = 1f / 6f;
        // Примерно 0.33
        private const float FivePlus = 2f / 6f;
        // Примерно 0.5
        private const float FourPlus = 3f / 6f;
        // Примерно 0.67
        private const float ThreePlus = 4f / 6f;
        // Примерно 0.83
        private const float TwoPlus = 5f / 6f;

        /// <summary>
        /// Вероятность выбросить нужное значение
        /// на кубе.
        /// </summary>
        public static float Roll(int input)
        {
            switch (input)
            {
                case <= 2:
                    return TwoPlus;

                case 3:
                    return ThreePlus;

                case 4:
                    return FourPlus;

                case 5:
                    return FivePlus;

                case >= 6:
                    return SixPlus;
            }
        }
        /// <summary>
        /// Вероятность выбросить нужное значение
        /// на кубе с реролом 1.
        /// </summary>
        public static float Roll_W_RR1(int input)
        {
            switch (input)
            {
                case <= 2:
                    return TwoPlus + SixPlus * TwoPlus;

                case 3:
                    return ThreePlus + SixPlus * ThreePlus;

                case 4:
                    return FourPlus + SixPlus * FourPlus;

                case 5:
                    return FivePlus + SixPlus * FivePlus;

                case >= 6:
                    return SixPlus + SixPlus * SixPlus;
            }
        }
        /// <summary>
        /// Вероятность выбросить нужное значение
        /// на кубе с полным реролом.
        /// </summary>
        public static float Roll_W_RR_All(int input)
        {
            switch (input)
            {
                case <= 2:
                    return TwoPlus + SixPlus * TwoPlus;

                case 3:
                    return ThreePlus + FivePlus * ThreePlus;

                case 4:
                    return FourPlus + FourPlus * FourPlus;

                case 5:
                    return FivePlus + ThreePlus * FivePlus;

                case >= 6:
                    return SixPlus + TwoPlus * SixPlus;
            }
        }
    }
}
