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
    public class RevokeTests
    {
        [Fact]
        public void Correct()
        {
            Revoke query = MiniSQLParser.Parse("REVOKE DELETE ON Table TO User") as Revoke;
            Assert.Equal("DELETE", query.PrivilegeName);
            Assert.Equal("Table", query.TableName);
            Assert.Equal("User", query.ProfileName);

            query = MiniSQLParser.Parse("REVOKE INSERT ON Table TO User") as Revoke;
            Assert.Equal("INSERT", query.PrivilegeName);
            Assert.Equal("Table", query.TableName);
            Assert.Equal("User", query.ProfileName);

            query = MiniSQLParser.Parse("REVOKE SELECT ON Table TO User") as Revoke;
            Assert.Equal("SELECT", query.PrivilegeName);
            Assert.Equal("Table", query.TableName);
            Assert.Equal("User", query.ProfileName);

            query = MiniSQLParser.Parse("REVOKE UPDATE ON Table TO User") as Revoke;
            Assert.Equal("UPDATE", query.PrivilegeName);
            Assert.Equal("Table", query.TableName);
            Assert.Equal("User", query.ProfileName);
        }

        [Fact]
        public void CorrectWithSpaces()
        {
            Revoke query = MiniSQLParser.Parse("REVOKE DELETE    ON Table TO User") as Revoke;
            Assert.Equal("DELETE", query.PrivilegeName);
            Assert.Equal("Table", query.TableName);
            Assert.Equal("User", query.ProfileName);

            query = MiniSQLParser.Parse("REVOKE INSERT ON Table    TO User") as Revoke;
            Assert.Equal("INSERT", query.PrivilegeName);
            Assert.Equal("Table", query.TableName);
            Assert.Equal("User", query.ProfileName);

            query = MiniSQLParser.Parse("REVOKE SELECT ON Table TO     User") as Revoke;
            Assert.Equal("SELECT", query.PrivilegeName);
            Assert.Equal("Table", query.TableName);
            Assert.Equal("User", query.ProfileName);

            query = MiniSQLParser.Parse("REVOKE    UPDATE     ON    Table    TO     User") as Revoke;
            Assert.Equal("UPDATE", query.PrivilegeName);
            Assert.Equal("Table", query.TableName);
            Assert.Equal("User", query.ProfileName);
        }

        [Fact]
        public void IncorrectProfileWithForbiddenChars()
        {
            Revoke query = MiniSQLParser.Parse("REVOKE DELETE ON Table TO User 1") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE INSERT ON Table TO Us er") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE SELECT ON Table TO User-1") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE UPDATE ON Table To User_2") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE UPDATE ON Table TO User") as Revoke;
            Assert.NotNull(query);
        }

        [Fact]
        public void IncorrectCapitalization()
        {
            Revoke query = MiniSQLParser.Parse("Revoke DELETE ON Table TO User") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE Insert ON Table TO User") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE SELECT on Table TO User") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE UPDATE ON Table To User") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE UPDATE ON Table TO User") as Revoke;
            Assert.NotNull(query);
        }

        [Fact]
        public void IncorrectPrivileges()
        {
            Revoke query = MiniSQLParser.Parse("REVOKE Remove ON Table TO User") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE REMOVE ON Table TO User") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE UPGRADE ON Table TO User") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE SET ON Table TO User") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE UPDATE ON Table TO User") as Revoke;
            Assert.NotNull(query);
        }

        [Fact]
        public void IncorrectWithoutOnePart()
        {
            Revoke query = MiniSQLParser.Parse("REVOKE ON Table TO User") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE SELECT ON TO User") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE SELECT TO User") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE SELECT ON Table TO") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE UPDATE ON Table TO User") as Revoke;
            Assert.NotNull(query);
        }

        //Test adicionales

        [Fact]
        public void InvalidTableStartsWithNumber()
        {
            //Tabla no puede empezar por un numero
            Revoke query = MiniSQLParser.Parse("REVOKE SELECT ON 1Table TO User") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE SELECT ON 123 TO User") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE SELECT ON Table1 TO User") as Revoke;
            Assert.NotNull(query); //si acabar
        }

        [Fact]
        public void InvalidUserStartsWithNumber()
        {
            //Usuario no puede empezar por numero 
            Revoke query = MiniSQLParser.Parse("REVOKE SELECT ON Table TO 1User") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE SELECT ON Table TO 99") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE SELECT ON Table TO User99") as Revoke;
            Assert.NotNull(query); //si acabar
        }

        [Fact]
        public void InvalidTableChars()
        {
            //Tabla con caracteres correctos
            Revoke query = MiniSQLParser.Parse("REVOKE SELECT ON Table-1 TO User") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE SELECT ON Table_Name TO User") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE SELECT ON Table.Name TO User") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE SELECT ON Table TO User") as Revoke;
            Assert.NotNull(query);
        }

        [Fact]
        public void InvalidExtraTokens()
        {
            //Debe terminar por User
            Revoke query = MiniSQLParser.Parse("REVOKE SELECT ON Table TO User ExtraToken") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE SELECT ON Table TO User;") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE SELECT ON Table TO User") as Revoke;
            Assert.NotNull(query);
        }

        [Fact]
        public void InvalidKeywords()
        {
            //Palabras clave inválidas
            Revoke query = MiniSQLParser.Parse("GRANT SELECT ON Table TO User") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE SELECT IN Table TO User") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE SELECT ON Table FROM User") as Revoke;
            Assert.Null(query);

            query = MiniSQLParser.Parse("REVOKE SELECT ON Table TO User") as Revoke;
            Assert.NotNull(query);
        }

        [Fact]
        public void ValidAlphanumericNames()
        {
            //Tabla y usuario correctos
            Revoke query = MiniSQLParser.Parse("REVOKE SELECT ON Table1 TO User1") as Revoke;
            Assert.Equal("SELECT", query.PrivilegeName);
            Assert.Equal("Table1", query.TableName);
            Assert.Equal("User1", query.ProfileName);
        }

        [Fact]
        public void ValidPrivilegesParsed()
        {
            //Privilegios válidos
            Revoke query = MiniSQLParser.Parse("REVOKE DELETE ON T TO U") as Revoke;
            Assert.Equal("DELETE", query.PrivilegeName);

            query = MiniSQLParser.Parse("REVOKE INSERT ON T TO U") as Revoke;
            Assert.Equal("INSERT", query.PrivilegeName);

            query = MiniSQLParser.Parse("REVOKE SELECT ON T TO U") as Revoke;
            Assert.Equal("SELECT", query.PrivilegeName);

            query = MiniSQLParser.Parse("REVOKE UPDATE ON T TO U") as Revoke;
            Assert.Equal("UPDATE", query.PrivilegeName);
        }
    }
}
