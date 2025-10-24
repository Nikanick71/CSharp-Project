
    namespace BookManagement
    {
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

            public virtual string Title
            {
                get => _title;
                protected set
                {
                    if (string.IsNullOrWhiteSpace(value))
                        throw new ArgumentException("Name cant be empty.");
                    _title = value.Trim();
                }
            }

            public virtual string Author
            {
                get => _author;
                protected set
                {
                    if (string.IsNullOrWhiteSpace(value))
                        throw new ArgumentException("Author cant be empty.");
                    _author = value.Trim();
                }
            }

            public virtual int Year
            {
                get => _year;
                protected set
                {
                    int thisYear = DateTime.Now.Year;
                    if (value <= 0 || value > thisYear)
                        throw new ArgumentOutOfRangeException(nameof(Year), $"Publication year must be in the range 1..{thisYear}.");
                    _year = value;
                }
            }

            public virtual string Display() =>
                $" Name: \"{Title}\", Author: {Author}, date: {Year}";
        }

        public class SpecialEditionBook : Book
        {
            public string EditionNote { get; }

            public SpecialEditionBook(string title, string author, int year, string note)
                : base(title, author, year)
            {
                EditionNote = string.IsNullOrWhiteSpace(note) ? "Special Edition" : note.Trim();
            }

            public override string Display() =>
                $"[Special Edition] Name: \"{Title}\", Author: {Author}, Date: {Year} — {EditionNote}";
        }

        public class BookManager
        {
            private readonly List<Book> _books = new();

            public void AddBook(Book book)
            {
                if (book == null) throw new ArgumentNullException(nameof(book));
                _books.Add(book);
            }

            public IReadOnlyList<Book> GetAllBooks() => _books.AsReadOnly();

            public List<Book> FindByTitle(string titleQuery)
            {
                var result = new List<Book>();
                if (string.IsNullOrWhiteSpace(titleQuery)) return result;

                string q = titleQuery.Trim();
                foreach (var b in _books)
                {
                    if (b.Title.IndexOf(q, StringComparison.CurrentCultureIgnoreCase) >= 0)
                        result.Add(b);
                }
                return result;
            }
        }

        class Program
        {
            static void Main()
            {
                var manager = new BookManager();
                Console.WriteLine("=== Book manager (C#) ===");

                while (true)
                {
                    Console.WriteLine();
                    Console.WriteLine("Choose Option:");
                    Console.WriteLine("1. Add new book");
                    Console.WriteLine("2. View all books");
                    Console.WriteLine("3. Search book by name");
                    Console.WriteLine("4. Exit");
                    Console.Write("Option: ");
                    var input = Console.ReadLine();

                    

                    if (input == "1")
                    {
                        TryAddBook(manager);
                    }
                    else if (input == "2")
                    {
                        ListAll(manager);
                    }
                    else if (input == "3")
                    {
                        SearchByTitle(manager);
                    }
                    else if (input == "4")
                    {
                        Console.WriteLine("End of program.");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Wrong Option — Option 1-4.");
                    }
                }
            }

            static void TryAddBook(BookManager manager)
            {
                try
                {
                    Console.Write("Enter Name of the book: ");
                    var title = Console.ReadLine() ?? "";
                    if (string.IsNullOrWhiteSpace(title))
                    {
                        Console.WriteLine("Name cant be empty!");
                        return;
                    }

                    Console.Write("Enter Author: ");
                    var author = Console.ReadLine() ?? "";
                    if (string.IsNullOrWhiteSpace(author))
                    {
                        Console.WriteLine("Author cant be empty");
                        return;
                    }

                    Console.Write("Enter release date (format: dd.MM.yyyy): ");
                    var dateInput = Console.ReadLine() ?? "";
                    if (string.IsNullOrWhiteSpace(dateInput))
                    {
                        Console.WriteLine("Date cant be empty!");
                        return;
                    }

                    if (!DateTime.TryParseExact(dateInput, "dd.MM.yyyy", System.Globalization.CultureInfo.InvariantCulture,
                        System.Globalization.DateTimeStyles.None, out DateTime parsedDate))
                    {
                        Console.WriteLine("Invalid format. Please enter date as dd.MM.yyyy (e.g. 02.05.1987).");
                        return;
                    }

                    int year = parsedDate.Year;


                    
                    Book book;
                    if (title.IndexOf("special", StringComparison.CurrentCultureIgnoreCase) >= 0
                        || title.IndexOf("Spec", StringComparison.CurrentCultureIgnoreCase) >= 0)
                    {
                        book = new SpecialEditionBook(title, author, year, "Limited edition");
                    }
                    else
                    {
                        book = new Book(title, author, year);
                    }

                    manager.AddBook(book);
                    Console.WriteLine("Book added sucsessfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }

            static void ListAll(BookManager manager)
            {
                var all = manager.GetAllBooks();
                if (all.Count == 0)
                {
                    Console.WriteLine("Book list is empty.");
                    return;
                }

                Console.WriteLine($"Books {all.Count} Book:");
                int i = 1;
                foreach (var b in all)
                {
                    Console.WriteLine($"{i++}. {b.Display()}");
                }
            }

            static void SearchByTitle(BookManager manager)
            {
                Console.Write("Enter part of the title or the full title.: ");
                var q = Console.ReadLine() ?? "";
                var found = manager.FindByTitle(q);
                if (found.Count == 0)
                {
                    Console.WriteLine("No result found.");
                    return;
                }

                Console.WriteLine($"Result — {found.Count} Book:");
                int i = 1;
                foreach (var b in found)
                {
                    Console.WriteLine($"{i++}. {b.Display()}");
                }
            }
        }
    }
