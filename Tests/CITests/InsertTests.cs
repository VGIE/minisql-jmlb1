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
            MiniSqlQuery result1 = MiniSQLParser.Parse("INSERT INTO tabla VALUES ('1','2','3')");
            Assert.NotNull(result1);
            Insert insert1 = Assert.IsType<Insert>(result1);
            Assert.Equal("tabla", insert1.Table);
            Assert.Equal("1", insert1.Values[0]);
            Assert.Equal("2", insert1.Values[1]);
            Assert.Equal("3", insert1.Values[2]);

            //Test 2: Strings simples
            MiniSqlQuery result2 = MiniSQLParser.Parse("INSERT INTO usuarios VALUES ('Juan ','Ana')");
            Assert.NotNull(result2);
            Insert insert2 = Assert.IsType<Insert>(result2);
            Assert.Equal("Juan ", insert2.Values[0]);
            Assert.Equal("Ana", insert2.Values[1]);

            //Test 3: Con espacios
            MiniSqlQuery result3 = MiniSQLParser.Parse("INSERT INTO usuarios VALUES ('1','Juan')");
            Assert.NotNull(result3);
            Insert insert3 = Assert.IsType<Insert>(result3);
            Assert.Equal("1", insert3.Values[0]);
            Assert.Equal("Juan", insert3.Values[1]);

            //Test 4: Double
            MiniSqlQuery result4 = MiniSQLParser.Parse("INSERT INTO tabla VALUES ('1.3','8.5','23.98')");
            Assert.NotNull(result4);
            Insert insert4 = Assert.IsType<Insert>(result4);
            Assert.Equal("tabla", insert4.Table);
            Assert.Equal("1.3", insert4.Values[0]);
            Assert.Equal("8.5", insert4.Values[1]);
            Assert.Equal("23.98", insert4.Values[2]);
        }

        [Fact]
        public void TestAllInvalidInserts()
        {
            //Test 1: Múltiples tablas
            MiniSqlQuery result1 = MiniSQLParser.Parse("INSERT INTO tabla1, tabla2 VALUES ('1', 'Juan')");
            Assert.Null(result1);

            //Test 2: Espacios y comas
            MiniSqlQuery result2 = MiniSQLParser.Parse("INSERT INTO usuarios VALUES ('1' 'Juan' 'Madrid')");
            Assert.Null(result2);

            MiniSqlQuery result3 = MiniSQLParser.Parse("INSERT INTO usuarios VALUES ('1 ' 'Juan' 'Madrid')");
            Assert.Null(result3);

            MiniSqlQuery result4 = MiniSQLParser.Parse("INSERT INTO usuarios VALUES ('1' 'Juan' 'Madrid' )");
            Assert.Null(result4);

            MiniSqlQuery result5 = MiniSQLParser.Parse("INSERT INTO usuarios VALUES ('1' 'Juan' 'M,adrid')");
            Assert.Null(result5);

            MiniSqlQuery result6 = MiniSQLParser.Parse("INSERT INTO usuarios VALUES ('1' 'J,uan' 'Madrid')");
            Assert.Null(result6);

            MiniSqlQuery result7 = MiniSQLParser.Parse("INSERT INTO usuarios VALUES ('1,' 'Juan' 'Madrid')");
            Assert.Null(result7);

            MiniSqlQuery result8 = MiniSQLParser.Parse("INSERT INTO usuarios VALUES ('1,' 'Ju,an' 'Madri,d')");
            Assert.Null(result8);

            MiniSqlQuery result9 = MiniSQLParser.Parse("INSERT INTO usuarios VALUES ('1,' 'Juan' 'Madri,d')");
            Assert.Null(result9);
        }
        
        [Fact]
        public void Execute()
        {
            //crear la base de datos
            Database db = new Database("admin", "admin");

            //creamos lista para las columnas
            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                //creamos las columnas
                new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "Edad")
            };

           
            //creamos la tabla
            Table table = new Table("TablaPrueba", columns);
            
            //añadir la tabla a la base de datos
            db.AddTable(table);

            //valores que vamos a insertar en cada fila
            List<String> value1 = new List<String>() { "Markel", "20" };
            List<String> value2 = new List<String>() { "Ainhoa", "21" };
            List<String> value3 = new List<String>() { "Jon", "22" };

            //operaciones
            Insert ins1 = new Insert("TablaPrueba", value1);
            Insert ins2 = new Insert("TablaPrueba", value2);
            Insert ins3 = new Insert("TablaPrueba", value3);

            string op1 = ins1.Execute(db);
            string op2 = ins2.Execute(db);
            string op3 = ins3.Execute(db);

            //probar que se han insertado
            Assert.Equal(Constants.InsertSuccess, op1);
            Assert.Equal(Constants.InsertSuccess, op2);
            Assert.Equal(Constants.InsertSuccess, op3);


            //casos no validos
            Insert ins4 = new Insert("TablaPrueba", new List<String>() { "Markel" });

            Insert ins5 = new Insert("TablaPrueba", new List<String>() { "Markel", "20", "Extra" });

            Insert ins6 = new Insert("TablaInexistente", new List<String>() { "Markel", "20" });

            string op4 = ins4.Execute(db);
            string op5 = ins5.Execute(db);
            string op6 = ins6.Execute(db);

            Assert.Equal(Constants.ColumnCountsDontMatch, op4);
            Assert.Equal(Constants.ColumnCountsDontMatch, op5);
            Assert.Equal(Constants.TableDoesNotExistError, op6);

        }
    }
}