using DbManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace SecurityParsingTests
{
    public class SelectTests
    {
        /*[Fact]
        public void Execute()
        {
            Database database = new Database("user", "password");
            database.ExecuteMiniSQLQuery("CREATE TABLE TestTable (Age INT,Name TEXT)");
            Assert.Equal("[Age]", database.ExecuteMiniSQLQuery("SELECT Age FROM TestTable"));
        }*/

        //test de pruebas para encontrar el error
        [Fact]
        public void TestDebugManualSelect()
        {
            Database db = new Database("admin", "admin");

            List<ColumnDefinition> columns = new List<ColumnDefinition>();
            columns.Add(new ColumnDefinition(ColumnDefinition.DataType.String, "nombre"));
            columns.Add(new ColumnDefinition(ColumnDefinition.DataType.Int, "edad"));
            db.CreateTable("users", columns);

            List<string> values = new List<string>();
            values.Add("Markel");
            values.Add("20");
            db.Insert("users", values);

            List<string> selectCols = new List<string>();
            selectCols.Add("nombre");
            selectCols.Add("edad");
            Select select = new Select("users", selectCols, null);

            string result = select.Execute(db);

            Assert.Contains("Markel", result);
        }
        [Fact]
        public void TestCreateTablePattern()
        {
            string query = "CREATE TABLE users (Nombre TEXT, Edad INT)";
            Match match = Regex.Match(query, @"CREATE\s+TABLE\s+([a-zA-Z][a-zA-Z0-9]*)\s*\((.*)\)");

            Assert.True(match.Success);
            Assert.Equal("users", match.Groups[1].Value);
            Assert.Equal("Nombre TEXT, Edad INT", match.Groups[2].Value);
        }
        [Fact]
        public void TestCreateTableOnly()
        {
            Database db = new Database("admin", "admin");
            string result = db.ExecuteMiniSQLQuery("CREATE TABLE users (name TEXT, age INT)");
            Assert.Equal(Constants.CreateTableSuccess, result);
        }

        [Fact]
        public void TestDebugColumnExists()
        {
            Database db = new Database("admin", "admin");

            db.ExecuteMiniSQLQuery("CREATE TABLE users (nombre TEXT, edad INT)");

            Table table = db.TableByName("users");
            Assert.NotNull(table);

            int numCols = table.NumColumns();
            Assert.Equal(2, numCols);

            // Verificar los nombres de las columnas
            ColumnDefinition col1 = table.GetColumn(0);
            ColumnDefinition col2 = table.GetColumn(1);

            Assert.NotNull(col1);
            Assert.NotNull(col2);

            // Verificar ColumnByName
            ColumnDefinition found = table.ColumnByName("nombre");
            Assert.NotNull(found);
        }
        [Fact]
        public void TestDebugDatabaseSelect()
        {
            Database db = new Database("admin", "admin");

            db.ExecuteMiniSQLQuery("CREATE TABLE users (nombre TEXT, edad INT)");

            db.ExecuteMiniSQLQuery("INSERT INTO users VALUES ('Markel','20')");

            List<string> columns = new List<string>();
            columns.Add("nombre");
            columns.Add("edad");

            Table result = db.Select("users", columns, null);

            Assert.NotNull(result);

            string resultString = result.ToString();
            Assert.Contains("Markel", resultString);
        }

        [Fact]
        public void TestDebugSelectParsing()
        {
            string query = "SELECT nombre,edad FROM users";
            MiniSqlQuery result = MiniSQLParser.Parse(query);

            Assert.NotNull(result);
            Select select = result as Select;
            Assert.NotNull(select);
            Assert.Equal(2, select.Columns.Count);
            Assert.Equal("nombre", select.Columns[0]);
            Assert.Equal("edad", select.Columns[1]);
        }

        [Fact]
        public void TestSimpleSelectExecute()
        {
            Database db = new Database("admin", "admin");

            List<ColumnDefinition> columns = new List<ColumnDefinition>();
            columns.Add(new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre"));
            db.CreateTable("users", columns);

            List<string> values = new List<string>();
            values.Add("Markel");
            db.Insert("users", values);

            List<string> selectCols = new List<string>();
            selectCols.Add("Nombre");
            Select select = new Select("users", selectCols, null);

            string result = select.Execute(db);

            Assert.NotNull(result);
        }
        [Fact]
        public void TestSelectEmptyTable()
        {
            Database db = new Database("admin", "admin");

            List<ColumnDefinition> columns = new List<ColumnDefinition>();
            columns.Add(new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre"));
            db.CreateTable("users", columns);

            //tabla vacía

            List<string> selectCols = new List<string>();
            selectCols.Add("Nombre");
            Select select = new Select("users", selectCols, null);

            string result = select.Execute(db);

            Assert.NotNull(result);
        }
        [Fact]
        public void TestSelectWithWhere()
        {
            Database db = new Database("admin", "admin");

            List<ColumnDefinition> columns = new List<ColumnDefinition>();
            columns.Add(new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre"));
            columns.Add(new ColumnDefinition(ColumnDefinition.DataType.Int, "Edad"));
            db.CreateTable("users", columns);

            List<string> values = new List<string>();
            values.Add("Markel");
            values.Add("20");
            db.Insert("users", values);

            List<string> selectCols = new List<string>();
            selectCols.Add("Nombre");
            selectCols.Add("Edad");

            Condition condition = new Condition("Edad", ">", "18");
            Select select = new Select("users", selectCols, condition);

            string result = select.Execute(db);

            Assert.NotNull(result);
            Assert.Contains("Markel", result);
            Assert.Contains("20", result);
        }

        [Fact]
        public void TestFullSelect()
        {
            Database db = new Database("admin", "admin");

            db.ExecuteMiniSQLQuery("CREATE TABLE users (Nombre TEXT, Edad INT)");
            db.ExecuteMiniSQLQuery("INSERT INTO users VALUES ('Markel','20')");

            string result = db.ExecuteMiniSQLQuery("SELECT Nombre,Edad FROM users");

            Assert.NotNull(result);
        }

        [Fact]

        public void validSelects()
        {
            //Valid cases

            Select query = MiniSQLParser.Parse("SELECT nombre FROM tabla") as Select;
            Assert.NotNull(query);

            Select query2 = MiniSQLParser.Parse("SELECT nombre2 FROM tabla1") as Select;
            Assert.NotNull(query2);

            Select query3 = MiniSQLParser.Parse("SELECT nombre,edad FROM tabla1") as Select;
            Assert.NotNull(query3);

            Select query4 = MiniSQLParser.Parse("SELECT nombre,edad FROM tabla1 WHERE edad>'18'") as Select;
            Assert.NotNull(query4);

            //Valid spaces
            Select query5 = MiniSQLParser.Parse("SELECT nombre,edad FROM tabla1 WHERE    edad>'18'") as Select;
            Assert.NotNull(query5);

            Select query6 = MiniSQLParser.Parse("SELECT   nombre,edad FROM tabla1 WHERE  edad>'18'") as Select;
            Assert.NotNull(query6);

            Select query7 = MiniSQLParser.Parse("SELECT nombre,edad   FROM    tabla1 WHERE  edad>'18'") as Select;
            Assert.NotNull(query7);
            
            // (-) , doubles accepted
            Select query8 = MiniSQLParser.Parse("SELECT angle FROM ta3ble WHERE grade>'-90'") as Select;
            Assert.NotNull(query8);

            Select query9 = MiniSQLParser.Parse("SELECT angle FROM table WHERE grade='9.123'") as Select;
            Assert.NotNull(query9);

            Select query10 = MiniSQLParser.Parse("SELECT angle FROM table WHERE grade<'-11.2324'") as Select;
            Assert.NotNull(query10);

            //Text accepeted (and with spaces)
            Select query11 = MiniSQLParser.Parse("SELECT a,b,c FROM table1 WHERE name='A'") as Select;
            Assert.NotNull(query11);

            Select query12 = MiniSQLParser.Parse("SELECT a,b,c FROM table12 WHERE name='Iker DelHoyo'") as Select;
            Assert.NotNull(query12);



        }

            [Fact]
        
        public void invalidSelects()
        {
            //Not valid cases

            Select query = MiniSQLParser.Parse("SELECT * FROM tabla") as Select;
            Assert.Null(query);

            //Invalid spaces
            Select query2 = MiniSQLParser.Parse("SELECT nombre, edad FROM tabla") as Select;
            Assert.Null(query2);

            Select query3 = MiniSQLParser.Parse("SELECT nombre FROM tabla WHERE age >'18'") as Select;
            Assert.Null(query3);

            Select query4 = MiniSQLParser.Parse("SELECT nombre FROM tabla WHERE age> '18'") as Select;
            Assert.Null(query4);

            Select query5= MiniSQLParser.Parse("SELECT nombre FROM tabla WHERE age>'18 '") as Select;
            Assert.Null(query5);

            Select query6 = MiniSQLParser.Parse("SELECT nombre FROM tabla WHERE age>' 18'") as Select;
            Assert.Null(query6);

            Select query7 = MiniSQLParser.Parse("SELECT nombre FROM tabla WHERE age >'18'") as Select;
            Assert.Null(query7);

            Select query8 = MiniSQLParser.Parse("SELECT name, age FROM tabla WHERE age>'18'") as Select;
            Assert.Null(query8);

            Select query9 = MiniSQLParser.Parse("SELECT name ,age FROM tabla WHERE age>'18'") as Select;
            Assert.Null(query9);

            //No collumns selected
            Select query10 = MiniSQLParser.Parse("SELECT  FROM tabla WHERE age >'18'") as Select;
            Assert.Null(query10);

            Select query11 = MiniSQLParser.Parse("SELECT FROM tabla") as Select;
            Assert.Null(query11);


            //Incorrect conditions 
            Select query12 = MiniSQLParser.Parse("SELECT name,age FROM tabla WHERE age>18") as Select;
            Assert.Null(query12);

            Select query13 = MiniSQLParser.Parse("SELECT name,age FROM tabla WHERE age>'''") as Select;
            Assert.Null(query13);

            Select query14 = MiniSQLParser.Parse("SELECT name,age FROM tabla WHERE age>='18'") as Select;
            Assert.Null(query14);

            Select query15 = MiniSQLParser.Parse("SELECT age FROM tabla WHERE name>'Iker.DelHoyo'") as Select;
            Assert.Null(query15);

            Select query16 = MiniSQLParser.Parse("SELECT age FROM tabla WHERE name>'-Iker'") as Select;
            Assert.Null(query16);

            Select query17 = MiniSQLParser.Parse("SELECT name,age FROM tabla WHERE age>'17.'") as Select;
            Assert.Null(query17);


            Select query18 = MiniSQLParser.Parse("SELECT age FROM tabla WHERE name>'Iker  DelHoyo'") as Select;
            Assert.Null(query18);


            Select query19 = MiniSQLParser.Parse("SELECT age FROM tabla WHERE name>' Iker DelHoyo '") as Select;
            Assert.Null(query19);


            Select query20 = MiniSQLParser.Parse("SELECT name,age FROM tabla WHERE age>'17. 212'") as Select;
            Assert.Null(query20);

   
        }
    }
}
