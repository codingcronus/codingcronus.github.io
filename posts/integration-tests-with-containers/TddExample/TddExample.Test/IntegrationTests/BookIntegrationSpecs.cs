using Microsoft.VisualStudio.TestTools.UnitTesting;
using TddExample.Core.Books;
using TddExample.Infrastructure.Books;
using TddExample.Test.Infrastructure;

namespace TddExample.Test.IntegrationTests
{
    [TestClass]
    public class BookIntegrationSpecs
    {
        private static readonly string _connectionString = "Server=127.0.0.1;Port=3306;Database=TestDb;Uid=root;Pwd=;";

        // Restore database per Test Fixture
        [ClassInitialize]
        public static void InitializeFixture(TestContext context)
        {
            var server = new MySqlDockerServer(_connectionString);
            server.Connect().Wait(); // Async Wait
        }

        // Restore database per Test Method (slower)
        /*
        [TestInitialize]
        public void Initialize()
        {
            var server = new MySqlDockerServer(_connectionString);
            var task = server.Connect();
            task.Wait();
        }
        */

        [TestMethod]
        public void Can_load_book_from_mysql_database()
        {
            // ARRANGE
            var bookId = "409b0915-b494-4993-9211-a533fb78f70d"; // From https://www.guidgenerator.com/online-guid-generator.aspx
            var sut = new BookRepositoryMySql(_connectionString);

            // ACT
            var result = sut.GetById(bookId);

            // ASSERT
            Assert.IsNotNull(result, "Book is null");
        }

        [TestMethod]
        public void Can_load_book_with_correct_title()
        {
            // ARRANGE
            var bookId = "409b0915-b494-4993-9211-a533fb78f70d";
            var sut = new BookRepositoryMySql(_connectionString);

            // ACT
            var result = sut.GetById(bookId);

            // ASSERT
            Assert.AreEqual("Clean Code", result.Title);
        }

        [TestMethod]
        public void Can_load_another_book_from_mysql_database()
        {
            // ARRANGE
            var bookId = "95aedbbc-e385-4762-b513-5b579cd0ac64";
            var sut = new BookRepositoryMySql(_connectionString);

            // ACT
            var result = sut.GetById(bookId);

            // ASSERT
            Assert.IsNotNull(result, "Book is null");
        }

        [TestMethod]
        public void Can_load_another_book_with_correct_title()
        {
            // ARRANGE
            var bookId = "95aedbbc-e385-4762-b513-5b579cd0ac64";
            var sut = new BookRepositoryMySql(_connectionString);

            // ACT
            var result = sut.GetById(bookId);

            // ASSERT
            Assert.AreEqual("Breakfast of Champions", result.Title);
        }

        [TestMethod]
        public void Can_save_book()
        {
            // ARRANGE
            var bookId = "2c85f1a7-fd98-4ac2-986d-27d20efe062e";
            var book = new Book(bookId, "Harry Potter");
            
            var sut = new BookRepositoryMySql(_connectionString);

            // ACT
            var result = sut.Save(book);

            // ASSERT
            Assert.IsTrue(result);
        }
    }
}
