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
    public class DropTableTests
    {
        [Fact]
        public void ValidDropTable()
        {
            //solo letras
            DropTable query = MiniSQLParser.Parse("DROP TABLE users") as DropTable;
            Assert.NotNull(query);
            Assert.Equal("users", query.Table);
            //letras y numeros
            DropTable query2 = MiniSQLParser.Parse("DROP TABLE Products123") as DropTable;
            Assert.NotNull(query2);
            Assert.Equal("Products123", query2.Table);
            //solo numeros
            DropTable query4 = MiniSQLParser.Parse("DROP TABLE 123") as DropTable;
            Assert.NotNull(query4);
            Assert.Equal("123", query4.Table);
            //muchos espacios
            DropTable query3 = MiniSQLParser.Parse("DROP    TABLE    accounts") as DropTable;
            Assert.NotNull(query3);
            Assert.Equal("accounts", query3.Table);
        }

        [Fact]
        public void InvalidDropTable()
        {
            //drop en minusculas
            DropTable query1 = MiniSQLParser.Parse("drop TABLE users") as DropTable;
            Assert.Null(query1);
            //table en minusculas
            DropTable query2 = MiniSQLParser.Parse("DROP table users") as DropTable;
            Assert.Null(query2);
            //sin nombre de tabla
            DropTable query3 = MiniSQLParser.Parse("DROP TABLE") as DropTable;
            Assert.Null(query3);
            //con guión bajo
            DropTable query5 = MiniSQLParser.Parse("DROP TABLE user_name") as DropTable;
            Assert.Null(query5);
            //texto extra
            DropTable query6 = MiniSQLParser.Parse("DROP TABLE users extra") as DropTable;
            Assert.Null(query6);
        }

        [Fact]
        public void DropTableExecute_ExistingTable_ReturnsSuccess()
        {
            //creo una bdd con un admin
            Database db = new Database("admin", "adminPassword");

            //creo una tabla y la añado
            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name")
            };
            Table table = new Table("People", columns);
            db.AddTable(table);

            //creo un comando DropTable
            DropTable drop = new DropTable("People");

            //lo ejecuto
            string result = drop.Execute(db);

            Assert.Equal(Constants.DropTableSuccess, result);

            //la tabla ya no existe
            Assert.Null(db.TableByName("People"));
        }

        [Fact]
        public void DropTableExecute_NonExistingTable_ReturnsError()
        {
            Database db = new Database("admin", "adminPassword");

            //no añado tabla
            DropTable drop = new DropTable("NoExiste");

            string result = drop.Execute(db);

            Assert.Equal(Constants.TableDoesNotExistError, result);
        }
    }
}
