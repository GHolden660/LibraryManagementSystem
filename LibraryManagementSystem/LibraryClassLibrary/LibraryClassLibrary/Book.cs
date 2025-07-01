namespace LibraryClassLibrary
{
    public class Book
    {
        public string Title { get; set; }

        public string Category { get; set; }

        public string Author { get; set; }

        Guid Id { get; init; }

        public bool IsAvailable { get; set; }

        

        public Book(string title, string category, string author)
        {
            Id = Guid.NewGuid();
            Title = title;
            Category = category;
            Author = author;
            IsAvailable = true;
        }

        public void EditBook(Book book)
        {
            Title = book.Title;
            Category = book.Category;
            Author = book.Author;
        }

        public Guid GetId()
        {
            return Id;
        }

        public void SetTitle(string title)
        {
            title.Trim();
            if (Title != title)
            {
                Title = title;
            } 
        }

        public void SetAuthor(string author)
        {
            author.Trim();
            if (Author != author)
            {
                Author = author;
            }
        }

        public void SetCategory(string category)
        {
            category.Trim();
            if (Category != category)
            {
                Category = category;
            }
        }


        public override string ToString()
        {
            var available = IsAvailable ? "Available" : "Checked Out";
            return $"{Title.PadRight(30)}{Author.PadRight(30)}{Category.PadRight(30)}{available}";
        }
    }

}
