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
                new ColumnDefinition(ColumnDefinition.DataType.Double, "Grades"),
            };

            List<String> rowValues = new List<String>()
            {
                "Jacinto", "37", "7.8"
            };

            Row testRow = new Row(columns, rowValues);

            return testRow;
        }
        private Row CreateTestRowNull()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "Age"),
                new ColumnDefinition(ColumnDefinition.DataType.Double, "Grades"),
            };

            List<String> rowValues1 = new List<String>()
            {
                "Joseba"
            };

            Row testRow1 = new Row(columns, rowValues1);

            return testRow1;
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

            Row testRow1 = CreateTestRowNull();
            //Age y Grades no existen en la lista de valores, debería devolver null
            Assert.Equal("Joseba", testRow1.GetValue("Name"));
            Assert.Null(testRow1.GetValue("Age"));
            Assert.Null(testRow1.GetValue("Grades"));
        }

        [Fact]
        public void TestIsTrue()
        {
            List<ColumnDefinition> columns = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Nombre"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "Edad"),
                new ColumnDefinition(ColumnDefinition.DataType.Double, "Altura"),
                
            };

            List<String> rowValues = new List<String>()
            {
                "Jacinto", "37", "7.8"
            };

            Row testRow = new Row(columns, rowValues);

            //Condiciones de nombres
            Condition equals = new Condition("Nombre", Condition.IgualQue, "Jacinto");
            Condition mayor = new Condition("Nombre", Condition.MayorQue, "Jacinto");

            Assert.True(testRow.IsTrue(equals));
            Assert.False(testRow.IsTrue(mayor));

            //Condiciones de edad
            Condition equalEdad = new Condition("Edad", Condition.IgualQue, "37");
            Condition mayorEdad = new Condition("Edad", Condition.MayorQue, "30");
            Condition menorEdad = new Condition("Edad", Condition.MenorQue, "40");

            Assert.True(testRow.IsTrue(equalEdad));
            Assert.True(testRow.IsTrue(mayorEdad));
            Assert.True(testRow.IsTrue(menorEdad));

            //Condiciones de altura
            Condition equalsAltura = new Condition("Altura", Condition.IgualQue, "7.8");
            Condition mayorAltura = new Condition("Altura", Condition.MayorQue, "7.0");
            Condition menorAltura = new Condition("Altura", Condition.MenorQue, "8.0");

            Assert.True(testRow.IsTrue(equalsAltura));
            Assert.True(testRow.IsTrue(mayorAltura));
            Assert.True(testRow.IsTrue(menorAltura));


        }
    }
}