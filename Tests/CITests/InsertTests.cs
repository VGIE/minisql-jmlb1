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
    public class InsertTests
    {
        [Fact]
        public void TestAllValidInserts()
        {
            //Test 1: Integers
            MiniSqlQuery result1 = MiniSQLParser.Parse("INSERT INTO tabla VALUES (1, 2, 3)");
            Assert.NotNull(result1);
            Insert insert1 = Assert.IsType<Insert>(result1);
            Assert.Equal("tabla", insert1.Table);
            Assert.Equal("1", insert1.Values[0]);
            Assert.Equal("2", insert1.Values[1]);
            Assert.Equal("3", insert1.Values[2]);

            //Test 2: Strings simples
            MiniSqlQuery result2 = MiniSQLParser.Parse("INSERT INTO usuarios VALUES ('Juan', 'Ana')");
            Assert.NotNull(result2);
            Insert insert2 = Assert.IsType<Insert>(result2);
            Assert.Equal("Juan", insert2.Values[0]);
            Assert.Equal("Ana", insert2.Values[1]);

            //Test 3: Strings con comas
            MiniSqlQuery result3 = MiniSQLParser.Parse("INSERT INTO usuarios VALUES ('Juan', 'Madrid, España')");
            Assert.NotNull(result3);
            Insert insert3 = Assert.IsType<Insert>(result3);
            Assert.Equal("Madrid, España", insert3.Values[1]);

            //Test 4: Con espacios
            MiniSqlQuery result4 = MiniSQLParser.Parse("INSERT INTO usuarios VALUES ( 1 , 'Juan' )");
            Assert.NotNull(result4);
            Insert insert4 = Assert.IsType<Insert>(result4);
            Assert.Equal("1", insert4.Values[0]);
            Assert.Equal("Juan", insert4.Values[1]);
        }

        [Fact]
        public void TestAllInvalidInserts()
        {
            //Test 1: Múltiples tablas
            MiniSqlQuery result1 = MiniSQLParser.Parse("INSERT INTO tabla1, tabla2 VALUES (1, 'Juan')");
            Assert.Null(result1);

            //Test 2: Faltan comas
            MiniSqlQuery result2 = MiniSQLParser.Parse("INSERT INTO usuarios VALUES (1 'Juan' 'Madrid')");
            Assert.Null(result2);
        }
    }
}