using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using DbManager.Security;
using DbManager;

namespace SecurityParsingTests
{
    public class DeleteUserTests
    {
        [Fact]
        public void Correct()
        {
            DeleteUser query = MiniSQLParser.Parse("DELETE USER user") as DeleteUser;
            Assert.Equal("user", query.Username);

            query = MiniSQLParser.Parse("DELETE USER OtherUser") as DeleteUser;
            Assert.Equal("OtherUser", query.Username);
        }

        [Fact]
        public void CorrectWithSpaces()
        {
            DeleteUser query = MiniSQLParser.Parse("DELETE     USER      USER") as DeleteUser;
            Assert.Equal("USER", query.Username);

            query = MiniSQLParser.Parse("DELETE USER    OtherUser") as DeleteUser;
            Assert.Equal("OtherUser", query.Username);
        }

        [Fact]
        public void IncorrectCapitalization()
        {
            DeleteUser query = MiniSQLParser.Parse("Delete User User") as DeleteUser;
            Assert.Null(query);

            query = MiniSQLParser.Parse("delete user User") as DeleteUser;
            Assert.Null(query);

            query = MiniSQLParser.Parse("DELETE USER User") as DeleteUser;
            Assert.NotNull(query);
        }

        [Fact]
        public void IncorrectUserWithForbiddenChars()
        {
            DeleteUser query = MiniSQLParser.Parse("DELETE USER User_1") as DeleteUser;
            Assert.Null(query);

            query = MiniSQLParser.Parse("DELETE USER User 1") as DeleteUser;
            Assert.Null(query);

            query = MiniSQLParser.Parse("DELETE USER User") as DeleteUser;
            Assert.NotNull(query);
        }

        [Fact]
        public void IncorrectWithoutProfile()
        {
            DeleteUser query = MiniSQLParser.Parse("DELETE USER") as DeleteUser;
            Assert.Null(query);

            query = MiniSQLParser.Parse("DELETE USER ") as DeleteUser;
            Assert.Null(query);

            query = MiniSQLParser.Parse("DELETE USER User") as DeleteUser;
            Assert.NotNull(query);
        }

        [Fact]
        public void IncorrectStartsWithNumber()
        {
            //No puede empezar por un número
            DeleteUser query = MiniSQLParser.Parse("DELETE USER 1User") as DeleteUser;
            Assert.Null(query);

            query = MiniSQLParser.Parse("DELETE USER 123") as DeleteUser;
            Assert.Null(query);

            query = MiniSQLParser.Parse("DELETE USER User1") as DeleteUser;
            Assert.NotNull(query);
        }

        [Fact]
        public void IncorrectWrongKeyword()
        {
            //Keyword incorrecta
            DeleteUser query = MiniSQLParser.Parse("REMOVE USER User") as DeleteUser;
            Assert.Null(query);

            query = MiniSQLParser.Parse("DROP USER User") as DeleteUser;
            Assert.Null(query);

            query = MiniSQLParser.Parse("DELETE USER User") as DeleteUser;
            Assert.NotNull(query);
        }

        [Fact]
        public void IncorrectMissingUSERKeyword()
        {
            DeleteUser query = MiniSQLParser.Parse("DELETE User") as DeleteUser;
            Assert.Null(query);

            query = MiniSQLParser.Parse("DELETE User User") as DeleteUser;
            Assert.Null(query);
        }

        [Fact]
        public void CorrectAlphanumeric()
        {
            //Primero letras luego numeros
            DeleteUser query = MiniSQLParser.Parse("DELETE USER User123") as DeleteUser;
            Assert.Equal("User123", query.Username);

            query = MiniSQLParser.Parse("DELETE USER x1x2x3") as DeleteUser;
            Assert.Equal("x1x2x3", query.Username);
        }

        [Fact]
        public void CorrectSingleChar()
        {
            DeleteUser query = MiniSQLParser.Parse("DELETE USER a") as DeleteUser;
            Assert.Equal("a", query.Username);

            query = MiniSQLParser.Parse("DELETE USER Z") as DeleteUser;
            Assert.Equal("Z", query.Username);
        }

        [Fact]
        public void IncorrectSpecialChars()
        {
            DeleteUser query = MiniSQLParser.Parse("DELETE USER User@Domain") as DeleteUser;
            Assert.Null(query);

            query = MiniSQLParser.Parse("DELETE USER User#1") as DeleteUser;
            Assert.Null(query);

            query = MiniSQLParser.Parse("DELETE USER User.Name") as DeleteUser;
            Assert.Null(query);

            query = MiniSQLParser.Parse("DELETE USER User-Name") as DeleteUser;
            Assert.Null(query);
        }

        [Fact]
        public void IncorrectExtraText()
        {
            //Final de cadena incorrecto
            DeleteUser query = MiniSQLParser.Parse("DELETE USER User x") as DeleteUser;
            Assert.Null(query);

            query = MiniSQLParser.Parse("DELETE USER User WHERE x2") as DeleteUser;
            Assert.Null(query);
        }
    }
}