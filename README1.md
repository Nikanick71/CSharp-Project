

class Program
{
    static void Main(string[] args)
    {

        string[] words = { "PROGRAMMING", "COMPUTER", "ALGORYTHM", "DATABASE", "NETWORK" };
        Random random = new Random();
        string wordToGuess = words[random.Next(words.Length)];
        char[] guessedWord = new char[wordToGuess.Length];
        List<char> guessedLetters = new List<char>();
        int incorrectGuesses = 0;
        int maxIncorrectGuesses = 6;


        for (int i = 0; i < wordToGuess.Length; i++)
        {
            guessedWord[i] = '_';
        }

        Console.WriteLine("Welcome to Hangman!");

        while (incorrectGuesses < maxIncorrectGuesses && new string(guessedWord) != wordToGuess)
        {

            DisplayHangman(incorrectGuesses);


            Console.WriteLine("\nWord: " + string.Join(" ", guessedWord));
            Console.WriteLine("Guessed letters: " + (guessedLetters.Count > 0 ? string.Join(", ", guessedLetters) : "None"));
            Console.Write("Enter a letter: ");


            string input = Console.ReadLine()?.ToUpper() ?? "";
            if (string.IsNullOrEmpty(input) || input.Length != 1 || !char.IsLetter(input[0]))
            {
                Console.WriteLine("Please enter a single letter!");
                continue;
            }

            char guess = input[0];


            if (guessedLetters.Contains(guess))
            {
                Console.WriteLine("You already guessed that letter!");
                continue;
            }

            guessedLetters.Add(guess);


            bool correctGuess = false;
            for (int i = 0; i < wordToGuess.Length; i++)
            {
                if (wordToGuess[i] == guess)
                {
                    guessedWord[i] = guess;
                    correctGuess = true;
                }
            }

            if (!correctGuess)
            {
                incorrectGuesses++;
                Console.WriteLine("Incorrect guess!");
            }
        }


        DisplayHangman(incorrectGuesses);
        Console.WriteLine("\nWord: " + string.Join(" ", guessedWord));
        if (new string(guessedWord) == wordToGuess)
        {
            Console.WriteLine("Congratulations! You won!");
        }
        else
        {
            Console.WriteLine($"Game Over! The word was {wordToGuess}.");
        }
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
            case 6:
                Console.WriteLine("  ____  ");
                Console.WriteLine(" |    | ");
                Console.WriteLine(" O    | ");
                Console.WriteLine("/|\\   | ");
                Console.WriteLine("/ \\   | ");
                Console.WriteLine("      | ");

                break;
        }
    }
}
