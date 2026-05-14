using DbManager;
using DbManager.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OurTests
{
    public class ParserTests
    {
        //SELECT 
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

            Select query5 = MiniSQLParser.Parse("SELECT nombre FROM tabla WHERE age>'18 '") as Select;
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





        //DROPTABLE
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



        //INSERT
        [Fact]
        public void TestAllValidInserts()
        {
            //Test 1: Integers
            MiniSqlQuery result1 = MiniSQLParser.Parse("INSERT INTO tabla VALUES ('1','2','3')");
            Assert.NotNull(result1);
            Insert insert1 = Assert.IsType<Insert>(result1);
            Assert.Equal("tabla", insert1.Table);
            Assert.Equal("1", insert1.Values[0]);
            Assert.Equal("2", insert1.Values[1]);
            Assert.Equal("3", insert1.Values[2]);

            //Test 2: Strings simples
            MiniSqlQuery result2 = MiniSQLParser.Parse("INSERT INTO usuarios VALUES ('Juan ','Ana')");
            Assert.NotNull(result2);
            Insert insert2 = Assert.IsType<Insert>(result2);
            Assert.Equal("Juan ", insert2.Values[0]);
            Assert.Equal("Ana", insert2.Values[1]);

            //Test 3: Con espacios
            MiniSqlQuery result3 = MiniSQLParser.Parse("INSERT INTO usuarios VALUES ('1','Juan')");
            Assert.NotNull(result3);
            Insert insert3 = Assert.IsType<Insert>(result3);
            Assert.Equal("1", insert3.Values[0]);
            Assert.Equal("Juan", insert3.Values[1]);

            //Test 4: Double
            MiniSqlQuery result4 = MiniSQLParser.Parse("INSERT INTO tabla VALUES ('1.3','8.5','23.98')");
            Assert.NotNull(result4);
            Insert insert4 = Assert.IsType<Insert>(result4);
            Assert.Equal("tabla", insert4.Table);
            Assert.Equal("1.3", insert4.Values[0]);
            Assert.Equal("8.5", insert4.Values[1]);
            Assert.Equal("23.98", insert4.Values[2]);
        }

        [Fact]
        public void TestAllInvalidInserts()
        {
            //Test 1: Múltiples tablas
            MiniSqlQuery result1 = MiniSQLParser.Parse("INSERT INTO tabla1, tabla2 VALUES ('1', 'Juan')");
            Assert.Null(result1);

            //Test 2: Espacios y comas
            MiniSqlQuery result2 = MiniSQLParser.Parse("INSERT INTO usuarios VALUES ('1' 'Juan' 'Madrid')");
            Assert.Null(result2);

            MiniSqlQuery result3 = MiniSQLParser.Parse("INSERT INTO usuarios VALUES ('1 ' 'Juan' 'Madrid')");
            Assert.Null(result3);

            MiniSqlQuery result4 = MiniSQLParser.Parse("INSERT INTO usuarios VALUES ('1' 'Juan' 'Madrid' )");
            Assert.Null(result4);

            MiniSqlQuery result5 = MiniSQLParser.Parse("INSERT INTO usuarios VALUES ('1' 'Juan' 'M,adrid')");
            Assert.Null(result5);

            MiniSqlQuery result6 = MiniSQLParser.Parse("INSERT INTO usuarios VALUES ('1' 'J,uan' 'Madrid')");
            Assert.Null(result6);

            MiniSqlQuery result7 = MiniSQLParser.Parse("INSERT INTO usuarios VALUES ('1,' 'Juan' 'Madrid')");
            Assert.Null(result7);

            MiniSqlQuery result8 = MiniSQLParser.Parse("INSERT INTO usuarios VALUES ('1,' 'Ju,an' 'Madri,d')");
            Assert.Null(result8);

            MiniSqlQuery result9 = MiniSQLParser.Parse("INSERT INTO usuarios VALUES ('1,' 'Juan' 'Madri,d')");
            Assert.Null(result9);
        }



        //CREATE TABLE
        [Fact]
        public void CreateTable()
        {

            // el comando solo sin columnas ni parentesis --> mal
            CreateTable op1 = MiniSQLParser.Parse("CREATE  TABLE") as CreateTable;
            Assert.Null(op1);

            //el comando solo en minúsculas y sin parentesis --> mal
            CreateTable op2 = MiniSQLParser.Parse("create table") as CreateTable;
            Assert.Null(op2);

            CreateTable op3 = MiniSQLParser.Parse("create table USER (Name TEXT)") as CreateTable;
            Assert.Null(op3);

            //el comando solo con parentesis vacios --> bien 
            CreateTable op4 = MiniSQLParser.Parse("CREATE TABLE USERS ()") as CreateTable;
            Assert.NotNull(op4);

            //sin el nombre de la tabla -> mal
            CreateTable op5 = MiniSQLParser.Parse("CREATE TABLE (Name TEXT)") as CreateTable;
            Assert.Null(op5);

            //una unica columna -> bien 
            CreateTable op6 = MiniSQLParser.Parse("CREATE TABLE User (Num INT)") as CreateTable;
            Assert.NotNull(op6);

            // una columna y varios espacios --> bien
            CreateTable op7 = MiniSQLParser.Parse("CREATE    TABLE   User      (Number     INT)") as CreateTable;
            Assert.NotNull(op7);
            //espacios entre comando, nombre de la tabla y columnas -->bien
            CreateTable op8 = MiniSQLParser.Parse("CREATE TABLE      User     (NAME TEXT,AGE INT)") as CreateTable;
            Assert.NotNull(op8);

            // varias columnas separadas por comas --> bien
            CreateTable op9 = MiniSQLParser.Parse("CREATE TABLE USER (Name TEXT,age INT)") as CreateTable;
            Assert.NotNull(op9);

            //varias columnas no separadas --> mal
            CreateTable op10 = MiniSQLParser.Parse("CREATE TABLE USER (Name TEXT age INT)") as CreateTable;
            Assert.Null(op10);

            //una coma de mas al final de la columna --> mal
            CreateTable op11 = MiniSQLParser.Parse("CREATE TABLE User    (Num INT,)") as CreateTable;
            Assert.Null(op11);

            //columnas fuera del parentesis --> mal
            CreateTable op12 = MiniSQLParser.Parse("CREATE TABLE USER Name TEXT") as CreateTable;
            Assert.Null(op12);

            //nombre de la tabla con numeros --> bien
            CreateTable op13 = MiniSQLParser.Parse("CREATE TABLE Users1 (Num INT)") as CreateTable;
            Assert.NotNull(op13);

            //nombre de la tabla empieza con numeros --> mal
            CreateTable op14 = MiniSQLParser.Parse("CREATE TABLE 1Users (Num INT)") as CreateTable;
            Assert.Null(op14);

        }



        [Fact]
        public void SpacesTest()
        {
            CreateTable op15 = MiniSQLParser.Parse("CREATE TABLE User (NAME TEXT , AGE INT)") as CreateTable;
            Assert.Null(op15);

            CreateTable op16 = MiniSQLParser.Parse("CREATE TABLE User (NAME TEXT ,AGE INT)") as CreateTable;
            Assert.Null(op16);

            CreateTable op17 = MiniSQLParser.Parse("CREATE TABLE User (NAME TEXT, AGE INT)") as CreateTable;
            Assert.Null(op17);
        }





        //UPDATE TABLE
        [Fact]
        public void TestIntegerStringDouble()
        {
            //Test 1: Integer 
            MiniSqlQuery result1 = MiniSQLParser.Parse("UPDATE tabla SET columna = '1' WHERE id = '2'");
            Assert.NotNull(result1);
            Update update1 = Assert.IsType<Update>(result1);
            Assert.Equal("tabla", update1.Table);
            Assert.Equal("columna", update1.Columns[0].ColumnName);
            Assert.Equal("1", update1.Columns[0].Value);
            Assert.NotNull(update1.Where);
            Assert.Equal("id", update1.Where.ColumnName);
            Assert.Equal("=", update1.Where.Operator);
            Assert.Equal("2", update1.Where.LiteralValue);

            //Test 2: Strings 
            MiniSqlQuery result2 = MiniSQLParser.Parse("UPDATE usuarios SET nombre = 'Juan' WHERE id = '1'");
            Assert.NotNull(result2);
            Update update2 = Assert.IsType<Update>(result2);
            Assert.Equal("usuarios", update2.Table);
            Assert.Equal("nombre", update2.Columns[0].ColumnName);
            Assert.Equal("Juan", update2.Columns[0].Value);
            Assert.NotNull(update2.Where);
            Assert.Equal("id", update2.Where.ColumnName);
            Assert.Equal("=", update2.Where.Operator);
            Assert.Equal("1", update2.Where.LiteralValue);

            //Test 3: Double
            MiniSqlQuery result3 = MiniSQLParser.Parse("UPDATE productos SET precio = '99.99' WHERE id = '5'");
            Assert.NotNull(result3);
            Update update3 = Assert.IsType<Update>(result3);
            Assert.Equal("productos", update3.Table);
            Assert.Equal("precio", update3.Columns[0].ColumnName);
            Assert.Equal("99.99", update3.Columns[0].Value);
            Assert.NotNull(update3.Where);
            Assert.Equal("id", update3.Where.ColumnName);
            Assert.Equal("=", update3.Where.Operator);
            Assert.Equal("5", update3.Where.LiteralValue);
        }

        [Fact]
        public void TestValid()
        {
            //Test 4: Con espacios
            MiniSqlQuery result4 = MiniSQLParser.Parse("UPDATE    usuarios    SET    nombre    =    'Juan'    WHERE    id    =    '1'");
            Assert.NotNull(result4);
            Update update4 = Assert.IsType<Update>(result4);
            Assert.Equal("usuarios", update4.Table);
            Assert.Equal("nombre", update4.Columns[0].ColumnName);
            Assert.Equal("Juan", update4.Columns[0].Value);
            Assert.NotNull(update4.Where);
            Assert.Equal("id", update4.Where.ColumnName);
            Assert.Equal("=", update4.Where.Operator);
            Assert.Equal("1", update4.Where.LiteralValue);

            //Test 5: Múltiples SET values
            MiniSqlQuery result5 = MiniSQLParser.Parse("UPDATE productos SET precio = '99.99',stock = '50' WHERE id = '5'");
            Assert.NotNull(result5);
            Update update5 = Assert.IsType<Update>(result5);
            Assert.Equal("productos", update5.Table);
            Assert.Equal(2, update5.Columns.Count);
            Assert.Equal("precio", update5.Columns[0].ColumnName);
            Assert.Equal("99.99", update5.Columns[0].Value);
            Assert.Equal("stock", update5.Columns[1].ColumnName);
            Assert.Equal("50", update5.Columns[1].Value);
            Assert.NotNull(update5.Where);
            Assert.Equal("id", update5.Where.ColumnName);
            Assert.Equal("=", update5.Where.Operator);
            Assert.Equal("5", update5.Where.LiteralValue);

            //Test 6: Strings con espacios internos
            MiniSqlQuery result6 = MiniSQLParser.Parse("UPDATE usuarios SET nombre = 'Juan Perez' WHERE id = '1'");
            Assert.NotNull(result6);
            Update update6 = Assert.IsType<Update>(result6);
            Assert.Equal("Juan Perez", update6.Columns[0].Value);

            //Test 7: UPDATE sin WHERE
            MiniSqlQuery result7 = MiniSQLParser.Parse("UPDATE usuarios SET activo = '1'");
            Assert.NotNull(result7);
            Update update7 = Assert.IsType<Update>(result7);
            Assert.Equal("usuarios", update7.Table);
            Assert.Equal("activo", update7.Columns[0].ColumnName);
            Assert.Equal("1", update7.Columns[0].Value);
            Assert.Null(update7.Where);
        }

        [Fact]
        public void TestAllInvalidUpdates()
        {
            //Test 1: Múltiples tablas
            MiniSqlQuery result1 = MiniSQLParser.Parse("UPDATE tabla1, tabla2 SET columna = '1' WHERE id = '2'");
            Assert.Null(result1);

            //Test 2: Formato del string mal (sin comillas)
            MiniSqlQuery result2 = MiniSQLParser.Parse("UPDATE usuarios SET nombre = Juan WHERE id = '1'");
            Assert.Null(result2);

            //Test 3: Comas mal en SET (sin comas)
            MiniSqlQuery result3 = MiniSQLParser.Parse("UPDATE usuarios SET nombre = 'Juan' apellido = 'Perez' WHERE id = '1'");
            Assert.Null(result3);

            //Test 4: Formato incorrecto WHERE
            MiniSqlQuery result4 = MiniSQLParser.Parse("UPDATE usuarios SET nombre = 'Juan' WHERE id '1'");
            Assert.Null(result4);

            //Test 5: WHERE sin condición
            MiniSqlQuery result5 = MiniSQLParser.Parse("UPDATE usuarios SET nombre = 'Juan' WHERE");
            Assert.Null(result5);

            //Test 6: SET vacío
            MiniSqlQuery result6 = MiniSQLParser.Parse("UPDATE usuarios SET WHERE id = '1'");
            Assert.Null(result6);

            //Test 7: Operador no permitido
            MiniSqlQuery result7 = MiniSQLParser.Parse("UPDATE usuarios SET nombre = 'Juan' WHERE id <> '1'");
            Assert.Null(result7);

            //Test 8: Números sin comillas (debe fallar)
            MiniSqlQuery result8 = MiniSQLParser.Parse("UPDATE tabla SET columna = 1 WHERE id = 2");
            Assert.Null(result8);

            //Test 9: Espacios alrededor de comas en SET (debe fallar)
            MiniSqlQuery result9 = MiniSQLParser.Parse("UPDATE productos SET precio = '99.99', stock = '50' WHERE id = '5'");
            Assert.Null(result9);
        }





        //DELETE
        [Fact]
        public void ValidDelete()
        {
            //con espacios
            Delete query1 = MiniSQLParser.Parse("DELETE FROM    users    WHERE   id='5'") as Delete;
            Assert.NotNull(query1);
            Assert.Equal("users", query1.Table);
            Assert.NotNull(query1.Where);

            //string 
            Delete query2 = MiniSQLParser.Parse("DELETE FROM users WHERE name='Jorge'") as Delete;
            Assert.NotNull(query2);
            Assert.Equal("users", query2.Table);
            Assert.NotNull(query2.Where);

            //int 
            Delete query3 = MiniSQLParser.Parse("DELETE FROM users WHERE id>'100'") as Delete;
            Assert.NotNull(query3);
            Assert.Equal("users", query3.Table);
            Assert.NotNull(query3.Where);

            //double
            Delete query4 = MiniSQLParser.Parse("DELETE FROM users WHERE id<'50.5'") as Delete;
            Assert.NotNull(query4);
            Assert.Equal("users", query4.Table);
            Assert.NotNull(query4.Where);
        }

        [Fact]
        public void InvalidDelete()
        {
            //varios operadores
            Delete query = MiniSQLParser.Parse("DELETE FROM users WHERE id<='5'") as Delete;
            Assert.Null(query);

            //con espacios en la condición
            Delete query0 = MiniSQLParser.Parse("DELETE FROM    users    WHERE   id    =    '5'") as Delete;
            Assert.Null(query0);

            //sin espacio 
            Delete query1 = MiniSQLParser.Parse("DELETEFROM users WHERE id='5'") as Delete;
            Assert.Null(query1);

            //where sin completar
            Delete query2 = MiniSQLParser.Parse("DELETE FROM users WHERE") as Delete;
            Assert.Null(query2);

            //sin operador
            Delete query3 = MiniSQLParser.Parse("DELETE FROM users WHERE name Jorge") as Delete;
            Assert.Null(query3);

            //multiples tablas
            Delete query4 = MiniSQLParser.Parse("DELETE FROM users, products WHERE id='5'") as Delete;
            Assert.Null(query4);

            //valor mal
            Delete query5 = MiniSQLParser.Parse("DELETE FROM users WHERE age=25") as Delete;
            Assert.Null(query5);

            //operador mal
            Delete query6 = MiniSQLParser.Parse("DELETE FROM users WHERE age!='25'") as Delete;
            Assert.Null(query6);

            //sin where
            Delete query7 = MiniSQLParser.Parse("DELETE FROM users") as Delete;
            Assert.Null(query7);
        }





        //CREATE SECURITY PROFILE
        [Fact]
        public void Correct()
        {
            CreateSecurityProfile query = MiniSQLParser.Parse("CREATE SECURITY PROFILE profile") as CreateSecurityProfile;
            Assert.Equal("profile", query.ProfileName);

            query = MiniSQLParser.Parse("CREATE SECURITY PROFILE OtherProfile") as CreateSecurityProfile;
            Assert.Equal("OtherProfile", query.ProfileName);
        }

        [Fact]
        public void CorrectWithSpaces()
        {
            CreateSecurityProfile query = MiniSQLParser.Parse("CREATE     SECURITY PROFILE      profile") as CreateSecurityProfile;
            Assert.Equal("profile", query.ProfileName);

            query = MiniSQLParser.Parse("CREATE SECURITY     PROFILE OtherProfile") as CreateSecurityProfile;
            Assert.Equal("OtherProfile", query.ProfileName);
        }

        [Fact]
        public void IncorrectWithSpaces()
        {
            CreateSecurityProfile query = MiniSQLParser.Parse("CREATE SECURITY PROFILE new profile") as CreateSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("CREATE SECURITY PROFILE prof ile") as CreateSecurityProfile;
            Assert.Null(query);
        }

        [Fact]
        public void IncorrectCapitalization()
        {
            CreateSecurityProfile query = MiniSQLParser.Parse("Create SECURITY PROFILE profile") as CreateSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("create security profile OtherProfile") as CreateSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("CREATE SECURITY PROFILE profile") as CreateSecurityProfile;
            Assert.NotNull(query);
        }

        [Fact]
        public void IncorrectProfileWithForbiddenChars()
        {
            CreateSecurityProfile query = MiniSQLParser.Parse("CREATE SECURITY PROFILE pro-file") as CreateSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("CREATE SECURITY PROFILE Pro file") as CreateSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("CREATE SECURITY PROFILE profile") as CreateSecurityProfile;
            Assert.NotNull(query);
        }

        [Fact]
        public void IncorrectWithoutProfile()
        {
            CreateSecurityProfile query = MiniSQLParser.Parse("CREATE SECURITY PROFILE ") as CreateSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("CREATE SECURITY PROFILE") as CreateSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("CREATE SECURITY PROFILE profile") as CreateSecurityProfile;
            Assert.NotNull(query);
        }


        [Fact]
        public void IncorrectProfileWithNumbers()
        {
            CreateSecurityProfile query = MiniSQLParser.Parse("CREATE SECURITY PROFILE prof123") as CreateSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("CREATE SECURITY PROFILE 123prof") as CreateSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("CREATE SECURITY PROFILE p1r2o3f") as CreateSecurityProfile;
            Assert.Null(query);
        }

        [Fact]
        public void IncorrectProfileWithUnserscore()
        {
            CreateSecurityProfile query = MiniSQLParser.Parse("CREATE SECURITY PROFILE profile_one") as CreateSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("CREATE SECURITY PROFILE _profile") as CreateSecurityProfile;
            Assert.Null(query);

            query = MiniSQLParser.Parse("CREATE SECURITY PROFILE profile_") as CreateSecurityProfile;
            Assert.Null(query);
        }


    }
}
