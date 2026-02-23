using DbManager;
using DbManager.Parser;

namespace OurTests
{
    public class DatabaseTest
    {
        //TODO DEADLINE 1B : Create your own tests for Database
        
        [Fact]
        public void TestAddTable()
        {
            Database db = new Database("admin", "admin");
            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "Edad")
            };
            Table table = new Table("users", columns);

            Assert.True(db.AddTable(table));
            Assert.False(db.AddTable(table));
        }
        [Fact]
        public void TestTableByName()
        {
            Database db = new Database("admin", "admin");

            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "Edad")
            };
            Table table1 = new Table("users", columns);

            List<ColumnDefinition> columns2 = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Titulo"),
                new ColumnDefinition(ColumnDefinition.DataType.Double, "Duracion")
            };
            Table table2 = new Table("songs", columns2);

            db.AddTable(table1);
            db.AddTable(table2);

            //tablas que existen 
            Assert.NotNull(db.TableByName("users"));
            Assert.NotNull(db.TableByName("songs"));

            //tablas que no existen
            Assert.Null(db.TableByName("animals"));
            Assert.Null(db.TableByName("players"));

            //comprobar que son misma instancia
            Assert.Same(table1, db.TableByName("users"));
            Assert.Same(table2 , db.TableByName("songs"));

        }
        [Fact]
        public void TestCreateTable()
        {
            Database db = new Database("admin", "admin");

            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "Edad")
            };

            //se crea 
            Assert.True(db.CreateTable("users", columns));
            Assert.Equal(Constants.CreateTableSuccess, db.LastErrorMessage);

            //ya existe la tabla
            Assert.False(db.CreateTable("users", columns));
            Assert.Equal(Constants.TableAlreadyExistsError, db.LastErrorMessage);

            //no nos dan columnas
            Assert.False(db.CreateTable("animals", null));
            Assert.Equal(Constants.DatabaseCreatedWithoutColumnsError, db.LastErrorMessage);

        }
        [Fact]
        public void TestDropTable()
        {
            Database db = new Database("admin", "admin");

            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "Edad")
            };
            Table table = new Table("users", columns);
            db.AddTable(table);

            Assert.True(db.DropTable("users"));
            Assert.Equal(Constants.DropTableSuccess, db.LastErrorMessage);

            Assert.False(db.DropTable("animals"));
            Assert.Equal(Constants.TableDoesNotExistError, db.LastErrorMessage);

        }
        [Fact]
        public void TestInsert()
        {
            Database db = new Database("admin", "admin");

            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "Edad")
            };
            Table table = new Table("users", columns);
            db.AddTable(table);

            //valores correctos 
            List<string> valoresCorrectos = new List<string>() { "Lucas", "21" };
            Assert.True(db.Insert("users", valoresCorrectos));
            Assert.Equal(Constants.InsertSuccess, db.LastErrorMessage);

            //demasidos valores
            List<string> valoresDeMas = new List<string>() { "Lucas", "21", "amarillo" };
            Assert.False(db.Insert("users", valoresDeMas));
            Assert.Equal(Constants.ColumnCountsDontMatch, db.LastErrorMessage);

            //pocos valores
            List<string> valoresDeMenos = new List<string>() { "Lucas" };
            Assert.False(db.Insert("users", valoresDeMenos));
            Assert.Equal(Constants.ColumnCountsDontMatch, db.LastErrorMessage);

            //no nos dan valores
            List<string> valores = new List<string>();
            Assert.False(db.Insert("users", valores));
            Assert.Equal(Constants.ColumnCountsDontMatch, db.LastErrorMessage);

            //comprobar que la tabla existe
            List<string> valoresTablaNoExiste = new List<string>() { "Lucas", "21"};
            Assert.False(db.Insert("animals", valoresTablaNoExiste));
            Assert.Equal(Constants.TableDoesNotExistError, db.LastErrorMessage);
        }
        [Fact]
        public void TestSelect()
        {
            Database db = new Database("admin", "adminPassword");
            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
                new ColumnDefinition(ColumnDefinition.DataType.Double, "Height"),
                new ColumnDefinition(ColumnDefinition.DataType.String, "Age")
            };

            Table table = new Table("TableTest", columns);

            List<String> values1 = new List<String>() { "Rodolfo", "1.62", "25" };
            List<String> values2 = new List<String>() { "Maider", "1.67", "67" };
            List<String> values3 = new List<String>() { "Pepe", "1.55", "51" };
            List<String> values4 = new List<String>() { "Maria", "1.58", "22" };
            List<String> values5 = new List<String>() { "Leire", "1.70", "21" };
            List<String> values6 = new List<String>() { "Bea", "1.65", "21" };

            table.AddRow(new Row(columns, values1));
            table.AddRow(new Row(columns, values2));
            table.AddRow(new Row(columns, values3));
            table.AddRow(new Row(columns, values4));
            table.AddRow(new Row(columns, values5));
            table.AddRow(new Row(columns, values6));

            db.AddTable(table);

            //1.condicion bien
            Condition condition1 = new Condition("Age", ">", "30");
            List<string> columnNames = new List<string> { "Name", "Age", "Height" };
            Table result1 = db.Select("TableTest", columnNames, condition1);
            Assert.NotNull(result1);
            Assert.Equal(2, result1.NumRows()); 
            Assert.Equal(3, result1.NumColumns());

            //2.select sin condicion, devuelva todas
            Table result2 = db.Select("TableTest", columnNames, null);
            Assert.NotNull(result2);
            Assert.Equal(6, result2.NumRows());
            Assert.Equal(3, result2.NumColumns());

            //3.tabla no existe
            Table result3 = db.Select("noexiste", columnNames, null);
            Assert.Null(result3);
            Assert.Equal(Constants.TableDoesNotExistError, db.LastErrorMessage);

            //4.columna que no existe
            Table result4 = db.Select("TableTest", new List<string> {"Surname"}, null);
            Assert.Null(result4);
            Assert.Equal(Constants.ColumnDoesNotExistError, db.LastErrorMessage);

            //5.condicion que no devuelve nada
            Condition condition2 = new Condition("Name", "=", "Jorge");
            Table result5 = db.Select("TableTest", columnNames, condition2);
            Assert.NotNull(result5);
            Assert.Equal(0, result5.NumRows());
            Assert.Equal(3, result5.NumColumns());
        }


        //UPDATE
        [Fact]
        public void Update_TableNotExist()
        {
            Database db = new Database("admin", "password");
            var updates = new List<SetValue> { new SetValue("Age", "30") };
            var condition = new Condition("Age", "=", "25");

            bool result = db.Update("NoExiste", updates, condition);

            Assert.False(result);
            Assert.Equal(Constants.TableDoesNotExistError, db.LastErrorMessage);
        }

        //Doesn´t exist the column
        [Fact]
        public void Update_ColumnNotExist()
        {
            Database db = new Database("admin", "password");
            Table tabla = Table.CreateTestTable();
            db.AddTable(tabla);

            var updates = new List<SetValue> { new SetValue("NoColumn", "30") };
            var condition = new Condition("Age", "=", "25");

            bool result = db.Update(tabla.Name, updates, condition);

            Assert.False(result);
            Assert.Equal(Constants.ColumnDoesNotExistError, db.LastErrorMessage);
        }

        //All checked (with condition)
        [Fact]
        public void Update_WithCondition()
        {
            Database db = new Database("admin", "password");
            Table tabla = Table.CreateTestTable();
            db.AddTable(tabla);
            // Supongamos que hay una fila con Age 70
            var updates = new List<SetValue> { new SetValue("Age", "100") };
            var condition = new Condition("Age", ">", "60");

            bool result = db.Update(tabla.Name, updates, condition);

            Assert.True(result);
        }
    }
}