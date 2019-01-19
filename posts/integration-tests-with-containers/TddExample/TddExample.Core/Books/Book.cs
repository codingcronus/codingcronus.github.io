using System;

namespace TddExample.Core.Books
{
    public class Book : Aggregate
    {
        public string Title { get; private set; }
        public string Author { get; private set; }
        public string Isbn { get; private set; }
        public int NumPages { get; private set; }

        private Book() : base("")
        {
        }

        public Book(string id, string title) : base(id)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentNullException("Title must have a value", nameof(title));

            Title = title;
        }
    }
}
