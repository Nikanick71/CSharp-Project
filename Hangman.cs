class Program
{
    static void Main()
    {
        string[] words = { "PROGRAMMING", "COMPUTER", "ALGORITHM", "DATABASE", "NETWORK" };
        var rng = new Random();

        while (true)
        {
            string wordToGuess = words[rng.Next(words.Length)];
            PlayGame(wordToGuess);

            Console.Write("Play again? (Y/N): ");
            var again = Console.ReadLine()?.Trim().ToUpperInvariant() ?? "N";
            if (again != "Y") break;
            Console.WriteLine();
        }
    }

    static void PlayGame(string wordToGuess)
    {
        const int maxIncorrect = 6;
        int incorrectGuesses = 0;
        char[] guessedWord = Enumerable.Repeat('_', wordToGuess.Length).ToArray();
        var guessedLetters = new HashSet<char>();

        Console.WriteLine("Welcome to Hangman!");

        while (incorrectGuesses < maxIncorrect && guessedWord.Contains('_'))
        {
            DisplayHangman(incorrectGuesses);
            Console.WriteLine("\nWord: " + string.Join(" ", guessedWord));
            Console.WriteLine("Guessed letters: " + (guessedLetters.Count > 0 ? string.Join(", ", guessedLetters) : "None"));
            Console.Write("Enter a letter (or try whole word): ");

            string input = Console.ReadLine()?.Trim().ToUpperInvariant() ?? "";
            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("Please enter something.");
                continue;
            }

            // Allow whole-word guess
            if (input.Length > 1)
            {
                if (input == wordToGuess)
                {
                    Console.WriteLine("You guessed the whole word! Congratulations!");
                    Console.WriteLine($"The word was: {wordToGuess}");
                    return;
                }
                else
                {
                    incorrectGuesses++;
                    Console.WriteLine("Wrong whole-word guess!");
                    continue;
                }
            }

            char guess = input[0];
            if (!char.IsLetter(guess))
            {
                Console.WriteLine("Please enter a letter (A-Z).");
                continue;
            }

            if (!guessedLetters.Add(guess))
            {
                Console.WriteLine("You already guessed that letter!");
                continue;
            }

            bool correct = false;
            for (int i = 0; i < wordToGuess.Length; i++)
            {
                if (wordToGuess[i] == guess)
                {
                    guessedWord[i] = guess;
                    correct = true;
                }
            }

            if (!correct)
            {
                incorrectGuesses++;
                Console.WriteLine("Incorrect guess!");
            }
        }

        DisplayHangman(incorrectGuesses);
        Console.WriteLine("\nWord: " + string.Join(" ", guessedWord));

        if (!guessedWord.Contains('_'))
            Console.WriteLine("Congratulations! You won!");
        else
            Console.WriteLine($"Game Over :( The word was {wordToGuess}.");
    }

    static void DisplayHangman(int incorrectGuesses)
    {
        Console.WriteLine("\nHangman:");
        switch (incorrectGuesses)
        {
            case 0:
                Console.WriteLine("  ____  ");
                Console.WriteLine(" |    | ");
                Console.WriteLine("      | ");
                Console.WriteLine("      | ");
                Console.WriteLine("      | ");
                Console.WriteLine("      | ");
                Console.WriteLine("Lives left: 6");
                break;
            case 1:
                Console.WriteLine("  ____  ");
                Console.WriteLine(" |    | ");
                Console.WriteLine(" O    | ");
                Console.WriteLine("      | ");
                Console.WriteLine("      | ");
                Console.WriteLine("      | ");
                Console.WriteLine("Lives left: 5");
                break;
            case 2:
                Console.WriteLine("  ____  ");
                Console.WriteLine(" |    | ");
                Console.WriteLine(" O    | ");
                Console.WriteLine(" |    | ");
                Console.WriteLine("      | ");
                Console.WriteLine("      | ");
                Console.WriteLine("Lives left: 4");
                break;
            case 3:
                Console.WriteLine("  ____  ");
                Console.WriteLine(" |    | ");
                Console.WriteLine(" O    | ");
                Console.WriteLine("/|    | ");
                Console.WriteLine("      | ");
                Console.WriteLine("      | ");
                Console.WriteLine("Lives left: 3");
                break;
            case 4:
                Console.WriteLine("  ____  ");
                Console.WriteLine(" |    | ");
                Console.WriteLine(" O    | ");
                Console.WriteLine("/|\\   | ");
                Console.WriteLine("      | ");
                Console.WriteLine("      | ");
                Console.WriteLine("Lives left: 2");
                break;
            case 5:
                Console.WriteLine("  ____  ");
                Console.WriteLine(" |    | ");
                Console.WriteLine(" O    | ");
                Console.WriteLine("/|\\   | ");
                Console.WriteLine("/     | ");
                Console.WriteLine("      | ");
                Console.WriteLine("Lives left: 1");
                break;
            default:
            case 6:
                Console.WriteLine("  ____  ");
                Console.WriteLine(" |    | ");
                Console.WriteLine(" O    | ");
                Console.WriteLine("/|\\   | ");
                Console.WriteLine("/ \\   | ");
                Console.WriteLine("      | ");
                Console.WriteLine("No lives left.");
                break;
        }
    }
}
