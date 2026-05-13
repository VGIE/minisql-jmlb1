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
    public class GrantTests
    {
        [Fact]
        public void Correct()
        {
            Grant query = MiniSQLParser.Parse("GRANT DELETE ON Table TO Admin") as Grant;
            Assert.Equal("DELETE", query.PrivilegeName);
            Assert.Equal("Table", query.TableName);
            Assert.Equal("Admin", query.ProfileName);

            query = MiniSQLParser.Parse("GRANT INSERT ON Table TO Admin") as Grant;
            Assert.Equal("INSERT", query.PrivilegeName);
            Assert.Equal("Table", query.TableName);
            Assert.Equal("Admin", query.ProfileName);

            query = MiniSQLParser.Parse("GRANT SELECT ON Table TO Admin") as Grant;
            Assert.Equal("SELECT", query.PrivilegeName);
            Assert.Equal("Table", query.TableName);
            Assert.Equal("Admin", query.ProfileName);

            query = MiniSQLParser.Parse("GRANT UPDATE ON Table TO Admin") as Grant;
            Assert.Equal("UPDATE", query.PrivilegeName);
            Assert.Equal("Table", query.TableName);
            Assert.Equal("Admin", query.ProfileName);
        }

        [Fact]
        public void ValidQueryWithAnotherPrivilege()
        {
            var query = MiniSQLParser.Parse("GRANT INSERT ON Products TO manager") as Grant;
            Assert.NotNull(query);
            Assert.Equal("INSERT", query.PrivilegeName);
            Assert.Equal("Products", query.TableName);
            Assert.Equal("manager", query.ProfileName);

            query = MiniSQLParser.Parse("GRANT UPDATE ON Table TO User") as Grant;
            Assert.Equal("UPDATE", query.PrivilegeName);
            Assert.Equal("Table", query.TableName);
            Assert.Equal("User", query.ProfileName);
        }


        [Fact]
        public void CorrectWithSpaces()
        {
            Grant query = MiniSQLParser.Parse("GRANT DELETE    ON Table TO Admin") as Grant;
            Assert.Equal("DELETE", query.PrivilegeName);
            Assert.Equal("Table", query.TableName);
            Assert.Equal("Admin", query.ProfileName);

            query = MiniSQLParser.Parse("GRANT INSERT ON Table    TO Admin") as Grant;
            Assert.Equal("INSERT", query.PrivilegeName);
            Assert.Equal("Table", query.TableName);
            Assert.Equal("Admin", query.ProfileName);

            query = MiniSQLParser.Parse("GRANT SELECT ON Table TO     Admin") as Grant;
            Assert.Equal("SELECT", query.PrivilegeName);
            Assert.Equal("Table", query.TableName);
            Assert.Equal("Admin", query.ProfileName);

            query = MiniSQLParser.Parse("GRANT    UPDATE     ON    Table    TO     Admin") as Grant;
            Assert.Equal("UPDATE", query.PrivilegeName);
            Assert.Equal("Table", query.TableName);
            Assert.Equal("Admin", query.ProfileName);
        }

        [Fact]
        public void IncorrectCapitalization()
        {
            Grant query = MiniSQLParser.Parse("Grant DELETE ON Table TO Admin") as Grant;
            Assert.Null(query);

            query = MiniSQLParser.Parse("GRANT Insert ON Table TO Admin") as Grant;
            Assert.Null(query);

            query = MiniSQLParser.Parse("GRANT SELECT on Table TO Admin") as Grant;
            Assert.Null(query);

            query = MiniSQLParser.Parse("GRANT UPDATE ON Table To Admin") as Grant;
            Assert.Null(query);

            query = MiniSQLParser.Parse("GRANT DELETE ON Table TO Admin") as Grant;
            Assert.NotNull(query);
        }

        [Fact]
        public void IncorrectProfileWithErrorProfileName()
        {
            Grant query = MiniSQLParser.Parse("GRANT DELETE ON Table TO admin 1") as Grant;
            Assert.Null(query);

            query = MiniSQLParser.Parse("GRANT INSERT ON Table TO adm in") as Grant;
            Assert.Null(query);

            query = MiniSQLParser.Parse("GRANT SELECT ON Table TO Admin-1") as Grant;
            Assert.Null(query);

            query = MiniSQLParser.Parse("GRANT UPDATE ON Table To Admin_2") as Grant;
            Assert.Null(query);

            query = MiniSQLParser.Parse("GRANT DELETE ON Table TO Admin ") as Grant;
            Assert.NotNull(query);
        }

        [Fact]
        public void IncorrectPrivileges()
        {
            Grant query = MiniSQLParser.Parse("GRANT Remove ON Table TO Admin") as Grant;
            Assert.Null(query);

            query = MiniSQLParser.Parse("GRANT REMOVE ON Table TO Admin") as Grant;
            Assert.Null(query);

            query = MiniSQLParser.Parse("GRANT UPGRADE ON Table TO Admin") as Grant;
            Assert.Null(query);

            query = MiniSQLParser.Parse("GRANT SET ON Table TO Admin") as Grant;
            Assert.Null(query);

            query = MiniSQLParser.Parse("GRANT DELETE ON Table TO Admin") as Grant;
            Assert.NotNull(query);
        }

        [Fact]
        public void IncorrectWithoutOnePart()
        {
            Grant query = MiniSQLParser.Parse("GRANT ON Table TO Admin") as Grant;
            Assert.Null(query);

            query = MiniSQLParser.Parse("GRANT SELECT ON TO Admin") as Grant;
            Assert.Null(query);

            query = MiniSQLParser.Parse("GRANT SELECT TO Admin") as Grant;
            Assert.Null(query);

            query = MiniSQLParser.Parse("GRANT SELECT ON Table TO") as Grant;
            Assert.Null(query);

            query = MiniSQLParser.Parse("GRANT DELETE ON Table TO User") as Grant;
            Assert.NotNull(query);
        }
        [Fact]
        public void CorrectWithLowerCaseIdentifiers()
        {
            Grant query = MiniSQLParser.Parse("GRANT SELECT ON customers TO sales") as Grant;
            Assert.Equal("SELECT", query.PrivilegeName);
            Assert.Equal("customers", query.TableName);
            Assert.Equal("sales", query.ProfileName);
        }

        [Fact]
        public void CorrectWithNumbersInIdentifiers()
        {
            Grant query = MiniSQLParser.Parse("GRANT INSERT ON table1 TO profile123") as Grant;
            Assert.Equal("INSERT", query.PrivilegeName);
            Assert.Equal("table1", query.TableName);
            Assert.Equal("profile123", query.ProfileName);
        }

        [Fact]
        public void IncorrectKeywordOrder()
        {
            Grant query = MiniSQLParser.Parse("SELECT GRANT ON Table TO Admin") as Grant;
            Assert.Null(query);

            query = MiniSQLParser.Parse("GRANT TO Admin ON Table SELECT") as Grant;
            Assert.Null(query);
        }

        [Fact]
        public void IncorrectWithoutKeywordTO()
        {
            Grant query = MiniSQLParser.Parse("GRANT DELETE ON Table Admin") as Grant;
            Assert.Null(query);
        }

        [Fact]
        public void IncompletePrivilegeShouldFail()
        {
            Grant query = MiniSQLParser.Parse("GRANT UPD ON Table TO Admin") as Grant;
            Assert.Null(query);
        }

        [Fact]
        public void ValidWithMultipleSpacesAndTabs()
        {
            Grant query = MiniSQLParser.Parse("GRANT\tSELECT \t ON\tTable\tTO\tAdmin") as Grant;
            Assert.Equal("SELECT", query.PrivilegeName);
            Assert.Equal("Table", query.TableName);
            Assert.Equal("Admin", query.ProfileName);
        }

        [Fact]
        public void CorrectWithCaseSensitiveTableAndProfile()
        {
            Grant query = MiniSQLParser.Parse("GRANT DELETE ON CustomerOrders TO SalesTeam") as Grant;
            Assert.Equal("DELETE", query.PrivilegeName);
            Assert.Equal("CustomerOrders", query.TableName);
            Assert.Equal("SalesTeam", query.ProfileName);
        }

    }
}