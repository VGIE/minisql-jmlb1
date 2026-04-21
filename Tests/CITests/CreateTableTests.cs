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
            CreateTable op9  = MiniSQLParser.Parse("CREATE TABLE USER (Name TEXT,age INT)") as CreateTable;
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

        /*

        [Fact]
        public void SpacesTest()
        {
            CreateTable op15 = MiniSQLParser.Parse("CREATE TABLE User (NAME TEXT , AGE INT)") as CreateTable;
            Assert.Null(op15);

            CreateTable op16 = MiniSQLParser.Parse("CREATE TABLE User (NAME TEXT ,AGE INT)") as CreateTable;
            Assert.Null(op16);

            CreateTable op17= MiniSQLParser.Parse("CREATE TABLE User (NAME TEXT, AGE INT)") as CreateTable;
            Assert.Null(op17);
        }
        */
    }
}
