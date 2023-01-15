using Newtonsoft.Json;
using static System.IO.File;

namespace quiz_game;

public class Question
{
    public string question { get; set; }
    public string[] answers { get; set; }
    public int correctIndex { get; set; }
}

static class Program
{
    // Results
    private static int _correctAnswers = 0;
    private static int _incorrectAnswers = 0;
    
    // User Details
    private static string? _name;
    
    // Table Width
    private static int tableWidth = 100;

    private static void AskQuestion(string question, string[] answers, int correctAnswerIdx)
    {
        string possibleAnswers = "";
        
        Console.WriteLine(question);
        int i = 1;
        foreach (string answer in answers)
        {
            possibleAnswers += i.ToString() + ") " + answer + "\n";
            i++;
        }

        i = 1;
        Console.Write(possibleAnswers);
        Console.Write("Answer: ");
        int answer1 = Convert.ToInt32(Console.ReadLine());
        if (answer1 != correctAnswerIdx)
        {
            _incorrectAnswers++;
        }
        else
        {
            _correctAnswers++;
        }
        Console.WriteLine();
    }

    private static void ShowResults()
    {
        // Calculate the percentage
        int totalQuestions = _incorrectAnswers + _correctAnswers;
        int percentComplete = (int)Math.Round((double)(100 * _correctAnswers) / totalQuestions);

        Console.Clear();
        PrintLine();
        PrintRow("Name", "Total Correct Answers", "Total Incorrect Answers", "Percentage");
        PrintLine();
        PrintRow(_name, _correctAnswers.ToString(), _incorrectAnswers.ToString(), percentComplete.ToString() + "%");
        PrintLine();
    }

    public static void Main(string[] args)
    {
        // Get user name
        Console.Write("Enter Name: ");
        _name = Console.ReadLine();
        Console.WriteLine();
    
        // Display Assignment title
        Console.WriteLine("Assignment title: Quiz Game");
        Console.WriteLine();

        // Get data from JSON
        string json = ReadAllText("/Users/scriptjumper/git/quiz-game/quiz-game/questions.json");
        var questions = JsonConvert.DeserializeObject<List<Question>>(json);
        foreach (var question in questions)
        {
            // Start asking questions
            AskQuestion(question.question, question.answers, question.correctIndex);
        }

        // Display Results
        ShowResults();
    }

    static void PrintLine()
    {
        Console.WriteLine(new string('-', tableWidth));
    }

    static void PrintRow(params string[] columns)
    {
        int width = (tableWidth - columns.Length) / columns.Length;
        string row = "|";

        foreach (string column in columns)
        {
            row += AlignCentre(column, width) + "|";
        }

        Console.WriteLine(row);
    }

    static string AlignCentre(string text, int width)
    {
        text = text.Length > width ? text.Substring(0, width - 3) + "..." : text;

        if (string.IsNullOrEmpty(text))
        {
            return new string(' ', width);
        }
        else
        {
            return text.PadRight(width - (width - text.Length) / 2).PadLeft(width);
        }
    }
}