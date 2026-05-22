using System.Collections.Generic;

namespace OnlineTest.Models
{
    public class Question
    {
        public string Text { get; set; }
        public List<string> Options { get; set; }
        public int CorrectIndex { get; set; }
        public int Points { get; set; }
        public DifficultyLevel Difficulty { get; set; }

        public Question(string text, List<string> options, int correctIndex,
                        int points, DifficultyLevel difficulty)
        {
            Text = text;
            Options = options;
            CorrectIndex = correctIndex;
            Points = points;
            Difficulty = difficulty;
        }
    }

    public enum DifficultyLevel
    {
        Easy,
        Medium,
        Hard
    }
}