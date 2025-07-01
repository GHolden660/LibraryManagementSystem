using LibraryClassLibrary;

Store store = new Store();
store.SetUp();

Console.WriteLine("Welcome to the library ");


(string, int) openingMenuMessage = ("Press {1} to manage inventory,\nPress {2} to borrow or return books\nPress {3} to search inventory\nPress {4} to generate reports", 4);
int choice1 = ChooseAction(openingMenuMessage);

while (choice1 != 0)
{
    switch (choice1)
    {
        case -1:
            {
                break;
            }
        case 1:
            {
                Console.WriteLine("\tWelcome to Book Inventory Management");
                (string, int) inventoryMessage = ("Press {1} View inventory\nPress {2} Add new book to inventory\nPress {3} Edit a book\nPress {4} Delete a book\nPress {5} Return to the previous menu", 5);
                int inventoryChoice = ChooseAction(inventoryMessage);
                while (inventoryChoice != 0)
                {
                    switch (inventoryChoice)
                    {
                        case -1:
                            {
                                inventoryChoice = ChooseAction(inventoryMessage);
                                break;
                            }
                        case 1:
                            {
                                Console.WriteLine("\tYou selected to view the book inventory");

                                store.ViewInventory();
                                inventoryChoice = -1;
                                break;
                            }
                        case 2:
                            {
                                string bookTitle = "";
                                string bookAuthor = "";
                                string bookCategory = "";

                                Console.WriteLine("You chose to add a new book to the inventory \n");

                                bookTitle = ReadStringValue("What is the book title?");
                                bookAuthor = ReadStringValue("What is the author's name?");
                                bookCategory = ReadStringValue("What is the category");
                                Book newBook = new Book(bookTitle, bookCategory, bookAuthor);
                                store.Books.Add(newBook);

                                store.ViewInventory();
                                inventoryChoice = -1;
                                break;
                            }
                        case 3:
                            {
                                Console.WriteLine("\tSelect book to edit");
                                store.ViewInventory();
                                (string message, int maxChoice) editMessage = ("Choose index of the book you wish to edit", store.Books.Count);
                                int editChoice = ChooseIndex(editMessage);
                                while (editChoice != -2)
                                {   
                                    switch (editChoice)
                                    {
                                        case -1:
                                            {
                                                editChoice = ChooseIndex(editMessage);
                                                break;
                                            }
                                        default:
                                            {
                                                Book book = store.Books[editChoice];
                                                Console.WriteLine("You have selected book: "+ book.Title);
                                                Console.WriteLine("Update title: ");
                                                book.SetTitle(ReadLine(book.Title));
                                                Console.WriteLine("Update author: ");
                                                book.SetAuthor(ReadLine(book.Author));
                                                Console.WriteLine("Update category: ");
                                                book.SetCategory(ReadLine(book.Category));
                                                Console.WriteLine("Updated\n " + book);

                                                editChoice = -2;
                                                inventoryChoice = -1;
                                                break;
                                            }
                                    }
                               
                                }
                                break;
                            }
                        case 4:
                            {
                                int editChoice;
                                (string message, int maxChoice) editMessage = ("Choose index of the book you wish to delete", store.Books.Count);

                                if (store.Books.Count > 0)
                                {
                                    Console.WriteLine("\tSelect a book to delete");
                                    store.ViewInventory();
                                    
                                    editChoice = ChooseIndex(editMessage);
                                } else
                                {
                                    Console.WriteLine("\tNo books in inventory. Try adding some books first");
                                    editChoice = -2;
                                    inventoryChoice = -1;
                                }
                                    while (editChoice != -2)
                                    {
                                        switch (editChoice)
                                        {
                                            case -1:
                                            {
                                                if (store.Books.Count > 0)
                                                {
                                                    editChoice = ChooseIndex(editMessage);
                                                }
                                                else
                                                {
                                                    Console.WriteLine("No books in inventory.");
                                                    editChoice = -2;
                                                    inventoryChoice = -1;
                                                }
                                                break;
                                                }
                                            default:
                                                {
                                                    Book book = store.Books[editChoice];
                                                    Console.WriteLine("Deleting book: " + book);
                                                    store.Books.RemoveAt(editChoice);

                                                    editChoice = -2;
                                                    inventoryChoice = -1;
                                                    break;
                                                }
                                        }

                                    }
                                break;
                            }
                        case 5:
                            {
                                Console.WriteLine("\tReturning to previous menu");
                                inventoryChoice = 0;
                                break;
                            }
                            
                    }
                }
                break;
            }
        case 2:
            {
                Console.WriteLine("\tYou selected book checkout");
                (string, int) checkoutMessage = ("Press {1} to checkout a book\nPress {2} to return a book\nPress {3} to return to the previous menu", 3);
                int checkoutChoice = ChooseAction(checkoutMessage);

                while (checkoutChoice != 0)
                {
                    switch (checkoutChoice)
                    {
                        case -1:
                            {
                                checkoutChoice = ChooseAction(checkoutMessage);
                                break;
                            }
                        case 1:
                            {
                                Console.WriteLine("\t Please select a user and book to checkout");
                                store.ViewUsers();
                                (string, int) issueUserMessage = ("Select index of user", store.Users.Count);
                                int userIndex = -1;
                                int chooseUser = -1;
                                // loop until valid userIndex selected
                                while (chooseUser != 0)
                                {  
                                    userIndex = ChooseIndex(issueUserMessage);
                                    if (userIndex < 0 || userIndex >= store.Users.Count)
                                    {
                                        chooseUser = -1;
                                    }
                                    else
                                    { chooseUser = 0; }       
                                }
                                Console.WriteLine("");
                                store.ViewInventory();
                                int bookIndex = -1;
                                int chooseBook = -1;
                                // loop until valid bookIndex selected
                                while (chooseBook != 0)
                                {
                                    bookIndex = ChooseIndex(("select available book to issue", store.Books.Count));
                                    if (bookIndex < 0 || bookIndex >= store.Books.Count)
                                    {
                                        chooseBook = -1;
                                    }
                                    else
                                    { chooseBook = 0; }
                                }

                                User user = store.Users[userIndex];
                                Book book = store.Books[bookIndex];

                                try
                                { 
                                    store.IssueBook(user, book, DateTime.Now);
                                    Console.WriteLine($"\t{book.Title} issued to {user.Name}");
                                    Console.WriteLine();
                                }
                                catch (BookUnavailableException ex)
                                {
                                    Console.WriteLine("{0}. Book:{1}", ex.Message, book);
                                }
                               
                                checkoutChoice = -1;
                                break;
                            }
                        case 2:
                            {
                                List<IssuedBook> unreturnedBooks = store.IssuedBookList.Where(x => x.ReturnDate == null).ToList();

                                if (unreturnedBooks.Count == 0)
                                {
                                    Console.WriteLine("\tNo books are currently issued");
                                }
                                else
                                {
                                    Console.WriteLine("\tYou selected to return a book. List of issued books:");
                                    store.ViewIssuedBooks(unreturnedBooks);

                                    int unreturnedIndex = -1;
                                    int chooseUnreturnedBook = -1;
                                    // loop until valid unreturnedIndex selected
                                    while (chooseUnreturnedBook != 0)
                                    {
                                        unreturnedIndex = ChooseIndex(("Select the index of book to return", unreturnedBooks.Count)); ;
                                        if (unreturnedIndex < 0 || unreturnedIndex >= store.Books.Count)
                                        {
                                            chooseUnreturnedBook = -1;
                                        }
                                        else
                                        { chooseUnreturnedBook = 0; }
                                    }

                                    store.ReturnBook(unreturnedBooks[unreturnedIndex]);

                                    Console.WriteLine("\tBook Returned Successfully");
                                    Console.WriteLine();

                                }
                                checkoutChoice = -1;
                                break;
                            }
                        case 3:
                            {
                                checkoutChoice = 0;
                                break;    
                            }

                    }
       
                }
                break;
            }
        case 3:
            {
                Console.WriteLine("\tYou selected search");
                (string, int) reportMessage = ("Press {1} to search by book title\nPress {2} to search by author name\nPress {3} to search by category\nPress {4} to return to previous menu", 4);
                int searchChoice = ChooseAction(reportMessage);
                while (searchChoice != 0)
                {
                    switch (searchChoice)
                    {
                        case -1:
                            {
                                searchChoice = ChooseAction(reportMessage);
                                break;
                            }
                        case 1:
                            {
                                Console.WriteLine("\tYou selected to search by book title");
                                string searchTerm = ReadStringValue("Enter search term");
                                store.SearchInventory(searchTerm, "Title");
                                searchChoice = -1;

                                break;
                            }
                        case 2:
                            {
                                Console.WriteLine("\tYou selected to search by book author");
                                string searchTerm = ReadStringValue("Enter search term");
                                store.SearchInventory(searchTerm, "Author");
                                searchChoice = -1;

                                break;
                            }
                        case 3:
                            {
                                Console.WriteLine("\tYou selected to search by book author");
                                string searchTerm = ReadStringValue("Enter search term");
                                store.SearchInventory(searchTerm, "Category");
                                searchChoice = -1;

                                break;
                            }
                        case 4:
                            {
                                Console.WriteLine("\tReturning to previous menu");
                                searchChoice = 0;
                                break;
                            }

                    }
                }
                break;
            }
        case 4:
        {
            Console.WriteLine("\t You selected book reporting");
            (string, int) reportMessage = ("Press {1} to generate a top 15 book report\nPress {2} to generate a report showing users with unreturned books\nPress {3} to return to the previous menu", 3);
            int reportChoice = ChooseAction(reportMessage);
            while (reportChoice != 0)
            {
                switch (reportChoice)
                {
                    case -1:
                        {
                            reportChoice = ChooseAction(reportMessage);
                            break;
                        }
                    case 1:
                        {
                            Console.WriteLine("\tYou selected the top 15 book report");
                            store.MostBorrowedBooksReport();
                            reportChoice = -1;

                            break;
                        }
                    case 2:
                        {
                            Console.WriteLine("\tGenerating users with unreturned books");
                            store.UnreturnedBooksReport();
                            reportChoice = -1;

                            break;
                        }
                    case 3:   
                        {
                            Console.WriteLine("\tReturning to previous menu");
                            reportChoice = 0;
                            break;
                        }
                            
                    }
            }
            break;
        }
    }
    choice1 = ChooseAction(openingMenuMessage);
}

    string ReadStringValue(string message)
{
    string value = "";
    while (value == null || value == "")
    {
        Console.WriteLine(message);
        try
        {
            value = Console.ReadLine();
            if (value == null || value == "")
            {
                throw new FormatException();
            }
        }
        catch (FormatException)
        { Console.WriteLine("Value cannot be empty. Please provide a value"); }    
    }
    return value;
}

int ChooseAction((string message, int maxOptions) selectionMessage)
{
    Console.WriteLine(selectionMessage.message);

    try
    {
        int choice = int.Parse(Console.ReadLine());
        if (choice >=1 && choice <= selectionMessage.maxOptions)
        {
            return choice;
        }
        else
        {
            throw new ArgumentOutOfRangeException();
        }
    }
    catch (FormatException ex)
    {
        Console.WriteLine(ex.Message+ " Please enter a number");
        return -1;
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine(ex.Message + " Please enter a valid option");
        return -1;
    }
}

int ChooseIndex((string message, int maxOptions) selectionMessage)
{
    Console.WriteLine(selectionMessage.message);

    try
    {
        int choice = int.Parse(Console.ReadLine());
        if (choice >= 0 && choice < selectionMessage.maxOptions)
        {
            return choice;
        }
        else
        {
            throw new ArgumentOutOfRangeException();
        }
    }
    catch (FormatException ex)
    {
        Console.WriteLine(ex.Message + " Please enter a number");
        return -1;
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine(ex.Message + " Please enter a valid option");
        return -1;
    }
}


// ReadLine from https://stackoverflow.com/questions/8962691/console-readlinedefault-text-editable-text-on-line 
static string ReadLine(string preText)
{
    Console.Write(preText);
    List<char> chars = string.IsNullOrEmpty(preText) ? [] : [.. preText];
    while (true)
    {
        var input = Console.ReadKey(true);
        if (!char.IsControl(input.KeyChar))
        {
            chars.Insert(Console.CursorLeft, input.KeyChar);
            PrintRight();
            Console.CursorLeft += 1;
        }

        switch (input.Key)
        {
            case ConsoleKey.RightArrow when chars.Count > Console.CursorLeft:
                Console.CursorLeft += 1;
                break;

            case ConsoleKey.LeftArrow when Console.CursorLeft > 0:
                Console.CursorLeft -= 1;
                break;

            case ConsoleKey.Backspace when Console.CursorLeft > 0:
                Console.CursorLeft -= 1;
                chars.RemoveAt(Console.CursorLeft);
                PrintRight();
                break;

            case ConsoleKey.Delete when chars.Count > Console.CursorLeft:
                var pos = Console.CursorLeft;
                var deleted = chars.ElementAtOrDefault(pos);
                if (deleted == default)
                    continue;

                chars.RemoveAt(pos);
                PrintRight();
                break;

            case ConsoleKey.Enter:
                Console.WriteLine();
                return new string(chars.ToArray()).Trim();
        }
    }

    void PrintRight()
    {
        var pos = Console.CursorLeft;
        Console.Write([.. chars[pos..], ' ']);
        Console.CursorLeft = pos;
    }
}