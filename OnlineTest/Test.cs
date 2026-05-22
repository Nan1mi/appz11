using System;
using System.Collections.Generic;
using System.Linq;

namespace OnlineTest.Models
{
    // Клас який зберігає всю інформацію про тест
    public class Test
    {
        public string Title { get; set; }// Назва
        public string Subject { get; set; } // Предмет
        public int TimeLimit { get; set; }// Час на проходження 
        public List<Question> Questions { get; set; } // Список питань
        public DifficultyLevel Difficulty { get; set; } // Складність

        public Test()
        {
            Questions = new List<Question>();
        }

        public int MaxScore
        {
            get { return Questions.Sum(q => q.Points); }
        }

        public void Display()
        {
            Console.WriteLine();
            Console.WriteLine("  Тест     : " + Title);
            Console.WriteLine("  Предмет  : " + Subject);
            Console.WriteLine("  Складн.  : " + DifficultyName(Difficulty));
            Console.WriteLine("  Питань   : " + Questions.Count);
            Console.WriteLine("  Макс. бал: " + MaxScore);
            Console.WriteLine("  Час      : " + TimeLimit + " хв");
            Console.WriteLine();
        }

        private static string DifficultyName(DifficultyLevel d)
        {
            switch (d)
            {
                case DifficultyLevel.Easy: return "Легкий";
                case DifficultyLevel.Medium: return "Середній";
                case DifficultyLevel.Hard: return "Важкий";
                default: return "Невідомо";
            }
        }
    }
}