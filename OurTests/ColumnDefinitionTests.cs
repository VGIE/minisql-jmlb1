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
        
    }
}