using System;
using System.Collections.Generic;
using OnlineTest.Models;

namespace OnlineTest.Patterns
{
    // ПАТЕРН: Builder
    // Дозволяє покроково будувати об'єкт Test через ланцюжок викликів.
    // TestDirector зберігає банки питань і динамічно генерує тести.

    public interface ITestBuilder
    {
        ITestBuilder SetTitle(string title);
        ITestBuilder SetSubject(string subject);
        ITestBuilder SetTimeLimit(int minutes);
        ITestBuilder SetDifficulty(DifficultyLevel difficulty);
        ITestBuilder AddQuestion(Question question);
        Test Build();
    }

    public class TestBuilder : ITestBuilder
    {
        private readonly Test _test;

        public TestBuilder()
        {
            _test = new Test();
        }

        public ITestBuilder SetTitle(string title)
        {
            _test.Title = title;
            return this;
        }

        public ITestBuilder SetSubject(string subject)
        {
            _test.Subject = subject;
            return this;
        }

        public ITestBuilder SetTimeLimit(int minutes)
        {
            _test.TimeLimit = minutes;
            return this;
        }

        public ITestBuilder SetDifficulty(DifficultyLevel difficulty)
        {
            _test.Difficulty = difficulty;
            return this;
        }

        public ITestBuilder AddQuestion(Question question)
        {
            _test.Questions.Add(question);
            return this;
        }

        public Test Build()
        {
            if (string.IsNullOrWhiteSpace(_test.Title))
                throw new InvalidOperationException("Тест не має назви.");
            if (_test.Questions.Count == 0)
                throw new InvalidOperationException("Тест не має питань.");
            return _test;
        }
    }

    public class TestDirector
    {
        private readonly ITestBuilder _builder;
        private readonly Random _random = new Random();

        public TestDirector(ITestBuilder builder)
        {
            _builder = builder;
        }

        // ── Банк питань: Патерни проектування ─────────────────────
        private List<Question> GetPatternsBank()
        {
            return new List<Question>
            {
                new Question(
                    "До якої групи належить патерн Builder?",
                    new List<string> { "Поведінкові", "Структурні", "Породжуючі", "Архітектурні" },
                    2, 10, DifficultyLevel.Easy),
                new Question(
                    "Який патерн гарантує єдиний екземпляр класу в програмі?",
                    new List<string> { "Prototype", "Singleton", "Factory Method", "Builder" },
                    1, 10, DifficultyLevel.Easy),
                new Question(
                    "Що визначає патерн Strategy?",
                    new List<string> {
                        "Єдину точку доступу до ресурсу",
                        "Скелет алгоритму в базовому класі",
                        "Сімейство взаємозамінних алгоритмів",
                        "Спосіб обходу колекції" },
                    2, 10, DifficultyLevel.Easy),
                new Question(
                    "Патерн Observer відноситься до групи...",
                    new List<string> { "Породжуючих", "Структурних", "Поведінкових", "Архітектурних" },
                    2, 10, DifficultyLevel.Easy),
                new Question(
                    "Яку проблему вирішує патерн Facade?",
                    new List<string> {
                        "Забезпечує підписку на події",
                        "Спрощує складну підсистему через єдиний інтерфейс",
                        "Дозволяє об'єктам змінювати поведінку залежно від стану",
                        "Гарантує один екземпляр класу" },
                    1, 10, DifficultyLevel.Easy),
                new Question(
                    "Що є спільним між Builder та Abstract Factory?",
                    new List<string> {
                        "Обидва є поведінковими патернами",
                        "Обидва породжуючі та приховують деталі створення об'єктів",
                        "Обидва гарантують єдиний екземпляр",
                        "Обидва використовують рекурсію" },
                    1, 10, DifficultyLevel.Easy),
                new Question(
                    "Який патерн дозволяє додати поведінку об'єкту без зміни його класу?",
                    new List<string> { "Singleton", "Decorator", "Factory Method", "Iterator" },
                    1, 10, DifficultyLevel.Easy),
                new Question(
                    "Для чого використовується патерн Template Method?",
                    new List<string> {
                        "Для створення об'єктів через інтерфейс",
                        "Для визначення скелету алгоритму з можливістю перевизначення кроків",
                        "Для управління доступом до об'єкту",
                        "Для перетворення інтерфейсу класу" },
                    1, 10, DifficultyLevel.Easy),
            };
        }

        // ── Банк питань: UML-діаграми класів ──────────────────────
        private List<Question> GetUmlBank()
        {
            return new List<Question>
            {
                new Question(
                    "Яким символом позначається агрегація на UML-діаграмі класів?",
                    new List<string> {
                        "Суцільний ромб біля власника",
                        "Порожній ромб біля цілого",
                        "Стрілка з відкритим наконечником",
                        "Пунктирна лінія зі стрілкою" },
                    1, 15, DifficultyLevel.Medium),
                new Question(
                    "Чим відрізняється композиція від агрегації в UML?",
                    new List<string> {
                        "Нічим, це синоніми",
                        "При композиції частина може існувати без цілого",
                        "При композиції частина не може існувати без цілого",
                        "Агрегація — сильніший зв'язок" },
                    2, 15, DifficultyLevel.Medium),
                new Question(
                    "Що означає множинність '0..*' на зв'язку в UML?",
                    new List<string> {
                        "Рівно нуль або один об'єкт",
                        "Від нуля до нескінченності об'єктів",
                        "Обов'язково хоча б один об'єкт",
                        "Від одного до нескінченності" },
                    1, 15, DifficultyLevel.Medium),
                new Question(
                    "Яким рядком позначається реалізація інтерфейсу в UML?",
                    new List<string> {
                        "Суцільна лінія з порожнім трикутником",
                        "Пунктирна лінія зі суцільним трикутником",
                        "Пунктирна лінія з порожнім трикутником",
                        "Суцільна лінія із суцільним трикутником" },
                    2, 15, DifficultyLevel.Medium),
                new Question(
                    "Що зображує стереотип <<interface>> в UML?",
                    new List<string> {
                        "Абстрактний клас без реалізації методів",
                        "Контракт, який клас зобов'язаний виконати",
                        "Статичний клас-утиліта",
                        "Клас з тільки приватними полями" },
                    1, 20, DifficultyLevel.Medium),
                new Question(
                    "Яким зв'язком позначається наслідування в UML?",
                    new List<string> {
                        "Пунктирна лінія з ромбом",
                        "Суцільна лінія з порожнім трикутником",
                        "Пунктирна стрілка",
                        "Суцільна лінія з суцільним трикутником" },
                    1, 15, DifficultyLevel.Medium),
                new Question(
                    "Що означає '+' перед атрибутом або методом у класі UML?",
                    new List<string> { "private", "protected", "public", "static" },
                    2, 15, DifficultyLevel.Medium),
                new Question(
                    "Яке позначення відповідає 'рівно один' у множинності UML?",
                    new List<string> { "0..1", "1", "1..*", "*" },
                    1, 15, DifficultyLevel.Medium),
            };
        }

        // ── Банк питань: Підходи до проектування ──────────────────
        private List<Question> GetDesignBank()
        {
            return new List<Question>
            {
                new Question(
                    "Що означає принцип DRY?",
                    new List<string> {
                        "Код повинен бути якомога коротшим",
                        "Кожен фрагмент знань має єдине представлення в системі",
                        "Класи мають бути незалежними один від одного",
                        "Інтерфейси важливіші за реалізацію" },
                    1, 20, DifficultyLevel.Hard),
                new Question(
                    "Принцип SOLID: що означає літера 'O'?",
                    new List<string> {
                        "Орієнтованість на об'єкти",
                        "Відкритість для розширення, закритість для змін",
                        "Обов'язкова наявність інтерфейсів",
                        "Один клас — одна відповідальність" },
                    1, 20, DifficultyLevel.Hard),
                new Question(
                    "Яка архітектура передбачає розподіл на Presentation, Business Logic, Data Access?",
                    new List<string> {
                        "Мікросервісна",
                        "Event-driven",
                        "N-рівнева (N-tier / Layered)",
                        "MVVM" },
                    2, 20, DifficultyLevel.Hard),
                new Question(
                    "Що таке Dependency Injection?",
                    new List<string> {
                        "Патерн, що забороняє залежності між класами",
                        "Техніка передачі залежностей ззовні, а не їх створення всередині",
                        "Спосіб організації бази даних",
                        "Різновид патерну Singleton" },
                    1, 25, DifficultyLevel.Hard),
                new Question(
                    "Чим MVVM відрізняється від MVC?",
                    new List<string> {
                        "У MVVM немає моделі",
                        "MVVM використовує ViewModel з двостороннім прив'язуванням даних замість Controller",
                        "MVC — для мобільних, MVVM — для веб",
                        "Вони ідентичні, лише різні назви" },
                    1, 25, DifficultyLevel.Hard),
                new Question(
                    "Що означає принцип KISS?",
                    new List<string> {
                        "Код має бути написаний якнайшвидше",
                        "Рішення має бути простим — уникати зайвої складності",
                        "Кожен клас відповідає лише за одну дію",
                        "Інтерфейси мають бути мінімальними" },
                    1, 20, DifficultyLevel.Hard),
                new Question(
                    "Принцип SOLID: що означає літера 'L' (принцип Ліскова)?",
                    new List<string> {
                        "Клас повинен мати лише один метод",
                        "Підклас має бути замінним базовим без порушення логіки програми",
                        "Залежності мають передаватися ззовні",
                        "Класи мають залежати від абстракцій, а не деталей" },
                    1, 25, DifficultyLevel.Hard),
                new Question(
                    "Що таке Separation of Concerns?",
                    new List<string> {
                        "Розподіл програми на мікросервіси",
                        "Принцип розділення програми на частини з різними відповідальностями",
                        "Заборона глобальних змінних",
                        "Обов'язкове використання інтерфейсів" },
                    1, 20, DifficultyLevel.Hard),
            };
        }

        // ── Динамічна генерація: випадкова вибірка N питань з банку ─
        private List<Question> PickRandom(List<Question> bank, int count)
        {
            // Перемішуємо банк (Fisher-Yates shuffle)
            for (int i = bank.Count - 1; i > 0; i--)
            {
                int j = _random.Next(i + 1);
                Question temp = bank[i];
                bank[i] = bank[j];
                bank[j] = temp;
            }
            // Повертаємо перші count елементів
            List<Question> result = new List<Question>();
            for (int i = 0; i < count && i < bank.Count; i++)
                result.Add(bank[i]);
            return result;
        }

        // Тест 1: Патерни проектування — 5 випадкових з 8
        public Test BuildPatternsBasicTest()
        {
            _builder
                .SetTitle("Патерни проектування: основи")
                .SetSubject("Патерни проектування")
                .SetTimeLimit(10)
                .SetDifficulty(DifficultyLevel.Easy);

            foreach (Question q in PickRandom(GetPatternsBank(), 5))
                _builder.AddQuestion(q);

            return _builder.Build();
        }

        // Тест 2: UML-діаграми класів — 5 випадкових з 8
        public Test BuildUmlClassDiagramTest()
        {
            _builder
                .SetTitle("UML: діаграми класів")
                .SetSubject("UML-моделювання")
                .SetTimeLimit(15)
                .SetDifficulty(DifficultyLevel.Medium);

            foreach (Question q in PickRandom(GetUmlBank(), 5))
                _builder.AddQuestion(q);

            return _builder.Build();
        }

        // Тест 3: Підходи до проектування — 5 випадкових з 8
        public Test BuildDesignApproachesTest()
        {
            _builder
                .SetTitle("Підходи до проектування ПЗ")
                .SetSubject("Архітектура та проектування")
                .SetTimeLimit(20)
                .SetDifficulty(DifficultyLevel.Hard);

            foreach (Question q in PickRandom(GetDesignBank(), 5))
                _builder.AddQuestion(q);

            return _builder.Build();
        }
    }
}