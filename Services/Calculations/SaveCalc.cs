using WhmCalcMaui.Models;

namespace WhmCalcMaui.Services.Calculations
{
    public static class SaveCalc
    {
        /// <summary>
        /// Третьим параметром принимает коллекцию выбранных модификаторов. Расчитывает сейв ролл.
        /// </summary>
        public static float ToSaveRoll(AttackerModel attacker, TargetModel target, ICollection<ModificatorModel> mods)
        {
            int resultedRoll = 0;
            // Если есть инвуль
            if (mods.Any(m => m.Id == 11))
            {
                var modWithInvul = mods.Single(m => m.Id == 11);
                int invulCon = modWithInvul.Condition ?? 0;
                // Если арморпен + армор больше инвуля, используем инвуль
                if (attacker.AttackerAP + target.TargetSv >= invulCon)
                {
                    resultedRoll = invulCon;
                    return DiceRoller.Roll(resultedRoll);
                }
                // Используем арморпен + армор, если это меньше инвуля
                resultedRoll = attacker.AttackerAP + target.TargetSv;
                return DiceRoller.Roll(resultedRoll);
            }
            // Если нет инвуля
            resultedRoll = attacker.AttackerAP + target.TargetSv;
            return DiceRoller.Roll(resultedRoll);
        }
    }
}
