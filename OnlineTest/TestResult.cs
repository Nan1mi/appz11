using System;
using System.Collections.Generic;

namespace OnlineTest.Models
{
    public class TestResult
    {
        public string StudentName { get; set; }// Ім'я 
        public string TestTitle { get; set; }// Назва тесту
        public int Score { get; set; }// Набрана кількість балів
        public int MaxScore { get; set; }// Максимальна кількість балів
        public int TotalQuestions { get; set; }// Кількість питань
        public int CorrectAnswers { get; set; }//Правильні відповіді
        public DateTime CompletedAt { get; set; } // Дата та час завершення тесту
        /// зберігає номер питання, чи була відповідь правильною, і скільки балів за неї дали
        public List<(int QuestionNum, bool IsCorrect, int Points)> Details { get; set; }
        //Конструктор
        //спрацьовує під час створення нового результату
        public TestResult()
        {
            Details = new List<(int, bool, int)>(); // Створюємо порожній список для деталей
            CompletedAt = DateTime.Now;// Фіксуємо поточний час як час завершення
        }
        //вираховує відсоток успішності
        public double Percentage
        {
            get { return MaxScore > 0 ? (double)Score / MaxScore * 100 : 0; }
        }
        // ставить оцінку залежно від відсотків
        public string Grade
        {
            get
            {
                if (Percentage >= 90) return "Відмінно";
                if (Percentage >= 75) return "Добре";
                if (Percentage >= 60) return "Задовільно";
                return "Незадовільно";
            }
        }
    }
}