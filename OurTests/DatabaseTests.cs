using DbManager;

namespace OurTests
{
    public class UnitTest1
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
    }
}