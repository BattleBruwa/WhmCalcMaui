using WhmCalcMaui.Models;

namespace WhmCalcMaui.Services.Calculations
{
    public static class WoundCalc
    {
        /// <summary>
        /// Вторым параметром принимает коллекцию выбранных модификаторов. Расчитывает вунд ролл.
        /// </summary>
        public static double ToWoundRoll(int rollNum, ICollection<ModificatorModel> mods)
        {
            // Есть -1 ту вунд
            if (mods.Any(m => m.Id == 8))
            {
                // С реролом 1
                if (mods.Any(m => m.Id == 3))
                {
                    return DiceRoller.Roll_W_RR1(rollNum + 1);
                }
                // С полным реролом
                if (mods.Any(m => m.Id == 4))
                {
                    return DiceRoller.Roll_W_RR_All(rollNum + 1);
                }
                // Без реролов
                return DiceRoller.Roll(rollNum + 1);
            }

            // Есть +1 ту вунд
            if (mods.Any(m => m.Id == 14))
            {
                // С реролом 1
                if (mods.Any(m => m.Id == 3))
                {
                    return DiceRoller.Roll_W_RR1(rollNum - 1);
                }
                // С полным реролом
                if (mods.Any(m => m.Id == 4))
                {
                    return DiceRoller.Roll_W_RR_All(rollNum - 1);
                }
                // Без реролов
                return DiceRoller.Roll(rollNum - 1);
            }

            // Без модификаторов
            // С реролом 1
            if (mods.Any(m => m.Id == 3))
            {
                return DiceRoller.Roll_W_RR1(rollNum);
            }
            // С полным реролом
            if (mods.Any(m => m.Id == 4))
            {
                return DiceRoller.Roll_W_RR_All(rollNum);
            }
            // Без реролов
            return DiceRoller.Roll(rollNum);
        }
    }
}
