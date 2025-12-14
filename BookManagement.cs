
using System.Globalization;


public class Book
{
    private string _title;
    private string _author;
    private int _year;

    public Book(string title, string author, int year)
    {
        Title = title;
        Author = author;
        Year = year;
    }

    public string Title
    {
        get => _title;
        protected set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Title can't be empty.");
            _title = value.Trim();
        }
    }

    public string Author
    {
        get => _author;
        protected set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("Author can't be empty.");
            _author = value.Trim();
        }
    }

    public int Year
    {
        get => _year;
        protected set
        {
            int currentYear = DateTime.Now.Year;
            if (value <= 0 || value > currentYear)
                throw new ArgumentOutOfRangeException(nameof(Year));
            _year = value;
        }
    }

    public virtual string Display()
        => $"\"{Title}\" by {Author}, {Year}";

    public virtual string Serialize()
        => $"NORMAL|{Title}|{Author}|{Year}";
}

public class SpecialEditionBook : Book
{
    public string Note { get; }

    public SpecialEditionBook(string title, string author, int year, string note)
        : base(title, author, year)
    {
        Note = string.IsNullOrWhiteSpace(note) ? "Special Edition" : note.Trim();
    }

    public override string Display()
        => $"[Special] \"{Title}\" by {Author}, {Year} — {Note}";

    public override string Serialize()
        => $"SPECIAL|{Title}|{Author}|{Year}|{Note}";
}



public class BookManager
{
    private readonly List<Book> _books = new();
    private const string FileName = "books.txt";

    public IReadOnlyList<Book> GetAll() => _books.AsReadOnly();

    public void Add(Book book)
    {
        _books.Add(book);
        Save();
    }

    public List<Book> Search(string query) =>
        string.IsNullOrWhiteSpace(query)
            ? new List<Book>()
            : _books.Where(b =>
                b.Title.Contains(query, StringComparison.OrdinalIgnoreCase)).ToList();

    public void Load()
    {
        if (!File.Exists(FileName)) return;

        _books.Clear();

        foreach (var line in File.ReadAllLines(FileName))
        {
            var p = line.Split('|');

            Book book = p[0] switch
            {
                "SPECIAL" when p.Length == 5 =>
                    new SpecialEditionBook(p[1], p[2], int.Parse(p[3]), p[4]),
                "NORMAL" =>
                    new Book(p[1], p[2], int.Parse(p[3])),
                _ => null
            };

            if (book != null)
                _books.Add(book);
        }
    }

    private void Save()
    {
        File.WriteAllLines(FileName, _books.Select(b => b.Serialize()));
    }
}



class Program
{
    static void Main()
    {
        var manager = new BookManager();
        manager.Load();

        while (true)
        {
            Console.WriteLine("\n1. Add book");
            Console.WriteLine("2. View books");
            Console.WriteLine("3. Search");
            Console.WriteLine("4. Exit");
            Console.Write("Choice: ");

            switch (Console.ReadLine())
            {
                case "1": AddBook(manager); break;
                case "2": ListBooks(manager); break;
                case "3": SearchBooks(manager); break;
                case "4": return;
                default: Console.WriteLine("Invalid choice."); break;
            }
        }
    }

    static void AddBook(BookManager manager)
    {
        Console.Write("Title: ");
        string title = Console.ReadLine() ?? "";
        if (string.IsNullOrWhiteSpace(title))
        {
            Console.WriteLine("Title required.");
            return;
        }

        Console.Write("Author: ");
        string author = Console.ReadLine() ?? "";
        if (string.IsNullOrWhiteSpace(author))
        {
            Console.WriteLine("Author required.");
            return;
        }

        Console.Write("Release date (dd.MM.yyyy): ");
        if (!DateTime.TryParseExact(
            Console.ReadLine(),
            "dd.MM.yyyy",
            CultureInfo.InvariantCulture,
            DateTimeStyles.None,
            out DateTime date))
        {
            Console.WriteLine("Invalid date.");
            return;
        }

        bool special = title.Contains("special", StringComparison.OrdinalIgnoreCase);

        Book book = special
            ? new SpecialEditionBook(title, author, date.Year, "Limited")
            : new Book(title, author, date.Year);

        manager.Add(book);
        Console.WriteLine("Book added.");
    }

    static void ListBooks(BookManager manager)
    {
        var books = manager.GetAll();
        if (books.Count == 0)
        {
            Console.WriteLine("No books found.");
            return;
        }

        int i = 1;
        foreach (var b in books)
            Console.WriteLine($"{i++}. {b.Display()}");
    }

    static void SearchBooks(BookManager manager)
    {
        Console.Write("Search title: ");
        var results = manager.Search(Console.ReadLine() ?? "");

        if (results.Count == 0)
        {
            Console.WriteLine("No results.");
            return;
        }

        int i = 1;
        foreach (var b in results)
            Console.WriteLine($"{i++}. {b.Display()}");
    }
}
