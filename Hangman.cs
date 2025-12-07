

class Program
{
    static readonly string[] Words =
        { "PROGRAMMING", "COMPUTER", "ALGORITHM", "DATABASE", "NETWORK" };

    static void Main()
    {
        ShowWelcome();
        var rng = new Random();

        while (true)
        {
            PlayGame(Words[rng.Next(Words.Length)]);
            if (!AskPlayAgain()) return;
            Console.Clear();
        }
    }

    static void ShowWelcome()
    {
        Console.WriteLine("=======================================");
        Console.WriteLine("        WELCOME TO HANGMAN GAME        ");
        Console.WriteLine("=======================================");
        Console.WriteLine("Guess the hidden word one letter at a time,");
        Console.WriteLine("or try to guess the whole word at once!");
        Console.WriteLine("You have 6 lives. Each wrong guess brings you closer to being hanged!");
        Console.WriteLine("Good luck! May your guesses be sharp.\n");
    }

    static bool AskPlayAgain()
    {
        while (true)
        {
            Console.Write("Play again? (Y/N): ");
            string input = Console.ReadLine()?.Trim().ToUpperInvariant() ?? "";
            if (input == "Y" || input == "N") return input == "Y";
            Console.WriteLine("Error: You must enter Y or N.\n");
        }
    }

    static void PlayGame(string word)
    {
        const int maxErrors = 6;
        var guessed = Enumerable.Repeat('_', word.Length).ToArray();
        var used = new HashSet<char>();
        int errors = 0;

        while (errors < maxErrors && guessed.Contains('_'))
        {
            DisplayHangman(errors);
            PrintState(guessed, used);

            string input = GetInput().Trim().ToUpperInvariant();
            if (string.IsNullOrEmpty(input)) continue;

            // FULL WORD GUESS
            if (input.Length > 1)
            {
                if (input == word.ToUpperInvariant())
                {
                    Array.Copy(word.ToCharArray(), guessed, word.Length);
                    Console.WriteLine($"You guessed the full word! GG! The word was: {word}");
                    break;
                }
                Console.WriteLine("Incorrect full-word guess!");
                errors++;
                continue;
            }

            char letter = input[0];
            if (!char.IsLetter(letter))
            {
                Console.WriteLine("Error: You must enter a valid letter (A-Z).");
                continue;
            }

            if (!used.Add(letter))
            {
                Console.WriteLine("You already guessed that letter!");
                continue;
            }

            if (!ApplyLetterGuess(letter, word, guessed)) errors++;
        }

        DisplayHangman(errors);
        PrintState(guessed, used);
        Console.WriteLine(guessed.Contains('_') ? $"Game Over! The word was: {word}" : "Congrats! You won!");
    }

    static bool ApplyLetterGuess(char letter, string word, char[] guessed)
    {
        bool match = false;
        for (int i = 0; i < word.Length; i++)
            if (word[i] == letter) { guessed[i] = letter; match = true; }

        if (!match) Console.WriteLine("Incorrect letter!");
        return match;
    }

    static string GetInput()
    {
        Console.Write("Enter a letter or full word: ");
        return Console.ReadLine() ?? "";
    }

    static void PrintState(char[] guessed, HashSet<char> used)
    {
        Console.WriteLine("Word: " + string.Join(" ", guessed));
        Console.WriteLine("Used letters: " + (used.Count > 0 ? string.Join(", ", used) : "None"));
        Console.WriteLine();
    }

    static void DisplayHangman(int errors)
    {
        string[] hangman = new string[7];
        hangman[0] = "  ____  \n |    | ";
        hangman[1] = errors >= 1 ? " O    |" : "      |";
        hangman[2] = errors >= 2 ? (errors >= 3 ? "/|\\" : "/|") : (errors == 2 ? " |    |" : "      |");
        hangman[3] = errors >= 5 ? "/" : " ";
        hangman[4] = errors == 6 ? "\\" : " ";
        hangman[5] = "      |";
        hangman[6] = errors == 6 ? "No lives left." : $"Lives left: {6 - errors}";

        Console.WriteLine("\nHangman:");
        foreach (var line in hangman) Console.WriteLine(line);
    }
}
