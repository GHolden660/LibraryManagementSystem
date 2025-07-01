using System.Reflection.Metadata;

namespace LibraryClassLibrary


{

    public interface IStore
    {
        void MostBorrowedBooksReport();

    }

    public class Store : IStore
    {
        public List<Book> Books { get; set; }
        public List<IssuedBook> IssuedBookList { get; set; }

        public List<User> Users { get; set; }

        public Store()
        {
            Books = new List<Book>();
            IssuedBookList = new List<IssuedBook>();
            Users = new List<User>();
        }

        public IssuedBook IssueBook(User user, Book book, DateTime issueDate)
        {
            if (!book.IsAvailable)
            {
                throw new BookUnavailableException("Book already issued. Unable to issue");
            }

            book.IsAvailable = false;
            user.BooksIssued++;
            IssuedBook issuedBook = new(user, book, issueDate);
            IssuedBookList.Add(issuedBook);
            return issuedBook;
        }

        public void ReturnBook(IssuedBook issuedBook)
        {
            var bookId = issuedBook.BookId;
            Book book = Books.Where(x => x.GetId() == bookId).First();
            var userId = issuedBook.UserId;
            User user = Users.Where(x => x.GetId() == userId).First();

            if (user != null && book != null)
            {
                user.BooksIssued--;
                book.IsAvailable = true;
                issuedBook.ReturnDate = DateTime.Now;
            }
        }

        int displayPadding = 30;
        public void SearchInventory(string searchTerm, string searchArea)

        {
            List<Book> searchedBooks;
            searchTerm = searchTerm.ToLower();

            switch (searchArea)
            {
                case "Title":
                    {
                        searchedBooks = Books
                            .Where(x => x.Title.ToLower().Contains(searchTerm))
                            .OrderBy(x => x.Title)
                            .ToList();
                        break;
                    }
                case "Author":
                    {
                        searchedBooks = Books
                            .Where(x => x.Author.ToLower().Contains(searchTerm))
                            .OrderBy(x => x.Author)
                            .ToList();
                        break;
                    }
                case "Category":
                    {
                        searchedBooks = Books
                            .Where(x => x.Category.ToLower()
                            .Contains(searchTerm))
                            .OrderBy(x => x.Title)
                            .ToList();
                        break;
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
            }
            if (searchedBooks.Count > 0)
            {
                Console.WriteLine(string.Format("   {0}{1}{2}{3}", "Title".PadRight(displayPadding), "Author".PadRight(displayPadding), "Category".PadRight(displayPadding), "Availability"));
                for (int i = 0; i < searchedBooks.Count; i++)
                {
                    Console.WriteLine($"{i}: {searchedBooks[i]}");
                }
            }
            else
            {
                Console.WriteLine($"\nNo results returned from search term: '{searchTerm}'");
            }

            Console.WriteLine("");
        }

        public void ViewInventory()
        {
            Console.WriteLine(string.Format("   {0}{1}{2}{3}", "Title".PadRight(displayPadding), "Author".PadRight(displayPadding), "Category".PadRight(displayPadding), "Availability"));
            for (int i = 0; i < Books.Count; i++)
            {
                Console.WriteLine($"{i}: {Books[i]}");
            }
            Console.WriteLine("");
        }

        public void ViewUsers()
        {
            Console.WriteLine(string.Format("   {0}{1}{2}", "Name".PadRight(displayPadding), "Email".PadRight(displayPadding), "Books Issued"));
            for (int i = 0; i < Users.Count; i++)
            {
                var user = Users[i];
                Console.WriteLine($"{i}: {user.Name.PadRight(displayPadding)}{user.Email.PadRight(displayPadding)}{user.BooksIssued}");
            }
            Console.WriteLine("");
        }

        public void ViewIssuedBooks(List<IssuedBook> unreturnedBooks)
        {
            Console.WriteLine(string.Format("   {0}{1}", "Title".PadRight(displayPadding), "Issue Date".PadRight(displayPadding)));
            for (int i = 0; i < unreturnedBooks.Count; i++)
            {
                Book book = Books.First(x => x.GetId() == unreturnedBooks[i].BookId);
                Console.WriteLine($"{i}: {book.Title.PadRight(30)}{unreturnedBooks[i]}");
            }
            Console.WriteLine("");
        }

        public void MostBorrowedBooksReport()
        {
            Console.WriteLine("\tRunning Report");
            Console.WriteLine("{0}{1}{2}{3}", "Times Issued".PadRight(15), "Book Title".PadRight(displayPadding), "Author".PadRight(displayPadding), "Category");

            var grouped = IssuedBookList
                .GroupBy(x => x.BookId)
                .OrderByDescending(x => x.Count())
                .Take(15);

            foreach (var issuedBook in grouped)
            {
                var bookId = issuedBook.Key;
                Book book = Books.Where(x => x.GetId() == bookId).First();
                var timesIssued = issuedBook.Count();
                Console.WriteLine("{0}{1}{2}{3}", timesIssued.ToString().PadRight(15), book.Title.PadRight(displayPadding), book.Author.PadRight(displayPadding), book.Category.PadRight(displayPadding));

            }
            Console.WriteLine("");
        }

        public void UnreturnedBooksReport()
        {
            Console.WriteLine("\tRunning Report");
            Console.WriteLine("{0}{1}{2}{3}", "Name".PadRight(displayPadding), "Email".PadRight(displayPadding), "Book Title".PadRight(displayPadding), "Author");

            var grouped = IssuedBookList.Where(x => x.ReturnDate == null);
            foreach (var issuedBook in grouped)
            {
                Book book = Books.Where(x => x.GetId() == issuedBook.BookId).First();
                User user = Users.Where(x => x.GetId() == issuedBook.UserId).First();

                Console.WriteLine("{0}{1}{2}{3}", user.Name.PadRight(displayPadding), user.Email.PadRight(displayPadding), book.Title.PadRight(displayPadding),book.Author);

            }
            Console.WriteLine("");
        }

        public void SetUp()
        {
            Book book1 = new Book("Dragon Ball Z: Issue 1", "Fantasy", "Akira Toriyama");
            Book book2 = new Book("All Systems Red", "Science Fiction", "Martha Wells");
            Book book3 = new Book("Dragon Keeper", "Fantasy", "Carole Wilkinson");
            Book book4 = new Book("Legends and Lattes", "Fantasy", "Travis Baldree");
            Book book5 = new Book("Dragon Rider", "Action", "Cornelia Funke");

            Books.Add(book1);
            Books.Add(book2);
            Books.Add(book3);
            Books.Add(book4);
            Books.Add(book5);

            User user1 = new User("Simon Garfunkel", "simon@garfunklel.com");
            User user2 = new User("Rebecca Bridges", "rebecca@bridges.com");

            Users.Add(user1);
            Users.Add(user2);

            IssuedBook issuedBook1 = IssueBook(user1, book1, new DateTime(2025,06,16));
            ReturnBook(issuedBook1);

            IssuedBook issuedBook2 = IssueBook(user1, book1, new DateTime(2025, 06, 16));
            ReturnBook(issuedBook2);

            IssuedBook issuedBook3 = IssueBook(user1, book1, new DateTime(2025, 06, 16));
            ReturnBook(issuedBook3);

            IssuedBook issuedBook4 = IssueBook(user1, book2, new DateTime(2025, 06, 17));
            IssuedBook issuedBook5 = IssueBook(user2, book3, new DateTime(2025, 06, 18));
        }
    }
    [Serializable]
    public class BookUnavailableException : Exception
    {
        public BookUnavailableException()
        {
        }

        public BookUnavailableException(string? message) : base(message)
        {
        }

        public BookUnavailableException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}