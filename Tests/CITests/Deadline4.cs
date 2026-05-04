using DbManager;
using DbManager.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SecurityParsingTests
{
    public class Deadline4
    {
        [Fact]
        public void grantDelete()
        {
            Grant query = MiniSQLParser.Parse("GRANT DELETE ON Table TO User") as Grant;
            Assert.Equal("DELETE", query.PrivilegeName);
            Assert.Equal("Table", query.TableName);
            Assert.Equal("User", query.ProfileName);
        }

        [Fact]
        public void grantInsert()
        {
            Grant query = MiniSQLParser.Parse("GRANT INSERT ON Table TO User") as Grant;
            Assert.Equal("INSERT", query.PrivilegeName);
            Assert.Equal("Table", query.TableName);
            Assert.Equal("User", query.ProfileName);
        }
        [Fact]
        public void grantSelect()
        {
            Grant query = MiniSQLParser.Parse("GRANT SELECT ON Table TO User") as Grant;
            Assert.Equal("SELECT", query.PrivilegeName);
            Assert.Equal("Table", query.TableName);
            Assert.Equal("User", query.ProfileName);
        }
        [Fact]
        public void grantUpdate()
        {
            Grant query = MiniSQLParser.Parse("GRANT UPDATE ON Table TO User") as Grant;
            Assert.Equal("UPDATE", query.PrivilegeName);
            Assert.Equal("Table", query.TableName);
            Assert.Equal("User", query.ProfileName);
        }


        [Fact]
        public void grantUpdate()
        {
            Grant query = MiniSQLParser.Parse("GRANT UPDATE ON Table TO User") as Grant;
            Assert.Equal("UPDATE", query.PrivilegeName);
            Assert.Equal("Table", query.TableName);
            Assert.Equal("User", query.ProfileName);
        }
    }
}
