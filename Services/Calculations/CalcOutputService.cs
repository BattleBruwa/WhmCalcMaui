using WhmCalcMaui.Models;

namespace WhmCalcMaui.Services.Calculations
{
    public class CalcOutputService
    {
        #region Частичный расчет
        private void ResolveAttacks(AttackerModel attacker, OutputModel output)
        {
            output.Attacks = AttackOrDamageCalc.CalculateAOrD(attacker.AttackerA);
        }

        // Вычисление SustainedHits
        private void SusteinedHits(OutputModel output, ICollection<ModificatorModel> mods)
        {
            var susHitsMod = mods.Single(m => m.Id == 6);
            int susHitsCon = susHitsMod.Condition ?? 0;
            // Без критов
            if (mods.Any(m => m.Id == 15) is false)
            {
                output.SustainedHits = output.Attacks * AccuracyCalc.ToHitRoll(6, mods) * susHitsCon;
                return;
            }
            // С критами
            if (mods.Any(m => m.Id == 15))
            {
                var critMod = mods.Single(m => m.Id == 15);
                int critCon = critMod.Condition ?? 0;

                output.SustainedHits = output.Attacks * AccuracyCalc.ToHitRoll(critCon, mods) * susHitsCon;
                return;
            }
        }

        // Вычисление LethalHits
        private void LethalHits(OutputModel output, ICollection<ModificatorModel> mods)
        {
            // Без критов
            if (mods.Any(m => m.Id == 15) is false)
            {
                output.AutoWounds = output.Attacks * AccuracyCalc.ToHitRoll(6, mods);
                return;
            }
            // С критами
            if (mods.Any(m => m.Id == 15))
            {
                var critMod = mods.Single(m => m.Id == 15);
                int critCon = critMod.Condition ?? 0;

                output.AutoWounds = output.Attacks * AccuracyCalc.ToHitRoll(critCon, mods);
                return;
            }
        }

        // Вычисление DevastatingWounds
        private void DevWounds(OutputModel output, ICollection<ModificatorModel> mods)
        {
            // Без Анти х
            if (mods.Any(m => m.Id == 9) is false)
            {
                output.DevWounds = output.AllHits * WoundCalc.ToWoundRoll(6, mods);
                return;
            }
            // С Анти х
            if (mods.Any(m => m.Id == 9))
            {
                var critMod = mods.Single(m => m.Id == 9);
                int critCon = critMod.Condition ?? 0;

                output.DevWounds = output.AllHits * WoundCalc.ToWoundRoll(critCon, mods);
                return;
            }
        }

        // Вычисление DamagePerWound
        private void DamagePerWound(AttackerModel attacker, OutputModel output, ICollection<ModificatorModel> mods)
        {
            double damage = AttackOrDamageCalc.CalculateAOrD(attacker.AttackerD);
            // Если есть ополовинивание урона
            if (mods.Any(m => m.Id == 13))
            {
                damage = damage / 2;
            }
            // -1 урон
            if (mods.Any(m => m.Id == 12))
            {
                damage -= 1;
            }
            // Урон не может быть меньше 1
            if (damage < 1)
            {
                damage = 1;
            }
            output.DmgPerWound = damage;
        }

        private void ResolveHits(AttackerModel attacker, OutputModel output, ICollection<ModificatorModel> mods)
        {
            // Наличие реролов проверяет и обрабатывает AccuracyCalc.ToHitRoll
            // Если меткость 0, то автохиты
            if (attacker.AttackerWS == 0)
            {
                output.NatHits = output.Attacks;
                return;
            }
            // Проверка на SustainedHits
            if (mods.Any(m => m.Id == 6))
            {
                SusteinedHits(output, mods);
            }
            else
            {
                output.SustainedHits = 0d;
            }
            // Проверка на LethalHits
            if (mods.Any(m => m.Id == 5))
            {
                LethalHits(output, mods);
            }
            else
            {
                output.AutoWounds = 0d;
            }
            // Без критов
            if (mods.Any(m => m.Id == 15) is false)
            {
                output.NatHits = output.Attacks * AccuracyCalc.ToHitRoll(attacker.AttackerWS, mods) - output.AutoWounds;
                return;
            }
            // C критами
            if (mods.Any(m => m.Id == 15))
            {
                var critMod = mods.Single(m => m.Id == 15);
                int critCon = critMod.Condition ?? 0;
                // Если меткость лучше крита
                if (attacker.AttackerWS <= critCon)
                {
                    output.NatHits = output.Attacks * AccuracyCalc.ToHitRoll(attacker.AttackerWS, mods) - output.AutoWounds;
                    return;
                }
                // Если меткость хуже крита
                if (attacker.AttackerWS > critCon)
                {
                    output.NatHits = output.Attacks * AccuracyCalc.ToHitRoll(critCon, mods) - output.AutoWounds;
                    return;
                }
            }
        }

        private void ResolveWounds(AttackerModel attacker,TargetModel target, OutputModel output, ICollection<ModificatorModel> mods)
        {
            // -1 ToWound, +1 ToWound и реролы обрабатываются в WoundCalc.ToWoundRoll
            int resultedRoll = 0;

            if (attacker.AttackerS == target.TargetT)
            {
                resultedRoll = 4;
            }
            if (attacker.AttackerS > target.TargetT && attacker.AttackerS < target.TargetT * 2)
            {
                resultedRoll = 3;
            }
            if (attacker.AttackerS >= target.TargetT * 2)
            {
                resultedRoll = 2;
            }
            if (attacker.AttackerS < target.TargetT && attacker.AttackerS * 2 > target.TargetT)
            {
                resultedRoll = 5;
            }
            if (attacker.AttackerS * 2 <= target.TargetT)
            {
                resultedRoll = 6;
            }

            // Проверка на DevastatingWounds
            if (mods.Any(m => m.Id == 7))
            {
                DevWounds(output, mods);
            }
            else
            {
                output.DevWounds = 0d;
            }

            // С анти х
            if (mods.Any(m => m.Id == 9))
            {
                var critMod = mods.Single(m => m.Id == 9);
                int critCon = critMod.Condition ?? 0;
                // Если анти х лучше resultedRoll
                if (critCon <= resultedRoll)
                {
                    output.NatWounds = output.AllHits * WoundCalc.ToWoundRoll(critCon, mods) - output.DevWounds;
                    return;
                }
                // Если resultedRoll лучше анти х
                if (critCon > resultedRoll)
                {
                    output.NatWounds = output.AllHits * WoundCalc.ToWoundRoll(resultedRoll, mods) - output.DevWounds;
                    return;
                }
            }
            // Без анти х
            if (mods.Any(m => m.Id == 9) is false)
            {
                output.NatWounds = output.AllHits * WoundCalc.ToWoundRoll(resultedRoll, mods) - output.DevWounds;
                return;
            }
        }

        private void ResolveSave(AttackerModel attacker, TargetModel target, OutputModel output, ICollection<ModificatorModel> mods)
        {
            // Инвуль обрабатывается в ArmorSaveCalc.ToSaveRoll
            double savedWounds = 0d;
            savedWounds = output.AllWounds * SaveCalc.ToSaveRoll(attacker, target, mods);
            output.UnSavedWounds = output.AllWounds - savedWounds;
        }

        private void ResolveDeadModels(AttackerModel attacker, TargetModel target, OutputModel output, ICollection<ModificatorModel> mods)
        {
            DamagePerWound(attacker, output, mods);

            int deadModels = 0;
            // ФНП
            if (mods.Any(m => m.Id == 10))
            {
                var fnpMod = mods.Single(m => m.Id == 10);
                int fnpCon = fnpMod.Condition ?? 0;
                // Урон с ФНП
                double fnpDmg = Math.Round(output.DmgPerWound - (output.DmgPerWound * ((7f - fnpCon) / 6f)), 2);
                // Если урон с фнп меньше хп цели
                if (fnpDmg < target.TargetW)
                {
                    for (int i = 1; i < output.UnSavedWounds; i++)
                    {
                        double c = fnpDmg;
                        while (c < target.TargetW && i < output.UnSavedWounds)
                        {
                            i++;
                            c += c;
                        }
                        if (c >= target.TargetW)
                        {
                            deadModels++;
                        }
                    }
                    // Для дев. вундс
                    for (int i = 1; i < output.DevWounds; i++)
                    {
                        double c = fnpDmg;
                        while (c < target.TargetW && i < output.DevWounds)
                        {
                            i++;
                            c += c;
                        }
                        if (c >= target.TargetW)
                        {
                            deadModels++;
                        }
                    }
                    output.DeadModels = deadModels;
                    return;
                }
                // Если урон с фнп больше или равен хп цели
                if (fnpDmg >= target.TargetW)
                {
                    deadModels = (int)(Math.Round(output.UnSavedWounds, 0) + Math.Round(output.DevWounds, 0));
                    output.DeadModels = deadModels;
                    return;
                }
            }
            // Без ФНП
            // Если урон меньше, чем хп цели
            if (output.DmgPerWound < target.TargetW)
            {
                for (int i = 1; i < output.UnSavedWounds; i++)
                {
                    double c = output.DmgPerWound;
                    while (c < target.TargetW && i < output.UnSavedWounds)
                    {
                        i++;
                        c += output.DmgPerWound;
                    }
                    if (c >= target.TargetW)
                    {
                        deadModels++;
                    }
                }
                // Для дев. вундс
                for (int i = 1; i < output.DevWounds; i++)
                {
                    double c = output.DmgPerWound;
                    while (c < target.TargetW && i < output.DevWounds)
                    {
                        i++;
                        c += output.DmgPerWound;
                    }
                    if (c >= target.TargetW)
                    {
                        deadModels++;
                    }
                }
                output.DeadModels = deadModels;
                return;
            }
            // Если урон больше
            if (output.DmgPerWound >= target.TargetW)
            {
                deadModels = (int)(Math.Round(output.UnSavedWounds, 0) + Math.Round(output.DevWounds, 0));
                output.DeadModels = deadModels;
                return;
            }
        }

        private void ResolveTotalDamage(OutputModel output)
        {
            output.TotalDamage = (output.UnSavedWounds + output.DevWounds) * output.DmgPerWound;
        }
        #endregion

        public void CalculateOutput(AttackerModel attacker, TargetModel target, OutputModel output, ICollection<ModificatorModel> mods)
        {
            ResolveAttacks(attacker, output);
            ResolveHits(attacker, output, mods);
            ResolveWounds(attacker, target, output, mods);
            ResolveSave(attacker, target, output, mods);
            ResolveDeadModels(attacker, target, output, mods);
            ResolveTotalDamage(output);
        }
    }
}
