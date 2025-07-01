
namespace LibraryClassLibrary
{
    public class User
    {
        public string Name { get; set; }

        public string Email { get; set; }

        Guid Id { get; init; }

        public int BooksIssued { get; set; } 

        public User(string name, string email)
        {
            Name = name;
            Email = email;
            Id = Guid.NewGuid();
            BooksIssued = 0;
        }
        public Guid GetId()
        {
            return Id;
        }
        public override string ToString()
        {
            return $"Name: {Name}, Email: {Email}, Books Issued: {BooksIssued}";
        }

        public string DisplayDetails()
        {
            return Name.PadRight(30) + Email.PadRight(30) + BooksIssued;
        }

    }
}
