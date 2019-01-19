namespace TddExample.Core.Books
{
    public interface IBookRepository
    {
        bool Save(Book book);
        Book GetById(string bookId);
    }
}
