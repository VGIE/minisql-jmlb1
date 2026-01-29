using DbManager;

namespace OurTests
{
    public class RowTests
    {
        //TODO DEADLINE 1A : Create your own tests for Row

        //crear metodo para inicializar los rows
        private Row CreateTestRow()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "Age"),
            };

            List<String> rowValues = new List<String>()
            {
                "Jacinto", "37"
            };
            
            Row testRow = new Row(columns, rowValues);
            return testRow;
        }
        
        [Fact]
        public void Test1()
        {
            //mirara que pase por todos los if para conseguir toda la cobertura

            Row testRow = CreateTestRow();

            Assert.Equal("Jacinto", testRow.GetValue("Name"));
            Assert.Equal("37", testRow.GetValue("Age"));

            testRow.SetValue("Name", "Maider");


            //si da null
            Assert.NotNull(testRow.GetValue("Name"));
            Assert.NotNull(testRow.GetValue("Age"));
            
        }
        
    }
}