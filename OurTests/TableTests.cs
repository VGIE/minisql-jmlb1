using DbManager;
using System.ComponentModel.DataAnnotations.Schema;

namespace OurTests
{
    public class TableTests
    {
        //TODO DEADLINE 1A : Create your own tests for Table
        /*
        [Fact]
        public void Test1()
        {

        }
        */
        /*

        [Fact]
        public void TestGetRow()
        {
            Table tabla = new Table("TestTable", new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
                new ColumnDefinition(ColumnDefinition.DataType.Double, "Height"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "Age")
            });

            tabla.Insert(new List<string>() { "Rodolfo", "1.62", "25" });
            tabla.Insert(new List<string>() { "Maider", "1.67", "67" });
            tabla.Insert(new List<string>() { "Rodolfo", "1.55", "51" });

            //filas que existen
            Row fila0 = tabla.GetRow(0);
            Assert.NotNull(fila0);
            Assert.Equal("Rodolfo", fila0.Values[0]);
            Row fila1 = tabla.GetRow(1);
            Assert.NotNull(fila1);
            Assert.Equal("Maider", fila1.Values[0]);
            Assert.Equal("67", fila1.Values[2]);
            //filas que no existen
            Assert.Null(tabla.GetRow(-1));
            Assert.Null(tabla.GetRow(3));
        }

        [Fact]
        public void TestAddRow()
        {
            List<ColumnDefinition> columnas = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
                new ColumnDefinition(ColumnDefinition.DataType.Double, "Height"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "Age")
            };
            Table tabla = new Table("Personas", columnas);
            Row fila1 = new Row(columnas, new List<string> { "Maria", "1.58", "22" });
            Row fila2 = new Row(columnas, new List<string> { "Jorge", "1.70", "21" });

            tabla.AddRow(fila1);
            tabla.AddRow(fila2);

            Assert.Equal(2, tabla.NumRows());
            Assert.Equal("Maria", tabla.GetRow(0).Values[0]);
            Assert.Equal("1.70", tabla.GetRow(1).Values[1]);
        }
        */
    }
}