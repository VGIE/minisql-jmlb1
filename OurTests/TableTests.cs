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


        [Fact]
        public void TestGetRow()
        {
            List<ColumnDefinition> columnas = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
                new ColumnDefinition(ColumnDefinition.DataType.Double, "Height"),
                new ColumnDefinition(ColumnDefinition.DataType.Int, "Age")
            };

            Table tabla = new Table("TestTable", columnas);

            //Usar AddRow 
            Row fila1 = new Row(columnas, new List<string>() { "Rodolfo", "1.62", "25" });
            Row fila2 = new Row(columnas, new List<string>() { "Maider", "1.67", "67" });
            Row fila3 = new Row(columnas, new List<string>() { "Pepe", "1.55", "51" });

            tabla.AddRow(fila1);
            tabla.AddRow(fila2);
            tabla.AddRow(fila3);

            //hay filas
            Assert.Equal(3, tabla.NumRows());

            //filas que existen
            Row fila0 = tabla.GetRow(0);
            Assert.NotNull(fila0);
            Assert.Equal("Rodolfo", fila0.Values[0]);

            Row fila1obtenida = tabla.GetRow(1);
            Assert.NotNull(fila1obtenida);
            Assert.Equal("Maider", fila1obtenida.Values[0]);
            Assert.Equal("67", fila1obtenida.Values[2]);

            Row fila2obtenida = tabla.GetRow(2);
            Assert.NotNull(fila2obtenida);
            Assert.Equal("Pepe", fila2obtenida.Values[0]);

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

        [Fact]     
        public void TestNumRows()
        {
            List<ColumnDefinition> columnas = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name")
            };
            Table tabla = new Table("Nombres", columnas);
            //tabla vacia
            Assert.Equal(0, tabla.NumRows());

            tabla.AddRow(new Row(columnas, new List<string> { "Rodolfo" }));
            tabla.AddRow(new Row(columnas, new List<string> { "Maider" }));
            tabla.AddRow(new Row(columnas, new List<string> { "Pepe" }));
            //tabla no vacia
            Assert.Equal(3, tabla.NumRows());

        }

        [Fact]
        public void TestGetColumn()
        {
            List<ColumnDefinition> columnas = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
                new ColumnDefinition(ColumnDefinition.DataType.Double, "Height"),
                new ColumnDefinition(ColumnDefinition.DataType.String, "Age")
            };

            Table tabla = new Table("TestTable", columnas);
            //columnas que existen 
            Assert.Equal("Name", tabla.GetColumn(0).Name);
            Assert.Equal("Height", tabla.GetColumn(1).Name);
            Assert.Equal("Age", tabla.GetColumn(2).Name);
            //columnas que no existen
            Assert.Null(tabla.GetColumn(-1));
            Assert.Null(tabla.GetColumn(3));
        }

        [Fact]
        public void TestNumColumns()
        {
            //vacia
            List<ColumnDefinition> columnas0 = new List<ColumnDefinition>();
            Table tabla = new Table("Vacía", columnas0);
            //no vacia
            List<ColumnDefinition> columnas1 = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
                new ColumnDefinition(ColumnDefinition.DataType.Double, "Height"),
                new ColumnDefinition(ColumnDefinition.DataType.String, "Age")
            };
            Table tabla1 = new Table("TestTable", columnas1);
            Assert.Equal(0, tabla.NumColumns());
            Assert.Equal(3, tabla1.NumColumns());
        }

        [Fact]
        public void TestColumnByName()
        {
            List<ColumnDefinition> columnas = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
                new ColumnDefinition(ColumnDefinition.DataType.Double, "Height"),
                new ColumnDefinition(ColumnDefinition.DataType.String, "Age")
            };

            Table tabla = new Table("TestTable", columnas);
            //nombres existen
            Assert.Equal("Name", tabla.ColumnByName("Name").Name);
            Assert.Equal("Height", tabla.ColumnByName("Height").Name);
            Assert.Equal("Age", tabla.ColumnByName("Age").Name);
            //nombres que no existen
            Assert.Null(tabla.ColumnByName("Year"));
            Assert.Null(tabla.ColumnByName(""));
            Assert.Null(tabla.ColumnByName(null));

        }

        [Fact]
        public void TestColumnIndexByName()
        {
            List<ColumnDefinition> columnas = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
                new ColumnDefinition(ColumnDefinition.DataType.Double, "Height"),
                new ColumnDefinition(ColumnDefinition.DataType.String, "Age")
            };

            Table tabla = new Table("TestTable", columnas);
            //nombres existen
            Assert.Equal(0, tabla.ColumnIndexByName("Name"));
            Assert.Equal(1, tabla.ColumnIndexByName("Height"));
            Assert.Equal(2, tabla.ColumnIndexByName("Age"));
            //nombres no existen
            Assert.Equal(-1, tabla.ColumnIndexByName("Year"));
            Assert.Equal(-1, tabla.ColumnIndexByName(""));
            Assert.Equal(-1, tabla.ColumnIndexByName(null));

        }

        [Fact]
        public void TestToString()
        {
            List<ColumnDefinition> columnas = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
                new ColumnDefinition(ColumnDefinition.DataType.Double, "Height"),
                new ColumnDefinition(ColumnDefinition.DataType.String, "Age")
            };

            Table tabla = new Table("TestTable", columnas);

            //tabla sin filas
            string resultadoVacio = tabla.ToString();
            Assert.Equal("['Name','Height','Age']", resultadoVacio);

            //una fila
            tabla.AddRow(new Row(columnas, new List<string>() { "Juan", "1.75", "30" }));
            string resultadoUnaFila = tabla.ToString();
            Assert.Equal("['Name','Height','Age']{'Juan','1.75','30'}", resultadoUnaFila);

            //con más filas
            tabla.AddRow(new Row(columnas, new List<string>() { "Ana", "1.65", "25" }));
            tabla.AddRow(new Row(columnas, new List<string>() { "Luis", "1.80", "35" }));

            string resultadoTresFilas = tabla.ToString();
            Assert.Equal("['Name','Height','Age']{'Juan','1.75','30'}{'Ana','1.65','25'}{'Luis','1.80','35'}", resultadoTresFilas);

            //tabla sin columnas
            List<ColumnDefinition> sinColumnas = new List<ColumnDefinition>();
            Table tablaVacia = new Table("Vacía", sinColumnas);
            Assert.Equal("", tablaVacia.ToString());
        }

        [Fact]
        public void TestDeleteIthRow()
        {
            List<ColumnDefinition> columnas = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
                new ColumnDefinition(ColumnDefinition.DataType.Double, "Height"),
                new ColumnDefinition(ColumnDefinition.DataType.String, "Age")
            };

            Table tabla = new Table("TestTable", columnas);

            Row fila1 = new Row(columnas, new List<string>() { "Rodolfo", "1.62", "25" });
            Row fila2 = new Row(columnas, new List<string>() { "Maider", "1.67", "67" });
            Row fila3 = new Row(columnas, new List<string>() { "Pepe", "1.55", "51" });

            tabla.AddRow(fila1);
            tabla.AddRow(fila2);
            tabla.AddRow(fila3);

            //eliminar maider
            tabla.DeleteIthRow(1);
            Assert.Equal(2, tabla.NumRows());
            Assert.Equal("Rodolfo", tabla.GetRow(0).Values[0]);
            Assert.Equal("Pepe", tabla.GetRow(1).Values[0]);

            //eliminar rodolfo
            tabla.DeleteIthRow(0);
            Assert.Equal(1, tabla.NumRows());
            Assert.Equal("Pepe", tabla.GetRow(0).Values[0]);

            //eliminar última 
            tabla.DeleteIthRow(0);
            Assert.Equal(0, tabla.NumRows());
        }

        [Fact]
        public void TestDeleteWhere()
        {
            List<ColumnDefinition> columnas = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
                new ColumnDefinition(ColumnDefinition.DataType.Double, "Height"),
                new ColumnDefinition(ColumnDefinition.DataType.String, "Age")
            };

            Table tabla = new Table("TestTable", columnas);

            Row fila1 = new Row(columnas, new List<string>() { "Rodolfo", "1.62", "25" });
            Row fila2 = new Row(columnas, new List<string>() { "Maider", "1.67", "67" });
            Row fila3 = new Row(columnas, new List<string>() { "Pepe", "1.55", "51" });

            tabla.AddRow(fila1);
            tabla.AddRow(fila2);
            tabla.AddRow(fila3);

            //1. eliminar todas las filas
            Condition condicion1= new Condition("Age", ">", "0");
            tabla.DeleteWhere(condicion1);
            Assert.Equal(0, tabla.NumRows());
            //2. eliminar 0 filas
            tabla.AddRow(fila1);
            tabla.AddRow(fila2);
            tabla.AddRow(fila3);
            Condition condicion2 = new Condition("Age", "=", "100");
            tabla.DeleteWhere(condicion2);
            Assert.Equal(3, tabla.NumRows());
            //3. eliminar 1 fila
            Condition condicion3 = new Condition("Age", "=", "67");
            tabla.DeleteWhere(condicion3);
            Assert.Equal(2, tabla.NumRows());
            Assert.Equal("Rodolfo", tabla.GetRow(0).Values[0]);
            Assert.Equal("Pepe", tabla.GetRow(1).Values[0]);
        }


        [Fact]
        public void TestSelect()
        {
            List<ColumnDefinition> columnas = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
                new ColumnDefinition(ColumnDefinition.DataType.Double, "Height"),
                new ColumnDefinition(ColumnDefinition.DataType.String, "Age")
            };

            Table tabla = new Table("TestTable", columnas);

            Row fila1 = new Row(columnas, new List<string>() { "Diego", "1.62", "25" });
            Row fila2 = new Row(columnas, new List<string>() { "Maider", "1.67", "67" });
            Row fila3 = new Row(columnas, new List<string>() { "Pepe", "1.55", "51" });

            tabla.AddRow(fila1);
            tabla.AddRow(fila2);
            tabla.AddRow(fila3);

            //Diferentes casos de columnas para el testeo

            List<string> columns = new List<string>() { "Name" };
            List<string> columns2 = new List<string>() { "Name", "Height" };
            List<string> columns3 = new List<string>() { "Name", "Height", "Age" };
            List<string> collumns4 = new List<string>() { "Name", "Age" };
            
            //En el caso de no tener condicion, debe devolver las 3 personas


            Table resultNull = tabla.Select(columns, null);

            Assert.Equal(3, resultNull.NumRows());
            Assert.Equal(1,resultNull.NumColumns());

            
            Table resultNull2 = tabla.Select(columns2, null);

            Assert.Equal(3, resultNull2.NumRows()); 
            Assert.Equal(2, resultNull2.NumColumns());
            Assert.Equal("1.67", resultNull2.GetRow(1).GetValue("Height"));

            //Diferentes condiciones

            //Edad menor que 60
            Condition conditionAge = new Condition("Age", "<", "60");
            

            Table resultAge = tabla.Select(columns3, conditionAge);

            Assert.Equal(2 , resultAge.NumRows());
            Assert.Equal(3, resultAge.NumColumns());
            Assert.Equal("25", resultAge.GetRow(0).GetValue("Age"));
            Assert.Equal("Pepe", resultAge.GetRow(1).GetValue("Name"));

            //Nombre coincide
            Condition conditionName = new Condition("Name", "=", "Diego");
           
            Table resultName = tabla.Select(collumns4, conditionName);

            Assert.Equal(1, resultName.NumRows());
            Assert.Equal(2, resultName.NumColumns());

            //Mayor altura que 1.60
            Condition conditionHeight = new Condition("Height", ">", "1.60");

            Table resultHeight = tabla.Select(columns2, conditionHeight);

            Assert.Equal(2, resultHeight.NumRows());
            Assert.Equal(2, resultHeight.NumColumns());
            Assert.Equal("Diego", resultHeight.GetRow(0).GetValue("Name"));
            Assert.Equal("1.67", resultHeight.GetRow(1).GetValue("Height"));
        }


        [Fact] 
        public void TestInsert()
        {
            //1.valido
            List<ColumnDefinition> columnas = new List<ColumnDefinition>()
            {
                new ColumnDefinition(ColumnDefinition.DataType.String, "Name"),
                new ColumnDefinition(ColumnDefinition.DataType.Double, "Height"),
                new ColumnDefinition(ColumnDefinition.DataType.String, "Age")
            };

            Table tabla = new Table("TestTable", columnas);

            Row fila1 = new Row(columnas, new List<string>() { "Rodolfo", "1.62", "25" });
            Row fila2 = new Row(columnas, new List<string>() { "Maider", "1.67", "67" });
            Row fila3 = new Row(columnas, new List<string>() { "Pepe", "1.55", "51" });

            tabla.AddRow(fila1);
            tabla.AddRow(fila2);
            tabla.AddRow(fila3);

            List<string> values = new List<string> { "María", "1.58", "22" };

            bool res = tabla.Insert(values);
            Assert.True(res);
            Assert.Equal(4, tabla.NumRows());

            //2. no valido faltan valores
            List<string> values2= new List<string> { "Maria", "1.75" };
            bool res2=tabla.Insert(values2);
            Assert.False(res2);
            Assert.Equal(4, tabla.NumRows());
        }

       
    }
}