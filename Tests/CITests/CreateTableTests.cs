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
    public class CreateTableTests
    {
        [Fact]
        public void CreateTable()
        {
            //solo create table sin más información incompleto --> mal
            CreateTable op1 = MiniSQLParser.Parse("CREATE TABLE") as CreateTable;
            Assert.Null(op1);

            //solo create table sin más información incompleto --> mal
            CreateTable op2 = MiniSQLParser.Parse("create table") as CreateTable;
            Assert.Null(op2);

            //Una columna -> bien
            /*CreateTable op3 = MiniSQLParser.Parse("CREATE TABLE User (Num INT)") as CreateTable;
            Assert.NotNull(op3);

            //Varias columnas --> bien
            CreateTable op4 = MiniSQLParser.Parse("CREATE TABLE User (Num INT, Name TEXT, Age INT)") as CreateTable;
            Assert.NotNull(op4);
            */

            //sin coma entre las columnas --> mal
            CreateTable op5 = MiniSQLParser.Parse("CREATE TABLE User (Num INT Name TEXT Age INT)") as CreateTable;
            Assert.Null(op5);

            //sin nombre para la tabla --> mal
            CreateTable op6 = MiniSQLParser.Parse("CREATE TABLE  (Num INT, Name TEXT, Age INT)") as CreateTable;
            Assert.Null(op6);

            /*
            //nombre de la tabla con numeros --> bien
            CreateTable op7 = MiniSQLParser.Parse("CREATE TABLE Users1 (Num INT, Name TEXT, Age INT)") as CreateTable;
            Assert.NotNull(op7);
            */

            //nombre de tabla EMPIEZA con número --> mal
            CreateTable op8 = MiniSQLParser.Parse("CREATE TABLE 2Users (Num INT, Name TEXT, Age INT)") as CreateTable;
            Assert.Null(op8);

            //columnas sin el parentesis --> mal
            CreateTable op9 = MiniSQLParser.Parse("CREATE TABLE Users Num INT") as CreateTable;
            Assert.Null(op9);

        }
    }
}
