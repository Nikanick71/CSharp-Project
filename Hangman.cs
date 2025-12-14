
public class HangmanGame
{
    private const int MaxErrors = 6;
    private const string WordsFile = "words.txt";
    private const string ResultsFile = "results.txt";

    private readonly Random rng = new Random();

    public void Run()
    {
        ShowWelcome();
        string[] words = LoadWords();

        while (true)
        {
            string word = words[rng.Next(words.Length)];
            GameResult result = PlayGame(word);

            SaveResult(result);

            if (!AskPlayAgain())
                break;

            Console.Clear();
        }
    }

   

    private string[] LoadWords()
    {
        if (!File.Exists(WordsFile))
        {
            File.WriteAllLines(WordsFile, new[]
            {
                "PROGRAMMING",
                "COMPUTER",
                "ALGORITHM",
                "DATABASE",
                "NETWORK"
            });
        }

        return File.ReadAllLines(WordsFile)
            .Where(w => !string.IsNullOrWhiteSpace(w))
            .Select(w => w.Trim().ToUpperInvariant())
            .ToArray();
    }

    private void SaveResult(GameResult result)
    {
        string log =
            $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} | " +
            $"{(result.Won ? "WIN" : "LOSS")} | " +
            $"WORD: {result.Word} | " +
            $"LIVES LEFT: {result.LivesLeft} | " +
            $"USED: {(result.UsedLetters.Count == 0 ? "None" : string.Join(",", result.UsedLetters))} | " +
            $"FINAL: {string.Join(" ", result.FinalState)}";

        File.AppendAllText(ResultsFile, log + Environment.NewLine);
    }

    

    private GameResult PlayGame(string word)
    {
        char[] guessed = Enumerable.Repeat('_', word.Length).ToArray();
        HashSet<char> used = new HashSet<char>();
        int errors = 0;

        while (errors < MaxErrors && guessed.Contains('_'))
        {
            DisplayStatus(guessed, used, errors);

            string input = Console.ReadLine()?.Trim().ToUpperInvariant() ?? "";
            if (input == "") continue;

            if (input.Length > 1)
            {
                if (input == word)
                {
                    for (int i = 0; i < word.Length; i++)
                        guessed[i] = word[i];
                    break;
                }
                errors++;
                continue;
            }

            char letter = input[0];
            if (!char.IsLetter(letter) || !used.Add(letter))
                continue;

            if (!ApplyGuess(letter, word, guessed))
                errors++;
        }

        bool won = !guessed.Contains('_');
        int livesLeft = MaxErrors - errors;

        Console.WriteLine(won
            ? "✔ You won."
            : $"✘ You lost. Word was: {word}");

        return new GameResult
        {
            Word = word,
            Won = won,
            LivesLeft = livesLeft,
            UsedLetters = used.OrderBy(c => c).ToList(),
            FinalState = guessed.ToArray()
        };
    }

    private bool ApplyGuess(char letter, string word, char[] guessed)
    {
        bool hit = false;

        for (int i = 0; i < word.Length; i++)
        {
            if (word[i] == letter)
            {
                guessed[i] = letter;
                hit = true;
            }
        }
        return hit;
    }

    private void DisplayStatus(char[] guessed, HashSet<char> used, int errors)
    {
        Console.WriteLine($"\nLives left: {MaxErrors - errors}");
        Console.WriteLine("Word: " + string.Join(" ", guessed));
        Console.WriteLine("Used: " + (used.Count == 0 ? "None" : string.Join(", ", used)));
        Console.Write("Input: ");
    }

    private bool AskPlayAgain()
    {
        Console.Write("\nPlay again? (Y/N): ");
        return Console.ReadLine()?.Trim().ToUpperInvariant() == "Y";
    }

    private void ShowWelcome()
    {
        Console.WriteLine("=== HANGMAN ===\n");
    }
}

public class GameResult
{
    public string Word { get; set; }
    public bool Won { get; set; }
    public int LivesLeft { get; set; }
    public List<char> UsedLetters { get; set; }
    public char[] FinalState { get; set; }
}
