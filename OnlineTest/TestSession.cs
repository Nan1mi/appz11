using System;
using System.Collections.Generic;
using OnlineTest.Models;
using OnlineTest.Patterns;

namespace OnlineTest.Core
{
    // ПАТЕРН: Template Method
    // Run() — шаблонний метод із незмінним порядком кроків.
    // Підкласи перевизначають окремі кроки через virtual-методи.

    public abstract class TestSession
    {
        protected readonly Test Test;
        protected readonly IScoringStrategy ScoringStrategy;
        protected string StudentName = string.Empty;

        protected TestSession(Test test, IScoringStrategy scoringStrategy)
        {
            Test = test;
            ScoringStrategy = scoringStrategy;
        }

        // Шаблонний метод
        public TestResult Run()
        {
            Initialize();
            ShowIntro();
            if (!ConfirmStart())
                return null;

            TestResult result = ExecuteQuestions();
            ShowResult(result);
            return result;
        }

        protected virtual void Initialize()
        {
            Console.Write("  Введіть ваше ім'я: ");
            string input = Console.ReadLine();
            StudentName = (input != null) ? input.Trim() : "";
            if (string.IsNullOrWhiteSpace(StudentName))
                StudentName = "Студент";
        }

        protected virtual void ShowIntro()
        {
            Test.Display();
            Console.WriteLine("  Стратегія оцінювання: " + ScoringStrategy.Name);
            Console.WriteLine("  " + ScoringStrategy.Describe());
            Console.WriteLine();
        }

        protected virtual bool ConfirmStart()
        {
            Console.Write("  Розпочати тест? (y/n): ");
            string input = Console.ReadLine();
            return input != null && input.Trim().ToLower() == "y";
        }

        protected virtual TestResult ExecuteQuestions()
        {
            TestResult result = new TestResult
            {
                StudentName = StudentName,
                TestTitle = Test.Title,
                MaxScore = Test.MaxScore,
                TotalQuestions = Test.Questions.Count
            };

            for (int i = 0; i < Test.Questions.Count; i++)
            {
                Question question = Test.Questions[i];
                bool isCorrect = AskQuestion(question, i + 1);
                int earned = ScoringStrategy.Calculate(isCorrect, question.Points, i + 1);

                result.Score += earned;
                if (isCorrect) result.CorrectAnswers++;
                result.Details.Add((i + 1, isCorrect, earned));
            }

            return result;
        }

        protected virtual bool AskQuestion(Question question, int number)
        {
            Console.WriteLine();
            Console.WriteLine("  [" + number + "/" + Test.Questions.Count + "] " + question.Text);
            for (int i = 0; i < question.Options.Count; i++)
                Console.WriteLine("    " + (i + 1) + ") " + question.Options[i]);

            int answer = ReadAnswer(question.Options.Count);
            return answer - 1 == question.CorrectIndex;
        }

        protected int ReadAnswer(int max)
        {
            while (true)
            {
                Console.Write("  Ваша відповідь (1-" + max + "): ");
                int answer;
                if (int.TryParse(Console.ReadLine(), out answer) && answer >= 1 && answer <= max)
                    return answer;
                Console.WriteLine("  Введіть число від 1 до " + max + ".");
            }
        }

        protected virtual void ShowResult(TestResult result)
        {
            if (result == null) return;
            Console.WriteLine();
            Console.WriteLine("  --- Результат ---");
            Console.WriteLine("  Студент  : " + result.StudentName);
            Console.WriteLine("  Тест     : " + result.TestTitle);
            Console.WriteLine("  Бали     : " + result.Score + " / " + result.MaxScore);
            Console.WriteLine("  Правильно: " + result.CorrectAnswers + " / " + result.TotalQuestions);
            Console.WriteLine("  Відсоток : " + result.Percentage.ToString("F1") + "%");
            Console.WriteLine("  Оцінка   : " + result.Grade);
        }
    }

    // Стандартна сесія — поведінка за замовчуванням
    public class StandardTestSession : TestSession
    {
        public StandardTestSession(Test test, IScoringStrategy strategy)
            : base(test, strategy) { }
    }

    // Сесія з підказками — після кожної відповіді показує правильний варіант
    public class HintTestSession : TestSession
    {
        public HintTestSession(Test test, IScoringStrategy strategy)
            : base(test, strategy) { }

        protected override bool AskQuestion(Question question, int number)
        {
            bool isCorrect = base.AskQuestion(question, number);

            if (isCorrect)
                Console.WriteLine("  Правильно!");
            else
                Console.WriteLine("  Неправильно. Правильна відповідь: "
                                  + question.Options[question.CorrectIndex]);

            return isCorrect;
        }

        protected override void ShowResult(TestResult result)
        {
            base.ShowResult(result);
            Console.WriteLine();
            Console.WriteLine("  Деталі по питаннях:");
            foreach (var item in result.Details)
            {
                string status = item.IsCorrect ? "OK" : "X ";
                Console.WriteLine("    " + status + "  Питання " + item.QuestionNum
                                  + ":  " + (item.IsCorrect ? "+" : "") + item.Points + " б.");
            }
        }
    }

    // Швидкий режим — без вступу, стартує одразу
    public class QuickTestSession : TestSession
    {
        public QuickTestSession(Test test, IScoringStrategy strategy)
            : base(test, strategy) { }

        protected override void ShowIntro()
        {
            Console.WriteLine("  [Швидкий режим] " + Test.Title
                              + "  (" + Test.Questions.Count + " питань)");
            Console.WriteLine();
        }

        protected override bool ConfirmStart()
        {
            return true;
        }
    }
}