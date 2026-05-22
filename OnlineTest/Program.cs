using System;
using System.Collections.Generic;
using OnlineTest.Core;
using OnlineTest.Models;
using OnlineTest.Patterns;

namespace OnlineTest
{
    class Program
    {
        private static IScoringStrategy _currentStrategy = new StandardScoringStrategy();
        private static string _sessionMode = "standard";
        private static readonly List<TestResult> _history = new List<TestResult>();

        static void Main()
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            bool running = true;
            while (running)
            {
                ShowMainMenu();
                string choice = Console.ReadLine();
                if (choice == null) choice = "";
                else choice = choice.Trim();

                switch (choice)
                {
                    case "1": SelectAndRunTest(); break;
                    case "2": ChangeStrategy(); break;
                    case "3": ChangeSessionMode(); break;
                    case "4": ShowHistory(); break;
                    case "0": running = false; break;
                    default:
                        Console.WriteLine("  Невiрний вибiр.");
                        break;
                }
            }

            Console.WriteLine("  До побачення!");
        }

        static void ShowMainMenu()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("  ОНЛАЙН-ТЕСТИ  (Варiант 11)");
            Console.WriteLine("  Стратегiя: " + _currentStrategy.Name
                              + "   Режим: " + SessionModeName(_sessionMode));
            Console.WriteLine();
            Console.WriteLine("  1. Пройти тест");
            Console.WriteLine("  2. Змiнити стратегiю оцiнювання");
            Console.WriteLine("  3. Змiнити режим сесiї");
            Console.WriteLine("  4. Iсторiя результатiв");
            Console.WriteLine("  0. Вихiд");
            Console.WriteLine();
            Console.Write("  Вибiр: ");
        }

        static void SelectAndRunTest()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("  Оберiть тест:");
            Console.WriteLine();
            Console.WriteLine("  1. Патерни проектування: основи  (5 питань, легкий, 10 хв)");
            Console.WriteLine("  2. UML: дiаграми класiв          (5 питань, середнiй, 15 хв)");
            Console.WriteLine("  3. Пiдходи до проектування ПЗ   (5 питань, важкий, 20 хв)");
            Console.WriteLine("  0. Назад");
            Console.WriteLine();
            Console.Write("  Вибiр: ");

            string choice = Console.ReadLine();
            if (choice == null) choice = "";
            else choice = choice.Trim();
            if (choice == "0") return;

            Test test = null;
            try
            {
                TestDirector director = new TestDirector(new TestBuilder());
                if (choice == "1") test = director.BuildPatternsBasicTest();
                else if (choice == "2") test = director.BuildUmlClassDiagramTest();
                else if (choice == "3") test = director.BuildDesignApproachesTest();

                if (test == null)
                {
                    Console.WriteLine("  Невiрний вибiр.");
                    Pause();
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("  Помилка: " + ex.Message);
                Pause();
                return;
            }

            TestSession session;
            if (_sessionMode == "hint") session = new HintTestSession(test, _currentStrategy);
            else if (_sessionMode == "quick") session = new QuickTestSession(test, _currentStrategy);
            else session = new StandardTestSession(test, _currentStrategy);

            TestResult result = session.Run();
            if (result != null)
                _history.Add(result);

            Pause();
        }

        static void ChangeStrategy()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("  Стратегiя оцiнювання:");
            Console.WriteLine();

            List<IScoringStrategy> strategies = new List<IScoringStrategy>
            {
                new StandardScoringStrategy(),
                new PenaltyScoringStrategy(),
                new ProgressiveScoringStrategy()
            };

            for (int i = 0; i < strategies.Count; i++)
            {
                string mark = strategies[i].Name == _currentStrategy.Name ? "  <-- поточна" : "";
                Console.WriteLine("  " + (i + 1) + ". " + strategies[i].Name + mark);
                Console.WriteLine("     " + strategies[i].Describe());
                Console.WriteLine();
            }

            Console.WriteLine("  0. Назад");
            Console.Write("  Вибiр: ");

            int c;
            if (int.TryParse(Console.ReadLine(), out c) && c >= 1 && c <= strategies.Count)
            {
                _currentStrategy = strategies[c - 1];
                Console.WriteLine("  Стратегiю змiнено: " + _currentStrategy.Name);
            }

            Pause();
        }

        static void ChangeSessionMode()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("  Режим сесiї:");
            Console.WriteLine();

            string[] modeIds = { "standard", "hint", "quick" };
            string[] modeDesc = {
                "Стандартний — базовий режим без пiдказок",
                "З пiдказками — показує правильну вiдповiдь пiсля кожного питання",
                "Швидкий     — без вступу, стартує одразу"
            };

            for (int i = 0; i < modeIds.Length; i++)
            {
                string mark = modeIds[i] == _sessionMode ? "  <-- поточний" : "";
                Console.WriteLine("  " + (i + 1) + ". " + modeDesc[i] + mark);
            }

            Console.WriteLine();
            Console.WriteLine("  0. Назад");
            Console.Write("  Вибiр: ");

            int c;
            if (int.TryParse(Console.ReadLine(), out c) && c >= 1 && c <= modeIds.Length)
            {
                _sessionMode = modeIds[c - 1];
                Console.WriteLine("  Режим змiнено: " + SessionModeName(_sessionMode));
            }

            Pause();
        }

        static void ShowHistory()
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("  Iсторiя результатiв:");
            Console.WriteLine();

            if (_history.Count == 0)
            {
                Console.WriteLine("  Немає результатiв.");
                Pause();
                return;
            }

            Console.WriteLine("  N   Студент            Тест                              Бали       Оцiнка");
            Console.WriteLine("  " + new string('-', 72));

            for (int i = 0; i < _history.Count; i++)
            {
                TestResult r = _history[i];
                Console.WriteLine("  " + (i + 1).ToString().PadRight(4)
                    + r.StudentName.PadRight(19)
                    + r.TestTitle.PadRight(34)
                    + (r.Score + "/" + r.MaxScore).PadRight(11)
                    + r.Grade);
            }

            Pause();
        }

        static void Pause()
        {
            Console.WriteLine();
            Console.Write("  Натиснiть Enter...");
            Console.ReadLine();
        }

        static string SessionModeName(string mode)
        {
            if (mode == "hint") return "З пiдказками";
            if (mode == "quick") return "Швидкий";
            return "Стандартний";
        }
    }
}