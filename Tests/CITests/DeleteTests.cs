using DbManager;
using DbManager.Parser;
using DbManager.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SecurityParsingTests
{
    public class DeleteTests
    {
        [Fact]
        public void ValidDelete()
        {
            //con espacios
            Delete query1 = MiniSQLParser.Parse("DELETE FROM    users    WHERE   id='5'") as Delete;
            Assert.NotNull(query1);
            Assert.Equal("users", query1.Table);
            Assert.NotNull(query1.Where);

            //string 
            Delete query2 = MiniSQLParser.Parse("DELETE FROM users WHERE name='Jorge'") as Delete;
            Assert.NotNull(query2);
            Assert.Equal("users", query2.Table);
            Assert.NotNull(query2.Where);

            //int 
            Delete query3 = MiniSQLParser.Parse("DELETE FROM users WHERE id>'100'") as Delete;
            Assert.NotNull(query3);
            Assert.Equal("users", query3.Table);
            Assert.NotNull(query3.Where);

            //double
            Delete query4 = MiniSQLParser.Parse("DELETE FROM users WHERE id<'50.5'") as Delete;
            Assert.NotNull(query4);
            Assert.Equal("users", query4.Table);
            Assert.NotNull(query4.Where);
        }

        [Fact]
        public void InvalidDelete()
        {
            //varios operadores
            Delete query = MiniSQLParser.Parse("DELETE FROM users WHERE id<='5'") as Delete;
            Assert.Null(query);

            //con espacios en la condición
            Delete query0 = MiniSQLParser.Parse("DELETE FROM    users    WHERE   id    =    '5'") as Delete;
            Assert.Null(query0);

            //sin espacio 
            Delete query1 = MiniSQLParser.Parse("DELETEFROM users WHERE id='5'") as Delete;
            Assert.Null(query1);

            //where sin completar
            Delete query2 = MiniSQLParser.Parse("DELETE FROM users WHERE") as Delete;
            Assert.Null(query2);

            //sin operador
            Delete query3 = MiniSQLParser.Parse("DELETE FROM users WHERE name Jorge") as Delete;
            Assert.Null(query3);

            //multiples tablas
            Delete query4 = MiniSQLParser.Parse("DELETE FROM users, products WHERE id='5'") as Delete;
            Assert.Null(query4);

            //valor mal
            Delete query5 = MiniSQLParser.Parse("DELETE FROM users WHERE age=25") as Delete;
            Assert.Null(query5);

            //operador mal
            Delete query6 = MiniSQLParser.Parse("DELETE FROM users WHERE age!='25'") as Delete;
            Assert.Null(query6);

            //sin where
            Delete query7 = MiniSQLParser.Parse("DELETE FROM users") as Delete;
            Assert.Null(query7);
        }
    }
}
