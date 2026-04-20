using DbManager;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace SecurityParsingTests
{
    public class SelectTests
    {
        /*[Fact]
        public void Execute()
        {
            Database database = new Database("user", "password");
            database.ExecuteMiniSQLQuery("CREATE TABLE TestTable (Age INT,Name TEXT)");
            Assert.Equal("[Age]", database.ExecuteMiniSQLQuery("SELECT Age FROM TestTable"));
        }*/

        [Fact]

        public void validSelects()
        {
            //Valid cases

            Select query = MiniSQLParser.Parse("SELECT nombre FROM tabla") as Select;
            Assert.NotNull(query);

            Select query2 = MiniSQLParser.Parse("SELECT nombre2 FROM tabla1") as Select;
            Assert.NotNull(query2);

            Select query3 = MiniSQLParser.Parse("SELECT nombre,edad FROM tabla1") as Select;
            Assert.NotNull(query3);

            Select query4 = MiniSQLParser.Parse("SELECT nombre,edad FROM tabla1 WHERE edad>'18'") as Select;
            Assert.NotNull(query4);

            //Valid spaces
            Select query5 = MiniSQLParser.Parse("SELECT nombre,edad FROM tabla1 WHERE    edad>'18'") as Select;
            Assert.NotNull(query5);

            Select query6 = MiniSQLParser.Parse("SELECT   nombre,edad FROM tabla1 WHERE  edad>'18'") as Select;
            Assert.NotNull(query6);

            Select query7 = MiniSQLParser.Parse("SELECT nombre,edad   FROM    tabla1 WHERE  edad>'18'") as Select;
            Assert.NotNull(query7);
            
            // (-) , doubles accepted
            Select query8 = MiniSQLParser.Parse("SELECT angle FROM ta3ble WHERE grade>'-90'") as Select;
            Assert.NotNull(query8);

            Select query9 = MiniSQLParser.Parse("SELECT angle FROM table WHERE grade='9.123'") as Select;
            Assert.NotNull(query9);

            Select query10 = MiniSQLParser.Parse("SELECT angle FROM table WHERE grade<'-11.2324'") as Select;
            Assert.NotNull(query10);

            //Text accepeted (and with spaces)
            Select query11 = MiniSQLParser.Parse("SELECT a,b,c FROM table1 WHERE name='A'") as Select;
            Assert.NotNull(query11);

            Select query12 = MiniSQLParser.Parse("SELECT a,b,c FROM table12 WHERE name='Iker DelHoyo'") as Select;
            Assert.NotNull(query12);



        }

            [Fact]
        
        public void invalidSelects()
        {
            //Not valid cases

            Select query = MiniSQLParser.Parse("SELECT * FROM tabla") as Select;
            Assert.Null(query);

            //Invalid spaces
            Select query2 = MiniSQLParser.Parse("SELECT nombre, edad FROM tabla") as Select;
            Assert.Null(query2);

            Select query3 = MiniSQLParser.Parse("SELECT nombre FROM tabla WHERE age >'18'") as Select;
            Assert.Null(query3);

            Select query4 = MiniSQLParser.Parse("SELECT nombre FROM tabla WHERE age> '18'") as Select;
            Assert.Null(query4);

            Select query5= MiniSQLParser.Parse("SELECT nombre FROM tabla WHERE age>'18 '") as Select;
            Assert.Null(query5);

            Select query6 = MiniSQLParser.Parse("SELECT nombre FROM tabla WHERE age>' 18'") as Select;
            Assert.Null(query6);

            Select query7 = MiniSQLParser.Parse("SELECT nombre FROM tabla WHERE age >'18'") as Select;
            Assert.Null(query7);

            Select query8 = MiniSQLParser.Parse("SELECT name, age FROM tabla WHERE age>'18'") as Select;
            Assert.Null(query8);

            Select query9 = MiniSQLParser.Parse("SELECT name ,age FROM tabla WHERE age>'18'") as Select;
            Assert.Null(query9);

            //No collumns selected
            Select query10 = MiniSQLParser.Parse("SELECT  FROM tabla WHERE age >'18'") as Select;
            Assert.Null(query10);

            Select query11 = MiniSQLParser.Parse("SELECT FROM tabla") as Select;
            Assert.Null(query11);


            //Incorrect conditions 
            Select query12 = MiniSQLParser.Parse("SELECT name,age FROM tabla WHERE age>18") as Select;
            Assert.Null(query12);

            Select query13 = MiniSQLParser.Parse("SELECT name,age FROM tabla WHERE age>'''") as Select;
            Assert.Null(query13);

            Select query14 = MiniSQLParser.Parse("SELECT name,age FROM tabla WHERE age>='18'") as Select;
            Assert.Null(query14);

            Select query15 = MiniSQLParser.Parse("SELECT age FROM tabla WHERE name>'Iker.DelHoyo'") as Select;
            Assert.Null(query15);

            Select query16 = MiniSQLParser.Parse("SELECT age FROM tabla WHERE name>'-Iker'") as Select;
            Assert.Null(query16);

            Select query17 = MiniSQLParser.Parse("SELECT name,age FROM tabla WHERE age>'17.'") as Select;
            Assert.Null(query17);


            Select query18 = MiniSQLParser.Parse("SELECT age FROM tabla WHERE name>'Iker  DelHoyo'") as Select;
            Assert.Null(query18);


            Select query19 = MiniSQLParser.Parse("SELECT age FROM tabla WHERE name>' Iker DelHoyo '") as Select;
            Assert.Null(query19);


            Select query20 = MiniSQLParser.Parse("SELECT name,age FROM tabla WHERE age>'17. 212'") as Select;
            Assert.Null(query20);

   
        }
    }
}
