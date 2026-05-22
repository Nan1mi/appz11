namespace OnlineTest.Patterns
{
    // ПАТЕРН: Strategy
    // Інкапсулює алгоритм підрахунку балів.
    // Стратегію можна змінювати у рантаймі без зміни класу TestSession.

    public interface IScoringStrategy
    {
        string Name { get; }
        int Calculate(bool isCorrect, int basePoints, int questionNumber);
        string Describe();
    }

    // Стандартна: правильна = повні бали, неправильна = 0
    public class StandardScoringStrategy : IScoringStrategy
    {
        public string Name { get { return "Стандартна"; } }

        public int Calculate(bool isCorrect, int basePoints, int questionNumber)
        {
            return isCorrect ? basePoints : 0;
        }

        public string Describe()
        {
            return "Правильна = повні бали, неправильна = 0";
        }
    }

    // З штрафом: неправильна відповідь знімає половину балів питання
    public class PenaltyScoringStrategy : IScoringStrategy
    {
        public string Name { get { return "З штрафом"; } }

        public int Calculate(bool isCorrect, int basePoints, int questionNumber)
        {
            return isCorrect ? basePoints : -(basePoints / 2);
        }

        public string Describe()
        {
            return "Правильна = повні бали, неправильна = -50% балів";
        }
    }

    // Прогресивна: множник зростає на 10% з кожним питанням
    public class ProgressiveScoringStrategy : IScoringStrategy
    {
        public string Name { get { return "Прогресивна"; } }

        public int Calculate(bool isCorrect, int basePoints, int questionNumber)
        {
            if (!isCorrect) return 0;
            double multiplier = 1.0 + (questionNumber - 1) * 0.1;
            return (int)(basePoints * multiplier);
        }

        public string Describe()
        {
            return "Правильна = початкові бали з додаванням 10% за кожне наступне значення після першого, неправильна = 0";
        }
    }
}