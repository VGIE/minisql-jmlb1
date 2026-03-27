using DbManager;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SecurityParsingTests
{
    public class UpdateTests
    {
        [Fact]
        public void TestIntegerStringDouble()
        {
            //Test 1: Integer 
            MiniSqlQuery result1 = MiniSQLParser.Parse("UPDATE tabla SET columna = '1' WHERE id = '2'");
            Assert.NotNull(result1);
            Update update1 = Assert.IsType<Update>(result1);
            Assert.Equal("tabla", update1.Table);
            Assert.Equal("columna", update1.Columns[0].ColumnName);
            Assert.Equal("1", update1.Columns[0].Value);
            Assert.NotNull(update1.Where);
            Assert.Equal("id", update1.Where.ColumnName);
            Assert.Equal("=", update1.Where.Operator);
            Assert.Equal("2", update1.Where.LiteralValue);

            //Test 2: Strings 
            MiniSqlQuery result2 = MiniSQLParser.Parse("UPDATE usuarios SET nombre = 'Juan' WHERE id = '1'");
            Assert.NotNull(result2);
            Update update2 = Assert.IsType<Update>(result2);
            Assert.Equal("usuarios", update2.Table);
            Assert.Equal("nombre", update2.Columns[0].ColumnName);
            Assert.Equal("Juan", update2.Columns[0].Value);
            Assert.NotNull(update2.Where);
            Assert.Equal("id", update2.Where.ColumnName);
            Assert.Equal("=", update2.Where.Operator);
            Assert.Equal("1", update2.Where.LiteralValue);

            //Test 3: Double
            MiniSqlQuery result3 = MiniSQLParser.Parse("UPDATE productos SET precio = '99.99' WHERE id = '5'");
            Assert.NotNull(result3);
            Update update3 = Assert.IsType<Update>(result3);
            Assert.Equal("productos", update3.Table);
            Assert.Equal("precio", update3.Columns[0].ColumnName);
            Assert.Equal("99.99", update3.Columns[0].Value);
            Assert.NotNull(update3.Where);
            Assert.Equal("id", update3.Where.ColumnName);
            Assert.Equal("=", update3.Where.Operator);
            Assert.Equal("5", update3.Where.LiteralValue);
        }

        [Fact]
        public void TestValid()
        {
            //Test 4: Con espacios
            MiniSqlQuery result4 = MiniSQLParser.Parse("UPDATE    usuarios    SET    nombre    =    'Juan'    WHERE    id    =    '1'");
            Assert.NotNull(result4);
            Update update4 = Assert.IsType<Update>(result4);
            Assert.Equal("usuarios", update4.Table);
            Assert.Equal("nombre", update4.Columns[0].ColumnName);
            Assert.Equal("Juan", update4.Columns[0].Value);
            Assert.NotNull(update4.Where);
            Assert.Equal("id", update4.Where.ColumnName);
            Assert.Equal("=", update4.Where.Operator);
            Assert.Equal("1", update4.Where.LiteralValue);

            //Test 5: Múltiples SET values
            MiniSqlQuery result5 = MiniSQLParser.Parse("UPDATE productos SET precio = '99.99',stock = '50' WHERE id = '5'");
            Assert.NotNull(result5);
            Update update5 = Assert.IsType<Update>(result5);
            Assert.Equal("productos", update5.Table);
            Assert.Equal(2, update5.Columns.Count);
            Assert.Equal("precio", update5.Columns[0].ColumnName);
            Assert.Equal("99.99", update5.Columns[0].Value);
            Assert.Equal("stock", update5.Columns[1].ColumnName);
            Assert.Equal("50", update5.Columns[1].Value);
            Assert.NotNull(update5.Where);
            Assert.Equal("id", update5.Where.ColumnName);
            Assert.Equal("=", update5.Where.Operator);
            Assert.Equal("5", update5.Where.LiteralValue);

            //Test 6: Strings con espacios internos
            MiniSqlQuery result6 = MiniSQLParser.Parse("UPDATE usuarios SET nombre = 'Juan Perez' WHERE id = '1'");
            Assert.NotNull(result6);
            Update update6 = Assert.IsType<Update>(result6);
            Assert.Equal("Juan Perez", update6.Columns[0].Value);

            //Test 7: UPDATE sin WHERE
            MiniSqlQuery result7 = MiniSQLParser.Parse("UPDATE usuarios SET activo = '1'");
            Assert.NotNull(result7);
            Update update7 = Assert.IsType<Update>(result7);
            Assert.Equal("usuarios", update7.Table);
            Assert.Equal("activo", update7.Columns[0].ColumnName);
            Assert.Equal("1", update7.Columns[0].Value);
            Assert.Null(update7.Where);
        }

        [Fact]
        public void TestAllInvalidUpdates()
        {
            //Test 1: Múltiples tablas
            MiniSqlQuery result1 = MiniSQLParser.Parse("UPDATE tabla1, tabla2 SET columna = '1' WHERE id = '2'");
            Assert.Null(result1);

            //Test 2: Formato del string mal (sin comillas)
            MiniSqlQuery result2 = MiniSQLParser.Parse("UPDATE usuarios SET nombre = Juan WHERE id = '1'");
            Assert.Null(result2);

            //Test 3: Comas mal en SET (sin comas)
            MiniSqlQuery result3 = MiniSQLParser.Parse("UPDATE usuarios SET nombre = 'Juan' apellido = 'Perez' WHERE id = '1'");
            Assert.Null(result3);

            //Test 4: Formato incorrecto WHERE
            MiniSqlQuery result4 = MiniSQLParser.Parse("UPDATE usuarios SET nombre = 'Juan' WHERE id '1'");
            Assert.Null(result4);

            //Test 5: WHERE sin condición
            MiniSqlQuery result5 = MiniSQLParser.Parse("UPDATE usuarios SET nombre = 'Juan' WHERE");
            Assert.Null(result5);

            //Test 6: SET vacío
            MiniSqlQuery result6 = MiniSQLParser.Parse("UPDATE usuarios SET WHERE id = '1'");
            Assert.Null(result6);

            //Test 7: Operador no permitido
            MiniSqlQuery result7 = MiniSQLParser.Parse("UPDATE usuarios SET nombre = 'Juan' WHERE id <> '1'");
            Assert.Null(result7);

            //Test 8: Números sin comillas (debe fallar)
            MiniSqlQuery result8 = MiniSQLParser.Parse("UPDATE tabla SET columna = 1 WHERE id = 2");
            Assert.Null(result8);

            //Test 9: Espacios alrededor de comas en SET (debe fallar)
            MiniSqlQuery result9 = MiniSQLParser.Parse("UPDATE productos SET precio = '99.99', stock = '50' WHERE id = '5'");
            Assert.Null(result9);
        }
    }
}
