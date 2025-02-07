using WhmCalcMaui.Models;

namespace WhmCalcMaui.Services.Calculations
{
    public static class AccuracyCalc
    {
        /// <summary>
        /// Вторым параметром принимает коллекцию выбранных модификаторов. Расчитывает вероятность попасть атакой по цели.
        /// </summary>
        public static double ToHitRoll(int accuracy, ICollection<ModificatorModel> mods)
        {
            // С реролом 1
            if (mods.Any(m => m.Id == 1))
            {
                return DiceRoller.Roll_W_RR1(accuracy);
            }
            // С полным реролом
            if (mods.Any(m => m.Id == 2))
            {
                return DiceRoller.Roll_W_RR_All(accuracy);
            }
            // Без реролов
            return DiceRoller.Roll(accuracy);
        }
    }
}
