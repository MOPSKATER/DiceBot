using System.Text.RegularExpressions;

namespace DiceBot
{
    internal class Dicer
    {
        private Random _random = new Random();
        private const string PATTERN = "^[0-9]{1,3}d[0-9]{1,3}(\\+[0-9]{1,2})?$";

        public string RollDice(string diceString)
        {
            Match match = Regex.Match(diceString, PATTERN);
            if (!match.Success) return "Your string does not match following pattern " + PATTERN;

            string[] parameters = diceString.Split('d');
            int amount = int.Parse(parameters[0]);
            int diceType;
            string param2 = parameters[1];
            bool modifier = param2.Contains('+');
            int modifierValue = 0;

            if (modifier)
            {
                string[] typeModifyer = param2.Split('+');
                diceType = int.Parse(typeModifyer[0]);
                modifierValue = int.Parse(typeModifyer[1]);
            }
            else
                diceType = int.Parse(parameters[1]);

            if (amount < 1) return "You must roll at lease 1 dice";
            if (diceType < 2) return "You must roll at least a dice with 2 faces";

            int sum = 0, successes = 0;
            int[] values = new int[amount];
            for (int i = 0; i < amount; i++)
            {
                int value = _random.Next(1, diceType + 1);
                values[i] = value;
                sum += value;

                if (value == 1)
                    successes--;
                else if (value == diceType)
                    successes += 2;
                else if (value >  diceType / 2 + 1)
                    successes++;
            }

            string output = modifier ? string.Format("∑: {0}+{4}={6}\n✓: {1}+{4}={5}\n{2} [{3}]+{4}", sum, successes, diceString, string.Join(' ', values), modifierValue, successes+modifierValue, sum+modifierValue) : string.Format("∑: {0}\n✓: {1}\n{2} [{3}]", sum, successes, diceString, string.Join(' ', values));

            return output;
        }
    }
}
