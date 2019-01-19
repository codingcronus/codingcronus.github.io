using Dapper;
using MySql.Data.MySqlClient;
using System.Data;
using System.Linq;
using TddExample.Core.Books;

namespace TddExample.Infrastructure.Books
{
    public class BookRepositoryMySql : IBookRepository
    {
        private readonly string connectionString;

        public BookRepositoryMySql(string connectionString)
        {
            this.connectionString = connectionString ?? throw new System.ArgumentNullException(nameof(connectionString));
        }

        public Book GetById(string bookId)
        { 
            var query = $"SELECT * FROM Books WHERE Id = @Id";

            using (IDbConnection db = new MySqlConnection(connectionString))
            {
                return db.Query<Book>(query, new { @Id = bookId }).SingleOrDefault();
            }
        }

        public bool Save(Book book)
        {
            var query = $"INSERT INTO Books (Id, Title, Author, Isbn, NumPages) VALUES (@Id, @Title, @Author, @Isbn, @NumPages)";

            //using (IDbConnection db = new MySqlConnection(connectionString))
            IDbConnection db = new MySqlConnection(connectionString);
            {
                db.Open();

                var affectedRows = db.Execute(query, book);

                return affectedRows > 0;
            }
        }
    }
}
