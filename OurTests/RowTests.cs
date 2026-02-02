using DbManager;

namespace OurTests
{
    public class RowTests
    {
        //TODO DEADLINE 1A : Create your own tests for Row

        [Fact]
        //crear metodo para inicializar los rows
        private Row CreateTestRow()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "Age"),
                new ColumnDefinition(ColumnDefinition.DataType.Double, "Grades"),
            };

            List<String> rowValues = new List<String>()
            {
                "Jacinto", "37", "7.8"
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
            Assert.Equal("7.8", testRow.GetValue("Grades"));

            testRow.SetValue("Name", "Maider");

            Assert.Equal("Maider", testRow.GetValue("Name"));
            Assert.Equal("37", testRow.GetValue("Age"));
            Assert.Equal("7.8", testRow.GetValue("Grades"));
        }
        
    }
}