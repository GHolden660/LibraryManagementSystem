using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LibraryClassLibrary
{
    public class IssuedBook


    {
        public Guid Id { get; }
        public Guid BookId { get; }

        public Guid UserId { get; }

        public DateTime IssueDate { get; init; }
        public DateTime? ReturnDate { get; set; }

        public IssuedBook(User user, Book book, DateTime issueDate)
        {
            Id = Guid.NewGuid();
            BookId = book.GetId();
            UserId = user.GetId();
            IssueDate = issueDate;
            ReturnDate = null;
        }
        public override string ToString()
        {
            return $"Issued: {IssueDate.Date}";
        }
    }
}
