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
    }
}