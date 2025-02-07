namespace WhmCalcMaui.Services.Calculations
{
    public static class AttackOrDamageCalc
    {
        /// <summary>
        /// Возвращает запаршенное из стринга количество атак или урон.
        /// </summary>
        public static double CalculateAOrD(string? input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return 0d;
            }

            bool result = double.TryParse(input, out var number);

            // Если result запарсился успешно, возвращаем number,
            if (result == true)
            {
                return number;
            }
            // если нет, ищем D
            else
            {
                double amount = 0d;
                int indexOfD = input.IndexOf('D', StringComparison.CurrentCultureIgnoreCase);

                double numBeforeD = 0d;
                if (indexOfD == 0 || input[indexOfD - 1].Equals(' ') || input[indexOfD - 1].Equals('1'))
                {
                    numBeforeD = 1d;
                }
                else
                {
                    numBeforeD = char.GetNumericValue(input[indexOfD - 1]);
                    if (numBeforeD == 0)
                    {
                        string _concatStringD = string.Concat(input[indexOfD - 2], input[indexOfD - 1]);
                        numBeforeD = Convert.ToDouble(_concatStringD);
                    }
                }

                double numAfterD = 0d;
                if (char.GetNumericValue(input[indexOfD + 1]) == 3)
                {
                    numAfterD = 2.0d;
                }
                if (char.GetNumericValue(input[indexOfD + 1]) == 6)
                {
                    numAfterD = 3.5d;
                }

                // Проверка на наличие в строке выражения
                int indexOfPlus = input.IndexOf('+');

                if (indexOfPlus != -1)
                {
                    // Число после плюса
                    double numAfterPlus = char.GetNumericValue(input[indexOfPlus + 1]);
                    if (numAfterPlus == 1)
                    {
                        if (indexOfPlus + 2 == input.Length)
                        {
                            numAfterPlus = 1d;
                        }
                        else
                        {
                            string _concatStringP = string.Concat(input[indexOfPlus + 1], input[indexOfPlus + 2]);
                            numAfterPlus = Convert.ToDouble(_concatStringP);
                        }
                    }

                    amount = numBeforeD * numAfterD + numAfterPlus;
                }
                else
                {
                    amount = numBeforeD * numAfterD;
                }
                return amount;
            }
        }
    }
}
