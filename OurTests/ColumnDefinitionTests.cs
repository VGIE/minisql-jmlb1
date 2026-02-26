using DbManager;
using System.Diagnostics.CodeAnalysis;

namespace OurTests
{
    public class ColumnDefinitionsTests
    {
        //TODO DEADLINE 1A : Create your own tests for Table
        
        [Fact]
        public void TestAsText()
        {
            //instancia de la clase Column
            ColumnDefinition column = new ColumnDefinition(ColumnDefinition.DataType.String, "columnaPrueba");

            //convertimos en texto
            string resultado = column.AsText();

            //comprobamos el formato
            Assert.Equal("String->columnaPrueba", resultado);
        }
        [Fact]
        public void TestParserValido()
        {
            //prueba válida
            String prueba = "String->Name";

            ColumnDefinition column = ColumnDefinition.Parse(prueba);

            Assert.Equal(ColumnDefinition.DataType.String, column.Type);

            Assert.Equal("Name", column.Name);
        }

        [Fact]
        public void TestParserDelimEncode()
        {
            //delimitador codificado (encode) 
            ColumnDefinition column = new ColumnDefinition(ColumnDefinition.DataType.String, "Test->Columns");
            
            String text = column.AsText();

            ColumnDefinition parsedColumn = ColumnDefinition.Parse(text);

            Assert.Equal(column.Type, parsedColumn.Type);
            Assert.Equal(column.Name, parsedColumn.Name);
        }
    }
}