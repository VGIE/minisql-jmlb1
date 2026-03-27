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

        [Fact]
        public void Execute()
        {
            //crear base de datos
            Database db = new Database("admin", "admin");
            //crear lista de columnas
            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                //crear las columnas 
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "Age")

            };

            Table tabla = new Table("TablaPrueba", columns);

            //creamos los valores para las filas
            List<String> value1 = new List<String>() { "Markel", "20" };
            List<String> value2 = new List<String>() { "Ainhoa", "21" };
            List<String> value3 = new List<String>() { "Jon", "22" };

            //creamos las filas y las añadimos a la tabla
            Row row1 = new Row(columns, value1);
            Row row2 = new Row(columns, value2);
            Row row3 = new Row(columns, value3);

            tabla.AddRow(row1);
            tabla.AddRow(row2);
            tabla.AddRow(row3);

            //añadimos la tabla a la base de datos
            db.AddTable(tabla);

            //primero tiene 3 lineas
            Assert.Equal(3, tabla.NumRows());

            //pruebas de borrar

            Condition con1 = new Condition("Name", "=", "Markel");
            DbManager.Parser.Delete del1 = new DbManager.Parser.Delete("TablaPrueba", con1);
            del1.Execute(db);

            Table tablacon1 = db.TableByName("TablaPrueba");
            //hemos eliminado una linea, quedan 2 
            Assert.Equal(2, tablacon1.NumRows());


            Condition con2 = new Condition("Age", ">", "24");
            DbManager.Parser.Delete del2 = new DbManager.Parser.Delete("TablaPrueba", con2);
            del2.Execute(db);

            Table tablacon2 = db.TableByName("TablaPrueba");
            //no hemos eliminado ninguna
            Assert.Equal(2, tablacon2.NumRows());

            Condition con3 = new Condition("Age", "<", "22");
            DbManager.Parser.Delete del3 = new DbManager.Parser.Delete("TablaPrueba", con3);
            del3.Execute(db);

            Table tablacon3 = db.TableByName("TablaPrueba");
            Assert.Equal(1, tablacon3.NumRows());

        }
    }
}
